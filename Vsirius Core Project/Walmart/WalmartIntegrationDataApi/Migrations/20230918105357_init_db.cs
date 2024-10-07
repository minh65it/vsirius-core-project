using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalmartIntegrationDataApi.Migrations
{
    /// <inheritdoc />
    public partial class init_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Walmarts",
                columns: table => new
                {
                    SKU = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    productIdType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    productId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    externalProductIdType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    externalProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    productName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pricePerUnitQuantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pricePerUnitUom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    multipackQuantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    keyFeatures = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mainImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    productSecondaryImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    electronicsIndicator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    batteryTechnologyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    chemicalAerosolPesticide = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prop65WarningText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    startDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    endDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    additionalOfferAttributeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    additionalOfferAttributeValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    msrp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shipsInOriginalPackaging = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MustShipAlone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stateRestrictionsText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    states = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    zipCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    smallPartsWarnings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ageGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    manufacturerPartNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modelNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    countPerPack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    count = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    colorCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ringStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ringSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gemstone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    secondaryStoneType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accentStone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numberOfGemstones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    chainPattern = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    measureChain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unitChain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    claspType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    earringStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    measureEar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unitEar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    backFinding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    braceletStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    necklaceStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bodyJewelryStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    braceletCharmStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    jewelrySetting = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certifyingAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diamondClarity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diamondColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diamondCut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    measurePrimary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unitPrimary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    measureTotal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unitTotal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stoneTreatment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stoneCreationMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gemstoneShape = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pearlType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    measureSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unitSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isResizable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    personalRelationship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    metal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    metalPurity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    material = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    religion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    birthstone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    style = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shape = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    occasion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bodyParts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isMadeFromRecycledMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    recycledMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    percentageOfRecycledMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    theme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    measureDiameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unitDiameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    character = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sportsTeam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sportsLeague = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    warrantyText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    warrantyURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    productAttributeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    productAttributeValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seasonYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seasonCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    variantGroupId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    removeVariantGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    variantAttributeNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isPrimaryVariant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    swatchVariantAttribute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    swatchImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Walmarts", x => x.SKU);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Walmarts");
        }
    }
}
