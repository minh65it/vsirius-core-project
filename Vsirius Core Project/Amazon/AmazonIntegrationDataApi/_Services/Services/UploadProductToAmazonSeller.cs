using AmazonIntegrationDataApi._Services.Interfaces;
using AmazonIntegrationDataApi.Helpers.Utilities;
using AmazonIntegrationDataApi.Models;
using FikaAmazonAPI;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.UploadProductToAmazon;
using AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.Update;
using AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.Remove;
using AmazonIntegrationDataApi.Helpers.MapVSiriusDBToAmazonDB;
using AmazonIntegrationDataApi.Dtos;
using FikaAmazonAPI.ReportGeneration;

namespace AmazonIntegrationDataApi._Services.Services
{
    public class UploadProductToAmazonSeller : IUploadProductToAmazonSeller
    {
        private readonly IConfiguration _config;
        private readonly IGetDataAmazonLocal _getDataAmazonLocal;
        private readonly IGetDataAmazonLocalForUpdate _getDataAmazonLocalForUpdate;
        private readonly AmazonConnection _amazonConnection;
        private readonly bool _isProduction = false;

        public UploadProductToAmazonSeller(IConfiguration config,
            IGetDataAmazonLocal getDataAmazonLocal,
            IGetDataAmazonLocalForUpdate getDataAmazonLocalForUpdate)
        {
            _config = config;
            _getDataAmazonLocal = getDataAmazonLocal;
            _getDataAmazonLocalForUpdate = getDataAmazonLocalForUpdate;
            _amazonConnection = new AmazonConnection(new AmazonCredential()
            {
                AccessKey = _config.GetSection("FikaAmazonAPI:AccessKey").Value,
                SecretKey = _config.GetSection("FikaAmazonAPI:SecretKey").Value,
                RoleArn = _config.GetSection("FikaAmazonAPI:RoleArn").Value,
                ClientId = _config.GetSection("FikaAmazonAPI:ClientId").Value,
                ClientSecret = _config.GetSection("FikaAmazonAPI:ClientSecret").Value,
                RefreshToken = _config.GetSection("FikaAmazonAPI:RefreshToken").Value,
                MarketPlaceID = _config.GetSection("FikaAmazonAPI:MarketPlaceID").Value,
                SellerID = _config.GetSection("FikaAmazonAPI:SellerId").Value,
                IsDebugMode = true,
                //Environment = Environments.Sandbox
            });
            _isProduction = _config.GetSection("FikaAmazonAPI:Environment").Value == "Production" ? true : false;
        }

        public async Task<OperationResult> RemoveProduct()
        {
            await Utilities2.WriteLogAsync($"Update Inventory START //===============================================");

            ////=========================================Pull Products in Amazon (Call api Amazon)============================================//
            Reports report = new Reports(_amazonConnection);
            var dataAmazon = report.GetAllProduct();
            Console.WriteLine("Pull list data from Amazon done!");
            List<AmazonJewelryDataFeedItemV3_Dto> AmazonDB = new List<AmazonJewelryDataFeedItemV3_Dto>();
            //dataAmazon = dataAmazon.Where(x => x.SellerSku.StartsWith("S-") || x.ItemName.Contains("#Q-")).ToList();

            ////=========================================Get Data AmazonDB ==========================================================//
            var dataAmazonDB = await _getDataAmazonLocalForUpdate.GetListPaginationResultForDel();
            var dataAmazonSKU = dataAmazon.Select(x => x.SellerSku).ToList();
            var dataAmazonDBSKU = dataAmazonDB.Select(x => x.item_sku).ToList();

            //Obsoleted trên QGold
            var listDel = dataAmazonSKU.Except(dataAmazonDBSKU).ToList();

            foreach (var item in dataAmazon)
            {
                AmazonDB.Add(new AmazonJewelryDataFeedItemV3_Dto()
                {
                   item_sku = item.SellerSku,
                   item_name = item.ItemName,
                   product_description = item.ItemDescription,
                   standard_price = item.Price.ToString(),
                });
            }

            //Giá Amazon trên $3500.00
            var dataFillter = Filters.filtersSKUSllerDelete(AmazonDB);
            dataFillter = Filters.filterPrice(dataFillter);
            var dataRemove = AmazonDB.Except(dataFillter).ToList();
            List<string> dataSkus = dataRemove.Select(x => x.item_sku).ToList()!;
            if (dataSkus is null) 
            {
                return new OperationResult() { IsSuccess = true, Data = null };
            }
            listDel = listDel.Where(x => x.StartsWith("Q-")).ToList();

            dataSkus.AddRange(listDel);
            //Call API remove
            if (_isProduction)
            {
                RemoveProductAmazonSeller rmAmazonSeller = new RemoveProductAmazonSeller(_amazonConnection);
                rmAmazonSeller.SubmitFeedDeleteAddProduct(dataSkus);
            }
            return new OperationResult() { IsSuccess = true };
        }

        public async Task<OperationResult> UpdateInventory()
        {
            await Utilities2.WriteLogAsync($"Update Inventory START //===============================================");

            ////=========================================Pull Products in Amazon (Call api Amazon)============================================//
            Reports report = new Reports(_amazonConnection);
            var dataAmazon = report.GetAllProduct();
            Console.WriteLine("Pull list data from Amazon done!");

            ////=========================================Get Data AmazonDB ==========================================================//
            var dataAmazonDB = await _getDataAmazonLocalForUpdate.GetListPaginationResultForUpdate();

            await Utilities2.WriteLogAsync($"dataAmazon: {dataAmazon?.Count()}, dataAmazonDB: {dataAmazonDB?.Count()}");
            try
            {
                if (dataAmazon != null && dataAmazonDB != null)
                {
                    List<AmazonJewelryDataForUpdate> dataUpdate = new List<AmazonJewelryDataForUpdate>();
                    var dataAmazonSKU = dataAmazon.Select(x => x.SellerSku).ToList();
                    var dataAmazonDBSKU = dataAmazonDB.Select(x => x.item_sku).ToList();

                    var listUp = dataAmazonSKU.Intersect(dataAmazonDBSKU).ToList();
                    var listDel = dataAmazonSKU.Except(dataAmazonDBSKU).ToList();

                    foreach (var item in listUp)
                    {
                        dataUpdate.Add(dataAmazonDB.Where(x => x.item_sku == item).First());
                    }
                    //foreach (var item in dataUpdate)
                    //{
                    //    if (item.item_sku.StartsWith("S-"))
                    //        item.quantity = "0";
                    //}
                    foreach (var item in listDel)
                    {
                        dataUpdate.Add(new AmazonJewelryDataForUpdate()
                        {
                            item_sku = item,
                            quantity = "0"
                        });
                    }

                    //=========================================Feeds (Call api Amazon)============================================//
                    Feeds feeds = new Feeds(_amazonConnection);
                    ////POST_INVENTORY_AVAILABILITY_DATA
                    if(_isProduction)
                    {
                        feeds.SubmitFeedInventory(dataUpdate);
                    }
                    await Utilities2.WriteLogAsync(@$"Update inventory Amazon DONE, {dataUpdate?.Count} //===============================================");
                }
                return new OperationResult() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync(ex.ToString());
                return new OperationResult() { IsSuccess = false };
            }
        }

        public async Task<OperationResult> UpdatePrice()
        {
            await Utilities2.WriteLogAsync($"Update Price START //===============================================");
            ////=========================================Pull Products in Amazon (Call api Amazon)============================================//
            Reports report = new Reports(_amazonConnection);
            var dataAmazon = report.GetAllProduct();

            ////=========================================Get Data AmazonDB ==========================================================//
            var dataAmazonDB = await _getDataAmazonLocalForUpdate.GetListPaginationResultForUpdate();
            await Utilities2.WriteLogAsync($"dataAmazon: {dataAmazon?.Count()}, dataAmazonDB: {dataAmazonDB?.Count()}");

            try
            {
                //=========================================Construct a feed (Save file .txt)================================//
                if (dataAmazon != null && dataAmazonDB != null)
                {
                    List<AmazonJewelryDataForUpdate> dataUpdate = new List<AmazonJewelryDataForUpdate>();
                    var dataAmazonSKU = dataAmazon.Select(x => x.SellerSku).ToList();
                    var dataAmazonDBSKU = dataAmazonDB.Select(x => x.item_sku).ToList();

                    var listUp = dataAmazonSKU.Intersect(dataAmazonDBSKU).ToList();

                    foreach (var item in listUp)
                    {
                        dataUpdate.Add(dataAmazonDB.Where(x => x.item_sku == item).First());
                    }

                    //=========================================Feeds (Call api Amazon)============================================//
                    Feeds feeds = new Feeds(_amazonConnection);

                    ////POST_PRODUCT_PRICING_DATA
                    if(_isProduction)
                    {
                        feeds.SubmitFeedPRICING(dataUpdate);
                    }
                    await Utilities2.WriteLogAsync(@$"Update price Amazon DONE, {dataUpdate?.Count} //===============================================");
                }
                return new OperationResult() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync(ex.ToString());
                return new OperationResult() { IsSuccess = false };
            }
        }

        public async Task<OperationResult> UploadProductToAmazon()
        {
            await Utilities2.WriteLogAsync($"Upload data to Amazon seller START //===============================================");
            ////=========================================Pull Products in Amazon (Call api Amazon)============================================//
            Reports report = new Reports(_amazonConnection);
            var dataAmazon = report.GetAllProduct();
            Console.WriteLine("Pull list data from Amazon done!");

            ////=========================================Get Data AmazonDB ==========================================================//

            var dataAmazonDB = await _getDataAmazonLocal.GetListPaginationResult();
            await Utilities2.WriteLogAsync($"Get list data from AmazonDB done has {dataAmazonDB?.Count}");


            //=========================================Construct a feed (Save file .txt)================================//
            if (dataAmazon != null && dataAmazonDB != null)
            {
                var dataAmazonSKU = dataAmazon.Select(x => x.SellerSku).ToList();
                var dataAddStuller = dataAmazonDB.Where(x => !dataAmazonSKU.Contains(x.item_sku) && x.item_sku.StartsWith("S-")).ToList(); //Only Stuller
                var dataAddQgold = dataAmazonDB.Where(x => !dataAmazonSKU.Contains(x.item_sku) && x.item_sku.StartsWith("Q-")).ToList(); //Only Qgold

                List<AmazonJewelryDataFeedItem> dataAddTMP = new List<AmazonJewelryDataFeedItem>();
                dataAddTMP.AddRange(dataAddStuller);
                dataAddTMP.AddRange(dataAddQgold);

                Random random = new Random();
                dataAddTMP = dataAddTMP.OrderBy(x => random.Next()).Take(100).ToList();
                List<AmazonJewelryDataFeedItem> dataAdd = new List<AmazonJewelryDataFeedItem>();
                foreach (var p in dataAddTMP)
                {
                    if (p.feed_product_type is null || p.item_name is null || p.product_description is null)
                        continue;
                    if (Filters.filtersSKU(p) && Filters.filterPrice(p))
                    {
                        p.external_product_id = null;
                        p.external_product_id_type = null;
                        p.bullet_point3 = null;
                        p.bullet_point4 = null;
                        p.bullet_point5 = null;
                        dataAdd.Add(p);
                    }
                }
                await Utilities2.WriteLogAsync($"Filer data Qgold has {dataAddQgold.Count}, Stuller has {dataAddStuller.Count}, Push data has {dataAdd.Count}");
                Utilities2.SaveFileTxt(FileNameConstant.Data_Flat_file, dataAdd);

                Feeds feeds = new Feeds(_amazonConnection);
                ////POST_FLAT_FILE_LISTINGS_DATA =========================================START Upload product ================================//
                if (_isProduction)
                {
                    feeds.CallFlatfileListings();
                }



                ////=========================================START Update product ================================//
                //var skuUpdate = dataAmazon.Select(x => x.SellerSku).OrderBy(x => random.Next()).Take(3000).ToList();
                ////var skuUpdate = dataAmazon.Where(x => x.SellerSku == "Q-QCM1498-8").Select(x => x.SellerSku).OrderBy(x => random.Next()).Take(200).ToList();
                //var dataUpdate = dataAmazonDB.Where(x => skuUpdate.Contains(x.item_sku)).ToList();

                //////SubmitUpdateProduct
                //if (_isProduction)
                //{
                //    feeds.SubmitUpdateProduct(dataUpdate);
                //}
                ////=========================================DONE Update product ================================//
                //await Utilities2.WriteLogAsync($"List update title: {string.Join(",", dataUpdate.Select(x => x.item_sku).ToList())}");
            }

            await Utilities2.WriteLogAsync("Push data to Amazon DONE! //===============================================");
            return new OperationResult() { IsSuccess = true };
        }
    }
}
