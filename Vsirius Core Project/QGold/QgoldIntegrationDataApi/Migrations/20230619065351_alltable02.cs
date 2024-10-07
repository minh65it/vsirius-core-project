using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QgoldIntegrationDataApi.Migrations
{
    public partial class alltable02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QgoldCsvDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributesText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Book = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriesText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country_Of_Origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CT_Weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer_Item = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gem_Weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image1Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image1MaxHeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image1MaxWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image1Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image2Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image2MaxHeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image2MaxWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image2Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image3Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image3MaxHeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image3MaxWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image3Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image4Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image4MaxHeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image4MaxWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image4Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image5Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image5MaxHeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image5MaxWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image5Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image6Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image6MaxHeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image6MaxWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image6Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image7Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image7MaxHeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image7MaxWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image7Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image8Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image8MaxHeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image8MaxWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image8Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image9Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image9MaxHeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image9MaxWidth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image9Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageLink_100 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageLink_1000 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageLink_275 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageLink_500 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Item = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Item_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ListOfSpecsText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Metal_Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Metal_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MSRP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Page = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductLine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qty_Avail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sizeable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UM_Sales = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UPC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Width = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QgoldCsvDatas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QgoldCsvDatas");
        }
    }
}
