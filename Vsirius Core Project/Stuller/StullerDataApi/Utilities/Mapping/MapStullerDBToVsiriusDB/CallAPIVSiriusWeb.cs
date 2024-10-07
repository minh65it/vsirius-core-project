using Newtonsoft.Json;
using OfficeOpenXml;
using System.Text;

namespace StullerDataApi.Utilities.Mapping.MapStullerDBToVsiriusDB
{
    public interface ICallAPIVSiriusWeb
    {
        public Task<List<string>> GetAllItem();
        public Task<bool> ImportExcel();
        public Task<string> DeleteMultiple(List<string> listItemSKUDatabase);
    }
    public class CallAPIVSiriusWeb : ICallAPIVSiriusWeb
    {

        private readonly string urlProduct = "";
        private readonly string urlIdentity = "";
        private readonly IConfiguration _config;

        public CallAPIVSiriusWeb(IConfiguration config)
        {
            _config = config;
            urlProduct = _config.GetSection("APIPIM:urlProduct").Value!;
            urlIdentity = _config.GetSection("APIVSirius:urlIdentity").Value!;
        }

        public async Task<List<string>> GetAllItem()
        {
            try
            {
                string token = await LoginIdentity();
                if (string.IsNullOrWhiteSpace(token))
                {
                    Console.WriteLine("Login for get item fail");
                }

                //GetData from database
                string urlGetData = urlProduct + "api/Product/GetAllItemSKU?item_sku_category=s";
                List<string> listItemSKUDatabase = new List<string>();
                using (var httpClient = new HttpClient())
                {
                    var httpRequestMessage = new HttpRequestMessage();

                    //Add params
                    var response = await httpClient.GetAsync(urlGetData);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        if (jsonString != string.Empty)
                            listItemSKUDatabase = JsonConvert.DeserializeObject<List<string>>(jsonString);
                        else
                            return null;
                        await Common.WriteLogAsync("Get all item sku done");
                    }
                }
                return listItemSKUDatabase;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await Common.WriteLogAsync("Could not get all item connect " + e.ToString());
                throw;
            }

        }

        public async Task<string> LoginIdentity()
        {
            string urlLogin = urlIdentity + "api/Auth/Login";
            string email = _config.GetSection("APIVSirius:email").Value!;
            string password = _config.GetSection("APIVSirius:password").Value!;

            using (var httpClient = new HttpClient())
            {
                var parameters = new Dictionary<string, string>
                {
                    { "email", email },
                    { "password", password }
                };

                // Content post
                var json = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Send request to urlLogin
                var response = await httpClient.PostAsync(urlLogin, content);

                // Get token from urlLogin
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    dynamic token = JsonConvert.DeserializeObject(result);
                    return token.token;
                }
                return null;
            }
        }

        public async Task<bool> ImportExcel()
        {
            try
            {
                await Common.WriteLogAsync("Start import excel to Vsirius api/VsiriusProduct/ImportExcel");
                string token = await LoginIdentity();
                if (string.IsNullOrWhiteSpace(token))
                {
                    return false;
                }

                string massage = "";

                for (int i = 1; i <= 21; i++)
                {
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    string folderPath = Path.Combine(baseDirectory, "Storage", "DataExport");
                    string filepathResult = folderPath + @$"\DataStullerToVSirius_{i}.csv";
                    if (!File.Exists(filepathResult))
                        continue;

                    string urlImport = urlProduct + "api/Product/ImportExcel";

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                        httpClient.Timeout = TimeSpan.FromHours(5);

                        using (var fileStream = new FileStream(filepathResult, FileMode.Open))
                        {
                            // Create content request http
                            var content = new MultipartFormDataContent();
                            content.Add(new StreamContent(fileStream), "fileExcel", Path.GetFileName(filepathResult));

                            using (var response = await httpClient.PostAsync(urlImport, content))
                            {
                                if (!response.IsSuccessStatusCode)
                                {
                                    massage = $"Error uploading file: {response.StatusCode} - {response.ReasonPhrase}";
                                }
                                else
                                {
                                    massage = "File uploaded successfully.";
                                }
                            }
                            fileStream.Close();
                        }
                    }
                }
                await Common.WriteLogAsync(massage);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await Common.WriteLogAsync("Could not import file excel connect " + e.ToString());
                return false;
            }
        }
        public async Task<string> DeleteMultiple(List<string> listItemSKUDatabase)
        {
            try
            {
                string token = await LoginIdentity();
                if (string.IsNullOrWhiteSpace(token))
                {
                    Console.WriteLine("Login for delete fail");
                    Console.WriteLine("Login for delete fail");
                    return null;
                }

                List<string> listItemSKUNotDelete = new List<string>();
                for (int i = 1; i < 21; i++)
                {
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string folderPath = Path.Combine(baseDirectory, "Storage", "DataExport");
                    string filepathResult = folderPath + @$"\DataStullerToVSirius_{i}.csv";
                    if (!File.Exists(filepathResult))
                        continue;

                    ExcelPackage targetWorkbook = new ExcelPackage(filepathResult);
                    var destWorksheet = targetWorkbook.Workbook.Worksheets[0];
                    for (int j = 4; j <= destWorksheet.Dimension.End.Row; j++)
                    {
                        var value = destWorksheet.Cells[j, 2].Value?.ToString();
                        if (value != null)
                        {
                            listItemSKUNotDelete.Add(value);
                        }
                    }
                    Console.WriteLine($"Get all item sku from excel file {i} done");
                }
                var listDelete = listItemSKUDatabase.Except(listItemSKUNotDelete.Select(x => x)).ToList();

                Console.WriteLine("Call api RemoveMultipleProduct");

                string urlDeleteMulitple = urlProduct + "api/Product/RemoveMultipleProduct";
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    httpClient.Timeout = TimeSpan.FromHours(3);
                    var json = JsonConvert.SerializeObject(listDelete);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PutAsync(urlDeleteMulitple, content);

                }
                Console.WriteLine("Successful delete!!!");
                await Common.WriteLogAsync("Delete done");
                return "Successful delete!!!";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await Common.WriteLogAsync("Could not delete connect " + e.ToString());
                return "delete SKU Vsirius false!!!";
            }
        }
    }
}
