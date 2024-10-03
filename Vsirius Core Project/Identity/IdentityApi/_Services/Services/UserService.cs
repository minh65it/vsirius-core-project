using AutoMapper;
using IdentityApi._Repositoties;
using IdentityApi._Services.Interfaces;
using IdentityApi.Dtos;
using IdentityApi.Dtos.User;
using IdentityApi.Helpers.Utilities;
using VsiriusDataModel.DataModel;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Azure.Core;
using IdentityApi._Repositoties.Interfaces;
using System.Web;
using VSiriusSystemLog.WriteLog;

namespace IdentityApi._Services.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<VsiriusUser> _userManager;
        private readonly SignInManager<VsiriusUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IFunctionUtilities _functionUtilities;
        private readonly IValidUser _validUser;
        private readonly IEmailSender _emailSender;
        private HttpRequest _request;
        private readonly IRepositoryAccessor _repositoryAccessor;
        private ILoggerCustom _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IAzureStorage _azureStorage;

        public UserService(UserManager<VsiriusUser> userManager,
            IMapper mapper,
            IFunctionUtilities functionUtilities,
            SignInManager<VsiriusUser> signInManager,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor,
            IRepositoryAccessor repositoryAccessor,
            IValidUser validUser,
            IConfiguration configuration,
            IAzureStorage azureStorage,
            ILoggerCustom logService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _functionUtilities = functionUtilities;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _repositoryAccessor = repositoryAccessor;
            _validUser = validUser;
            _configuration = configuration;
            _azureStorage = azureStorage;
            _logService = logService;
        }


        public async Task<OperationResult> CreateUser(UserDto Dto, IUrlHelper urlHelper, string originUrl)
        {
            //check mail
            var checkValid = _functionUtilities.IsValidEmail(Dto.Email);
            if (!checkValid)
            {
                return new OperationResult { Message = "Your email is not in the correct format", Success = false };
            }
            var userCheck = await _userManager.FindByEmailAsync(Dto.Email);
            if (userCheck != null)
            {
                return new OperationResult { Message = "Account already exists", Success = false };
            }
            // check password
            var checkPassword = _functionUtilities.IsValidPassword(Dto.Password);
            if (!checkPassword)
            {
                return new OperationResult { Message = "Password must contain uppercase letters, lowercase letters, and numbers", Success = false };
            }

            if (!string.IsNullOrWhiteSpace(Dto?.PhoneNumber))
            {
                var checkValidPhone = _functionUtilities.IsValidPhone(Dto.PhoneNumber);
                if (!checkValidPhone)
                {
                    return new OperationResult { Message = "Your phone number is not in the correct format", Success = false };
                }
            }
            var user = _mapper.Map<VsiriusUser>(Dto);
            user.UserName = Dto.Email;
            user.IsActive = true;
            user.TwoFactorEnabled = false;
            user.EmailConfirmed = false;
            user.Id = Guid.NewGuid().ToString();

            try
            {
                if (Dto.Avatar != null)
                {
                    var UploadRs = await _azureStorage.UploadAsync(Dto.Avatar);
                    if(UploadRs.Error == true)
                    {
                        return new OperationResult { Message = "Can not upload image, please try again", Success = false };
                    }
                    user.Avatar = UploadRs.Blob.Uri;
                }
                var result = await _userManager.CreateAsync(user, Dto.Password);
                //Send mail confirm
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _request = _httpContextAccessor.HttpContext.Request;
                originUrl = originUrl != null ? originUrl : _configuration["FrontEndUrl"];
                var confirmationLink = urlHelper.Action("Index", "Confirm", new { email = Dto.Email, token = token, redirect = originUrl }, _request.Scheme);
                var checkMail = await _emailSender.SendEmailSendgird(Dto.Email, "Confirm your email from Vsirius", $"Confirm email for Vsirius at: <a href='{confirmationLink}'>{confirmationLink}</a>.");
                if (!checkMail)
                {
                    await _userManager.DeleteAsync(user);
                    await _azureStorage.DeleteAsync(Common.GetFileNameFromBlob(user.Avatar));
                    //_functionUtilities.DeleteFile(user.Avatar);
                    return new OperationResult { Message = "Registered email does not exist", Success = false };
                }
                //await SendSMS(Dto.Email);
                if (result.Succeeded)
                {
                    return new OperationResult { Message = "Successful registration, check your email to verify your account", Success = result.Succeeded };
                }
                else
                {
                    await _userManager.DeleteAsync(user);
                    await _azureStorage.DeleteAsync(Common.GetFileNameFromBlob(user.Avatar));
                    //_functionUtilities.DeleteFile(user.Avatar);
                    return new OperationResult { Message = "An error has occurred, contact Vsirus for support", Success = result.Succeeded };
                }
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new OperationResult { Message = "An error has occurred, contact Vsirus for support", Success = false };
            }
        }
        public async Task ConfirmEmail(string email, IUrlHelper urlHelper, string originUrl)
        {
            try
            {
                var checkValid = _functionUtilities.IsValidEmail(email);
                if (!checkValid)
                {
                    return;
                }
                _request = _httpContextAccessor.HttpContext.Request;
                originUrl ??= _configuration["FrontEndUrl"];

                var user = await _userManager.FindByEmailAsync(email);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = $"{originUrl}/Confirm?email={HttpUtility.UrlEncode(email)}&token={HttpUtility.UrlEncode(token)}";

                await _emailSender.SendEmailSendgird(email, 
                    "Confirm your email from Vsirius", $"Confirm email for Vsirius at:<br/> " +
                    $"<button class = 'btn-primary'> <a href='{confirmationLink}'>Confirm</a> </button>" +
                    $"<a href='{confirmationLink}'>{confirmationLink}</a>");
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
            }
        }
        public async Task SendSMS(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);

                var message = $"Your authentication code is: {code}";

                var sendSMS = await _functionUtilities.SendSMSWithAzure(user.PhoneNumber, message);
                if (!sendSMS)
                    new OperationResult { Message = "Gửi mã xác thực bị lỗi", Success = false };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
            }
        }

        public async Task<OperationResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            try
            {

                var roles = await _userManager.GetRolesAsync(user);
                foreach (var item in roles)
                {
                    await _userManager.RemoveFromRoleAsync(user, item);
                }
                //_functionUtilities.DeleteFile(user.Avatar); 
                await _azureStorage.DeleteAsync(Common.GetFileNameFromBlob(user.Avatar));
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return new OperationResult { Message = "Deleted Success", Success = true };
                else
                    return new OperationResult { Message = "Deleted Error", Success = false };
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new OperationResult { Message = "Deleted Error", Success = false, Caption = ex.ToString() };
            }
        }

        public async Task<PaginationUtility<UserDto>> GetAllUser(PaginationParam pagination, string inputSearch)
        {
            var predicate = PredicateBuilder.New<VsiriusUser>(true);
            if (!string.IsNullOrWhiteSpace(inputSearch))
            {
                predicate = predicate.And(x => x.Email.ToLower().Contains(inputSearch.ToLower()) ||
                    x.UserName.ToLower().Contains(inputSearch.ToLower()) ||
                    x.PhoneNumber.ToLower().Contains("+1" + inputSearch.ToLower()) ||
                    x.Address1.ToLower().Contains(inputSearch.ToLower()) ||
                    x.Address2.ToLower().Contains(inputSearch.ToLower())
                    );
            }

            try
            {
                var data = await _userManager.Users.Where(predicate).ToListAsync();
                var result = _mapper.Map<List<UserDto>>(data);
                return PaginationUtility<UserDto>.Create(result, pagination.PageNumber, pagination.PageSize, true);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return null;
            }

        }

        public async Task<UserDto> GetUserDetail(string mail)
        {
            try
            {
                if (string.IsNullOrEmpty(mail))
                {
                    return null;
                }
                var data = await _userManager.FindByEmailAsync(mail);
                var result = _mapper.Map<UserDto>(data);

                // tạm thời không cần xác thực phone
                result.PhoneNumberConfirmed = true;
                result.TwoFactorEnabled = true;
                return result;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new UserDto();
            }

        }

        public async Task<UserForLoggedDto> Login(UserForLoginParam param)
        {
            try
            {
                VsiriusUser user = new VsiriusUser();
                if (param.IsAuthen)
                {
                    if (param.Provider == "Google")
                    {
                        // Gửi yêu cầu xác thực đến Google API sử dụng access token
                        var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync($"https://www.googleapis.com/oauth2/v3/userinfo?access_token={param.Password}");

                        if (response.IsSuccessStatusCode)
                        {
                            // Đọc và xử lý phản hồi từ Google API
                            var content = await response.Content.ReadAsStringAsync();
                            var googleUser = JsonConvert.DeserializeObject<GoogleUser>(content);

                            if (googleUser.email_verified)
                            {
                                user = await _repositoryAccessor.user.FindSingle(x => x.Email == googleUser.email && x.IsActive == true);
                                if (user == null)
                                {
                                    user = new VsiriusUser
                                    {
                                        Email = googleUser.email,
                                        UserName = googleUser.email,
                                        FullName = googleUser.name,
                                        NormalizedEmail = googleUser.email.ToUpper(),
                                        NormalizedUserName = googleUser.email.ToUpper(),
                                        Avatar = googleUser.picture,
                                        EmailConfirmed = true,
                                        IsActive = true
                                    };
                                    _repositoryAccessor.user.Add(user);
                                    await _repositoryAccessor.Save();
                                }
                                else
                                {
                                    user.EmailConfirmed = true;
                                    _repositoryAccessor.user.Update(user);
                                    await _repositoryAccessor.Save();
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    if (param.Provider == "Facebook")
                    {
                        var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync($"https://graph.facebook.com/v18.0/me?fields=id,name,email,picture&access_token={param.Password}");

                        if (response.IsSuccessStatusCode)
                        {
                            // Đọc và xử lý phản hồi từ Google API
                            var content = await response.Content.ReadAsStringAsync();
                            var fbUser = JsonConvert.DeserializeObject<FacebookUser>(content);
                            var email = string.IsNullOrEmpty(fbUser.Email) ? param.Email : fbUser.Email;

                            user = await _repositoryAccessor.user.FindSingle(x => x.Email == email && x.IsActive == true);

                            if (user == null)
                            {
                                user = new VsiriusUser
                                {
                                    Email = email,
                                    UserName = email,
                                    FullName = fbUser.Name,
                                    NormalizedEmail = fbUser.Email.ToUpper(),
                                    NormalizedUserName = email.ToUpper(),
                                    Avatar = fbUser.Picture.Data.Url,
                                    EmailConfirmed = false,
                                    IsActive = true
                                };
                                _repositoryAccessor.user.Add(user);
                                await _repositoryAccessor.Save();
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    var checkMail = _functionUtilities.IsValidEmail(param.Email);
                    if (!checkMail)
                    {
                        return null;
                    }
                    if (string.IsNullOrEmpty(param.Email))
                    {
                        return null;
                    }
                    var checkData = await _signInManager.PasswordSignInAsync(param.Email,
                                  param.Password, true, lockoutOnFailure: true);
                    if (!checkData.Succeeded) return null;
                    user = await _userManager.FindByEmailAsync(param.Email);
                }
                //create claims details based on the user information
                if (user == null) return null;
                if (user.IsActive != true) return null;
                var rolesToken = new string[] { "Customer" };
                var token = _functionUtilities.GenerateToken(user.UserName, user.Email, rolesToken, 10);

                //export info when login
                var result = new UserForLoggedDto
                {
                    isLogin = true,
                    user = _mapper.Map<UserDto>(user),
                    token = token,
                    Message = "Login Success"
                };
                return result;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new UserForLoggedDto();
            }

        }

        public Task<OperationResult> LogOut()
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> UpdateUser(UserDto Dto)
        {
            var user = await _userManager.FindByIdAsync(Dto.Id);
            if (user == null)
            {
                return new OperationResult { Message = "Account does not exist", Success = false };
            }
            Dto.Email = user.Email;
            var checkDto = _validUser.IsValidUpdateUser(Dto);
            if (!checkDto.Success)
                return checkDto;

            if (Dto.PasswordHash != user.PasswordHash)
            {
                return new OperationResult { Message = "You can't change information", Success = false };
            }

            if (user.PhoneNumber != Dto?.PhoneNumber)
            {
                user.TwoFactorEnabled = false;
                user.PhoneNumberConfirmed = false;
            }
            user.PhoneNumber = Dto?.PhoneNumber;
            user.Address1 = Dto?.Address1;
            user.Address2 = Dto?.Address2;
            user.State = Dto?.State;
            user.City = Dto?.City;
            user.Country = Dto?.Country;
            user.FullName = Dto?.FullName;
            user.PostCode = Dto?.PostCode;
            user.IsActive = Dto?.IsActive;

            if (!string.IsNullOrWhiteSpace(Dto.Avatar))
            {
                await _azureStorage.DeleteAsync(Common.GetFileNameFromBlob(user.Avatar));

                //_functionUtilities.DeleteFile(user.Avatar);
                var updateRs = await _azureStorage.UploadAsync(Dto.Avatar);
                if(updateRs.Error == true)
                {
                    return new OperationResult { Message = "An error has occurred, contact Vsirus for support", Success = false };
                }
                user.Avatar = updateRs.Blob.Uri;
            }

            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return new OperationResult { Message = "Successful updated", Success = true };
                else
                    return new OperationResult { Message = "An error has occurred, contact Vsirus for support", Success = false };
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new OperationResult { Message = "An error has occurred, contact Vsirus for support", Success = false, Caption = ex.ToString() };
            }
        }

        public string GetState()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("AL", "Alabama");
            data.Add("AK", "Alaska");
            data.Add("AZ", "Arizona");
            data.Add("AR", "Arkansas");
            data.Add("CA", "California");
            data.Add("CO", "Colorado");
            data.Add("CT", "Connecticut");
            data.Add("DE", "Delaware");
            data.Add("FL", "Florida");
            data.Add("GA", "Georgia");
            data.Add("HI", "Hawaii");
            data.Add("ID", "Idaho");
            data.Add("IL", "Illinois");
            data.Add("IN", "Indiana");
            data.Add("IA", "Iowa");
            data.Add("KS", "Kansas");
            data.Add("KY", "Kentucky");
            data.Add("LA", "Louisiana");
            data.Add("ME", "Maine");
            data.Add("MD", "Maryland");
            data.Add("MA", "Massachusetts");
            data.Add("MI", "Michigan");
            data.Add("MN", "Minnesota");
            data.Add("MS", "Mississippi");
            data.Add("MO", "Missouri");
            data.Add("MT", "Montana");
            data.Add("NE", "Nebraska");
            data.Add("NV", "Nevada");
            data.Add("NH", "New Hampshire");
            data.Add("NJ", "New Jersey");
            data.Add("NM", "New Mexico");
            data.Add("NY", "New York");
            data.Add("NC", "North Carolina");
            data.Add("ND", "North Dakota");
            data.Add("OH", "Ohio");
            data.Add("OK", "Oklahoma");
            data.Add("OR", "Oregon");
            data.Add("PA", "Pennsylvania");
            data.Add("RI", "Rhode Island");
            data.Add("SC", "South Carolina");
            data.Add("SD", "South Dakota");
            data.Add("TN", "Tennessee");
            data.Add("TX", "Texas");
            data.Add("UT", "Utah");
            data.Add("VT", "Vermont");
            data.Add("VA", "Virginia");
            data.Add("WA", "Washington");
            data.Add("WV", "West Virginia");
            data.Add("WI", "Wisconsin");
            data.Add("WY", "Wyoming");
            data.Add("DC", "District of Columbia");
            data.Add("AS", "American Samoa");
            data.Add("GU", "Guam");
            data.Add("MP", "Northern Mariana Islands");
            data.Add("PR", "Puerto Rico");
            data.Add("UM", "United States Minor Outlying Islands");
            data.Add("VI", "Virgin Islands U.S.");
            var result = "";
            foreach (var item in data)
            {
                result += item.Key + "|" + item.Value + ", ";
            }
            return result;
        }

        public async Task<OperationResult> VerifySMS(VerifySMS dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);
            if (user == null)
            {
                return new OperationResult { Message = "Unauthenticated", Success = false };
            }
            var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, dto.Code, user.PhoneNumber);
            if (!result)
            {
                return new OperationResult { Message = "Invalid token.", Success = false };
            }
            try
            {
                user.PhoneNumberConfirmed = true;
                var resultUpdate = await _userManager.UpdateAsync(user);
                if (resultUpdate.Succeeded)
                    return new OperationResult { Message = "Verify phone", Success = true };
                else
                    return new OperationResult { Message = "Phone number validation failed", Success = false };
            }
            catch (Exception)
            {

                return new OperationResult { Message = "An error has occurred, contact Vsirus for support", Success = false };
            }
            //await _userManager.SetTwoFactorEnabledAsync(user, true);
            //return new OperationResult { Message = "Verify phone", Success = true};

        }
        public async Task<OperationResult> UpdatePassword(UpdatePasswordDto Dto)
        {

            var user = await _userManager.FindByIdAsync(Dto.Id);
            if (user == null)
            {
                return new OperationResult { Message = "Account does not exist", Success = false };
            }
            if (!string.IsNullOrWhiteSpace(Dto.OldPassword) && user.PasswordHash == null)
            {
                return new OperationResult { Message = "You are already signed with Google", Success = false };
            }
            //check password
            var checkPassword = _functionUtilities.IsValidPassword(Dto.Password);
            if (!checkPassword)
            {
                return new OperationResult { Message = "Password must contain uppercase letters, lowercase letters, and numbers", Success = false };
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(Dto.OldPassword))
                {
                    var IsCheckPassword = await _userManager.CheckPasswordAsync(user, Dto?.OldPassword);
                    if (!IsCheckPassword)
                    {
                        return new OperationResult { Message = "Wrong password", Success = false };
                    }
                    if (Dto.Password != null)
                    {
                        var savePassword = await _userManager.ChangePasswordAsync(user, Dto.OldPassword, Dto.Password);
                        if (savePassword.Succeeded == false)
                        {
                            return new OperationResult { Message = "Save password error", Success = false };
                        }
                    }
                }
                else
                {
                    if (Dto.Password != null)
                    {
                        var result = await _userManager.AddPasswordAsync(user, Dto.Password);
                        if (result.Succeeded == false)
                        {
                            return new OperationResult { Message = "Save password error", Success = false };
                        }
                    }
                }
                return new OperationResult { Message = "Successful updated", Success = true };
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new OperationResult { Message = "An error has occurred, contact Vsirus for support", Success = false, Caption = ex.ToString() };
            }
        }

        public async Task<OperationResult> UpdatePasswordByAdmin(UpdatePasswordDto Dto)
        {
            var user = await _userManager.FindByIdAsync(Dto.Id);
            if (user == null)
            {
                return new OperationResult { Message = "Account does not exist", Success = false };
            }
            if (user.PasswordHash == null)
            {
                return new OperationResult { Message = "You are already signed with Google, Password null", Success = false };
            }

            try
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var newPassword = string.IsNullOrWhiteSpace(Dto.Password) ? "welcome123" : Dto.Password;

                var savePassword = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (savePassword.Succeeded == false)
                {
                    return new OperationResult { Message = "Save password error", Success = false };
                }
                return new OperationResult { Message = "Successful updated", Success = true };
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new OperationResult { Message = "An error has occurred, contact IT for support", Success = false, Caption = ex.ToString() };
            }
        }
    }
}
