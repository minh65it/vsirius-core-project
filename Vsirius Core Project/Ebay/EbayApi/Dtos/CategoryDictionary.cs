using System.Collections.Generic;
//https://www.isoldwhat.com/?1=1&RootID=281&L2C=10968#10968
//Fashion Jewelry # 10968
//Anklets   Go to eBay # 43205 (leaf)
//Bracelets & Charms   Go to eBay # 261987 (leaf)
//Brooches & Pins   Go to eBay # 50677 (leaf)
//Earrings   Go to eBay # 50647 (leaf)
//Hair & Head Jewelry   Go to eBay # 110620 (leaf)
//Jewelry Sets   Go to eBay # 50692 (leaf)
//Necklaces & Pendants   Go to eBay # 155101 (leaf)
//Rings   Go to eBay # 67681 (leaf)
//Toe Rings   Go to eBay # 140010 (leaf)
namespace EbayApi.Dtos
{
    public static class CategoryDictionary
    {
        public static List<KeyValuePair<string, string>> Categories = GetCategories();
        private static List<KeyValuePair<string, string>> GetCategories()
        {
            if (Categories == null)
            {
                return new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Anklets","43205"),
                    new KeyValuePair<string, string>("Bracelets & Charms","261987"),
                    new KeyValuePair<string, string>("Brooches & Pins","50677"),
                    new KeyValuePair<string, string>("Earrings","50647"),
                    new KeyValuePair<string, string>("Hair & Head Jewelry","110620"),
                    new KeyValuePair<string, string>("Jewelry Sets","50692"),
                    new KeyValuePair<string, string>("Necklaces & Pendants","155101"),
                    new KeyValuePair<string, string>("Rings","67681"),
                    new KeyValuePair<string, string>("Toe Rings","140010"),
                    new KeyValuePair<string, string>("Watches","260325"),
                    new KeyValuePair<string, string>("Jewelry Findings","262018"),
                    new KeyValuePair<string, string>("Cufflinks","262006"),
                    new KeyValuePair<string, string>("Other Jewelry","262023"),
                    new KeyValuePair<string, string>("Wallets","2996|45258"),
                    new KeyValuePair<string, string>("Key Chains, Rings & Finders","45237|52373"),
                };
            }
            return Categories;
        }

        public static readonly Dictionary<string, string> CategoryPIM = GetCategoryPim();

        private static Dictionary<string, string> GetCategoryPim()
        {
            if (CategoryPIM == null)
            {
                var rs = new Dictionary<string, string>
                {
                    { "jewelryfinding", "Jewelry Findings" },
                    { "necklace", "Necklaces & Pendants" },
                    { "bracelet", "Bracelets & Charms" },
                    { "keychain", "Key Chains, Rings & Finders" },
                    { "earring", "Earrings" },
                    { "watches", "Watches" },
                    { "piercingjewelry", "Other Jewelry" },
                    { "accessory", "Other Jewelry" },
                    { "apparelpin", "Brooches & Pins" },
                    { "cufflink", "Cufflinks" },
                    { "jewelryset", "Jewelry Sets" },
                    { "fineother", "Other Jewelry" },
                    { "ring", "Rings" },
                    { "wallet", "Wallets" },
                    { "charm", "Bracelets & Charms" }
                };
                return rs;

            }
            else
            {
                return CategoryPIM;
            }
        }
    }
}