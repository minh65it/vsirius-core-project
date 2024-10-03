using AutoMapper;
using IdentityApi._Repositoties;
using IdentityApi._Services.Interfaces;
using IdentityApi.Data;
using IdentityApi.Dtos.User;
using IdentityApi.Helpers.Utilities;
using IdentityApi.Models.User;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace IdentityApi._Services.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<VsiriusUser> _userManager;
        private readonly SignInManager<VsiriusUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IFunctionUtilities _functionUtilities;
        private readonly IEmailSender _emailSender;
        private HttpRequest _request;
        private readonly IRepositoryAccessor _repositoryAccessor;
        private readonly IConfiguration _configuration;
        private ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityContext _context;
        public UserService(UserManager<VsiriusUser> userManager, IMapper mapper,
            IFunctionUtilities functionUtilities, SignInManager<VsiriusUser> signInManager, 
            IEmailSender emailSender, IHttpContextAccessor httpContextAccessor, 
            IRepositoryAccessor repositoryAccessor, IConfiguration configuration, 
            ILogService logService, IdentityContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _functionUtilities = functionUtilities;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _repositoryAccessor = repositoryAccessor;
            _configuration = configuration;
            _logService = logService;
            _context = context;
        }


        public async Task<OperationResult> CreateUser(UserDto Dto, IUrlHelper urlHelper)
        {
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
            if (!string.IsNullOrWhiteSpace(Dto?.PhoneNumber))
            {
                Dto.PhoneNumber = "+1" + Dto.PhoneNumber;
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
                if(Dto.Avatar != null)
                {
                    user.Avatar = @"upload/user/" + await _functionUtilities.UploadAsync(Dto.Avatar, @"upload/user", user.Id);
                }
                var result = await _userManager.CreateAsync(user, Dto.Password);
                //Send mail confirm
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _request = _httpContextAccessor.HttpContext.Request;
                var confirmationLink = urlHelper.Action("Index", "Confirm", new { email = Dto.Email, token = token }, _request.Scheme);
                var checkMail = await _emailSender.CheckEmailExists(Dto.Email, "Confirm your email", $"Please confirm your email by clicking this link: <a href='{confirmationLink}'>link</a>.");
                if (!checkMail)
                {
                    await _userManager.DeleteAsync(user);
                    _functionUtilities.DeleteFile(user.Avatar);
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
                    _functionUtilities.DeleteFile(user.Avatar);
                    return new OperationResult { Message = "An error has occurred, contact Vsirus for support", Success = result.Succeeded };
                }
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Message = "An error has occurred, contact Vsirus for support", Success = false };
            }
        }
        public async Task ConfirmEmail(string email, IUrlHelper urlHelper)
        {
            try
            {
                var checkValid = _functionUtilities.IsValidEmail(email);
                if (!checkValid)
                {
                    return;
                }
                var user = await _userManager.FindByEmailAsync(email);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _request = _httpContextAccessor.HttpContext.Request;
                var confirmationLink = urlHelper.Action("Index", "Confirm", new { email = email, token = token }, _request.Scheme);

                await _emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your email by clicking this link: <a href='{confirmationLink}'>link</a>.");
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
            }
        }
        public async Task SendSMS(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);


                var twilioAccountSid = _configuration["Twilio:AccountSid"];
                var twilioAuthToken = _configuration["Twilio:AuthToken"];
                var twilioFromPhoneNumber = _configuration["Twilio:FromPhoneNumber"];

                TwilioClient.Init(twilioAccountSid, twilioAuthToken);

                var message = $"Your authentication code is: {code}";
                await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(twilioFromPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(user.PhoneNumber)
                );
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
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
                _functionUtilities.DeleteFile(user.Avatar);
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return new OperationResult { Message = "Deleted Success", Success = true };
                else
                    return new OperationResult { Message = "Deleted Error", Success = false };
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Message = "Deleted Error", Success = false, Caption = ex.ToString() };
            }
        }

        public async Task<PaginationUtility<UserDto>> GetAllUser(PaginationParam pagination, string inputSearch)
        {
            var predicate = PredicateBuilder.New<VsiriusUser>(x => x.IsActive != false);
            if (!string.IsNullOrWhiteSpace(inputSearch))
            {
                predicate = predicate.And(x => x.Email.ToLower().Contains(inputSearch.ToLower()) ||
                    x.UserName.ToLower().Contains(inputSearch.ToLower()) ||
                    x.PhoneNumber.ToLower().Contains(inputSearch.ToLower()) ||
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
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
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
                return result;
            }
            catch(Exception ex) 
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new UserDto();
            }
            
        }

        public async Task<UserForLoggedDto> Login(UserForLoginParam param)
        {
            try
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
                VsiriusUser user = new VsiriusUser();
                if (param.IsAuthen)
                {
                    user = await _repositoryAccessor.user.FindSingle(x => x.Email == param.Email);
                    if (user == null)
                    {
                        var newUser = new VsiriusUser { UserName = param.Email, Email = param.Email };
                        newUser.UserName = param.Email;
                        newUser.NormalizedUserName = param.Email.ToUpper();
                        newUser.NormalizedEmail = param.Email.ToUpper();
                        _repositoryAccessor.user.Add(newUser);
                        await _repositoryAccessor.Save();
                        user = newUser;
                    }
                }
                else
                {
                    var checkData = await _signInManager.PasswordSignInAsync(param.Email,
                                  param.Password, true, lockoutOnFailure: true);
                    if (!checkData.Succeeded) return null;
                    user = await _userManager.FindByEmailAsync(param.Email);
                }
                //create claims details based on the user information
                if (user == null) return null;
                var rolesToken = new string[] { "Customer" };
                var token = _functionUtilities.GenerateToken(user.UserName, user.Email, rolesToken, 10);

                //export info when login
                var result = new UserForLoggedDto
                {
                    isLogin = true,
                    user = _mapper.Map<UserDto>(user),
                    token = token
                };
                return result;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
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
            if(!string.IsNullOrWhiteSpace(Dto.OldPassword) && user.PasswordHash == null)
            {
                return new OperationResult { Message = "You are already signed with Google, Password null", Success = false };
            }
            if(!string.IsNullOrWhiteSpace(Dto?.PhoneNumber))
            {
                Dto.PhoneNumber = "+1" + Dto.PhoneNumber;
                var checkValidPhone = _functionUtilities.IsValidPhone(Dto.PhoneNumber);
                if (!checkValidPhone)
                {
                    return new OperationResult { Message = "Your phone number is not in the correct format", Success = false };
                }
            }
            if (!string.IsNullOrWhiteSpace(Dto.OldPassword))
            {
                var IsCheckPassword = await _userManager.CheckPasswordAsync(user, Dto?.OldPassword);
                if (!IsCheckPassword)
                {
                    return new OperationResult { Message = "Wrong password", Success = false };
                }
                if(Dto.Password != null)
                {
                    var savePassword =  await _userManager.ChangePasswordAsync(user, Dto.OldPassword, Dto.Password);
                    if (savePassword.Succeeded == false)
                    {
                        return new OperationResult { Message = "Save password error", Success = false };
                    }
                }
            }
            else
            {
                if(Dto.Password != null)
                {
                    var result =  await _userManager.AddPasswordAsync(user, Dto.Password);
                    if (result.Succeeded == false)
                    {
                        return new OperationResult { Message = "Save password error", Success = false };
                    }
                }
            }

            if (user.PhoneNumber != Dto?.PhoneNumber)
            {
                user.TwoFactorEnabled = false;
            }
            user.PhoneNumber = Dto?.PhoneNumber;
            user.Address1 = Dto?.Address1;
            user.IsActive = true;
            user.Address2 =Dto?.Address2;
            user.City = Dto?.City;
            user.Country = Dto?.Country;
            user.FullName = Dto?.FullName;
            user.PostCode = Dto?.PostCode;
            
            if (Dto.Avatar != null)
            {
                _functionUtilities.DeleteFile(user.Avatar);
                user.Avatar = @"upload/user/" + await _functionUtilities.UploadAsync(Dto.Avatar, @"upload/user", user.Id);
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
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
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
                result += item.Key + "|" +item.Value + ", ";
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
            await _userManager.SetTwoFactorEnabledAsync(user, true);
            return new OperationResult { Message = "Verify phone", Success = true};

        }
    }
}
