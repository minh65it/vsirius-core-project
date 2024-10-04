using static FikaAmazonAPI.ConstructFeed.BaseXML;
namespace FikaAmazonAPI.ConstructFeed.Messages
{
    public partial class ProductMessage
    {
        public string SKU { get; set; }

        public StandardProductID StandardProductID { get; set; }
        public DescriptionDataJewelry DescriptionData { get; set; }

        public Condition Condition { get; set; }
    }
    public partial class DescriptionDataJewelry
    {

        private string titleField;

        private string brandField;

        private string designerField;

        private string descriptionField;

        private string[] bulletPointField;

        //private Dimensions itemDimensionsField;

        //private Dimensions packageDimensionsField;

        //private PositiveWeightDimension packageWeightField;

        //private PositiveWeightDimension shippingWeightField;

        private string merchantCatalogNumberField;

        private CurrencyAmount mSRPField;

        //private CurrencyAmount mSRPWithTaxField;

        //private string maxOrderQuantityField;

        //private bool serialNumberRequiredField;

        //private bool serialNumberRequiredFieldSpecified;

        //private bool prop65Field;

        //private bool prop65FieldSpecified;

        //private ProductDescriptionDataCPSIAWarning[] cPSIAWarningField;

        //private string cPSIAWarningDescriptionField;

        //private string legalDisclaimerField;

        private string manufacturerField;

        //private string mfrPartNumberField;

        //private string searchTermsField;

        //private string[] platinumKeywordsField;

        //private bool memorabiliaField;

        //private bool memorabiliaFieldSpecified;

        //private bool autographedField;

        //private bool autographedFieldSpecified;

        //private string[] usedForField;

        //private string itemTypeField;

        //private string[] otherItemAttributesField;

        //private string[] targetAudienceField;

        //private string[] subjectContentField;

        //private bool isGiftWrapAvailableField;

        //private bool isGiftWrapAvailableFieldSpecified;

        //private bool isGiftMessageAvailableField;

        //private bool isGiftMessageAvailableFieldSpecified;

        //private string[] promotionKeywordsField;

        //private bool isDiscontinuedByManufacturerField;

        //private bool isDiscontinuedByManufacturerFieldSpecified;

        //private string deliveryScheduleGroupIDField;

        //private DeliveryChannel[] deliveryChannelField;

        //private string externalProductInformationField;

        //private string maxAggregateShipQuantityField;

        //private string[] recommendedBrowseNodeField;

        //private string merchantShippingGroupNameField;

        //private string fEDAS_IDField;

        //private ProductDescriptionDataTSDAgeWarning tSDAgeWarningField;

        //private bool tSDAgeWarningFieldSpecified;

        //private ProductDescriptionDataTSDWarning[] tSDWarningField;

        //private ProductDescriptionDataTSDLanguage[] tSDLanguageField;

        //private ProductDescriptionDataOptionalPaymentTypeExclusion optionalPaymentTypeExclusionField;

        //private bool optionalPaymentTypeExclusionFieldSpecified;

        //private DistributionDesignationValues distributionDesignationField;

        //private bool distributionDesignationFieldSpecified;

        //private string[] externalTestingCertificationField;

        //private Battery batteryField;

        //private BatteryCellTypeValues batteryCellTypeField;

        //private bool batteryCellTypeFieldSpecified;

        //private WeightDimension batteryWeightField;

        //private string numberOfLithiumMetalCellsField;

        //private string numberOfLithiumIonCellsField;

        //private ProductDescriptionDataLithiumBatteryPackaging lithiumBatteryPackagingField;

        //private bool lithiumBatteryPackagingFieldSpecified;

        //private EnergyConsumptionDimension lithiumBatteryEnergyContentField;

        //private WeightDimension lithiumBatteryWeightField;

        //private WeightDimension itemWeightField;

        //private VolumeDimension itemVolumeField;

        //private string flashPointField;

        //private ProductDescriptionDataGHSClassificationClass[] gHSClassificationClassField;

        //private ProductDescriptionDataSupplierDeclaredDGHZRegulation[] supplierDeclaredDGHZRegulationField;

        //private string hazmatUnitedNationsRegulatoryIDField;

        //private string safetyDataSheetURLField;



        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Brand
        {
            get
            {
                return this.brandField;
            }
            set
            {
                this.brandField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Designer
        {
            get
            {
                return this.designerField;
            }
            set
            {
                this.designerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BulletPoint", DataType = "normalizedString")]
        public string[] BulletPoint
        {
            get
            {
                return this.bulletPointField;
            }
            set
            {
                this.bulletPointField = value;
            }
        }



        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string MerchantCatalogNumber
        {
            get
            {
                return this.merchantCatalogNumberField;
            }
            set
            {
                this.merchantCatalogNumberField = value;
            }
        }
        /// <remarks/>
        public CurrencyAmount MSRP
        {
            get
            {
                return this.mSRPField;
            }
            set
            {
                this.mSRPField = value;
            }
        }

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        //public string MaxOrderQuantity
        //{
        //    get
        //    {
        //        return this.maxOrderQuantityField;
        //    }
        //    set
        //    {
        //        this.maxOrderQuantityField = value;
        //    }
        //}

        ///// <remarks/>
        //public bool SerialNumberRequired
        //{
        //    get
        //    {
        //        return this.serialNumberRequiredField;
        //    }
        //    set
        //    {
        //        this.serialNumberRequiredField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool SerialNumberRequiredSpecified
        //{
        //    get
        //    {
        //        return this.serialNumberRequiredFieldSpecified;
        //    }
        //    set
        //    {
        //        this.serialNumberRequiredFieldSpecified = value;
        //    }
        //}

        ///// <remarks/>
        //public bool Prop65
        //{
        //    get
        //    {
        //        return this.prop65Field;
        //    }
        //    set
        //    {
        //        this.prop65Field = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool Prop65Specified
        //{
        //    get
        //    {
        //        return this.prop65FieldSpecified;
        //    }
        //    set
        //    {
        //        this.prop65FieldSpecified = value;
        //    }
        //}



        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        //public string CPSIAWarningDescription
        //{
        //    get
        //    {
        //        return this.cPSIAWarningDescriptionField;
        //    }
        //    set
        //    {
        //        this.cPSIAWarningDescriptionField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        //public string LegalDisclaimer
        //{
        //    get
        //    {
        //        return this.legalDisclaimerField;
        //    }
        //    set
        //    {
        //        this.legalDisclaimerField = value;
        //    }
        //}

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        public string Manufacturer
        {
            get
            {
                return this.manufacturerField;
            }
            set
            {
                this.manufacturerField = value;
            }
        }

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        //public string MfrPartNumber
        //{
        //    get
        //    {
        //        return this.mfrPartNumberField;
        //    }
        //    set
        //    {
        //        this.mfrPartNumberField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        //public string SearchTerms
        //{
        //    get
        //    {
        //        return this.searchTermsField;
        //    }
        //    set
        //    {
        //        this.searchTermsField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("PlatinumKeywords", DataType = "normalizedString")]
        //public string[] PlatinumKeywords
        //{
        //    get
        //    {
        //        return this.platinumKeywordsField;
        //    }
        //    set
        //    {
        //        this.platinumKeywordsField = value;
        //    }
        //}

        ///// <remarks/>
        //public bool Memorabilia
        //{
        //    get
        //    {
        //        return this.memorabiliaField;
        //    }
        //    set
        //    {
        //        this.memorabiliaField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool MemorabiliaSpecified
        //{
        //    get
        //    {
        //        return this.memorabiliaFieldSpecified;
        //    }
        //    set
        //    {
        //        this.memorabiliaFieldSpecified = value;
        //    }
        //}

        ///// <remarks/>
        //public bool Autographed
        //{
        //    get
        //    {
        //        return this.autographedField;
        //    }
        //    set
        //    {
        //        this.autographedField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool AutographedSpecified
        //{
        //    get
        //    {
        //        return this.autographedFieldSpecified;
        //    }
        //    set
        //    {
        //        this.autographedFieldSpecified = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("UsedFor", DataType = "normalizedString")]
        //public string[] UsedFor
        //{
        //    get
        //    {
        //        return this.usedForField;
        //    }
        //    set
        //    {
        //        this.usedForField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        //public string ItemType
        //{
        //    get
        //    {
        //        return this.itemTypeField;
        //    }
        //    set
        //    {
        //        this.itemTypeField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("OtherItemAttributes", DataType = "normalizedString")]
        //public string[] OtherItemAttributes
        //{
        //    get
        //    {
        //        return this.otherItemAttributesField;
        //    }
        //    set
        //    {
        //        this.otherItemAttributesField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("TargetAudience", DataType = "normalizedString")]
        //public string[] TargetAudience
        //{
        //    get
        //    {
        //        return this.targetAudienceField;
        //    }
        //    set
        //    {
        //        this.targetAudienceField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("SubjectContent", DataType = "normalizedString")]
        //public string[] SubjectContent
        //{
        //    get
        //    {
        //        return this.subjectContentField;
        //    }
        //    set
        //    {
        //        this.subjectContentField = value;
        //    }
        //}

        ///// <remarks/>
        //public bool IsGiftWrapAvailable
        //{
        //    get
        //    {
        //        return this.isGiftWrapAvailableField;
        //    }
        //    set
        //    {
        //        this.isGiftWrapAvailableField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool IsGiftWrapAvailableSpecified
        //{
        //    get
        //    {
        //        return this.isGiftWrapAvailableFieldSpecified;
        //    }
        //    set
        //    {
        //        this.isGiftWrapAvailableFieldSpecified = value;
        //    }
        //}

        ///// <remarks/>
        //public bool IsGiftMessageAvailable
        //{
        //    get
        //    {
        //        return this.isGiftMessageAvailableField;
        //    }
        //    set
        //    {
        //        this.isGiftMessageAvailableField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool IsGiftMessageAvailableSpecified
        //{
        //    get
        //    {
        //        return this.isGiftMessageAvailableFieldSpecified;
        //    }
        //    set
        //    {
        //        this.isGiftMessageAvailableFieldSpecified = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("PromotionKeywords", DataType = "normalizedString")]
        //public string[] PromotionKeywords
        //{
        //    get
        //    {
        //        return this.promotionKeywordsField;
        //    }
        //    set
        //    {
        //        this.promotionKeywordsField = value;
        //    }
        //}

        ///// <remarks/>
        //public bool IsDiscontinuedByManufacturer
        //{
        //    get
        //    {
        //        return this.isDiscontinuedByManufacturerField;
        //    }
        //    set
        //    {
        //        this.isDiscontinuedByManufacturerField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool IsDiscontinuedByManufacturerSpecified
        //{
        //    get
        //    {
        //        return this.isDiscontinuedByManufacturerFieldSpecified;
        //    }
        //    set
        //    {
        //        this.isDiscontinuedByManufacturerFieldSpecified = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        //public string DeliveryScheduleGroupID
        //{
        //    get
        //    {
        //        return this.deliveryScheduleGroupIDField;
        //    }
        //    set
        //    {
        //        this.deliveryScheduleGroupIDField = value;
        //    }
        //}



        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        //public string ExternalProductInformation
        //{
        //    get
        //    {
        //        return this.externalProductInformationField;
        //    }
        //    set
        //    {
        //        this.externalProductInformationField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        //public string MaxAggregateShipQuantity
        //{
        //    get
        //    {
        //        return this.maxAggregateShipQuantityField;
        //    }
        //    set
        //    {
        //        this.maxAggregateShipQuantityField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("RecommendedBrowseNode", DataType = "positiveInteger")]
        //public string[] RecommendedBrowseNode
        //{
        //    get
        //    {
        //        return this.recommendedBrowseNodeField;
        //    }
        //    set
        //    {
        //        this.recommendedBrowseNodeField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        //public string MerchantShippingGroupName
        //{
        //    get
        //    {
        //        return this.merchantShippingGroupNameField;
        //    }
        //    set
        //    {
        //        this.merchantShippingGroupNameField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        //public string FEDAS_ID
        //{
        //    get
        //    {
        //        return this.fEDAS_IDField;
        //    }
        //    set
        //    {
        //        this.fEDAS_IDField = value;
        //    }
        //}


        ///// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool TSDAgeWarningSpecified
        //{
        //    get
        //    {
        //        return this.tSDAgeWarningFieldSpecified;
        //    }
        //    set
        //    {
        //        this.tSDAgeWarningFieldSpecified = value;
        //    }
        //}




        ///// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool OptionalPaymentTypeExclusionSpecified
        //{
        //    get
        //    {
        //        return this.optionalPaymentTypeExclusionFieldSpecified;
        //    }
        //    set
        //    {
        //        this.optionalPaymentTypeExclusionFieldSpecified = value;
        //    }
        //}


        ///// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool DistributionDesignationSpecified
        //{
        //    get
        //    {
        //        return this.distributionDesignationFieldSpecified;
        //    }
        //    set
        //    {
        //        this.distributionDesignationFieldSpecified = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("ExternalTestingCertification", DataType = "normalizedString")]
        //public string[] ExternalTestingCertification
        //{
        //    get
        //    {
        //        return this.externalTestingCertificationField;
        //    }
        //    set
        //    {
        //        this.externalTestingCertificationField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool BatteryCellTypeSpecified
        //{
        //    get
        //    {
        //        return this.batteryCellTypeFieldSpecified;
        //    }
        //    set
        //    {
        //        this.batteryCellTypeFieldSpecified = value;
        //    }
        //}


        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        //public string NumberOfLithiumMetalCells
        //{
        //    get
        //    {
        //        return this.numberOfLithiumMetalCellsField;
        //    }
        //    set
        //    {
        //        this.numberOfLithiumMetalCellsField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "positiveInteger")]
        //public string NumberOfLithiumIonCells
        //{
        //    get
        //    {
        //        return this.numberOfLithiumIonCellsField;
        //    }
        //    set
        //    {
        //        this.numberOfLithiumIonCellsField = value;
        //    }
        //}


        ///// <remarks/>
        //[System.Xml.Serialization.XmlIgnoreAttribute()]
        //public bool LithiumBatteryPackagingSpecified
        //{
        //    get
        //    {
        //        return this.lithiumBatteryPackagingFieldSpecified;
        //    }
        //    set
        //    {
        //        this.lithiumBatteryPackagingFieldSpecified = value;
        //    }
        //}



        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "normalizedString")]
        //public string FlashPoint
        //{
        //    get
        //    {
        //        return this.flashPointField;
        //    }
        //    set
        //    {
        //        this.flashPointField = value;
        //    }
        //}


        ///// <remarks/>
        //public string HazmatUnitedNationsRegulatoryID
        //{
        //    get
        //    {
        //        return this.hazmatUnitedNationsRegulatoryIDField;
        //    }
        //    set
        //    {
        //        this.hazmatUnitedNationsRegulatoryIDField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI")]
        //public string SafetyDataSheetURL
        //{
        //    get
        //    {
        //        return this.safetyDataSheetURLField;
        //    }
        //    set
        //    {
        //        this.safetyDataSheetURLField = value;
        //    }
        //}

    }
}
