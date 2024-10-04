using AmazonIntegrationDataApi.Models;
using FikaAmazonAPI.AmazonSpApiSDK.Models.Feeds;
using FikaAmazonAPI.ConstructFeed.Messages;
using FikaAmazonAPI.ConstructFeed;
using FikaAmazonAPI.Utils;
using FikaAmazonAPI;
using static FikaAmazonAPI.Utils.Constants;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using static FikaAmazonAPI.ConstructFeed.BaseXML;

namespace AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.UploadProductToAmazon
{
    public class Feeds
    {
        AmazonConnection amazonConnection;
        public Feeds(AmazonConnection amazonConnection)
        {
            this.amazonConnection = amazonConnection;
        }



        public void CallFlatfileProduct(List<AmazonJewelryDataFeedItem> listDB)
        {
            var envelope = ConstructFeedProduct.FeedProduct(amazonConnection.GetCurrentSellerID);
            var list = new List<PriceMessage>();
            int index = 1;
            foreach (var item in listDB)
            {
                BaseMessage mes = new BaseMessage()
                {
                    MessageID = index,
                    OperationType = OperationType.Update,
                    Product = new ProductMessageJewelry()
                    {
                        SKU = item.item_sku,
                        StandardProductID = new FikaAmazonAPI.ConstructFeed.Messages.StandardProductID()
                        {
                            Type = item.external_product_id_type,
                            Value = item.external_product_id,
                        },
                        DescriptionData = new DescriptionDataJewelry()
                        {
                            Title = item.item_name,
                            Description = item.product_description,
                            BulletPoint = new string[] { item.bullet_point1, item.bullet_point2, item.bullet_point3, item.bullet_point4, item.bullet_point5 },
                            MSRP = new CurrencyAmount()
                            {
                                currency = FikaAmazonAPI.ConstructFeed.BaseXML.BaseCurrencyCode.USD,
                                Value = decimal.Parse(item.standard_price),
                                currencySpecified = true,
                            },
                            Manufacturer = item.manufacturer,
                        }
                    }
                };
                envelope.Message.Add(mes);
                index++;
            }
            var xmlString = LinqHelper.SerializeObject(envelope);

            ////save file
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            filePath = filePath + @"\\DataJewelry.xml";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(xmlString);
            }

            var feedresultTXT = amazonConnection.Feed.SubmitFeed(filePath
                                                    , FeedType.POST_PRODUCT_DATA
                                                    , new List<string>() { "ATVPDKIKX0DER" }
                                                    , null
                                                    , ContentType.XML);

            string pathURL = string.Empty;
            while (pathURL == string.Empty)
            {
                Thread.Sleep(1000 * 30);
                var feedOutput = amazonConnection.Feed.GetFeed(feedresultTXT);
                if (feedOutput.ProcessingStatus == FikaAmazonAPI.AmazonSpApiSDK.Models.Feeds.Feed.ProcessingStatusEnum.DONE)
                {
                    var outPut = amazonConnection.Feed.GetFeedDocument(feedOutput.ResultFeedDocumentId);

                    pathURL = outPut.Url;
                    Utilities2.WriteLog($"CallFlatfileProduct() " + pathURL);
                }
            }
        }
        public void CallFlatfileString(string xmlString)
        {
            var feedresultTXT = amazonConnection.Feed.SubmitFeed(xmlString
                                                    , FeedType.POST_PRODUCT_DATA
                                                    , new List<string>() { "ATVPDKIKX0DER" }
                                                    , null
                                                    , ContentType.XML);

            string pathURL = string.Empty;
            while (pathURL == string.Empty)
            {
                Thread.Sleep(1000 * 30);
                var feedOutput = amazonConnection.Feed.GetFeed(feedresultTXT);
                if (feedOutput.ProcessingStatus == FikaAmazonAPI.AmazonSpApiSDK.Models.Feeds.Feed.ProcessingStatusEnum.DONE)
                {
                    var outPut = amazonConnection.Feed.GetFeedDocument(feedOutput.ResultFeedDocumentId);

                    pathURL = outPut.Url;
                    Utilities2.WriteLog($"CallFlatfileString() " + pathURL);
                }
            }
        }
        public void CallListFlatfileListings()
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage");
            if (!Directory.Exists(folderPath + @"\Data"))
            {
                return;
            }

            for (int i = 1; i <= 15; i++)
            {
                string filepath = folderPath + @$"\Data\\Data_{i}.txt";
                if (!File.Exists(filepath))
                {
                    return;
                }
                string text = System.IO.File.ReadAllText(filepath);
                var feedresultTXT = amazonConnection.Feed.SubmitFeed(text
                                                        , FeedType.POST_FLAT_FILE_LISTINGS_DATA
                                                        , new List<string>() { "ATVPDKIKX0DER" }
                                                        , null
                                                        , ContentType.TXT);

                string pathURL = string.Empty;
                while (pathURL == string.Empty)
                {
                    Thread.Sleep(1000 * 30);
                    var feedOutput = amazonConnection.Feed.GetFeed(feedresultTXT);
                    if (feedOutput.ProcessingStatus == FikaAmazonAPI.AmazonSpApiSDK.Models.Feeds.Feed.ProcessingStatusEnum.DONE)
                    {
                        var outPut = amazonConnection.Feed.GetFeedDocument(feedOutput.ResultFeedDocumentId);

                        pathURL = outPut.Url;
                        Utilities2.WriteLog($"Done CallFlatfileListings() file Data_{i}.txt " + pathURL);
                    }
                }
            }
        }
        public void CallFlatfileListings()
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage");
            if (!Directory.Exists(folderPath + @"\Data"))
            {
                return;
            }
            string filepath = folderPath + @$"\Data\\{FileNameConstant.Data_Flat_file}";
            if (!File.Exists(filepath))
            {
                return;
            }
            string text = System.IO.File.ReadAllText(filepath);
            var feedresultTXT = amazonConnection.Feed.SubmitFeed(text
                                                    , FeedType.POST_FLAT_FILE_LISTINGS_DATA
                                                    , new List<string>() { "ATVPDKIKX0DER" }
                                                    , null
                                                    , ContentType.TXT);

            string pathURL = string.Empty;
            while (pathURL == string.Empty)
            {
                Thread.Sleep(1000 * 30);
                var feedOutput = amazonConnection.Feed.GetFeed(feedresultTXT);
                if (feedOutput.ProcessingStatus == FikaAmazonAPI.AmazonSpApiSDK.Models.Feeds.Feed.ProcessingStatusEnum.DONE)
                {
                    var outPut = amazonConnection.Feed.GetFeedDocument(feedOutput.ResultFeedDocumentId);

                    pathURL = outPut.Url;
                    Utilities2.WriteLog($"Done CallFlatfileListings() file Data_Flat_file.txt has feedId: " + feedresultTXT);
                }
            }
        }

        public void SubmitFeedInventory(List<AmazonJewelryDataForUpdate> listDB)
        {
            ConstructFeedService createDocument = new ConstructFeedService(amazonConnection.GetCurrentSellerID, "1.02");
            var list = new List<InventoryMessage>();

            foreach (var item in listDB)
            {
                list.Add(new InventoryMessage()
                {
                    SKU = item.item_sku,
                    Quantity = int.Parse(item.quantity)
                });
            }

            createDocument.AddInventoryMessage(list);
            var xml = createDocument.GetXML();

            var feedID = amazonConnection.Feed.SubmitFeed(xml, FeedType.POST_INVENTORY_AVAILABILITY_DATA);
            GetFeedDetails(feedID);
            Utilities2.WriteLog($"SubmitFeedInventory() {feedID}");
        }
        public void SubmitFeedImages(List<AmazonJewelryDataFeedItem> listDB)
        {
            ConstructFeedService createDocument = new ConstructFeedService(amazonConnection.GetCurrentSellerID, "1.02");
            var list = new List<ProductImageMessage>();

            foreach (var item in listDB)
            {
                list.Add(new ProductImageMessage()
                {
                    SKU = item.item_sku,
                    ImageType = ImageType.Main,
                    ImageLocation = item.main_image_url
                });
            }

            createDocument.AddProductImageMessage(list);
            var xml = createDocument.GetXML();

            var feedID = amazonConnection.Feed.SubmitFeed(xml, FeedType.POST_PRODUCT_IMAGE_DATA);
            GetFeedDetails(feedID);
            Utilities2.WriteLog($"one SubmitFeedImages() {feedID}");
        }

        public void SubmitFeedPRICING(List<AmazonJewelryDataForUpdate> listDB)
        {
            ConstructFeedService createDocument = new ConstructFeedService(amazonConnection.GetCurrentSellerID, "1.02");

            var list = new List<PriceMessage>();

            foreach (var item in listDB)
            {
                list.Add(new PriceMessage()
                {
                    SKU = item.item_sku,
                    StandardPrice = new StandardPrice()
                    {
                        currency = amazonConnection.GetCurrentMarketplace.CurrencyCode.ToString(),
                        Value = item.standard_price,
                    }
                });
            }

            createDocument.AddPriceMessage(list);

            var xml = createDocument.GetXML();

            var feedID = amazonConnection.Feed.SubmitFeed(xml, FeedType.POST_PRODUCT_PRICING_DATA);

            GetFeedDetails(feedID);
            Utilities2.WriteLog($"Done SubmitFeedPRICING() {feedID}");
        }
        public void SubmitUpdateProduct(List<AmazonJewelryDataFeedItem> listInput)
        {

            ConstructFeedService createDocument = new ConstructFeedService(amazonConnection.GetCurrentSellerID, "1.02");

            var list = new List<ProductMessage>();
            foreach (var item in listInput)
            {
                list.Add(new ProductMessage()
                {
                    SKU = item.item_sku,
                    DescriptionData = new FikaAmazonAPI.ConstructFeed.Messages.DescriptionDataJewelry
                    {
                        Title = item.item_name,
                        Description = item.product_description,
                        BulletPoint = new string[] {
                                            item.bullet_point1,
                                            item.bullet_point2, 
                        }
                    }
                });
            }
            createDocument.AddProductMessage(list, OperationType.Update);
            var xml = createDocument.GetXML();

            var feedID = amazonConnection.Feed.SubmitFeedContent(xml, FeedType.POST_PRODUCT_DATA);

            GetFeedDetails(feedID);


            //var envelope = ConstructFeedProduct.FeedProduct(amazonConnection.GetCurrentSellerID);
            //int index = 1;
            //foreach (var item in listInput)
            //{
            //    BaseMessage mes = new BaseMessage()
            //    {
            //        MessageID = index,
            //        OperationType = OperationType.PartialUpdate,
            //        Product = new ProductMessage()
            //        {
            //            SKU = item.item_sku,
            //            StandardProductID = new FikaAmazonAPI.ConstructFeed.Messages.StandardProductID()
            //            {
            //                Type = null,
            //                Value = null,
            //            },
            //            DescriptionData = new FikaAmazonAPI.ConstructFeed.Messages.DescriptionDataJewelry()
            //            {
            //                Title = "Cheryl M Sterling Silver Rhodium-plated Brilliant-cut CZ Multi-stone Ring style QCM1498-8",
            //                Description = item.product_description,
            //                BulletPoint = new string[] {
            //                    item.bullet_point1,
            //                    item.bullet_point2,
            //                    item.bullet_point3,
            //                    item.bullet_point4,
            //                    item.bullet_point5 }
            //            },
            //        }
            //    };
            //    envelope.Message.Add(mes);
            //    index++;
            //}
            //var xmlString = LinqHelper.SerializeObject(envelope);


            //////save file
            //string folderPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\net7.0", "");
            //if (!Directory.Exists(folderPath))
            //{
            //    Directory.CreateDirectory(folderPath);
            //}
            //string filePath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\net7.0", "") + "Data\\DataJewelry.xml";
            //using (StreamWriter writer = new StreamWriter(filePath))
            //{
            //    writer.Write(xmlString);
            //}

            //var feedresultTXT = amazonConnection.Feed.SubmitFeed(filePath
            //                                        , FeedType.POST_PRODUCT_DATA
            //                                        , new List<string>() { "ATVPDKIKX0DER" }
            //                                        , null
            //                                        , ContentType.XML);

            //string pathURL = string.Empty;
            //while (pathURL == string.Empty)
            //{
            //    Thread.Sleep(1000 * 30);
            //    var feedOutput = amazonConnection.Feed.GetFeed(feedresultTXT);
            //    if (feedOutput.ProcessingStatus == FikaAmazonAPI.AmazonSpApiSDK.Models.Feeds.Feed.ProcessingStatusEnum.DONE)
            //    {
            //        var outPut = amazonConnection.Feed.GetFeedDocument(feedOutput.ResultFeedDocumentId);

            //        pathURL = outPut.Url;
            //        Utilities2.WriteLog($"{DateTime.Now} - CallFlatfileProduct() " + pathURL);
            //    }
            //}
        }

        public void GetFeeds()
        {

            var data = amazonConnection.Feed.GetFeeds(new FikaAmazonAPI.Parameter.Feed.ParameterGetFeed()
            {
                processingStatuses = ProcessingStatuses.DONE,
                pageSize = 100,
                feedTypes = new List<FeedType> { FeedType.POST_PRODUCT_PRICING_DATA },
                createdSince = DateTime.UtcNow.AddDays(-6),
                createdUntil = DateTime.UtcNow.AddDays(-1),
                marketplaceIds = new List<string> { MarketPlace.UnitedArabEmirates.ID }
            });
        }

        public void CreateFeedDocument()
        {
            var data = amazonConnection.Feed.CreateFeedDocument(ContentType.XML);
        }

        private void GetFeedDetails(string feedID)
        {
            string ResultFeedDocumentId = string.Empty;
            while (string.IsNullOrEmpty(ResultFeedDocumentId))
            {
                var feedOutput = amazonConnection.Feed.GetFeed(feedID);
                if (feedOutput.ProcessingStatus == Feed.ProcessingStatusEnum.DONE)
                {
                    var outPut = amazonConnection.Feed.GetFeedDocument(feedOutput.ResultFeedDocumentId);

                    var reportOutput = outPut.Url;
                    Utilities2.WriteLog($"GetFeedDetails() " + reportOutput);
                    var processingReport = amazonConnection.Feed.GetFeedDocumentProcessingReport(outPut);

                    DisplayProcessingReportMessage(processingReport);

                    break;
                }

                if (!(feedOutput.ProcessingStatus == Feed.ProcessingStatusEnum.INPROGRESS ||
                    feedOutput.ProcessingStatus == Feed.ProcessingStatusEnum.INQUEUE))
                    break;
                else Thread.Sleep(10000);
            }
        }
        private void DisplayProcessingReportMessage(ProcessingReportMessage processingReport)
        {
            Console.WriteLine("MessagesProcessed=" + processingReport.ProcessingSummary.MessagesProcessed);
            Console.WriteLine("MessagesSuccessful= " + processingReport.ProcessingSummary.MessagesSuccessful);
            Console.WriteLine("MessagesWithError=" + processingReport.ProcessingSummary.MessagesWithError);
            Console.WriteLine("MessagesWithWarning=" + processingReport.ProcessingSummary.MessagesWithWarning);

            if (processingReport.Result != null && processingReport.Result.Count > 0)
            {
                foreach (var itm in processingReport.Result)
                {
                    Console.WriteLine("ResultDescription=" + (itm.AdditionalInfo?.SKU ?? string.Empty) + " > " + itm.ResultDescription);
                }
            }
        }
    }
}
