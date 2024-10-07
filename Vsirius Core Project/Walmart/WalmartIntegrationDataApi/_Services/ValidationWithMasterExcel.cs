using System.Linq;
using WalmartIntegrationDataApi._Services.Interfaces;
using WalmartIntegrationDataApi.Common;
using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Dtos.Product;

namespace WalmartIntegrationDataApi._Services
{
    public interface IValidationWithMasterExcel : IValidation
    {
        public Dictionary<string, object[]> ReadValidatedValueInExcelFile();
    }

    public class ValidationWithMasterExcel : IValidationWithMasterExcel
    {

        public ValidationWithMasterExcel()
        {
        }

        //private readonly IWebHostEnvironment _env;
        //public ValidationWithMasterExcel(IWebHostEnvironment env)
        //{
        //    _env = env;
        //}
        public Dictionary<string, object[]> ReadValidatedValueInExcelFile()
        {
            return Util.dataValidValueInExcel;
        }

        public WalmartDTO Validate(WalmartDTO data)
        {
            //StringBuilder msgErr = new StringBuilder();
            Dictionary<string, object[]> dataExcel;
            WalmartDTO msg = new();
            #region read valdate value in excel template
            try
            {
                dataExcel = Util.dataValidValueInExcel;
            }
            catch (Exception)
            {

                throw;
            }
            if (dataExcel == null)
            {
                msg.Note = "Valid Excel not exists";
                return msg;
            }
            #endregion

            //SKU
            if (string.IsNullOrWhiteSpace(data.SKU))
            {
                msg.SKU = MessageErr.Required;
                //msgErr.Append("SKU " + MessageErr.Required + "; ");
            }

            //productIdType
            if (!string.IsNullOrWhiteSpace(data.productIdType))
            {
                if (!dataExcel["productIdType"].Contains(data.productIdType))
                {
                    msg.productIdType = MessageErr.Exists;
                    //msgErr.Append("productIdType " + MessageErr.Exists + "; ");
                }
            }

            //productId
            if (!string.IsNullOrWhiteSpace(data.productId))
            {
                if (data.productId.Length > 14)
                {
                    msg.productId = string.Format(MessageErr.MaxLength, 14);
                    //msgErr.Append("productId " + string.Format(MessageErr.MaxLength, 14) + "; ");
                }
            }

            //externalProductIdType
            if (!string.IsNullOrWhiteSpace(data.externalProductIdType))
            {
                if (!dataExcel["externalProductIdType"].Contains(data.externalProductIdType))
                {
                    msg.externalProductIdType = MessageErr.Exists;
                    //msgErr.Append("externalProductIdType " + MessageErr.Exists + "; ");
                }
            }

            //externalProductId
            if (!string.IsNullOrWhiteSpace(data.externalProductId))
            {
                if (data.externalProductId.Length > 10)
                {
                    msg.externalProductId = string.Format(MessageErr.MaxLength, 10);
                    //msgErr.Append("externalProductId " + string.Format(MessageErr.MaxLength, 10) + "; ");
                }
            }

            //productName
            if (!string.IsNullOrWhiteSpace(data.productName))
            {
                if (data.productName.Length > 200)
                {
                    msg.productName = string.Format(MessageErr.MaxLength, 200);
                    //msgErr.Append("productName " + string.Format(MessageErr.MaxLength, 200) + "; ");
                }
            }

            //brand
            if (!string.IsNullOrWhiteSpace(data.brand))
            {
                if (data.brand.Length > 60)
                {
                    msg.brand = string.Format(MessageErr.MaxLength, 60);
                    //msgErr.Append("brand " + string.Format(MessageErr.MaxLength, 60) + "; ");
                }
            }

            //pricePerUnitQuantity
            if (!string.IsNullOrWhiteSpace(data.pricePerUnitQuantity))
            {

                if (!data.pricePerUnitQuantity.IsNumber())
                {

                    msg.pricePerUnitQuantity = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + "; ");
                    if (data.pricePerUnitQuantity.Length > 9)
                    {
                        msg.pricePerUnitQuantity += ", " + string.Format(MessageErr.MaxLength, 9);
                        //msgErr.Append("pricePerUnitQuantity " + string.Format(MessageErr.MaxLength, 9));
                    }
                }

            }

            //pricePerUnitUom
            if (!string.IsNullOrWhiteSpace(data.pricePerUnitUom))
            {
                if (!dataExcel["pricePerUnitUom"].Contains(data.pricePerUnitUom))
                {
                    msg.pricePerUnitUom = MessageErr.Exists;
                    //msgErr.Append("pricePerUnitUom " + MessageErr.Exists + "; ");
                }
            }

            //multipackQuantity
            if (!string.IsNullOrWhiteSpace(data.multipackQuantity))
            {
                if (!data.multipackQuantity.IsNumber())
                {
                    msg.multipackQuantity = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + "; ");
                    if (data.multipackQuantity.Length > 4)
                    {
                        msg.multipackQuantity += ", " + string.Format(MessageErr.MaxLength, 4);
                        //msgErr.Append("multipackQuantity " + string.Format(MessageErr.MaxLength, 4));
                    }
                }
            }

            //shortDescription
            if (!string.IsNullOrWhiteSpace(data.shortDescription))
            {
                if (data.shortDescription.Length > 4000)
                {
                    msg.shortDescription = string.Format(MessageErr.MaxLength, 4000);
                    //msgErr.Append("shortDescription " + string.Format(MessageErr.MaxLength, 4000) + "; ");
                }
            }

            //keyFeatures
            if (!string.IsNullOrWhiteSpace(data.keyFeatures))
            {
                foreach (var feature in data.keyFeatures.Split("|"))
                {
                    if (feature.Length > 4000)
                    {
                        msg.keyFeatures = string.Format(MessageErr.MaxLength, 4000);
                        break;
                        //msgErr.Append("keyFeatures " + string.Format(MessageErr.MaxLength, 4000) + "; ");
                    }
                }
            }

            //mainImageUrl
            if (!string.IsNullOrWhiteSpace(data.mainImageUrl))
            {
                if (!data.mainImageUrl.IsUrl())
                {
                    msg.mainImageUrl = MessageErr.Url;
                    //msgErr.Append(", " + MessageErr.Url + "; ");
                    if (data.mainImageUrl.Length > 2000)
                    {
                        msg.mainImageUrl += ", " + string.Format(MessageErr.MaxLength, 2000);
                        //msgErr.Append("mainImageUrl " + string.Format(MessageErr.MaxLength, 2000));
                    }
                }
            }

            //productSecondaryImageURL
            if (!string.IsNullOrWhiteSpace(data.productSecondaryImageURL))
            {
                foreach (var item in data.productSecondaryImageURL.Split("|"))
                {
                    if (!item.IsUrl())
                    {
                        msg.productSecondaryImageURL = MessageErr.Url;
                        //msgErr.Append(", " + MessageErr.Url + ";  ");
                        if (data.productSecondaryImageURL.Length > 2000)
                        {
                            msg.productSecondaryImageURL += ", " + string.Format(MessageErr.MaxLength, 2000);
                            //msgErr.Append("productSecondaryImageURL " + string.Format(MessageErr.MaxLength, 2000));
                        }
                        break;
                    }
                }
            }

            //electronicsIndicator
            if (!string.IsNullOrWhiteSpace(data.electronicsIndicator))
            {
                if (!dataExcel["electronicsIndicator"].Contains(data.electronicsIndicator))
                {
                    msg.electronicsIndicator = MessageErr.Exists;
                    //msgErr.Append("electronicsIndicator " + MessageErr.Exists + "; ");
                }
            }

            //batteryTechnologyType
            if (!string.IsNullOrWhiteSpace(data.batteryTechnologyType))
            {
                if (!dataExcel["batteryTechnologyType"].Contains(data.batteryTechnologyType))
                {
                    msg.batteryTechnologyType = MessageErr.Exists;
                    //msgErr.Append("batteryTechnologyType " + MessageErr.Exists + "; ");
                }
            }

            //chemicalAerosolPesticide
            if (!string.IsNullOrWhiteSpace(data.chemicalAerosolPesticide))
            {
                if (!dataExcel["chemicalAerosolPesticide"].Contains(data.chemicalAerosolPesticide))
                {
                    msg.chemicalAerosolPesticide = MessageErr.Exists;
                    //msgErr.Append("chemicalAerosolPesticide " + MessageErr.Exists + "; ");
                }
            }

            //prop65WarningText
            if (!string.IsNullOrWhiteSpace(data.prop65WarningText))
            {
                if (data.prop65WarningText.Length > 5000)
                {
                    msg.prop65WarningText = string.Format(MessageErr.MaxLength, 5000);
                    //msgErr.Append("prop65WarningText " + string.Format(MessageErr.MaxLength, 5000) + "; ");
                }
            }

            //price
            if (!string.IsNullOrWhiteSpace(data.price))
            {

                if (!data.price.IsNumber())
                {
                    msg.price = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + "; ");
                    if (data.price.Length > 9)
                    {
                        msg.price += ", " + string.Format(MessageErr.MaxLength, 9);
                        //msgErr.Append("price " + string.Format(MessageErr.MaxLength, 9));
                    }
                }

            }

            //startDate
            if (!string.IsNullOrWhiteSpace(data.startDate))
            {
                if (!data.startDate.IsDate())
                {
                    msg.startDate = MessageErr.Format;
                    //msgErr.Append("startDate " + MessageErr.Format + "; ");
                }
            }

            //endDate
            if (!string.IsNullOrWhiteSpace(data.endDate))
            {
                if (!data.endDate.IsDate())
                {
                    msg.endDate = MessageErr.Format;
                    //msgErr.Append("endDate " + MessageErr.Format + "; ");
                }
            }

            //additionalOfferAttributeName
            if (!string.IsNullOrWhiteSpace(data.additionalOfferAttributeName))
            {
                if (data.additionalOfferAttributeName.Length > 100)
                {
                    msg.additionalOfferAttributeName = string.Format(MessageErr.MaxLength, 100);
                    //msgErr.Append("additionalOfferAttributeName " + string.Format(MessageErr.MaxLength, 100) + "; ");
                }
            }

            //additionalOfferAttributeValue
            if (!string.IsNullOrWhiteSpace(data.additionalOfferAttributeValue))
            {
                if (data.additionalOfferAttributeValue.Length > 100)
                {
                    msg.additionalOfferAttributeValue = string.Format(MessageErr.MaxLength, 100);
                    //msgErr.Append("additionalOfferAttributeValue " + string.Format(MessageErr.MaxLength, 100) + "; ");
                }
            }

            //msrp
            if (!string.IsNullOrWhiteSpace(data.msrp))
            {

                if (!data.msrp.IsNumber())
                {
                    msg.msrp = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + "; ");
                    if (data.msrp.Length > 8)
                    {
                        msg.msrp += ", " + string.Format(MessageErr.MaxLength, 8);
                        //msgErr.Append("price " + string.Format(MessageErr.MaxLength, 8));
                    }
                }

            }

            //shipsInOriginalPackaging
            if (!string.IsNullOrWhiteSpace(data.shipsInOriginalPackaging))
            {
                if (!dataExcel["shipsInOriginalPackaging"].Contains(data.shipsInOriginalPackaging))
                {
                    msg.shipsInOriginalPackaging = MessageErr.Exists;
                    //msgErr.Append("shipsInOriginalPackaging " + MessageErr.Exists + "; ");
                }
            }

            //MustShipAlone
            if (!string.IsNullOrWhiteSpace(data.MustShipAlone))
            {
                if (!dataExcel["MustShipAlone"].Contains(data.MustShipAlone))
                {
                    msg.MustShipAlone = MessageErr.Exists;
                    //msgErr.Append("MustShipAlone " + MessageErr.Exists + "; ");
                }
            }

            //ShippingWeight
            if (!string.IsNullOrWhiteSpace(data.ShippingWeight))
            {

                if (!data.ShippingWeight.IsNumber())
                {
                    msg.ShippingWeight = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + "; ");
                    if (data.ShippingWeight.Length > 9)
                    {
                        msg.ShippingWeight += ", " + string.Format(MessageErr.MaxLength, 9);
                        //msgErr.Append("ShippingWeight " + string.Format(MessageErr.MaxLength, 9));
                    }
                }

            }

            //stateRestrictionsText
            if (!string.IsNullOrWhiteSpace(data.stateRestrictionsText))
            {
                if (!dataExcel["stateRestrictionsText"].Contains(data.stateRestrictionsText))
                {
                    msg.stateRestrictionsText = MessageErr.Exists;
                    //msgErr.Append("stateRestrictionsText " + MessageErr.Exists + "; ");
                }
            }

            //states
            if (!string.IsNullOrWhiteSpace(data.states))
            {
                if (data.states.Length > 4000)
                {
                    msg.states = string.Format(MessageErr.MaxLength, 4000);
                    //msgErr.Append("states " + string.Format(MessageErr.MaxLength, 4000) + "; ");
                }
            }

            //zipCodes
            if (!string.IsNullOrWhiteSpace(data.zipCodes))
            {
                if (data.zipCodes.Length > 4000)
                {
                    msg.zipCodes = string.Format(MessageErr.MaxLength, 4000);
                    //msgErr.Append("zipCodes " + string.Format(MessageErr.MaxLength, 4000) + "; ");
                }
            }

            //smallPartsWarnings
            if (!string.IsNullOrWhiteSpace(data.smallPartsWarnings))
            {
                if (!dataExcel["smallPartsWarnings"].Contains(data.smallPartsWarnings))
                {
                    msg.smallPartsWarnings = MessageErr.Exists;
                    //msgErr.Append("smallPartsWarnings " + MessageErr.Exists + "; ");
                }
            }

            //gender
            if (!string.IsNullOrWhiteSpace(data.gender))
            {
                if (!dataExcel["gender"].Contains(data.gender))
                {
                    msg.gender = MessageErr.Exists;
                    //msgErr.Append("gender " + MessageErr.Exists + "; ");
                }
            }

            //size
            if (!string.IsNullOrWhiteSpace(data.size))
            {
                if (data.size.Length > 500)
                {
                    msg.size = string.Format(MessageErr.MaxLength, 500);
                    //msgErr.Append("size " + string.Format(MessageErr.MaxLength, 500) + "; ");
                }
            }

            //ageGroup
            if (!string.IsNullOrWhiteSpace(data.ageGroup))
            {
                if (!dataExcel["ageGroup"].Contains(data.ageGroup))
                {
                    msg.ageGroup = MessageErr.Exists;
                    //msgErr.Append("ageGroup " + MessageErr.Exists + "; ");
                }
            }

            //manufacturer
            if (!string.IsNullOrWhiteSpace(data.manufacturer))
            {
                if (data.manufacturer.Length > 60)
                {
                    msg.manufacturer = string.Format(MessageErr.MaxLength, 60);
                    //msgErr.Append("manufacturer " + string.Format(MessageErr.MaxLength, 60) + "; ");
                }
            }

            //manufacturerPartNumber
            if (!string.IsNullOrWhiteSpace(data.manufacturerPartNumber))
            {
                if (data.manufacturerPartNumber.Length > 60)
                {
                    msg.manufacturerPartNumber = string.Format(MessageErr.MaxLength, 60);
                    //msgErr.Append("manufacturerPartNumber " + string.Format(MessageErr.MaxLength, 60) + "; ");
                }
            }

            //modelNumber
            if (!string.IsNullOrWhiteSpace(data.modelNumber))
            {
                if (data.modelNumber.Length > 60)
                {
                    msg.modelNumber = string.Format(MessageErr.MaxLength, 60);
                    //msgErr.Append("modelNumber " + string.Format(MessageErr.MaxLength, 60) + "; ");
                }
            }

            //countPerPack
            if (!string.IsNullOrWhiteSpace(data.countPerPack))
            {
                if (!data.countPerPack.IsNumber())
                {
                    msg.countPerPack = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + "; ");
                    if (data.countPerPack.Length > 9)
                    {
                        msg.countPerPack += ", " + string.Format(MessageErr.MaxLength, 9);
                        //msgErr.Append("countPerPack " + string.Format(MessageErr.MaxLength, 9));
                    }
                }
            }

            //count
            if (!string.IsNullOrWhiteSpace(data.count))
            {
                if (data.count.Length > 50)
                {
                    msg.count = string.Format(MessageErr.MaxLength, 50);
                    //msgErr.Append("count " + string.Format(MessageErr.MaxLength, 50) + "; ");
                }
            }

            //color
            if (!string.IsNullOrWhiteSpace(data.color))
            {
                if (data.color.Length > 600)
                {
                    msg.color = string.Format(MessageErr.MaxLength, 600);
                    //msgErr.Append("color " + string.Format(MessageErr.MaxLength, 600) + "; ");
                }
            }

            //colorCategory
            if (!string.IsNullOrWhiteSpace(data.colorCategory))
            {
                if (!dataExcel["colorCategory"].Contains(data.colorCategory))
                {
                    msg.colorCategory = MessageErr.Exists;
                    //msgErr.Append("colorCategory " + MessageErr.Exists + "; ");
                }
            }

            //ringStyle
            if (!string.IsNullOrWhiteSpace(data.ringStyle))
            {
                if (data.ringStyle.Length > 200)
                {
                    msg.ringStyle = string.Format(MessageErr.MaxLength, 200);
                    //msgErr.Append("ringStyle " + string.Format(MessageErr.MaxLength, 200) + "; ");
                }
            }

            //ringSize
            if (!string.IsNullOrWhiteSpace(data.ringSize))
            {
                if (!data.ringSize.IsNumber())
                {
                    msg.ringSize += ", " + MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + "; ");
                    if (data.ringSize.Length > 9)
                    {
                        msg.ringSize = string.Format(MessageErr.MaxLength, 9);
                        //msgErr.Append("ringSize " + string.Format(MessageErr.MaxLength, 9));
                    }
                }
            }

            //gemstone
            if (!string.IsNullOrWhiteSpace(data.gemstone))
            {
                if (!dataExcel["gemstone"].Contains(data.gemstone))
                {
                    msg.gemstone = MessageErr.Exists;
                    //msgErr.Append("gemstone " + MessageErr.Exists + "; ");
                }
            }

            //secondaryStoneType
            if (!string.IsNullOrWhiteSpace(data.secondaryStoneType))
            {
                if (!dataExcel["secondaryStoneType"].Contains(data.secondaryStoneType))
                {
                    msg.secondaryStoneType = MessageErr.Exists;
                    //msgErr.Append("secondaryStoneType " + MessageErr.Exists + "; ");
                }
            }

            //accentStone
            if (!string.IsNullOrWhiteSpace(data.accentStone))
            {
                if (!dataExcel["accentStone"].Contains(data.accentStone))
                {
                    msg.accentStone = MessageErr.Exists;
                    //msgErr.Append("accentStone " + MessageErr.Exists + "; ");
                }
            }

            //numberOfGemstones
            if (!string.IsNullOrWhiteSpace(data.numberOfGemstones))
            {
                if (!data.numberOfGemstones.IsNumber())
                {
                    msg.numberOfGemstones = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + "; ");
                    if (data.numberOfGemstones.Length > 9)
                    {
                        msg.numberOfGemstones += ", " + string.Format(MessageErr.MaxLength, 9);
                        //msgErr.Append("numberOfGemstones " + string.Format(MessageErr.MaxLength, 9));
                    }
                }
            }

            //chainPattern
            if (!string.IsNullOrWhiteSpace(data.chainPattern))
            {
                if (!dataExcel["chainPattern"].Contains(data.chainPattern))
                {
                    msg.chainPattern = MessageErr.Exists;
                    //msgErr.Append("chainPattern " + MessageErr.Exists + "; ");
                }
            }

            //measureChain
            if (!string.IsNullOrWhiteSpace(data.measureChain))
            {
                if (!data.measureChain.IsNumber())
                {
                    msg.measureChain = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + "; ");
                    if (data.measureChain.Length > 9)
                    {
                        msg.measureChain += ", " + string.Format(MessageErr.MaxLength, 9);
                        //msgErr.Append("measureChain " + string.Format(MessageErr.MaxLength, 9));
                    }
                }
            }

            //unitChain
            if (!string.IsNullOrWhiteSpace(data.unitChain))
            {
                if (!dataExcel["unitChain"].Contains(data.unitChain))
                {
                    msg.unitChain = MessageErr.Exists;
                    //msgErr.Append("unitChain " + MessageErr.Exists + "; ");
                }
            }

            //claspType
            if (!string.IsNullOrWhiteSpace(data.claspType))
            {
                if (!dataExcel["claspType"].Contains(data.claspType))
                {
                    msg.claspType = MessageErr.Exists;
                    //msgErr.Append("claspType " + MessageErr.Exists + "; ");
                }
            }

            //earringStyle
            if (!string.IsNullOrWhiteSpace(data.earringStyle))
            {
                if (!dataExcel["earringStyle"].Contains(data.earringStyle))
                {
                    msg.earringStyle = MessageErr.Exists;
                    //msgErr.Append("earringStyle " + MessageErr.Exists + "; ");
                }
            }

            //measureEar
            if (!string.IsNullOrWhiteSpace(data.measureEar))
            {
                if (!data.measureEar.IsNumber())
                {
                    msg.measureEar = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + "; ");
                    if (data.measureEar.Length > 9)
                    {
                        msg.measureEar += ", " + string.Format(MessageErr.MaxLength, 9);
                        //msgErr.Append("measureEar " + string.Format(MessageErr.MaxLength, 9));
                    }
                }
            }

            //unitEar
            if (!string.IsNullOrWhiteSpace(data.unitEar))
            {
                if (!dataExcel["unitEar"].Contains(data.unitEar))
                {
                    //msgErr.Append("unitEar " + MessageErr.Exists + "; ");
                    msg.unitEar = MessageErr.Exists;
                }
            }

            //backFinding
            if (!string.IsNullOrWhiteSpace(data.backFinding))
            {
                if (!dataExcel["backFinding"].Contains(data.backFinding))
                {
                    //msgErr.Append("backFinding " + MessageErr.Exists + "; ");
                    msg.backFinding = MessageErr.Exists;
                }
            }

            //braceletStyle
            if (!string.IsNullOrWhiteSpace(data.braceletStyle))
            {
                if (!dataExcel["braceletStyle"].Contains(data.braceletStyle))
                {
                    //msgErr.Append("braceletStyle " + MessageErr.Exists + "; ");
                    msg.braceletStyle = MessageErr.Exists;
                }
            }

            //necklaceStyle
            if (!string.IsNullOrWhiteSpace(data.necklaceStyle))
            {
                if (!dataExcel["necklaceStyle"].Contains(data.necklaceStyle))
                {
                    //msgErr.Append("necklaceStyle " + MessageErr.Exists + "; ");
                    msg.necklaceStyle = MessageErr.Exists;
                }
            }

            //bodyJewelryStyle
            if (!string.IsNullOrWhiteSpace(data.bodyJewelryStyle))
            {
                if (data.bodyJewelryStyle.Length > 400)
                {
                    msg.bodyJewelryStyle = string.Format(MessageErr.MaxLength, 400);
                    //msgErr.Append("bodyJewelryStyle " + string.Format(MessageErr.MaxLength, 400) + "; ");
                }
            }

            //braceletCharmStyle
            if (!string.IsNullOrWhiteSpace(data.braceletCharmStyle))
            {
                if (data.braceletCharmStyle.Length > 40)
                {
                    msg.braceletCharmStyle = string.Format(MessageErr.MaxLength, 40);
                    //msgErr.Append("braceletCharmStyle " + string.Format(MessageErr.MaxLength, 40) + "; ");
                }
            }

            //jewelrySetting
            if (!string.IsNullOrWhiteSpace(data.jewelrySetting))
            {
                if (!dataExcel["jewelrySetting"].Contains(data.jewelrySetting))
                {
                    msg.jewelrySetting = MessageErr.Exists;
                    //msgErr.Append("jewelrySetting " + MessageErr.Exists + "; ");
                }
            }

            //certifyingAgent
            if (!string.IsNullOrWhiteSpace(data.certifyingAgent))
            {
                if (data.certifyingAgent.Length > 4000)
                {
                    msg.certifyingAgent = string.Format(MessageErr.MaxLength, 4000);
                    //msgErr.Append("certifyingAgent " + string.Format(MessageErr.MaxLength, 4000) + "; ");
                }
            }

            //diamondClarity
            if (!string.IsNullOrWhiteSpace(data.diamondClarity))
            {
                if (!dataExcel["diamondClarity"].Contains(data.diamondClarity))
                {
                    msg.diamondClarity = MessageErr.Exists;
                    //msgErr.Append("diamondClarity " + MessageErr.Exists + "; ");
                }
            }

            //diamondColor
            if (!string.IsNullOrWhiteSpace(data.diamondColor))
            {
                if (!dataExcel["diamondColor"].Contains(data.diamondColor))
                {
                    msg.diamondColor = MessageErr.Exists;
                    //msgErr.Append("diamondColor " + MessageErr.Exists + "; ");
                }
            }

            //diamondCut
            if (!string.IsNullOrWhiteSpace(data.diamondCut))
            {
                if (!dataExcel["diamondCut"].Contains(data.diamondCut))
                {
                    msg.diamondCut = MessageErr.Exists;
                    //msgErr.Append("diamondCut " + MessageErr.Exists + "; ");
                }
            }

            //measurePrimary
            if (!string.IsNullOrWhiteSpace(data.measurePrimary))
            {

                if (!data.measurePrimary.IsNumber())
                {
                    msg.measurePrimary = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + "; ");
                    if (data.measurePrimary.Length > 9)
                    {
                        msg.measurePrimary += ", " + string.Format(MessageErr.MaxLength, 9);
                        //msgErr.Append("measurePrimary " + string.Format(MessageErr.MaxLength, 9));
                    }
                }

            }

            //unitPrimary
            if (!string.IsNullOrWhiteSpace(data.unitPrimary))
            {
                if (!dataExcel["unitPrimary"].Contains(data.unitPrimary))
                {
                    msg.unitPrimary = MessageErr.Exists;
                    //msgErr.Append("unitPrimary " + MessageErr.Exists + "; ");
                }
            }

            //measureTotal
            if (!string.IsNullOrWhiteSpace(data.measureTotal))
            {

                if (!data.measureTotal.IsNumber())
                {
                    msg.measureTotal = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + "; ");
                    if (data.measureTotal.Length > 9)
                    {
                        msg.measureTotal += ", " + string.Format(MessageErr.MaxLength, 9);
                        //msgErr.Append("measureTotal " + string.Format(MessageErr.MaxLength, 9));
                    }
                }

            }

            //unitTotal
            if (!string.IsNullOrWhiteSpace(data.unitTotal))
            {
                if (!dataExcel["unitTotal"].Contains(data.unitTotal))
                {
                    msg.unitTotal = MessageErr.Exists;
                    //msgErr.Append("unitTotal " + MessageErr.Exists + "; ");
                }
            }

            //stoneTreatment
            if (!string.IsNullOrWhiteSpace(data.stoneTreatment))
            {
                if (data.stoneTreatment.Length > 400)
                {
                    msg.stoneTreatment = string.Format(MessageErr.MaxLength, 400);
                    //msgErr.Append("stoneTreatment " + string.Format(MessageErr.MaxLength, 400) + "; ");
                }
            }

            //stoneCreationMethod
            if (!string.IsNullOrWhiteSpace(data.stoneCreationMethod))
            {
                if (data.stoneCreationMethod.Length > 400)
                {
                    msg.stoneCreationMethod = string.Format(MessageErr.MaxLength, 400);
                    //msgErr.Append("stoneCreationMethod " + string.Format(MessageErr.MaxLength, 400) + "; ");
                }
            }

            //gemstoneShape
            if (!string.IsNullOrWhiteSpace(data.gemstoneShape))
            {
                if (!dataExcel["gemstoneShape"].Contains(data.gemstoneShape))
                {
                    msg.gemstoneShape = MessageErr.Exists;
                    //msgErr.Append("gemstoneShape " + MessageErr.Exists + "; ");
                }
            }

            //pearlType
            if (!string.IsNullOrWhiteSpace(data.pearlType))
            {
                if (data.pearlType.Length > 4000)
                {
                    msg.pearlType = string.Format(MessageErr.MaxLength, 4000);
                    //msgErr.Append("pearlType " + string.Format(MessageErr.MaxLength, 4000) + "; ");
                }
            }

            //measureSize
            if (!string.IsNullOrWhiteSpace(data.measureSize))
            {
                if (!data.measureSize.IsNumber())
                {
                    msg.measureSize = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + "; ");
                    if (data.measureSize.Length > 9)
                    {
                        msg.measureSize += ", " + string.Format(MessageErr.MaxLength, 9);
                        //msgErr.Append("measureSize " + string.Format(MessageErr.MaxLength, 9));
                    }
                }
            }

            //unitSize  
            if (!string.IsNullOrWhiteSpace(data.unitSize))
            {
                if (!dataExcel["unitSize"].Contains(data.unitSize))
                {
                    msg.unitSize = MessageErr.Exists;
                    //msgErr.Append("unitSize " + MessageErr.Exists + "; ");
                }
            }

            //isResizable
            if (!string.IsNullOrWhiteSpace(data.isResizable))
            {
                if (!dataExcel["isResizable"].Contains(data.isResizable))
                {
                    msg.isResizable = MessageErr.Exists;
                    //msgErr.Append("isResizable " + MessageErr.Exists + "; ");
                }
            }

            //personalRelationship
            if (!string.IsNullOrWhiteSpace(data.personalRelationship))
            {
                if (data.personalRelationship.Length > 400)
                {
                    msg.personalRelationship = string.Format(MessageErr.MaxLength, 400);
                    //msgErr.Append("personalRelationship " + string.Format(MessageErr.MaxLength, 400) + "; ");
                }
            }

            //metal
            if (!string.IsNullOrWhiteSpace(data.metal))
            {
                if (!dataExcel["metal"].Contains(data.metal))
                {
                    msg.metal = MessageErr.Exists;
                    //msgErr.Append("metal " + MessageErr.Exists + "; ");
                }
            }

            //metalPurity
            if (!string.IsNullOrWhiteSpace(data.metalPurity))
            {
                if (data.metalPurity.Length > 16)
                {
                    msg.metalPurity = string.Format(MessageErr.MaxLength, 16);
                    //msgErr.Append("metalPurity " + string.Format(MessageErr.MaxLength, 16) + "; ");
                }
            }

            //material
            if (!string.IsNullOrWhiteSpace(data.material))
            {
                if (data.material.Length > 1000)
                {
                    msg.material = string.Format(MessageErr.MaxLength, 1000);
                    //msgErr.Append("material " + string.Format(MessageErr.MaxLength, 1000) + "; ");
                }
            }

            //religion
            if (!string.IsNullOrWhiteSpace(data.religion))
            {
                if (!dataExcel["religion"].Contains(data.religion))
                {
                    msg.religion = MessageErr.Exists;
                    //msgErr.Append("religion " + MessageErr.Exists + "; ");
                }
            }

            //birthstone
            if (!string.IsNullOrWhiteSpace(data.birthstone))
            {
                if (!dataExcel["birthstone"].Contains(data.birthstone))
                {
                    msg.birthstone = MessageErr.Exists;
                    //msgErr.Append("birthstone " + MessageErr.Exists + "; ");
                }
            }

            //style
            if (!string.IsNullOrWhiteSpace(data.style))
            {
                if (data.style.Length > 400)
                {
                    msg.style = string.Format(MessageErr.MaxLength, 400);
                    //msgErr.Append("style " + string.Format(MessageErr.MaxLength, 400) + "; ");
                }
            }

            //shape
            if (!string.IsNullOrWhiteSpace(data.shape))
            {
                if (data.shape.Length > 200)
                {
                    msg.shape = string.Format(MessageErr.MaxLength, 200);
                    //msgErr.Append("shape " + string.Format(MessageErr.MaxLength, 200) + "; ");
                }
            }

            //occasion
            if (!string.IsNullOrWhiteSpace(data.occasion))
            {
                if (data.occasion.Length > 2000)
                {
                    msg.occasion = string.Format(MessageErr.MaxLength, 2000);
                    //msgErr.Append("occasion " + string.Format(MessageErr.MaxLength, 2000) + "; ");
                }
            }

            //bodyParts
            if (!string.IsNullOrWhiteSpace(data.bodyParts))
            {
                if (data.bodyParts.Length > 500)
                {
                    msg.bodyParts = string.Format(MessageErr.MaxLength, 500);
                    //msgErr.Append("bodyParts " + string.Format(MessageErr.MaxLength, 500) + "; ");
                }
            }

            //isMadeFromRecycledMaterial
            if (!string.IsNullOrWhiteSpace(data.isMadeFromRecycledMaterial))
            {
                if (!dataExcel["isMadeFromRecycledMaterial"].Contains(data.isMadeFromRecycledMaterial))
                {
                    msg.isMadeFromRecycledMaterial = MessageErr.Exists;
                    //msgErr.Append("isMadeFromRecycledMaterial " + MessageErr.Exists + "; ");
                }
            }

            //recycledMaterial
            if (!string.IsNullOrWhiteSpace(data.recycledMaterial))
            {
                if (data.recycledMaterial.Length > 4000)
                {
                    msg.recycledMaterial = string.Format(MessageErr.MaxLength, 4000);
                    //msgErr.Append("recycledMaterial " + string.Format(MessageErr.MaxLength, 4000) + "; ");
                }
            }

            //percentageOfRecycledMaterial
            if (!string.IsNullOrWhiteSpace(data.percentageOfRecycledMaterial))
            {
                if (!data.percentageOfRecycledMaterial.IsNumber())
                {
                    msg.percentageOfRecycledMaterial = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + " ;");
                    if (data.percentageOfRecycledMaterial.Length > 9)
                    {
                        msg.percentageOfRecycledMaterial += ", " + string.Format(MessageErr.MaxLength, 9);
                        //msgErr.Append("percentageOfRecycledMaterial " + string.Format(MessageErr.MaxLength, 9));
                    }
                }
            }

            //theme
            if (!string.IsNullOrWhiteSpace(data.theme))
            {
                if (data.theme.Length > 800)
                {
                    msg.theme = string.Format(MessageErr.MaxLength, 800);
                    //msgErr.Append("theme " + string.Format(MessageErr.MaxLength, 800) + "; ");
                }
            }


            //measureDiameter
            if (!string.IsNullOrWhiteSpace(data.measureDiameter))
            {
                if (!data.measureDiameter.IsNumber())
                {
                    msg.measureDiameter = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + " ;");
                    if (data.measureDiameter.Length > 9)
                    {
                        msg.measureDiameter += ", " + string.Format(MessageErr.MaxLength, 9);
                        //msgErr.Append("measureDiameter " + string.Format(MessageErr.MaxLength, 9));
                    }
                }
            }

            //unitDiameter
            if (!string.IsNullOrWhiteSpace(data.unitDiameter))
            {
                if (!dataExcel["unitDiameter"].Contains(data.unitDiameter))
                {
                    msg.unitDiameter = MessageErr.Exists;
                    //msgErr.Append("unitDiameter " + MessageErr.Exists + "; ");
                }
            }

            //character
            if (!string.IsNullOrWhiteSpace(data.character))
            {
                if (data.character.Length > 400)
                {
                    msg.character = string.Format(MessageErr.MaxLength, 400);
                    //msgErr.Append("character " + string.Format(MessageErr.MaxLength, 400) + "; ");
                }
            }


            //sportsTeam
            if (!string.IsNullOrWhiteSpace(data.sportsTeam))
            {
                if (data.sportsTeam.Length > 200)
                {
                    msg.sportsTeam = string.Format(MessageErr.MaxLength, 200);
                    //msgErr.Append("sportsTeam " + string.Format(MessageErr.MaxLength, 200) + "; ");
                }
            }

            //sportsLeague
            if (!string.IsNullOrWhiteSpace(data.sportsLeague))
            {
                if (data.sportsLeague.Length > 100)
                {
                    msg.sportsLeague = string.Format(MessageErr.MaxLength, 100);
                    //msgErr.Append("sportsLeague " + string.Format(MessageErr.MaxLength, 100) + "; ");
                }
            }

            //warrantyText
            if (!string.IsNullOrWhiteSpace(data.warrantyText))
            {
                if (data.warrantyText.Length > 20000)
                {
                    msg.warrantyText = string.Format(MessageErr.MaxLength, 20000);
                    //msgErr.Append("warrantyText " + string.Format(MessageErr.MaxLength, 20000) + "; ");
                }
            }

            //warrantyURL
            if (!string.IsNullOrWhiteSpace(data.warrantyURL))
            {
                if (!data.warrantyURL.IsUrl())
                {
                    msg.warrantyURL = MessageErr.Url;
                    //msgErr.Append("warrantyURL " + MessageErr.Url + "; ");
                }
            }

            //productAttributeName
            if (!string.IsNullOrWhiteSpace(data.productAttributeName))
            {
                if (data.productAttributeName.Length > 100)
                {
                    msg.productAttributeName = string.Format(MessageErr.MaxLength, 100);
                    //msgErr.Append("productAttributeName " + string.Format(MessageErr.MaxLength, 100) + "; ");
                }
            }

            //productAttributeValue
            if (!string.IsNullOrWhiteSpace(data.productAttributeValue))
            {
                if (data.productAttributeValue.Length > 4000)
                {
                    msg.productAttributeValue = string.Format(MessageErr.MaxLength, 4000);
                    //msgErr.Append("productAttributeValue " + string.Format(MessageErr.MaxLength, 4000) + "; ");
                }
            }

            //seasonYear
            if (!string.IsNullOrWhiteSpace(data.seasonYear))
            {
                if (!data.seasonYear.IsNumber())
                {
                    msg.seasonYear = MessageErr.Number;
                    //msgErr.Append(", " + MessageErr.Number + " ;");
                    if (data.seasonYear.Length > 4)
                    {
                        msg.seasonYear += ", " + string.Format(MessageErr.MaxLength, 4);
                        //msgErr.Append("seasonYear " + string.Format(MessageErr.MaxLength, 4));
                    }
                }
            }

            //seasonCode
            if (!string.IsNullOrWhiteSpace(data.seasonCode))
            {
                if (!dataExcel["seasonCode"].Contains(data.seasonCode))
                {
                    msg.seasonCode = MessageErr.Exists;
                    //msgErr.Append("seasonCode " + MessageErr.Exists + "; ");
                }
            }

            //variantGroupId
            if (!string.IsNullOrWhiteSpace(data.variantGroupId))
            {
                if (data.variantGroupId.Length > 300)
                {
                    msg.variantGroupId = string.Format(MessageErr.MaxLength, 300);
                    //msgErr.Append("variantGroupId " + string.Format(MessageErr.MaxLength, 300) + "; ");
                }
            }

            //removeVariantGroup
            if (!string.IsNullOrWhiteSpace(data.removeVariantGroup))
            {
                if (!dataExcel["removeVariantGroup"]
                        .OfType<string>()
                        .Any(x => x.Equals(data.removeVariantGroup, StringComparison.OrdinalIgnoreCase)))
                {
                    msg.removeVariantGroup = MessageErr.Exists;
                    //msgErr.Append("removeVariantGroup " + MessageErr.Exists + "; ");
                }
            }

            //variantAttributeNames
            if (!string.IsNullOrWhiteSpace(data.variantAttributeNames))
            {
                if (!dataExcel["variantAttributeNames"].Contains(data.variantAttributeNames))
                {
                    msg.variantAttributeNames = MessageErr.Exists;
                    //msgErr.Append("isPrimaryVariant " + MessageErr.Exists + "; ");
                }
            }

            //isPrimaryVariant
            if (!string.IsNullOrWhiteSpace(data.isPrimaryVariant))
            {
                if (!dataExcel["isPrimaryVariant"].Contains(data.isPrimaryVariant))
                {
                    msg.isPrimaryVariant = MessageErr.Exists;
                    //msgErr.Append("isPrimaryVariant " + MessageErr.Exists + "; ");
                }
            }

            //swatchVariantAttribute
            if (!string.IsNullOrWhiteSpace(data.swatchVariantAttribute))
            {
                if (!dataExcel["swatchVariantAttribute"].Contains(data.swatchVariantAttribute))
                {
                    msg.swatchVariantAttribute = MessageErr.Exists;
                    //msgErr.Append("swatchVariantAttribute " + MessageErr.Exists + "; ");
                }
            }

            //swatchImageUrl
            if (!string.IsNullOrWhiteSpace(data.swatchImageUrl))
            {
                if (!data.swatchImageUrl.IsUrl())
                {
                    msg.swatchImageUrl = MessageErr.Url;
                    //msgErr.Append(", " + MessageErr.Url + "; ");
                    if (data.swatchImageUrl.Length > 2000)
                    {
                        msg.swatchImageUrl += ", " + string.Format(MessageErr.MaxLength, 2000);
                        //msgErr.Append("swatchImageUrl " + string.Format(MessageErr.MaxLength, 2000));
                    }
                }
            }

            return msg;
        }
    }
}
