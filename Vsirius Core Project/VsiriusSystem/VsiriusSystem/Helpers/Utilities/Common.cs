using System.Text;

namespace VsiriusSystem.Helpers.Utilities
{
    public static class Common
    {
        public static MemoryStream ReadFile(IFormFile file)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                MemoryStream stream = new MemoryStream();
                Stream fileStream = file.OpenReadStream();
                fileStream.CopyTo(stream);
                byte[] byteArray = stream.ToArray();
                stream.Position = 0;

                return stream;
            }
            catch (Exception ex) { return null; }
        }
    }
}
