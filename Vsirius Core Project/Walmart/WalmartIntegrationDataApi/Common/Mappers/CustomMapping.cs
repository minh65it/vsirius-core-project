using System.Configuration.Provider;
using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Dtos.Order;
using WalmartIntegrationDataApi.Dtos.Vsirius;
using WalmartIntegrationDataApi.Models;
using WalmartIntegrationDataApi.Models.Temp;

namespace WalmartIntegrationDataApi.Common.Mappers
{
    public static class CustomMapping
    {
        public static List<OrderDetailDTO> ConvertToWalmartOrderDTO(List<Order> walmarts, List<OrderMarketplace> lstQgold, List<Dtos.Stuller.StullerOrderDto> lstStuller)
        {
            var result = new List<OrderDetailDTO>();

            foreach (var walmart in walmarts)
            {
                var providerOrderId = lstQgold.FirstOrDefault(x => x.Seller_Order_ID == walmart.purchaseOrderId)?.OrderId
                                                ?? lstStuller.FirstOrDefault(x => x.marketplaceOrderId == walmart.purchaseOrderId)?.orderID;
                var providerName = Util.GetProviderName(walmart.orderLines.orderLine[0].item.sku);
                result.Add(new OrderDetailDTO()
                {
                    customerOrderId = walmart.customerOrderId,
                    purchaseOrderId = walmart.purchaseOrderId,
                    SupplierOrderId = providerOrderId,
                    Supplier = providerName,
                    BuyerName = walmart.shippingInfo.postalAddress.name,
                    Address1 = walmart.shippingInfo.postalAddress.address1,
                    Address2 = walmart.shippingInfo.postalAddress.address2,
                    City = walmart.shippingInfo.postalAddress.city,
                    Country = walmart.shippingInfo.postalAddress.country,
                    customerEmailId = walmart.customerEmailId,
                    Phone = walmart.shippingInfo.phone,
                    estimatedDeliveryDate = Util.ConvertEpochUTCToDateTime(walmart.shippingInfo.estimatedDeliveryDate),
                    estimatedShipDate = Util.ConvertEpochUTCToDateTime(walmart.shippingInfo.estimatedShipDate),
                    IsSubmitted = null,
                    methodShipping = walmart.shippingInfo.methodCode,
                    orderDate = Util.ConvertEpochUTCToDateTime(walmart.orderDate),
                    PostalCode = walmart.shippingInfo.postalAddress.postalCode,
                    State = walmart.shippingInfo.postalAddress.state,
                    CarrierName = walmart.orderLines?.orderLine[0]?.orderLineStatuses?.orderLineStatus[0]?.trackingInfo?.carrierName?.carrier,
                    TrackingNumber = walmart.orderLines?.orderLine[0]?.orderLineStatuses?.orderLineStatus[0]?.trackingInfo?.trackingNumber,
                    Items = MapItemWalmart(walmart.orderLines!.orderLine)
                });
            }

            return result;
        }

        public static List<WalmartItemDTO> MapItemWalmart(List<OrderLine> orderLine)
        {
            var result = new List<WalmartItemDTO>();

            foreach (var item in orderLine)
            {
                result.Add(new WalmartItemDTO()
                {
                    OrderStatus = item.orderLineStatuses.orderLineStatus[0].status,
                    Price = item.charges?.charge[0]?.chargeAmount?.amount,
                    ProductName = item.item.productName,
                    quantity = item.orderLineQuantity.amount,
                    Sku = item.item.sku,
                    Tax = item.charges?.charge[0]?.tax?.taxAmount?.amount
                });
            }

            return result;
        }

        public static Walmart TransformVsiriusProductToWalmart(VsiriusProductDto vsirius)
        {
            Walmart product = new()
            {
                SKU = $"{VsiriusFunctionMapper.GetSKU(vsirius)}"
            };
            product.productIdType = $"{VsiriusFunctionMapper.GetProductIdType(vsirius.external_product_id_type, false)}";
            product.productId = $"{VsiriusFunctionMapper.GetProductId(vsirius.external_product_id, vsirius.external_product_id_type)}";
            product.externalProductIdType = string.Empty;
            product.externalProductId = string.Empty;
            //product.externalProductIdType = $"{VsiriusFunctionMapper.GetProductIdType(vsirius.external_product_id_type; true)}";
            //product.externalProductId = $"{VsiriusFunctionMapper.GetExternalProductId(vsirius.item_sku; vsirius.external_product_id_type)}";
            product.productName = $"{vsirius.item_name}";
            product.brand = $"{vsirius.brand_name}";
            product.pricePerUnitQuantity = null;
            product.pricePerUnitUom = null;
            product.multipackQuantity = $"{vsirius.item_package_quantity}";
            product.shortDescription = $"{vsirius.product_description}";
            product.keyFeatures = $"{VsiriusFunctionMapper.GetKeyFeatures(vsirius)}";
            product.mainImageUrl = vsirius.main_image_url;
            product.productSecondaryImageURL = VsiriusFunctionMapper.GetProductSecondaryImageURL(vsirius);
            product.electronicsIndicator = null;
            product.batteryTechnologyType = null;
            product.chemicalAerosolPesticide = null;
            product.prop65WarningText = VsiriusFunctionMapper.GetProp65WarningText(vsirius);
            product.price = VsiriusFunctionMapper.GetPrice(vsirius);
            product.startDate = vsirius.sale_from_date;
            product.endDate = vsirius.sale_end_date;
            product.additionalOfferAttributeName = null;
            product.additionalOfferAttributeValue = null;
            product.msrp = VsiriusFunctionMapper.GetMsrpForWalmart(vsirius.map_price);
            product.shipsInOriginalPackaging = "Yes";
            product.MustShipAlone = "Yes";
            product.ShippingWeight = VsiriusFunctionMapper.GetShippingWeight(vsirius);
            product.stateRestrictionsText = $"None";
            product.states = null;
            product.zipCodes = null;
            product.smallPartsWarnings = VsiriusFunctionMapper.GetSmallPartsWarnings(vsirius.cpsia_cautionary_statement1);
            product.gender = vsirius.target_gender;
            product.size = vsirius.size_name;
            product.ageGroup = VsiriusFunctionMapper.GetAgeGroup(vsirius.department_name1);
            product.manufacturer = vsirius.manufacturer;
            product.manufacturerPartNumber = product.SKU;
            product.modelNumber = $"{VsiriusFunctionMapper.GetSKU(vsirius)}";
            product.countPerPack = vsirius.item_package_quantity;
            product.count = vsirius.package_contains_quantity;
            product.color = vsirius.color_name;
            product.colorCategory = VsiriusFunctionMapper.GetColorCategory(vsirius.color_map);
            product.ringStyle = vsirius.feed_product_type.Equals("ring") ? vsirius.item_type : "";
            product.ringSize = vsirius.ring_size.IsNumber() ? vsirius.ring_size : "";
            product.gemstone = VsiriusFunctionMapper.GetGemStone(vsirius.gem_type1);
            product.secondaryStoneType = VsiriusFunctionMapper.GetGemStone(vsirius.gem_type2);
            product.accentStone = VsiriusFunctionMapper.GetGemStone(vsirius.gem_type3);
            product.numberOfGemstones = vsirius.number_of_stones;
            product.chainPattern = VsiriusFunctionMapper.GetChainType(vsirius.chain_type);
            product.measureChain = VsiriusFunctionMapper.GetMeasureChain(vsirius);
            product.unitChain = string.IsNullOrWhiteSpace(vsirius.chain_length_derived) ? "" : "in";
            product.claspType = VsiriusFunctionMapper.GetClaspType(vsirius.clasp_type);
            product.earringStyle = vsirius.feed_product_type.Contains("earring") ? VsiriusFunctionMapper.GetEarringStyle(vsirius.item_type) : "";
            product.measureEar = vsirius.feed_product_type.Contains("earring") ? VsiriusFunctionMapper.GetEarringLength(vsirius) : "";
            product.unitEar = vsirius.feed_product_type.Contains("earring") ? VsiriusFunctionMapper.GetEarringLengthUnit(vsirius) : "";
            product.backFinding = VsiriusFunctionMapper.GetBackFinding(vsirius.back_finding);
            product.braceletStyle = vsirius.feed_product_type.Equals("bracelet") ? VsiriusFunctionMapper.GetBraceletStyle(vsirius.item_type) : "";
            product.necklaceStyle = vsirius.feed_product_type.Equals("necklace") ? VsiriusFunctionMapper.GetNecklaceStyle(vsirius.item_type) : "";
            product.bodyJewelryStyle = null;
            product.braceletCharmStyle = vsirius.feed_product_type.Equals("charm") ? "pendants" : "";
            product.jewelrySetting = VsiriusFunctionMapper.GetJewelrySetting(vsirius.setting_type);
            product.certifyingAgent = VsiriusFunctionMapper.GetCertifyingAgent(vsirius);
            if (vsirius.gem_type1 != null)
            {
                product.diamondClarity = vsirius.gem_type1.Contains("diamond", StringComparison.OrdinalIgnoreCase) ? VsiriusFunctionMapper.GetDiamondClarity(vsirius.stone_clarity1) : "";
                product.diamondColor = vsirius.gem_type1.Contains("diamond", StringComparison.OrdinalIgnoreCase) ? VsiriusFunctionMapper.GetDiamondColor(vsirius.stone_color1) : "";
                product.diamondCut = vsirius.gem_type1.Contains("diamond", StringComparison.OrdinalIgnoreCase) ? VsiriusFunctionMapper.GetDiamondCut(vsirius.stone_cut1) : "";
            }
            product.measurePrimary = null;
            product.unitPrimary = null;
            product.measureTotal = VsiriusFunctionMapper.GetDiamondWeightTotal(vsirius);
            product.unitTotal =
            string.IsNullOrWhiteSpace(vsirius.total_diamond_weight_unit_of_measure)
            || string.IsNullOrWhiteSpace(vsirius.total_diamond_weight) ? "" : "ct";
            product.stoneTreatment = vsirius.stone_treatment_method1;
            product.stoneCreationMethod = vsirius.stone_creation_method1;
            product.gemstoneShape = VsiriusFunctionMapper.GetGemstoneShape(vsirius.stone_shape1);
            product.pearlType = vsirius.pearl_type;
            product.measureSize = VsiriusFunctionMapper.GetMeasureSize(vsirius.size_per_pearl);
            product.unitSize = string.IsNullOrWhiteSpace(VsiriusFunctionMapper.GetMeasureSize(vsirius.size_per_pearl)) ? "" : "mm";
            product.isResizable = vsirius.is_resizable;
            product.personalRelationship = null;
            product.metal = VsiriusFunctionMapper.GetMetalType(vsirius.metal_type);
            product.metalPurity = vsirius.metal_stamp;
            product.material = VsiriusFunctionMapper.GetMaterialType(vsirius);
            product.religion = null;
            product.birthstone = null;
            product.style = vsirius.style_name;
            product.shape = vsirius.item_shape;
            product.occasion = VsiriusFunctionMapper.GetOccasion(vsirius);
            product.bodyParts = null;
            product.isMadeFromRecycledMaterial = null;
            product.recycledMaterial = null;
            product.percentageOfRecycledMaterial = null;
            product.theme = $"{(string.IsNullOrWhiteSpace(vsirius.theme) ? "" : vsirius.theme + ";")} {vsirius.variation_theme}";
            product.measureDiameter = VsiriusFunctionMapper.GetMeasureDiameter(vsirius);
            product.unitDiameter = VsiriusFunctionMapper.GetUnitDiameter(vsirius);
            product.character = $"{vsirius.initial_character}";
            product.sportsTeam = vsirius.team_name;
            product.sportsLeague = vsirius.league_name;
            product.warrantyText = vsirius.warranty_description;
            product.warrantyURL = null;
            product.productAttributeName = null;
            product.productAttributeValue = null;
            product.seasonYear = null;
            product.seasonCode = VsiriusFunctionMapper.GetSeasonCode(vsirius.collection_name);
            product.variantGroupId = null;
            product.removeVariantGroup = null;
            product.variantAttributeNames = null;
            product.isPrimaryVariant = null;
            product.swatchVariantAttribute = null;
            product.swatchImageUrl = null;
            product.quantity = vsirius.quantity;


            return product;
        }
    }
}
