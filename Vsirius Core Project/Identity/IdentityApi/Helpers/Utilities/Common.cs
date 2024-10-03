using System.Web;

namespace IdentityApi.Helpers.Utilities
{
    public static class Common
    {
        public static string GetFileNameFromBlob(string link)
        {
            try
            {
                string decodedUrl = HttpUtility.UrlDecode(link);

                string filePath = decodedUrl[(decodedUrl.LastIndexOf('/') + 1)..];

                filePath = filePath.Replace("%5C", "/");
                return filePath;
            }
            catch
            {
                return string.Empty;
            }        
        }

        public static IFormFile ConvertStringBase64ToFile (string base64)
        {
            try
            {
                // Tách phần dữ liệu base64 từ chuỗi
                string[] base64Parts = base64.Split(',');

                // Kiểm tra xem chuỗi có đúng định dạng base64 không
                if (base64Parts.Length != 2 || !base64Parts[0].StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException("Invalid base64 string format.");
                }

                // Lấy dữ liệu base64 thô
                string base64Data = base64Parts[1];

                // Giải mã base64 thành mảng byte
                byte[] bytes = Convert.FromBase64String(base64Data);

                // Tạo một tên tạm thời cho file
                string fileName = Guid.NewGuid().ToString() + ".png"; // Đổi phần mở rộng tùy thuộc vào loại dữ liệu

                // Tạo đối tượng IFormFile từ mảng byte
                var file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, null, fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png" // Đổi kiểu mime type tùy thuộc vào loại dữ liệu
                };

                return file;
            }
            catch 
            {
                return null;
            }
            
        }
    }
}
