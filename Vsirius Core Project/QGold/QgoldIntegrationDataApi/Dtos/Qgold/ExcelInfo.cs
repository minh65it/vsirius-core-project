using OfficeOpenXml;

namespace QgoldIntegrationDataApi.Dtos.Qgold
{
    public class ExcelInfo
    {
        public ExcelPackage ExcelPackage { get; set; }
        public string FileName { get; set; }
    }
}
