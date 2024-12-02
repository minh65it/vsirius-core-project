using OfficeOpenXml;
using StullerApiClientDomain.VSirius;
using StullerDataApi.Params;
using StullerDataApi.Services.IServices;
using StullerDataApi.Utilities.Mapping.Transformer;

namespace StullerDataApi.Utilities.Mapping.MapStullerDBToVsiriusDB
{
    public interface IExportDataVsirius
    {
        public Task ExportExcel();
    }
    public class ExportDataVsirius : IExportDataVsirius
    {
        Dictionary<string, string> dataValid;
        private readonly IProductService _productService;

        public ExportDataVsirius(IProductService productService)
        {
            _productService = productService;
            dataValid = ReadDataExcelTemplate.ReadExcel();
        }

        public async Task ExportExcel()
        {
            PaginationParam page = new PaginationParam() 
            {
                PageNumber = 1,
                PageSize = 1000
            };
            ProductParams param = new ProductParams() { };
            var ret = await _productService.Filter(param, page);
            int totalPages = (int) ret.TotalCount / ret.PageSize;
            //int totalPages = 123;
            totalPages = totalPages > 0 ? totalPages : 1;
            int begin = 1;
            int step = totalPages >= 15 ? totalPages / 15 : 1;

            int end = step;
            int indexExcel = 1;

            List<Task> listTask = new List<Task>();
            int maxConcurrency = 3; // Số lượng tác vụ đồng thời chạy tối đa
            SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);
            //Delete old file
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string folderPath = Path.Combine(baseDirectory, "Storage", "DataExport");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            else
            {
                Directory.Delete(folderPath, true);
                Directory.CreateDirectory(folderPath);
            }
            //Get data
            while (end < totalPages)
            {
                listTask.Add(GetDataThreads(begin, end, indexExcel, semaphore));
                begin = begin + step;
                end = end + step;
                indexExcel++;
            }

            listTask.Add(GetDataThreads(begin, totalPages, indexExcel, semaphore));

            await Task.WhenAll(listTask);
            Console.WriteLine($"Save file excel done!!!");

        }

        public async Task GetDataThreads(int pageNumberStart, int pageNumberEnd, int indexExcel, SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync(); // Chờ đến khi có giấy phép (permit)
            List<ProductVSirius> dataList = new List<ProductVSirius>();

            try
            {
                while (pageNumberStart <= pageNumberEnd)
                {
                    var data = await GetData(pageNumberStart);
                    dataList.AddRange(data);
                    pageNumberStart++;
                }

                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                string folderPath = Path.Combine(baseDirectory, "ResourcesStullerApi");
                string sourceFilePath = folderPath + @"\DataStullerMapping.xlsx";

                string filepath = Path.Combine(baseDirectory, "Storage", "DataExport", "DataStullerToVSirius");

                using (ExcelPackage workbook = new ExcelPackage(sourceFilePath))
                {

                    var ws = workbook.Workbook.Worksheets[0];

                    int rowIndex = 4;
                    foreach (var data in dataList)
                    {
                        int columnIndex = 1;
                        foreach (var property in typeof(ProductVSirius).GetProperties())
                        {
                            var value = property.GetValue(data, null);
                            ws.Cells[rowIndex, columnIndex].Value = value;
                            columnIndex++;
                        }
                        rowIndex++;
                    }
                    workbook.SaveAs(new FileInfo($"{filepath}_{indexExcel}.csv"));
                    await Common.WriteLogAsync($"Save file {indexExcel} excel done");
                }
            }
            catch (Exception ex)
            {

                await Common.WriteLogAsync(ex.ToString());
            }
            finally
            {
                semaphore.Release(); // lệnh giải phóng và run task vụ tiếp theo
            }
        }
        public async Task<List<ProductVSirius>> GetData(int pageNumber)
        {

            PaginationParam page = new PaginationParam()
            {
                PageNumber = pageNumber,
                PageSize = 1000,

            };
            ProductParams param = new ProductParams() { };
            var data = await _productService.Filter(param, page);
            List<ProductVSirius> listData = new List<ProductVSirius>();
            //mapper
            StullerJewelry_To_VSiriusJewelry_Transformer mapper = new StullerJewelry_To_VSiriusJewelry_Transformer();
            foreach (var item in data.Data)
            {
                if ((item.ProductType == "Rings" || item.ProductType == "Charms" ||
                    item.ProductType == "Cuff Links" || item.ProductType == "Earring Jackets" ||
                    item.ProductType == "Earring Components" || item.ProductType == "Earrings" ||
                    item.ProductType == "Necklaces" || item.ProductType == "Pins" ||
                    item.ProductType == "Bracelets" || item.ProductType == "Bracelet Slides") &&
                    (item.Images != null || item.GroupImages != null) &&
                    item.Description != null &&
                    item.Price != null &&
                    item.Status != "Made To Order" &&
                    item.Orderable == true
                )
                {
                    var amazon = await mapper.Transformer(item, dataValid);
                    if (amazon.quantity != "0" && amazon.main_image_url != null)
                    {
                        listData.Add(amazon);
                    }
                }
            }
            return listData;
        }
    }
}
