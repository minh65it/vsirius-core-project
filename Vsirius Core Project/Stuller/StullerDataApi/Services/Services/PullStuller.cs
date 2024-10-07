using MongoDB.Bson;
using MongoDB.Driver;
using StullerApiClient.Models;
using StullerDataApi.Services.IServices;
using StullerDataApi.Utilities;
using StullerDataApi.Utilities.PullStuller;
using System.Net;
using System.Text.Json;
using VSiriusSystemLog.WriteLog;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StullerDataApi.Services.Services
{
    public class PullStuller : IPullStuller
    {
        private readonly IConfiguration _config;
        private readonly IProductAI _productAI;
        private readonly IUtilitiesStorageBlobs _utilitiesStorageBlobs;
        private string? _baseUrl = "";
        private string? conn = "";
        private string? stuller_account = "";
        private string? stuller_password = "";
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Product> _productBackupCollection;
        private string? dataBase = "";
        private string? productCollection = "";
        private string? productBackupCollection = "";
        private ILoggerCustom _logService;
        private List<string> FiltersArray = (new string[] {
                                            ProductsRequestPostFilterLookup.OnPriceList,
                                            ProductsRequestPostFilterLookup.Orderable,
                                            ProductsRequestPostFilterLookup.Finished,
                                            ProductsRequestPostFilterLookup.InStock
                                           }).ToList();

        public PullStuller(IConfiguration config,
            IUtilitiesStorageBlobs utilitiesStorageBlobs,
            IProductAI productAI,
            ILoggerCustom logService)
        {
            _config = config;

            _baseUrl = _config.GetSection("PullStuller:_baseUrl").Value;
            conn = _config.GetSection("Database:ConnectionString").Value;
            stuller_account = _config.GetSection("PullStuller:stuller_account").Value;
            stuller_password = _config.GetSection("PullStuller:stuller_password").Value;
            dataBase = _config.GetSection("Database:DatabaseName").Value;
            productCollection = _config.GetSection("PullStuller:productCollection").Value;
            productBackupCollection = _config.GetSection("PullStuller:productBackupCollection").Value;

            //connect database
            var mongoClient = new MongoClient(conn);
            var mongoDatabase = mongoClient.GetDatabase(dataBase);

            _productCollection = mongoDatabase.GetCollection<Product>(productCollection);
            _productBackupCollection = mongoDatabase.GetCollection<Product>(productBackupCollection);
            _utilitiesStorageBlobs = utilitiesStorageBlobs;
            _productAI = productAI;
            _logService = logService;
        }

        public async Task PullNewProduct()
        {
            ProductsRequestPostFilter filter = new ProductsRequestPostFilter();

            filter.Page = 1;
            filter.PageSize = 500;
            filter.Filter = FiltersArray;
            filter.Include = new List<string>(new string[] { "1" }).ToList(); // "0" Default Returns all descriptive elements, media, and ringsizing elements

            var ret = GetProducts(filter);
            int totalPages = ret.TotalNumberOfProducts / ret.PageSize;
            //int totalPages = 123;
            int remained = ret.TotalNumberOfProducts % ret.PageSize;
            if (remained > 0)
            {
                totalPages = totalPages + 1;
            }
            await Common.WriteLogAsync("Start Pull Stuller //==============================================");

            int begin = 1;
            int step = totalPages / 10;
            int end = step;

            var mongoClient = new MongoClient(conn);
            var mongoDatabase = mongoClient.GetDatabase(dataBase);
            var collectionBackup = await mongoDatabase.ListCollectionsAsync(new ListCollectionsOptions { Filter = new BsonDocument("name", productBackupCollection) });
            var collectionBackupExists = await collectionBackup.AnyAsync();

            if (collectionBackupExists)
            {
                mongoDatabase.DropCollection(productBackupCollection);
            }
            var collectionProduct = await mongoDatabase.ListCollectionsAsync(new ListCollectionsOptions { Filter = new BsonDocument("name", productCollection) });
            var collectionProductExists = await collectionProduct.AnyAsync();

            if (collectionProductExists)
            {
                mongoDatabase.RenameCollection(productCollection, productBackupCollection);
            }
            mongoDatabase.CreateCollection(productCollection);

            List<Task> listTask = new List<Task>();
            var dataProductBackup = await _productBackupCollection.Find(_ => true).ToListAsync();
            while (end < totalPages)
            {
                listTask.Add(LoadAllStullerProducts_Thread(begin, end, dataProductBackup));
                begin = begin + step;
                end = end + step;
            }

            listTask.Add(LoadAllStullerProducts_Thread(begin, totalPages, dataProductBackup));
            await Task.WhenAll(listTask);

            //// Insert title, description to ProductAIs
            //await Common.WriteLogAsync("Call api ProductAI START");
            //var queryFillter = Builders<Product>.Filter.Empty;
            //var options = new FindOptions<Product>
            //{
            //    Projection = Builders<Product>.Projection.Include("SKU").Include("Description")
            //};
            //var data =  await _productCollection.FindAsync(queryFillter, options);
            //var data1 = await data.ToListAsync();
            //await _productAI.PushToProductAI(data1);

            await Common.WriteLogAsync("Service is pull Stuller data succeeded DONE //==============================================");
        }
        private GetProductsResponse GetProducts(ProductsRequestPostFilter filter)
        {
            try
            {
                string url = _baseUrl + "products";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonSerializer.Serialize(filter);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }
                string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(stuller_account + ":" + stuller_password));

                request.Headers.Add("Authorization", "Basic " + encoded);
                request.PreAuthenticate = true;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string jsonResponse = reader.ReadToEnd();

                return JsonSerializer.Deserialize<GetProductsResponse>(jsonResponse);

            }
            catch (Exception e)
            {
                Common.WriteLog("Service get data error");
                throw;
            }
        }
        private async Task LoadAllStullerProducts_Thread(int startPage, int endPage, List<Product> dataProductBackup)
        {

            ProductsRequestPostFilter filter = new ProductsRequestPostFilter();
            filter.Filter = FiltersArray;
            filter.Include = new List<string>(new string[] { "1" }).ToList(); // "0" Default Returns all descriptive elements, media, and ringsizing elements
            filter.PageSize = 500;

            List<Product> Products;
            UtilitiesStorageBlobs utilities;
            for (var page = startPage; page <= endPage; page++)
            {
                filter.Page = page;
                try
                {
                    Products = new List<Product>();
                    Products = GetProducts(filter).Products;

                    // Update Images in data before insert
                    await Common.WriteLogAsync($"Update Images from Database, Call Microsoft Azure Storage");
                    Products = await _utilitiesStorageBlobs.UpdateImages(dataProductBackup, Products);

                    await _productCollection.InsertManyAsync(Products);
                }
                catch (Exception e)
                {
                    List<string> logs = new List<string>();
                    logs.Add("Error on page " + page);
                    logs.Add(e.Message);
                    await Common.WriteLogAsync("Service LoadAllStullerProducts_Thread get data error " + string.Join(", ", logs));
                    await _logService.CreateLogError(e.Message, "Service LoadAllStullerProducts_Thread get data error " + string.Join(", ", logs));
                }
            }
        }
    }
}
