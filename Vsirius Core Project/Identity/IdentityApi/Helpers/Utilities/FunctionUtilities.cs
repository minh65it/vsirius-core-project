using Azure;
using Azure.Communication.Sms;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace IdentityApi.Helpers.Utilities
{
    public interface IFunctionUtilities
    {
        string GenerateToken(string UserName, string Email, IList<string> Roles, int expires = 1);
        Task<string> UploadAsync(string file, string subFolder = "", string rawFileName = null);
        bool DeleteFile(string file, string subFolder = "");
        public string HashPasswordUser(string password);
        public bool IsValidEmail(string email);
        public bool IsValidPhone(string phoneNumber);
        public bool IsValidPassword(string password);
        public Task<bool> SendSMSWithAzure(string ToPhoneNumber, string Message);
    }
    public class FunctionUtilities : IFunctionUtilities
    {
        private readonly string _path;
        private readonly IConfiguration _configuration;

        public FunctionUtilities(IConfiguration configuration)
        {
            _path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            _configuration = configuration;
        }
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Kiểm tra định dạng của địa chỉ email
                var emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (!emailRegex.IsMatch(email))
                    return false;

                // Kiểm tra tính hợp lệ của địa chỉ email bằng cách gửi một email đến địa chỉ đó
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteFile(string fileName, string subFolder = "")
        {
            if (string.IsNullOrEmpty(fileName) || fileName.Contains("no-image.jpg"))
            {
                return false;
            }
            string folderPath = Path.Combine(_path, subFolder);
            string filePath = Path.Combine(folderPath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

        public string GenerateToken(string UserName, string Email, IList<string> Roles, int expires)
        {
            Claim[] claims = new Claim[0];
            if (Roles != null)
            {
                claims = new Claim[Roles.Count()];
                for (int i = 0; i < Roles.Count(); i++)
                {
                    claims[i] = new Claim(ClaimTypes.Role, Roles[i]);
                }
                claims.Append(new Claim("UserName", UserName));
                claims.Append(new Claim("Email", Email));
            }
            else
            {
                claims = new Claim[]{
                    new Claim("UserName", UserName),
                    new Claim("Email", Email)
                };
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMonths(expires),
                signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        //Upload a base64 string file to server folder.
        public async Task<string> UploadAsync(string file, string subFolder = "upload", string rawFileName = null)
        {
            if (string.IsNullOrEmpty(file))
            {
                return null;
            }
            string folderPath = Path.Combine(_path, subFolder);
            //string folderPath = Path.Combine("C:\\Users\\247tq\\OneDrive\\Máy tính", subFolder);
            string extension = $".{file.Split(';')[0].Split('/')[1]}";
            if (string.IsNullOrEmpty(extension))
            {
                return null;
            }
            string fileName = $"{Guid.NewGuid().ToString()}{extension}";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            if (!string.IsNullOrEmpty(rawFileName))
            {
                fileName = $"{rawFileName}{extension}";
            }
            string filePath = Path.Combine(folderPath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            string base64String = file.Substring(file.IndexOf(',') + 1);
            byte[] fileData = Convert.FromBase64String(base64String);
            try
            {
                await File.WriteAllBytesAsync(filePath, fileData);
                return fileName;
            }
            catch (System.Exception)
            {
                return null;
            }

        }
        public string HashPasswordUser(string password)
        {
            return GetSHA512(password).Substring(0, 75).ToString();
        }
        private string GetSHA512(string text)
        {
            System.Text.UnicodeEncoding UE = new System.Text.UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            var hashString = System.Security.Cryptography.SHA512.Create();
            string hex = "";

            hashValue = hashString.ComputeHash(message);

            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public bool IsValidPhone(string phoneNumber)
        {
            Regex regex = new Regex(@"^\+1\s?\d{3}\s?\d{3}\s?\d{4}$");
            return regex.IsMatch(phoneNumber);
        }

        public async Task<bool> SendSMSWithAzure(string ToPhoneNumber, string Message)
        {
            string connectionString = _configuration["AzureSMS:Connectionstring"];

            SmsClient smsClient = new SmsClient(connectionString);
            Response<IReadOnlyList<SmsSendResult>> response = await smsClient.SendAsync(
                from: _configuration["AzureSMS:FromPhoneNumber"],
                to: new string[] { ToPhoneNumber },
                message: Message,
                options: new SmsSendOptions(enableDeliveryReport: true) // OPTIONAL
                {
                    // custom tags
                });

            IEnumerable<SmsSendResult> results = response.Value;
            foreach (SmsSendResult result in results)
            {
                if (!result.Successful)
                    return false;
            }
            return true;
        }

        public bool IsValidPassword(string password)
        {
            const string pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,}$";
            return Regex.IsMatch(password, pattern);
        }
    }
}
