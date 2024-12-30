namespace VsiriusMAUI.Common.Utilities
{
	public class Common
	{
		public async static Task<MemoryStream> ReadFile(string path)
		{
			try
			{
				MemoryStream stream = new MemoryStream();

				using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
				{
					await fileStream.CopyToAsync(stream);
				}
				stream.Position = 0;

				return stream;
			}
			catch
			{
			
				return new MemoryStream();
			}
		}
	}
}
