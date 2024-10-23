using IdentityApi.Dtos.User;
using System.Text.RegularExpressions;

namespace IdentityApi.Helpers.Utilities
{
    public interface IValidUser
    {
        public OperationResult IsValidUpdateUser(UserDto dto);
    }
    public class ValidUser : IValidUser
    {
        public OperationResult IsValidUpdateUser(UserDto dto)
        {
            //check Phone
            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            { 
                Regex regex = new Regex(@"^\+1\s?\d{3}\s?\d{3}\s?\d{4}$");
                if(!regex.IsMatch(dto.PhoneNumber)) 
                {
                    return new OperationResult() {Message = "Your phone number is not valid", Success = false };
                }
            }
            // check mail
            try
            {
                // Kiểm tra định dạng của địa chỉ email
                Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (!emailRegex.IsMatch(dto.Email))
                    return new OperationResult() { Message = "Your email address is not valid", Success = false };

                // Kiểm tra tính hợp lệ của địa chỉ email bằng cách gửi một email đến địa chỉ đó
                var addr = new System.Net.Mail.MailAddress(dto.Email);
                if(addr.Address != dto.Email)
                {
                    return new OperationResult() { Message = "Your email address is not valid", Success = false };
                }
            }
            catch
            {
                return new OperationResult() { Message = "Your email address is not valid", Success = false };
            }
            //check Password
            if (!string.IsNullOrWhiteSpace(dto.Password)) 
            {
                Regex regexPassword = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$");
                if (!regexPassword.IsMatch(dto.Password))
                {
                    return new OperationResult() { Message = "Password must contain uppercase letters, lowercase letters, and numbers", Success = false };
                }
            }
            //check Contruy
            if (!string.IsNullOrWhiteSpace(dto.Country))
            {
                if(dto.Country != "United States" && dto.Country != "Canada")
                {
                    return new OperationResult() { Message = "Your country is invalid, select United States or Canada", Success = false };
                }
            }
            //check Postcode
            if (!string.IsNullOrWhiteSpace(dto.PostCode))
            {
                if(dto.Country == "United States")
                {
                    Regex checkPostCode = new Regex(@"^[0-9]{5}$");
                    if (!checkPostCode.IsMatch(dto.PostCode))
                    {
                        return new OperationResult() { Message = "American post code is 5 numbers", Success = false };
                    }
                }
                if(dto.Country == "Canada")
                {
                    Regex checkPostCode = new Regex(@"^[a-zA-Z0-9]{3}\s[a-zA-Z0-9]{3}$");
                    if (!checkPostCode.IsMatch(dto.PostCode))
                    {
                        return new OperationResult() { Message = "Canada's post code is 6 characters with spaces between", Success = false };
                    }

                }
            }
            //check Address
            Regex checkAddress = new Regex(@"^[\sa-zA-Z0-9_,-]{1,50}$");
            if (!string.IsNullOrWhiteSpace(dto.Address1))
            {
                if (!checkAddress.IsMatch(dto.Address1))
                {
                    return new OperationResult() { Message = "Your address is not valid", Success = false };
                }
            }
            if (!string.IsNullOrWhiteSpace(dto.Address2))
            {
                if (!checkAddress.IsMatch(dto.Address2))
                {
                    return new OperationResult() { Message = "Your address is not valid", Success = false };
                }
            }
            //check Sate
            Regex checkStateCity = new Regex(@"^[\sa-zA-Z_-]{1,50}$");
            if (!string.IsNullOrWhiteSpace(dto.State))
            {
                if (!checkStateCity.IsMatch(dto.State))
                {
                    return new OperationResult() { Message = "Your state is invalid", Success = false };
                }
            }
            //check City
            if (!string.IsNullOrWhiteSpace(dto.City))
            {
                if (!checkStateCity.IsMatch(dto.City))
                {
                    return new OperationResult() { Message = "Your city is invalid", Success = false };
                }
            }
            //check FullName
            if (!string.IsNullOrWhiteSpace(dto.FullName))
            {
                Regex checkName = new Regex(@"^[a-zA-Z\s]{1,50}$");
                if (!checkName.IsMatch(dto.FullName))
                {
                    return new OperationResult() { Message = "Your name is not in the correct format, it is only 40 characters long", Success = false };
                }
            }
            // return OK
            return new OperationResult() { Success = true };
        }
    }
}
