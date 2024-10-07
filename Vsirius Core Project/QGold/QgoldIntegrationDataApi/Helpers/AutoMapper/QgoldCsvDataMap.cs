using CsvHelper.Configuration;
using QgoldIntegrationDataApi.Models;

namespace QgoldIntegrationDataApi.Helpers.AutoMapper
{
    public class QgoldCsvDataMap : ClassMap<QgoldProduct>
    {
        public QgoldCsvDataMap()
        {
            Map(m => m.Item).Index(0);
            Map(m => m.Description).Index(1);
            Map(m => m.Status).Index(2);
            Map(m => m.Weight).Index(3);
            Map(m => m.CT_Weight).Index(4);
            Map(m => m.Gem_Weight).Index(5);
            Map(m => m.Size).Index(6);
            Map(m => m.Length).Index(7);
            Map(m => m.Width).Index(8);
            Map(m => m.Sizeable).Index(9);
            Map(m => m.Metal_Desc).Index(10);
            Map(m => m.Image).Index(11);
            Map(m => m.UM_Sales).Index(12);
            Map(m => m.Qty_Avail).Index(13);
            Map(m => m.Metal_Price).Index(14);
            Map(m => m.Price).Index(15);
            Map(m => m.Customer_Item).Index(16);
            Map(m => m.CategoriesText).Index(17);
            Map(m => m.AttributesText).Index(18);
            Map(m => m.MSRP).Index(19);
            Map(m => m.Item_Type).Index(20);
            Map(m => m.DateAdded).Index(21);
            Map(m => m.ProductLine).Index(22);
            Map(m => m.ImageLink_100).Index(23);
            Map(m => m.ImageLink_275).Index(24);
            Map(m => m.ImageLink_500).Index(25);
            Map(m => m.ImageLink_1000).Index(26);
            Map(m => m.Image1Name).Index(27);
            Map(m => m.Image1MaxHeight).Index(28);
            Map(m => m.Image1MaxWidth).Index(29);
            Map(m => m.Image1Link).Index(30);
            Map(m => m.Image2Name).Index(31);
            Map(m => m.Image2MaxHeight).Index(32);
            Map(m => m.Image2MaxWidth).Index(33);
            Map(m => m.Image2Link).Index(34);
            Map(m => m.Image3Name).Index(35);
            Map(m => m.Image3MaxHeight).Index(36);
            Map(m => m.Image3MaxWidth).Index(37);
            Map(m => m.Image3Link).Index(38);
            Map(m => m.Image4Name).Index(39);
            Map(m => m.Image4MaxHeight).Index(40);
            Map(m => m.Image4MaxWidth).Index(41);
            Map(m => m.Image4Link).Index(42);
            Map(m => m.Image5Name).Index(43);
            Map(m => m.Image5MaxHeight).Index(44);
            Map(m => m.Image5MaxWidth).Index(45);
            Map(m => m.Image5Link).Index(46);
            Map(m => m.Image6Name).Index(47);
            Map(m => m.Image6MaxHeight).Index(48);
            Map(m => m.Image6MaxWidth).Index(49);
            Map(m => m.Image6Link).Index(50);
            Map(m => m.Image7Name).Index(51);
            Map(m => m.Image7MaxHeight).Index(52);
            Map(m => m.Image7MaxWidth).Index(53);
            Map(m => m.Image7Link).Index(54);
            Map(m => m.Image8Name).Index(55);
            Map(m => m.Image8MaxHeight).Index(56);
            Map(m => m.Image8MaxWidth).Index(57);
            Map(m => m.Image8Link).Index(58);
            Map(m => m.Image9Name).Index(59);
            Map(m => m.Image9MaxHeight).Index(60);
            Map(m => m.Image9MaxWidth).Index(61);
            Map(m => m.Image9Link).Index(62);
            Map(m => m.Country_Of_Origin).Index(63);
            Map(m => m.ListOfSpecsText).Index(64);
            Map(m => m.UPC).Index(65);
            Map(m => m.Book).Index(66);
            Map(m => m.Page).Index(67);
        }
    }
}
