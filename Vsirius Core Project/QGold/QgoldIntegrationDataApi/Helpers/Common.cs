using Abp.MimeTypes;
using OfficeOpenXml;
using QgoldIntegrationDataApi.Dtos.Qgold;
using System.Text.RegularExpressions;

namespace QgoldIntegrationDataApi.Helpers
{
    public class Common
    {
        public static string _logPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Storage", "Logs");

        #region Read Excel by aspose.cell
        //public static List<QgoldCsvData_Dto> ReadExcelFileToDynamic1(IFormFile excelFile)
        //{
        //    var stream = excelFile.OpenReadStream();
        //    // Load the Excel file using Aspose.Cells
        //    Workbook workbook = new Workbook(stream);

        //    // Access the first worksheet
        //    Worksheet worksheet = workbook.Worksheets["58772_Extract"];
        //    List<QgoldCsvData_Dto> data = new List<QgoldCsvData_Dto>();
        //    Dictionary<string, int> indexModelExcel = new();

        //    //Read header excel to add dictionary
        //    for (int c = 0; c < worksheet.Cells.MaxDataColumn; c++)
        //    {
        //        if (worksheet.Cells[0, c].Value == null)
        //        {
        //            break;
        //        }
        //        indexModelExcel.Add(worksheet.Cells[0, c].Value.ToString(), c);
        //    }
        //    //Read value add to list data
        //    for (int row = 1; row <= worksheet.Cells.MaxDataRow; row++)
        //    {
        //        var product = new QgoldCsvData_Dto
        //        {
        //            Item = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Item")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Item")].Value.ToString() : "",
        //            Description = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Description")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Description")].Value.ToString() : "",
        //            Status = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Status")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Status")].Value.ToString() : "",
        //            Weight = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Weight")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Weight")].Value.ToString() : "",
        //            CT_Weight = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("CT_Weight")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("CT_Weight")].Value.ToString() : "",
        //            Gem_Weight = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Gem_Weight")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Gem_Weight")].Value.ToString() : "",
        //            Size = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Size")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Size")].Value.ToString() : "",
        //            Length = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Length")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Length")].Value.ToString() : "",
        //            Width = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Width")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Width")].Value.ToString() : "",
        //            Sizeable = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Sizeable")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Sizeable")].Value.ToString() : "",
        //            Metal_Desc = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Metal_Desc")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Metal_Desc")].Value.ToString() : "",
        //            Image = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image")].Value.ToString() : "",
        //            UM_Sales = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("UM_Sales")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("UM_Sales")].Value.ToString() : "",
        //            Qty_Avail = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Qty_Avail")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Qty_Avail")].Value.ToString() : "",
        //            Metal_Price = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Metal_Price")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Metal_Price")].Value.ToString() : "",
        //            Price = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Price")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Price")].Value.ToString() : "",
        //            Customer_Item = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Customer_Item")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Customer_Item")].Value.ToString() : "",
        //            CategoriesText = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Categories")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Categories")].Value.ToString() : "",
        //            AttributesText = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Attributes")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Attributes")].Value.ToString() : "",
        //            MSRP = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("MSRP")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("MSRP")].Value.ToString() : "",
        //            Item_Type = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Item_Type")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Item_Type")].Value.ToString() : "",
        //            DateAdded = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("DateAdded")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("DateAdded")].Value.ToString() : "",
        //            ProductLine = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ProductLine")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ProductLine")].Value.ToString() : "",
        //            ImageLink_100 = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ImageLink_100")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ImageLink_100")].Value.ToString() : "",
        //            ImageLink_275 = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ImageLink_275")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ImageLink_275")].Value.ToString() : "",
        //            ImageLink_500 = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ImageLink_500")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ImageLink_500")].Value.ToString() : "",
        //            ImageLink_1000 = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ImageLink_1000")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ImageLink_1000")].Value.ToString() : "",
        //            Image1Name = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image1Name")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image1Name")].Value.ToString() : "",
        //            Image1MaxHeight = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image1MaxHeight")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image1MaxHeight")].Value.ToString() : "",
        //            Image1MaxWidth = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image1MaxWidth")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image1MaxWidth")].Value.ToString() : "",
        //            Image1Link = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image1Link")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image1Link")].Value.ToString() : "",
        //            Image2Name = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image2Name")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image2Name")].Value.ToString() : "",
        //            Image2MaxHeight = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image2MaxHeight")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image2MaxHeight")].Value.ToString() : "",
        //            Image2MaxWidth = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image2MaxWidth")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image2MaxWidth")].Value.ToString() : "",
        //            Image2Link = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image2Link")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image2Link")].Value.ToString() : "",
        //            Image3Name = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image3Name")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image3Name")].Value.ToString() : "",
        //            Image3MaxHeight = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image3MaxHeight")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image3MaxHeight")].Value.ToString() : "",
        //            Image3MaxWidth = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image3MaxWidth")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image3MaxWidth")].Value.ToString() : "",
        //            Image3Link = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image3Link")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image3Link")].Value.ToString() : "",
        //            Image4Name = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image4Name")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image4Name")].Value.ToString() : "",
        //            Image4MaxHeight = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image4MaxHeight")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image4MaxHeight")].Value.ToString() : "",
        //            Image4MaxWidth = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image4MaxWidth")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image4MaxWidth")].Value.ToString() : "",
        //            Image4Link = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image4Link")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image4Link")].Value.ToString() : "",
        //            Image5Name = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image5Name")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image5Name")].Value.ToString() : "",
        //            Image5MaxHeight = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image5MaxHeight")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image5MaxHeight")].Value.ToString() : "",
        //            Image5MaxWidth = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image5MaxWidth")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image5MaxWidth")].Value.ToString() : "",
        //            Image5Link = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image5Link")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image5Link")].Value.ToString() : "",
        //            Image6Name = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image6Name")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image6Name")].Value.ToString() : "",
        //            Image6MaxHeight = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image6MaxHeight")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image6MaxHeight")].Value.ToString() : "",
        //            Image6MaxWidth = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image6MaxWidth")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image6MaxWidth")].Value.ToString() : "",
        //            Image6Link = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image6Link")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image6Link")].Value.ToString() : "",
        //            Image7Name = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image7Name")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image7Name")].Value.ToString() : "",
        //            Image7MaxHeight = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image7MaxHeight")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image7MaxHeight")].Value.ToString() : "",
        //            Image7MaxWidth = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image7MaxWidth")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image7MaxWidth")].Value.ToString() : "",
        //            Image7Link = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image7Link")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image7Link")].Value.ToString() : "",
        //            Image8Name = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image8Name")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image8Name")].Value.ToString() : "",
        //            Image8MaxHeight = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image8MaxHeight")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image8MaxHeight")].Value.ToString() : "",
        //            Image8MaxWidth = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image8MaxWidth")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image8MaxWidth")].Value.ToString() : "",
        //            Image8Link = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image8Link")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image8Link")].Value.ToString() : "",
        //            Image9Name = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image9Name")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image9Name")].Value.ToString() : "",
        //            Image9MaxHeight = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image9MaxHeight")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image9MaxHeight")].Value.ToString() : "",
        //            Image9MaxWidth = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image9MaxWidth")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image9MaxWidth")].Value.ToString() : "",
        //            Image9Link = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image9Link")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image9Link")].Value.ToString() : "",
        //            Country_Of_Origin = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Country_Of_Origin")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Country_Of_Origin")].Value.ToString() : "",
        //            ListOfSpecsText = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ListOfSpecs")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ListOfSpecs")].Value.ToString() : "",
        //            UPC = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("UPC")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("UPC")].Value.ToString() : "",
        //            Book = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Book")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Book")].Value.ToString() : "",
        //            Page = worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Page")].Value != null ? worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Page")].Value.ToString() : "",
        //            isDeleted = false
        //        };
        //        data.Add(product);
        //    }

        //    return data;
        //}
        #endregion

        public static List<QgoldCsvData_DTO> ReadExcelFileToDynamic(IFormFile excelFile)
        {
            using (var stream = excelFile.OpenReadStream())
            {
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets["58772_Extract"];
                    List<QgoldCsvData_DTO> data = new List<QgoldCsvData_DTO>();
                    Dictionary<string, int> indexModelExcel = new ();

                    // Read header excel to add dictionary
                    for (int c = 1; c <= worksheet.Dimension.End.Column; c++)
                    {
                        var headerValue = worksheet.Cells[1, c].Value;
                        if (headerValue == null)
                        {
                            break;
                        }
                        indexModelExcel.Add(headerValue.ToString(), c);
                    }

                    // Read values and add to the list data
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var product = new QgoldCsvData_DTO
                        {
                            Item = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Item")]),
                            Description = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Description")]),
                            Status = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Status")]),
                            Weight = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Weight")]),
                            CT_Weight = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("CT_Weight")]),
                            Gem_Weight = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Gem_Weight")]),
                            Size = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Size")]),
                            Length = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Length")]),
                            Width = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Width")]),
                            Sizeable = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Sizeable")]),
                            Metal_Desc = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Metal_Desc")]),
                            Image = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image")]),
                            UM_Sales = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("UM_Sales")]),
                            Qty_Avail = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Qty_Avail")]),
                            Metal_Price = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Metal_Price")]),
                            Price = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Price")]),
                            Customer_Item = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Customer_Item")]),
                            CategoriesText = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Categories")]),
                            AttributesText = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Attributes")]),
                            MSRP = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("MSRP")]),
                            Item_Type = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Item_Type")]),
                            DateAdded = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("DateAdded")]),
                            ProductLine = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ProductLine")]),
                            ImageLink_100 = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ImageLink_100")]),
                            ImageLink_275 = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ImageLink_275")]),
                            ImageLink_500 = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ImageLink_500")]),
                            ImageLink_1000 = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ImageLink_1000")]),
                            Image1Name = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image1Name")]),
                            Image1MaxHeight = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image1MaxHeight")]),
                            Image1MaxWidth = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image1MaxWidth")]),
                            Image1Link = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image1Link")]),
                            Image2Name = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image2Name")]),
                            Image2MaxHeight = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image2MaxHeight")]),
                            Image2MaxWidth = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image2MaxWidth")]),
                            Image2Link = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image2Link")]),
                            Image3Name = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image3Name")]),
                            Image3MaxHeight = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image3MaxHeight")]),
                            Image3MaxWidth = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image3MaxWidth")]),
                            Image3Link = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image3Link")]),
                            Image4Name = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image4Name")]),
                            Image4MaxHeight = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image4MaxHeight")]),
                            Image4MaxWidth = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image4MaxWidth")]),
                            Image4Link = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image4Link")]),
                            Image5Name = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image5Name")]),
                            Image5MaxHeight = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image5MaxHeight")]),
                            Image5MaxWidth = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image5MaxWidth")]),
                            Image5Link = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image5Link")]),
                            Image6Name = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image6Name")]),
                            Image6MaxHeight = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image6MaxHeight")]),
                            Image6MaxWidth = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image6MaxWidth")]),
                            Image6Link = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image6Link")]),
                            Image7Name = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image7Name")]),
                            Image7MaxHeight = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image7MaxHeight")]),
                            Image7MaxWidth = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image7MaxWidth")]),
                            Image7Link = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image7Link")]),
                            Image8Name = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image8Name")]),
                            Image8MaxHeight = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image8MaxHeight")]),
                            Image8MaxWidth = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image8MaxWidth")]),
                            Image8Link = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image8Link")]),
                            Image9Name = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image9Name")]),
                            Image9MaxHeight = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image9MaxHeight")]),
                            Image9MaxWidth = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image9MaxWidth")]),
                            Image9Link = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Image9Link")]),
                            Country_Of_Origin = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Country_Of_Origin")]),
                            ListOfSpecsText = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("ListOfSpecs")]),
                            UPC = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("UPC")]),
                            Book = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Book")]),
                            Page = GetValue(worksheet.Cells[row, indexModelExcel.GetValueOrDefault("Page")]),
                            isDeleted = false
                        };
                        data.Add(product);
                    }

                    return data;
                }
            }
        }

        private static string GetValue(ExcelRange cell)
        {
            return cell.Value != null ? cell.Value.ToString() : "";
        }

        public static void WriteLog(string Message)
        {
            CreateFolder(_logPath);
            string filePath = Path.Combine(_logPath, $"Log_{DateTime.Now:yyyyMMdd}.txt");

            try
            {
                StreamWriter writer = File.AppendText(filePath);
                if (!string.IsNullOrWhiteSpace(Message))
                {
                    writer.WriteLine($"[{DateTime.Now}] {Message}");
                    Console.WriteLine($"[{DateTime.Now}] {Message}");
                }
                writer.Flush();
                writer.Close();
            }
            catch (IOException)
            {
                Task.Delay(10).Wait();
                WriteLog(Message);
            }
        }

        public static void CreateFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public static List<List<T>> SplitListIntoBatches<T>(List<T> list, int batchSize)
        {
            List<List<T>> batches = new List<List<T>>();
            for (int i = 0; i < list.Count; i += batchSize)
            {
                List<T> batch = list.GetRange(i, Math.Min(batchSize, list.Count - i));
                batches.Add(batch);
            }
            return batches;
        }

        public static string SKUQgold(string sku)
        {
            return Regex.Match(sku, @"Q.*?-(.*)").Groups[1].Value;
        }

        public static string GetFileContentType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            MimeTypeMap mimeMap = new ();
            return mimeMap.GetMimeType(extension, false);
        }
    }
}
