using OfficeOpenXml;

namespace StullerDataApi.Utilities.Mapping.MapStullerDBToVsiriusDB
{
    public class ReadDataExcelTemplate
    {
        public static Dictionary<string, string> ReadExcel()
        {
            var result = new Dictionary<string, string>();

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sourceFilePath = Path.Combine(baseDirectory, "Resources");
            sourceFilePath = sourceFilePath + "\\AmazonTemplate.xlsm";

            ExcelPackage targetWorkbook = new ExcelPackage(sourceFilePath);
            var destWorksheet = targetWorkbook.Workbook.Worksheets["Valid Values"];
            for (int i = 2; i <= destWorksheet.Dimension.End.Row; i++)
            {
                if (destWorksheet.Cells[i, 2].Value == null)
                    continue;
                var dataValid = "";
                int j = 3;
                do
                {
                    dataValid += destWorksheet.Cells[i, j].Value.ToString() + "|";
                    j++;
                } while (!string.IsNullOrWhiteSpace(destWorksheet.Cells[i, j].Value?.ToString()));

                result.Add(destWorksheet.Cells[i, 2].Value?.ToString(), dataValid?.TrimEnd('|'));
            }
            return result;
        }
    }
}
