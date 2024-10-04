using FikaAmazonAPI.ReportGeneration;
using FikaAmazonAPI;

namespace AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.UploadProductToAmazon
{
    public class Reports
    {
        AmazonConnection amazonConnection;
        public Reports(AmazonConnection amazonConnection)
        {
            this.amazonConnection = amazonConnection;
        }
        public List<ProductsRow> GetAllProduct()
        {
            ReportManager reportManager = new ReportManager(amazonConnection);
            var products = reportManager.GetProducts(); //GET_MERCHANT_LISTINGS_ALL_DATA
            return products;
        }
    }
}
