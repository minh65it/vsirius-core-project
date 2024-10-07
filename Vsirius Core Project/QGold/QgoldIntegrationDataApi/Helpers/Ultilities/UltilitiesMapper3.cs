using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using QgoldIntegrationDataApi.Dtos.Marketplace;
using qgoldDataDictionary = QgoldIntegrationDataApi.Helpers.Dictionarys.MapDictionary;

namespace QgoldIntegrationDataApi.Helpers.Ultilities
{
    public static class UltilitiesMapper3
    {
        #region Required
        public static string GetFeedProductTypeAttribute(string item_Type, string categories)
        {
            if (item_Type != null)
            {
                string[] itemType = item_Type.Split(':');
                if (itemType.Length > 1)
                {
                    if (itemType[0] == "Jewelry")
                    {
                        return "Jewelry";
                    }
                    switch (itemType[1])
                    {
                        case " Beads":
                        case " Coin":
                        case " Pendants & Charms":
                            return "charm";

                        case " Bracelets":
                            return "bracelet";

                        case " Earrings":
                            return "earring";

                        case " Chains":
                        case " Necklaces":
                            return "necklace";

                        case " Rings":
                            return "ring";

                        case " Anklets":
                        case " Anlets":
                            return "bracelet";

                        case " Button Covers":
                        case " Cuff Links":
                            return "cufflink";

                        case " Tie Tacs / Tie Bars":
                        case " Pins & Hair Accessories":
                        case " Pin & Hair Accessories":
                            return "apparelpin";

                        case " Card Holders":
                            return "wallet";

                        case " Key Rings":
                        case " Key Rings, Money Clips & Wallets":
                            return "keychain";

                        case " Money Clips":
                            return "accessory";

                        case " Miscellaneous":
                            return "jewelryfinding";

                        case "Watch":
                        case " Children's":
                        case " Wrist":
                            return "watches";

                        case " Eyebrow Jewelry":
                        case " Labrets":
                        case " Naval Jewelry":
                        case " Nostril Jewelry":
                            return "piercingjewelry";

                        case "":
                        case "Jewelry":
                        case "Gift":
                        case " Mountings":
                            if (categories.Contains("Bracelets", StringComparison.OrdinalIgnoreCase)
                                || categories.Contains("Bangle", StringComparison.OrdinalIgnoreCase)) return "bracelet";
                            if (categories.Contains("Earrings", StringComparison.OrdinalIgnoreCase)) return "earring";
                            if (categories.Contains("Pendants", StringComparison.OrdinalIgnoreCase)) return "charm";
                            if (categories.Contains("Necklaces", StringComparison.OrdinalIgnoreCase)) return "necklace";
                            if (categories.Contains("Rings", StringComparison.OrdinalIgnoreCase)) return "ring";
                            if (categories.Contains("Cuff Links", StringComparison.OrdinalIgnoreCase)) return "cufflink";
                            break;
                    }
                    return "fineother";
                }
                return "fineother";
            }
            return string.Empty;
        }
        public static string GetFeedProductType(string item_Type, string categories)
        {
            if (item_Type != null)
            {
                string[] itemType = item_Type.Split(':');
                int indexArray = 0;
                if (itemType.Length > 1)
                {
                    indexArray = 1;
                }
                switch (itemType[indexArray].Trim())
                {
                    case "Beads":
                    case "Coin":
                    case "Pendants & Charms":
                        return "charm";

                    case "Bracelets":
                        return "bracelet";
                    case "Sets":
                        return "jewelryset";
                    case "Earrings":
                        return "earring";

                    case "Chains":
                    case "Necklaces":
                        return "necklace";

                    case "Rings":
                        return "ring";

                    case "Anklets":
                    case "Anlets":
                        return "bracelet";

                    case "Button Covers":
                    case "Cuff Links":
                        return "cufflink";

                    case "Tie Tacs / Tie Bars":
                    case "Pins & Hair Accessories":
                    case "Pin & Hair Accessories":
                        return "apparelpin";

                    case "Card Holders":
                        return "wallet";

                    case "Key Rings":
                    case "Key Rings, Money Clips & Wallets":
                        return "keychain";

                    case "Money Clips":
                        return "accessory";

                    case "Miscellaneous":
                        return "jewelryfinding";

                    case "Watch":
                    case "Children's":
                    case "Wrist":
                        return "watches";

                    case "Eyebrow Jewelry":
                    case "Labrets":
                    case "Naval Jewelry":
                    case "Nostril Jewelry":
                        return "piercingjewelry";

                    case "":
                    case "Jewelry":
                    case "Gift":
                    case "Mountings":
                        if (categories.Contains("Bracelets", StringComparison.OrdinalIgnoreCase)
                            || categories.Contains("Bangle", StringComparison.OrdinalIgnoreCase)) return "bracelet";
                        if (categories.Contains("Earrings", StringComparison.OrdinalIgnoreCase)) return "earring";
                        if (categories.Contains("Pendants", StringComparison.OrdinalIgnoreCase)) return "charm";
                        if (categories.Contains("Necklaces", StringComparison.OrdinalIgnoreCase)) return "necklace";
                        if (categories.Contains("Rings", StringComparison.OrdinalIgnoreCase)) return "ring";
                        if (categories.Contains("Cuff Links", StringComparison.OrdinalIgnoreCase)) return "cufflink";
                        break;
                }
                return "fineother";
            }
            return string.Empty;
        }

        public static string GetItemType(string item_Type, string categories)
        {
            if (item_Type != null)
            {
                string[] itemType = item_Type.Split(':');
                if (itemType.Length == 2)
                {
                    switch (itemType[1])
                    {
                        case " Beads":
                        case " Coin":
                        case " Pendants & Charms":
                            return "charm";
                        case "jewelryset":
                            return "jewelry-sets";
                        case " Bracelets":
                            return "bracelet";

                        case " Earrings":
                            return "earring";

                        case " Chains":
                        case " Necklaces":
                            return "necklace";

                        case " Rings":
                            return "ring";

                        case " Anklets":
                        case " Anlets":
                            return "bracelet";

                        case " Button Covers":
                        case " Cuff Links":
                            return "cufflink";

                        case " Tie Tacs / Tie Bars":
                        case " Pins & Hair Accessories":
                        case " Pin & Hair Accessories":
                            return "apparelpin";

                        case " Card Holders":
                            return "wallet";

                        case " Key Rings":
                        case " Key Rings, Money Clips & Wallets":
                            return "keychain";

                        case " Money Clips":
                            return "accessory";

                        case " Miscellaneous":
                            return "jewelryfinding";

                        case "Watch":
                        case " Children's":
                        case " Wrist":
                            return "watches";

                        case " Eyebrow Jewelry":
                        case " Labrets":
                        case " Naval Jewelry":
                        case " Nostril Jewelry":
                            return "piercingjewelry";

                        case "":
                        case "Jewelry":
                        case "Gift":
                        case " Mountings":
                            if (categories.Contains("Bracelets", StringComparison.OrdinalIgnoreCase)
                                || categories.Contains("Bangle", StringComparison.OrdinalIgnoreCase)) return "bracelet";
                            if (categories.Contains("Earrings", StringComparison.OrdinalIgnoreCase)) return "earring";
                            if (categories.Contains("Pendants", StringComparison.OrdinalIgnoreCase)) return "charm";
                            if (categories.Contains("Necklaces", StringComparison.OrdinalIgnoreCase)) return "necklace";
                            if (categories.Contains("Rings", StringComparison.OrdinalIgnoreCase)) return "ring";
                            if (categories.Contains("Cuff Links", StringComparison.OrdinalIgnoreCase)) return "cufflink";
                            break;
                    }
                    return "fineother";
                }
                return "fineother";
            }
            return string.Empty;
        }

        public static string GetValueExternalProductIdType(string upc)
        {
            if (!string.IsNullOrEmpty(upc))
            {
                return "UPC";
            }

            return string.Empty;
        }

        public static string GenerateItemName(string description, string item)
        {
            string name = $"{description} Q{item}";

            return name.Replace("  ", " ");
        }

        public static string GetValueLengthNumeric(string length, string item_Type, string categories, bool OUM)
        {
            string productType = GetFeedProductType(item_Type, categories);

            if (productType.Equals("bracelet", StringComparison.InvariantCultureIgnoreCase))
            {
                if (OUM)
                {
                    return "Inches";
                }
                else
                {

                    return string.IsNullOrWhiteSpace(length) ? "10" : length;
                }
            }
            return string.Empty;
        }

        public static string GetValuePrice(string orgPrice)
        {
            try
            {
                decimal
                margin = 1.1m, // profit margin of all investment    
                ship1 = 0, // ship from qgold to VS
                ship2 = 3, // ship from VS to customer 
                operation_cost_per_item = 0, // email, crm, software ...
                marketing_per_item = 0; // according to initial research, it cost $12.00 per sale in PPC  

                if (decimal.Parse(orgPrice) < 100)
                {
                    margin = (decimal)1.3;
                }
                else if (300 >= decimal.Parse(orgPrice) && decimal.Parse(orgPrice) >= 200)
                {
                    margin = (decimal)1.2;
                }

                decimal total_fix_cost_per_sale = decimal.Parse(orgPrice) + ship1 + ship2 + marketing_per_item + operation_cost_per_item;

                decimal price = total_fix_cost_per_sale * margin;

                decimal amazon_fee_percentage = 0.2m;
                price /= (1 - amazon_fee_percentage);

                return (decimal.Round(price, 2)).ToString();

            }
            catch (Exception)
            {
                Common.WriteLog("GetValuePrice Failed");

                return null;
            }
        }

        public static string GetValueGender(string listOfSpecsText)
        {
            Dictionary<string, string> listOfSpec = GetValueListOfSpecsText(listOfSpecsText);
            if (listOfSpec == null) return string.Empty;
            if (listOfSpec.ContainsKey("Gender"))
            {
                if (listOfSpec["Gender"] == "Unisex") return "Unisex";
                if (listOfSpec["Gender"] == "Women's") return "Female";
                if (listOfSpec["Gender"] == "Men's") return "Male";
            }
            return "Unisex";
        }

        public static string GetItemType(string item_Type, string categories, string listOfSpecsText, string description)
        {
            string productType = GetFeedProductType(item_Type, categories);
            Dictionary<string, string> listOfSpec = GetValueListOfSpecsText(listOfSpecsText);

            if (productType == "coinpursepouch")
            {
                return "coin-purses";
            }

            if (productType == "ring")
            {
                if (categories.Contains("Wedding Bands", StringComparison.OrdinalIgnoreCase))
                {
                    return "wedding-bands";
                }

                if (categories.Contains("Engagement Rings", StringComparison.OrdinalIgnoreCase))
                {
                    return "engagement-rings";
                }

                if (categories.Contains("Stackable", StringComparison.OrdinalIgnoreCase))
                {
                    return "stackable-rings";
                }

                if (categories.Contains("Anniversary Rings", StringComparison.OrdinalIgnoreCase))
                {
                    return "anniversary-rings";
                }

                return "rings";
            }

            if (productType == "cufflink")
            {
                return "cuff-links";
            }

            if (productType == "earring")
            {
                if (listOfSpec.ContainsKey("Earring Type"))
                {
                    if (listOfSpec["Earring Type"] == "Ball" ||
                        listOfSpec["Earring Type"] == "Button") return "ball-earrings";
                    if (listOfSpec["Earring Type"] == "Drop & Dangle") return "dangle-earrings";
                    if (listOfSpec["Earring Type"] == "Hoop") return "hoop-earrings";
                    if (listOfSpec["Earring Type"] == "Stud") return "stud-earrings";
                    if (listOfSpec["Earring Type"] == "Earring Jackets & Enhancers") return "earring-jackets";
                }
                return "earrings";
            }

            if (productType == "keychain")
            {
                return "key-chains";
            }

            if (productType == "piercingjewelry")
            {
                if (description.Contains("Ring"))
                {
                    return "body-piercing-rings";
                }

                if (description.Contains("Barbell"))
                {
                    return "body-piercing-barbells";
                }

                if (description.Contains("Screwback"))
                {
                    return "body-piercing-screws";
                }

                return "body-piercing-studs";
            }

            if (productType == "watches")
            {
                if (categories.Contains("Pocket Watches", StringComparison.OrdinalIgnoreCase))
                {
                    return "pocket-watches";
                }
                return "watches";
            }

            if (productType == "jewelryfinding")
            {
                return "jewelry-clasps";
            }

            if (productType == "wallet")
            {
                if (categories.Contains("Card Holders", StringComparison.OrdinalIgnoreCase))
                {
                    return "credit-card-holders";
                }
                return "wallets";
            }

            if (productType == "bracelet")
            {
                if (categories.Contains("Charm Bracelets", StringComparison.OrdinalIgnoreCase))
                {
                    return "charm-bracelets";
                }

                if (categories.Contains("Link Bracelets", StringComparison.OrdinalIgnoreCase))
                {
                    return "link-bracelets";
                }

                if (categories.Contains("Cords and Leather", StringComparison.OrdinalIgnoreCase)
                    || categories.Contains("Cords & Leather", StringComparison.OrdinalIgnoreCase)
                    || categories.Contains("Cord, Leather and Rubber", StringComparison.OrdinalIgnoreCase))
                {
                    return "strand-bracelets";
                }

                if (categories.Contains("Tennis Bracelets", StringComparison.OrdinalIgnoreCase)
                    || categories.Contains("Tennis Bracelet Jackets", StringComparison.OrdinalIgnoreCase))
                {
                    return "tennis-bracelets";
                }

                if (categories.Contains("Wrap Bracelets", StringComparison.OrdinalIgnoreCase))
                {
                    return "wrap-bracelets";
                }

                if (categories.Contains("Cuff Bracelets", StringComparison.OrdinalIgnoreCase))
                {
                    return "cuff-bracelets";
                }

                if (item_Type.Contains("Anklets", StringComparison.OrdinalIgnoreCase) ||
                    item_Type.Contains("Anlets", StringComparison.OrdinalIgnoreCase))
                {
                    return "anklets";
                }

                return "bracelets";
            }

            if (productType == "necklace")
            {
                if (categories.Contains("Chains"))
                {
                    return "chain-necklaces";
                }

                if (categories.Contains("Chokers Collars and Wires"))
                {
                    return "choker-necklaces";
                }

                if (categories.Contains("Cords with Pendants"))
                {
                    return "pendant-necklaces";
                }

                if (categories.Contains("Drop"))
                {
                    return "y-shaped-necklaces";
                }

                if (categories.Contains("Pearls"))
                {
                    return "pearl-strands";
                }

                if (categories.Contains("Lockets"))
                {
                    return "locket-necklaces";
                }

                return "necklaces";
            }

            if (productType == "jewelryset")
            {
                return "jewelry-sets";
            }

            if (productType == "charm")
            {
                if (listOfSpec.ContainsKey("Pendant/Charm Type"))
                {
                    return "pendant-necklaces";
                }
                return "pendants";
            }

            if (productType == "apparelpin")
            {
                if (item_Type.Contains("Pin & Hair Accessories", StringComparison.OrdinalIgnoreCase) ||
                    item_Type.Contains("Pins & Hair Accessories", StringComparison.OrdinalIgnoreCase))
                {
                    return "brooches-and-pins";
                }
                return "tie-pins";
            }

            if (productType == "fineother")
            {
                return "faux-body-piercing-jewelry";
            }

            if (productType == "accessory")
            {
                return "money-clips";
            }

            return string.Empty;
        }

        public static string GetMetalType(string metal_Desc, string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "guildjewelry" || productType == "fineother")
            {
                if (qgoldDataDictionary.MetalTypesDictionary2.ContainsKey(metal_Desc))
                {
                    return qgoldDataDictionary.MetalTypesDictionary2[metal_Desc];
                }
                return "no-metal-type";
            }
            else if (productType == "earring" || productType == "bracelet" || productType == "necklace"
                || productType == "watchband" || productType == "piercingjewelry")
            {
                if (qgoldDataDictionary.MetalTypesDictionary3.ContainsKey(metal_Desc))
                {
                    return qgoldDataDictionary.MetalTypesDictionary3[metal_Desc];
                }
                else if (productType == "earring")
                {
                    return "No Metal";
                }
                else if (metal_Desc.Contains("Tri-Color"))
                {
                    return "Yellow Gold";
                }
                return "Yellow Gold";
            }
            else
            {
                if (qgoldDataDictionary.MetalTypesDictionary1.ContainsKey(metal_Desc))
                {
                    return qgoldDataDictionary.MetalTypesDictionary1[metal_Desc];
                }
            }

            return "No Metal Type";
        }

        public static string GetValueMaterial(string item_Type, string categories, string listOfSpecsText)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "coinpursepouch" || productType == "apparelpin" || productType == "charm" || productType == "cufflink"
                || productType == "bracelet" || productType == "jewelryset" || productType == "piercingjewelry")
            {
                return "Metal";
            }

            if (productType == "fineother")
            {
                return "metal";
            }

            if (productType == "keychain")
            {
                return "Titanium";
            }

            if (productType == "ring" || productType == "Jewelry" || productType == "charm")
            {
                return "Precious Metal";
            }

            if (productType == "earring" || productType == "necklace")
            {
                Dictionary<string, string> listOfSpec = GetValueListOfSpecsText(listOfSpecsText);

                if (listOfSpec.ContainsKey(" Primary"))
                {

                    if (listOfSpec[" Primary"] == "Gold" || listOfSpec[" Primary"] == "Rose (Authentic)"
                        || listOfSpec[" Primary"] == "Metal" || listOfSpec[" Primary"] == "Leaf (Authentic)" || listOfSpec[" Primary"] == "Base Metal"
                        || listOfSpec[" Primary"] == "Flower (Authentic)")
                    {
                        if (productType == "earring")
                        {
                            return "Yellow Gold";
                        }

                        if (productType == "necklace")
                        {
                            return "Gold";
                        }
                    }
                    if (listOfSpec[" Primary"] == "Brass/White") return "Brass";

                    return listOfSpec[" Primary"];
                }


                if (listOfSpec.ContainsKey("Primary"))
                {
                    if (listOfSpec["Primary"] == "Gold" || listOfSpec["Primary"] == "Rose (Authentic)"
                        || listOfSpec["Primary"] == "Metal" || listOfSpec["Primary"] == "Leaf (Authentic)" || listOfSpec["Primary"] == "Base Metal"
                        || listOfSpec["Primary"] == "Flower (Authentic)")
                    {
                        if (productType == "earring")
                        {
                            return "Yellow Gold";
                        }

                        if (productType == "necklace")
                        {
                            return "Gold";
                        }
                    }
                    if (listOfSpec["Primary"] == "Brass/White") return "Brass";

                    return listOfSpec["Primary"];
                }

                if (productType == "earring")
                {
                    return "Yellow Gold";
                }

                if (productType == "necklace")
                {
                    return "Gold";
                }
            }

            if (productType == "watches" || productType == "accessory" || productType == "jewelryfinding")
            {
                return "None";
            }

            if (productType == "wallet")
            {
                return "Polyester";
            }

            return string.Empty;
        }

        public static string GetValueDepartmentName(string item_Type, string categories, string listOfSpecsText)
        {
            string productType = GetFeedProductType(item_Type, categories);
            Dictionary<string, string> listOfSpec = GetValueListOfSpecsText(listOfSpecsText);

            if (productType == "fineother")
            {
                return "unisex-adult";
            }

            if (productType == "watches")
            {
                return "Unisex-adult";
            }

            if (listOfSpec.ContainsKey("Gender"))
            {
                if (listOfSpec["Gender"] == "Unisex")
                {
                    if (productType == "fineother")
                    {
                        return "unisex-adult";
                    }
                    if (productType == "watches")
                    {
                        return "Unisex-adult";
                    }
                    return "Unisex Adult";
                }
                if (listOfSpec["Gender"] == "Women's")
                {
                    if (productType == "fineother")
                    {
                        return "womens";
                    }
                    if (productType == "watches")
                    {
                        return "Womens";
                    }
                    return "Women";
                }

                if (listOfSpec["Gender"] == "Men's")
                {
                    if (productType == "fineother")
                    {
                        return "mens";
                    }
                    if (productType == "watches")
                    {
                        return "Mens";
                    }
                    return "Men";
                }
            }
            return "Unisex Adult";
        }

        public static string GetGemType(string item_Type, string categories, string listOfSpecsText, int gemStoneNumber)
        {
            string gemtype = string.Empty;
            string productType = GetFeedProductType(item_Type, categories);
            List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            if (productType != "fineother")
            {
                GemStones.ForEach(item => item.GemType = item.GemType.Replace("-", " "));
            }

            var gemstone = GemStones.Select(p => p.GemType).ToList();
            //if (gemStoneNumber == 0)
            //{
            //    string stoneType = string.Join(", ", gemstone);
            //    gemtype = textInfo.ToTitleCase(stoneType.ToLower());

            //    return gemtype == "not known" ? "" : gemtype;
            //}
            if (gemStoneNumber == 1)
            {
                if (productType == "necklace"
                    || productType == "bracelet"
                    || productType == "charm"
                    || productType == "ring"
                    || productType == "earring"
                    || productType == "apparelpin"
                    || productType == "watches"
                    )
                {
                    try
                    {
                        if (gemstone[0].ToLower() == "zircon gemstone") return "Zircon";
                        if (gemstone[0].ToLower() == "white topaz" || gemstone[0].ToLower() == "london blue topaz") return "Topaz";
                        if (gemstone[0].ToLower() == "prasiolite quartz" || gemstone[0].ToLower() == "smoky quartz") return "Quartz";
                        if (gemstone[0].ToLower() == "rhodolite garnet") return "Garnet";
                    }
                    catch
                    {
                    }
                    return "No Gemstone";
                }
                if (gemstone.Count > 1)
                {
                    if (gemstone[0].ToLower() == "not known") return "";
                    if (gemstone[0].ToLower() == "mystic topaz" || gemstone[0].ToLower() == "swiss blue topaz") return "Topaz";
                }
            }
            if (gemStoneNumber == 2)
            {
                if (gemstone.Count > 1)
                {
                    if (gemstone[1].ToLower() == "not known") return "";
                    if (gemstone[1].ToLower() == "mystic topaz" || gemstone[1].ToLower() == "swiss blue topaz") return "Topaz";
                    if (productType == "necklace")
                    {
                        if (gemstone[1].ToLower() == "zircon gemstone") return "Zircon";
                        if (gemstone[1].ToLower() == "white topaz" || gemstone[1].ToLower() == "london blue topaz") return "Topaz";
                        if (gemstone[1].ToLower() == "prasiolite quartz" || gemstone[1].ToLower() == "smoky quartz") return "Quartz";
                        if (gemstone[1].ToLower() == "rhodolite garnet") return "Garnet";
                        if (gemstone[1].ToLower() == "spinel" || gemstone[1].ToLower() == "rose de france" || gemstone[1].ToLower() == "tourmaline")
                            return "";
                    }
                }
            }
            if (gemStoneNumber == 3)
            {
                if (gemstone.Count > 2)
                {
                    if (gemstone[2].ToLower() == "not known") return "";
                    if (gemstone[2].ToLower() == "mystic topaz" || gemstone[2].ToLower() == "swiss blue topaz") return "Topaz";
                    if (productType == "necklace")
                    {
                        if (gemstone[2].ToLower() == "zircon gemstone") return "Zircon";
                        if (gemstone[2].ToLower() == "white topaz" || gemstone[2].ToLower() == "london blue topaz") return "Topaz";
                        if (gemstone[2].ToLower() == "prasiolite quartz" || gemstone[2].ToLower() == "smoky quartz") return "Quartz";
                        if (gemstone[2].ToLower() == "rhodolite garnet") return "Garnet";
                        if (gemstone[2].ToLower() == "spinel" || gemstone[2].ToLower() == "rose de france" || gemstone[2].ToLower() == "tourmaline")
                            return "";
                    }
                }
            }

            if (productType == "fineother")
            {
                for (int i = 0; i < gemstone.Count; i++)
                {
                    if (gemStoneNumber == (i + 1))
                    {
                        gemtype = gemstone[i].ToLower();
                        return gemtype == "not known" ? "" : gemtype;
                    }
                }
            }
            else
            {
                for (int i = 0; i < gemstone.Count; i++)
                {
                    if (gemStoneNumber == (i + 1))
                    {
                        gemtype = textInfo.ToTitleCase(gemstone[i].ToLower());
                        return gemtype == "not known" ? "" : gemtype;
                    }
                }
            }
            return "";
        }

        public static string GetValueColor(string listOfSpecsText, string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);

            if (productType == "jewelryfinding") return "yellow";

            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);
            if (listOfSpecs.ContainsKey(" Primary - Color"))
            {
                if (listOfSpecs[" Primary - Color"] == "Gold Tone" || listOfSpecs[" Primary - Color"] == "Copper Tone"
                    || listOfSpecs[" Primary - Color"] == "Brass Tone" || listOfSpecs[" Primary - Color"] == "Champagne")
                    return "Yellow";

                if (listOfSpecs[" Primary - Color"] == "Silver Tone" || listOfSpecs[" Primary - Color"] == "Gunmetal"
                    || listOfSpecs[" Primary - Color"] == "Pewter Tone" || listOfSpecs[" Primary - Color"] == "Pewter")
                    return "Grey";

                if (listOfSpecs[" Primary - Color"] == "Cherry" || listOfSpecs[" Primary - Color"] == "Maroon")
                    return "Red";

                if (listOfSpecs[" Primary - Color"] == "Rose" || listOfSpecs[" Primary - Color"] == "Rose Tone")
                    return "Pink";

                if (listOfSpecs[" Primary - Color"] == "Mahogany" || listOfSpecs[" Primary - Color"] == "Bronze Tone"
                    || listOfSpecs[" Primary - Color"] == "Oak" || listOfSpecs[" Primary - Color"] == "Walnut")
                    return "Brown";

                if (listOfSpecs[" Primary - Color"] == "Navy Blue")
                    return "Blue";

                if (listOfSpecs[" Primary - Color"] == "Cream")
                    return "White";

                if (listOfSpecs[" Primary - Color"] == "Ebony")
                    return "Green";

                if (listOfSpecs[" Primary - Color"] == "Two-Tone" ||
                    listOfSpecs[" Primary - Color"] == "Multi-colored" ||
                    listOfSpecs[" Primary - Color"] == "Tri-Color" ||
                    listOfSpecs[" Primary - Color"] == "Tan") return "Yellow";


                return listOfSpecs[" Primary - Color"];
            }
            return "Yellow";
        }

        public static string GetValueCurrency(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "watches")
            {
                return "usd";
            }
            return "USD";
        }

        public static string GetValueAgeRangeDescription(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);
            return productType switch
            {
                "watches"
                or "wallet"
                or "keychain"
                or "coinpursepouch"
                or "acessory"
                or "bracelet"
                or "necklace"
                or "ring"
                => "Adult",
                _ => string.Empty,
            };
        }

        public static string GetValueWatchMovementType(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "watches")
            {
                return "Automatic";
            }
            return string.Empty;
        }

        public static string GetValueTargetAudience(string item_Type, string categories, int targetAudienceNumber)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "watches")
            {
                if (targetAudienceNumber == 1) return "boys";
                if (targetAudienceNumber == 2) return "girls";
                if (targetAudienceNumber == 3) return "men";
                if (targetAudienceNumber == 4) return "unisex-adult";
                if (targetAudienceNumber == 5) return "women";
            }

            if (productType == "jewelryfinding")
            {
                if (targetAudienceNumber == 1) return "Women";
                if (targetAudienceNumber == 2) return "Men";
                if (targetAudienceNumber == 3) return "Boys";
                if (targetAudienceNumber == 4) return "Unisex Adult";
                if (targetAudienceNumber == 5) return "Girls";
            }

            if (productType == "fashionearring"
                || productType == "fashionnecklacebraceletanklet"
                || productType == "finering"
                || productType == "fineearring"
                || productType == "fashionother"
                || productType == "fashionring"
                || productType == "fineother"
                || productType == "finenecklacebraceletanklet"
                || productType == "ring"
                || productType == "charm"
                )
            {
                if (targetAudienceNumber == 1) return "Adults";
                if (targetAudienceNumber == 2) return "Teens";
                if (targetAudienceNumber == 3) return "Men";
                if (targetAudienceNumber == 4) return "People";
                if (targetAudienceNumber == 5) return "Seniors";
            }


            return string.Empty;
        }

        public static string GetValueDisplayType(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "watches")
            {
                return "analog-digital";
            }
            return string.Empty;
        }

        public static string GetValueClosure(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "wallet" || productType == "keychain" || productType == "coinpursepouch" || productType == "necktie")
            {
                return "No Closure";
            }
            if (productType == "piercingjewelry")
            {
                return "Push Back";
            }
            if (productType == "apparel" || productType == "apparelbelt")
            {
                return "Hook and Eye";
            }
            if (productType == "charm" || productType == "necklace")
            {
                return "None";
            }
            return null;
        }

        public static string GetProductDescription(string description, string size, string listOfSpecsText)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(description);
            sb.Append("</br>");
            if (!string.IsNullOrEmpty(size))
            {
                sb.Append("</br>Size: " + size);
            }

            if (GetLengthInListOfSpecs(listOfSpecsText, false) != null)
                sb.Append("</br>Length: " + GetLengthInListOfSpecs(listOfSpecsText, false) + " " + GetLengthInListOfSpecs(listOfSpecsText, true));

            if (GetValueListOfSpecsText(listOfSpecsText) != null)
            {
                foreach (var spec in GetValueListOfSpecsText(listOfSpecsText))
                {
                    if (sb.Length < 1850)
                    {
                        sb.Append("</br>" + spec.Key + ": " + spec.Value);
                    }
                }
            }

            sb.Append("</br></br>Customized items can not be returned</br>");
            return sb.ToString().Replace("_", " ");
        }

        public static string GetValueBulletPoint(string listOfSpecsText, int bulletPointNumber)
        {
            var str = string.Empty;
            if (bulletPointNumber == 1)
                str = SetBulletPoint(listOfSpecsText, bulletPointNumber);

            if (bulletPointNumber == 2)
                str = SetBulletPoint(listOfSpecsText, bulletPointNumber);

            return str.Replace("_", " ");
        }

        public static string GetValueItemTypeName(string item_Type, string categories, string listOfSpecsText, string description)
        {
            string productType = GetFeedProductType(item_Type, categories);
            string itemTypeName = GetItemType(item_Type, categories, listOfSpecsText, description);

            if (productType == "earring")
            {
                if (itemTypeName == "ball-earrings") return "Ball Earrings";
                if (itemTypeName == "hoop-earrings") return "Hoop Earrings";
                if (itemTypeName == "stud-earrings") return "Stud Earrings";
                if (itemTypeName == "dangle-earrings") return "Dangle Earrings";
                if (itemTypeName == "earring-jackets") return "Earring Jackets";
                return "Earrings";
            }

            if (productType == "watches")
            {
                if (itemTypeName == "pocket-watches") return "Pocket Watch";
                return "Watch";
            }

            if (productType == "apparelbelt")
            {
                return "ID Bracelet";
            }

            if (productType == "wallet")
            {
                if (itemTypeName == "credit-card-holders") return "Travel Accessory- Envelope Card Holder";
                return "Travel Accessory- Bi-Fold Wallet";
            }

            if (productType == "keychain")
            {
                return "Key Chain";
            }

            if (productType == "coinpursepouch")
            {
                return "Billfold";
            }

            if (productType == "apparelbelt")
            {
                return "Coin Purse";
            }

            if (productType == "watchband")
            {
                return "Bracelets";
            }

            if (productType == "ring" || productType == "charm")
            {
                if (itemTypeName == "engagement-rings") return "Engagement Rings";
                if (itemTypeName == "stackable-rings") return "Stackable Rings";
                if (itemTypeName == "wedding-bands") return "Wedding Bands";
                if (itemTypeName == "anniversary-rings") return "Anniversary Rings";
                return "Rings";
            }

            if (productType == "bracelet")
            {
                if (itemTypeName == "anklets") return "Anklets";
                if (itemTypeName == "charm-bracelets") return "Charm Bracelets";
                if (itemTypeName == "link-bracelets") return "Link Bracelets";
                if (itemTypeName == "strand-bracelets") return "Strand Bracelets";
                if (itemTypeName == "tennis-bracelets") return "Tennis Bracelets";
                if (itemTypeName == "wrap-bracelets") return "Wrap Bracelets";
                if (itemTypeName == "cuff-bracelets") return "Cuff Bracelets";
                return "Bracelets";
            }

            if (productType == "necklace")
            {
                if (itemTypeName == "chain-necklaces") return "Chain Necklaces";
                if (itemTypeName == "pendant-necklaces") return "Pendant Necklaces";
                if (itemTypeName == "choker-necklaces") return "Choker Necklaces";
                if (itemTypeName == "y-shaped-necklaces") return "Y Shaped Necklaces";
                if (itemTypeName == "locket-necklaces") return "Locket Necklaces";
                if (itemTypeName == "pearl-strands") return "Pearl Strands";
                return "Necklaces";
            }

            return "";
        }

        public static string GetValueItemLengthOUM(string listOfSpecsText)
        {
            return GetLengthInListOfSpecs(listOfSpecsText, true);
        }

        public static string GetWidthInListOfSpecs(string listOfSpecsText, bool isUOM)
        {
            try
            {
                Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);
                if (listOfSpecs.ContainsKey("Width of Item"))
                {
                    var widthSplit = listOfSpecs["Width of Item"].Split(' ');
                    if (!isUOM)
                    {
                        if (widthSplit[0].Contains('-'))
                        {
                            var widthDash = widthSplit[0].Split("-");
                            return widthDash[0];
                        }
                        if (widthSplit[0].Contains("."))
                        {
                            return widthSplit[0];
                        }
                        return widthSplit[0];
                    }
                    else
                    {
                        if (widthSplit.Length == 2)
                        {
                            return widthSplit[1].ToUpper();
                        }
                        else if (widthSplit.Length == 1)
                        {
                            return "MM";
                        }
                        else if (widthSplit.Length == 5)
                        {
                            return widthSplit[3].ToUpper();
                        }
                        else
                        {
                            return "MM";
                        }
                    }
                }
                else if (listOfSpecs.ContainsKey("Ring Top Width"))
                {
                    var widthSplit = listOfSpecs["Ring Top Width"].Split(' ');
                    if (!isUOM)
                    {
                        if (widthSplit[0].Contains('-'))
                        {
                            var widthDash = widthSplit[0].Split("-");
                            return widthDash[0];
                        }
                        if (widthSplit[0].Contains("."))
                        {
                            return widthSplit[0];
                        }
                        return widthSplit[0];
                    }
                    else
                    {
                        if (widthSplit.Length == 2)
                        {
                            return widthSplit[1].ToUpper();
                        }
                        else if (widthSplit.Length == 1)
                        {
                            return "MM";
                        }
                        else if (widthSplit.Length == 5)
                        {
                            return widthSplit[3].ToUpper();
                        }
                        else
                        {
                            return "MM";
                        }
                    }
                }
                return "";
            }
            catch (Exception)
            {

                if (!isUOM)
                {
                    return "5";
                }
                else
                {
                    return "MM";
                }
            }

        }

        public static string GetValueCountryofOrigin(string CountryofOrigin)
        {
            if (!string.IsNullOrEmpty(CountryofOrigin))
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

                if (textInfo.ToTitleCase(CountryofOrigin.ToLower()) == "Myanmar/Burma") return "Burma (Myanmar)";
                return textInfo.ToTitleCase(CountryofOrigin.ToLower());
            }
            return "United States";
        }

        public static string GetValueNumberOfItem(string item_Type, string categories, string listOfSpecsText)
        {
            string productType = GetFeedProductType(item_Type, categories);
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);

            if (productType == "jewelryset")
                if (listOfSpecs.ContainsKey("Items per Pack"))
                {
                    return listOfSpecs["Items per Pack"];
                }


            return "1";
        }

        public static string GetValueSize(string item_Type, string categories, string size)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (!string.IsNullOrEmpty(size))
            {
                if (decimal.TryParse(size, out decimal number))
                {
                    string formattedNumber = number.ToString("N2");
                    if (formattedNumber.EndsWith(".50"))
                    {
                        return formattedNumber.Substring(0, formattedNumber.Length - 1);
                    }
                    else if (formattedNumber.EndsWith(".00"))
                    {
                        return formattedNumber.Substring(0, formattedNumber.Length - 3);
                    }

                    if (number.ToString("N2") == "2.25" || number.ToString("N2") == "1.75") return "2";
                    return number.ToString("N2");
                }
            }

            if (productType == "fineother")
            {
                return "Adjustable";
            }
            return string.Empty;
        }

        public static string GetSettingType(string item_Type, string categories, string listOfSpecsText)
        {
            string productType = GetFeedProductType(item_Type, categories);
            List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
            var qgoldSetting = (GemStones == null || GemStones.Count == 0 ? "" : GemStones[0].Setting) ?? "";

            if (productType == "charm" || productType == "earring")
            {
                return null;
            }

            if (productType == "earring")
            {
                if (qgoldDataDictionary.SettingTypesDictionary1.ContainsKey(qgoldSetting))
                    return qgoldDataDictionary.SettingTypesDictionary1[qgoldSetting];
                return null;
            }
            else if (productType == "bracelet" || productType == "necklace")
            {
                if (qgoldDataDictionary.SettingTypesDictionary3.ContainsKey(qgoldSetting))
                    return qgoldDataDictionary.SettingTypesDictionary3[qgoldSetting];
                return "Wrap Setting";
            }
            else if (productType == "fineother")
            {
                if (qgoldDataDictionary.SettingTypesDictionary4.ContainsKey(qgoldSetting))
                    return qgoldDataDictionary.SettingTypesDictionary4[qgoldSetting];
                return "Other-Setting-Type";
            }
            else
            {
                if (qgoldDataDictionary.SettingTypesDictionary2.ContainsKey(qgoldSetting))
                    return qgoldDataDictionary.SettingTypesDictionary2[qgoldSetting];
                return "";
            }
        }

        public static string GetValueStyleName(string item_Type, string categories, string listOfSpecsText, string description)
        {
            string productType = GetFeedProductType(item_Type, categories);
            string itemTypeName = GetItemType(item_Type, categories, listOfSpecsText, description);
            if (productType == "earring")
            {
                if (itemTypeName == "hoop-earrings") return "Hoop";
                if (itemTypeName == "stud-earrings") return "Stud";
                if (itemTypeName == "dangle-earrings") return "Dangle";
                if (itemTypeName == "earring-jackets") return "Jacket";
                return "Stud";
            }
            else if (productType == "apparelpin" || productType == "jewelryset" || productType == "wallet" || productType == "keychain" ||
                productType == "jewelryfinding" || productType == "jewelrystorage")
            {
                return "Modern";
            }
            else if (productType == "apparelbelt")
            {
                return "Classic";
            }
            else if (productType == "watchband")
            {
                return "Vintage";
            }

            return "None";
        }

        public static string GetValueCareInstruction(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "keychain" || productType == "necktie" || productType == "apparel" || productType == "apparelbelt")
            {
                return "Dry Clean Only";
            }
            if (productType == "charm" || productType == "necklace" || productType == "piercingjewelry")
            {
                return "None";
            }
            return null;
        }

        public static string GetValueImportDesignation(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "watches" || productType == "fineother" || productType == "accessory")
            {
                return "Made in USA";
            }
            if (productType == "keychain" || productType == "necktie" || productType == "apparel" || productType == "apparelbelt")
            {
                return "Made in the USA";
            }
            return null;
        }

        public static string GetValueOuterMaterialType(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "wallet")
            {
                return "Hardside";
            }
            if (productType == "coinpursepouch")
            {
                return "Aluminum";
            }
            if (productType == "charm" || productType == "necklace")
            {
                return "None";
            }
            return null;
        }

        #endregion

        #region Images
        public static string GetImage(string linkImage, string sku, int index)
        {
            if (!string.IsNullOrEmpty(linkImage))
            {
                linkImage = linkImage.Replace(" ", "");
                return linkImage;
                //Uri uri = new Uri(linkImage);

                //string fullPath = uri.PathAndQuery;

                //string fileName = $"{index}{Path.GetExtension(fullPath).Split("?")[0]}";

                //string newUrl = Path.Combine(sku, fileName);

                //var result = _blobStorage.UploadWithLink(linkImage, newUrl).Result;

                //if (result.IsSuccess)
                //{
                //    return result.Message;
                //}
                //else
                //{
                //    Common.WriteLog($"SKU: {sku}.Can not upload img link:{linkImage}. Err: {result.Message} ");
                //}
            }
            return null;
        }
        #endregion

        #region Variation
        public static string GetValueRelationshipType(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "watches")
            {
                return "Variation";
            }
            return "Accessory";
        }

        public static string GetValueVariationTheme(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);

            if (productType == "ring" || productType == "charm") return "Size";
            if (productType == "cufflink" || productType == "earring" || productType == "piercingjewelry"
                || productType == "charm") return "Metal Type";

            return string.Empty;
        }

        #endregion

        #region Discovery
        public static string GetValueStoneColor(string listOfSpecsText, string item_Type, string categories, int stone_colorNumber)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "ring"
                || productType == "charm"
                || productType == "bracelet"
                || productType == "earring"
                || productType == "necklace"
                )
            {
                return string.Empty;
            }
            List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
            if (GemStones != null && GemStones.Count > 0)
            {
                var colors = GemStones.Select(p => p.Color).ToList();

                if (colors == null || colors.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    string color = string.Empty;

                    if (stone_colorNumber == 0)
                    {
                        color = string.Join(", ", colors);
                    }
                    else
                    {

                        if (stone_colorNumber == 1) color = colors[0];
                        if (stone_colorNumber == 2)
                            if (colors.Count > 1) color = colors[1];
                        if (stone_colorNumber == 3)
                            if (colors.Count > 2) color = colors[2];

                        if (color == "Purple" || color == "Grey" || color == "Phenomena" || color == "Tan" || color == "Zebra") color = "Other Color";
                        if (color == "Multi-color") color = "Multiple Colors";
                        if (color == "Rose") color = "Pink";
                    }

                    if (color == null) return null;
                    if (productType == "fineother")
                    {
                        if (color == "Other Color") return "other-color";
                        if (color == "Multiple Colors") return "multiple-colors";
                        return color.ToLower();
                    }
                    return color;
                }
            }
            return null;
        }

        public static string GetValueOccasion(string item_Type, string categories, int occasionNumber)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "charm" || productType == "keychain")
            {
                if (occasionNumber == 1) return "Wedding";
                if (occasionNumber == 2) return "Anniversary";
                if (occasionNumber == 3) return "Valentines Day";
                if (occasionNumber == 4) return "Birthday";
                if (occasionNumber == 5) return "Mothers Day";
            }

            if (productType == "ring" || productType == "charm")
            {
                if (occasionNumber == 1) return "Engagement";
                if (occasionNumber == 2) return "Anniversary";
                if (occasionNumber == 3) return "Valentines Day";
                if (occasionNumber == 4) return "Wedding";
                if (occasionNumber == 5) return "Mothers Day";
            }

            if (productType == "bracelet")
            {
                if (occasionNumber == 1) return "Black Friday";
                if (occasionNumber == 2) return "Anniversary";
                if (occasionNumber == 3) return "Valentines Day";
                if (occasionNumber == 4) return "Wedding";
                if (occasionNumber == 5) return "Mothers Day";
            }

            return "Anniversary";
        }

        public static string GetValueClaspType(string item_Type, string categories, string listOfSpecsText)
        {
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);
            string productType = GetFeedProductType(item_Type, categories);
            if (listOfSpecs.ContainsKey("Clasp /Connector"))
            {
                if (productType == "bracelet" || productType == "necklace" || productType == "jewelryset")
                {
                    switch (listOfSpecs["Clasp /Connector"])
                    {
                        case "Lobster":
                            if (productType == "bracelet") return "Lobster Claw";
                            if (productType == "necklace") return "Lobster";
                            if (productType == "watches") return "lobster-claw-clasp";
                            if (productType == "fineother") return "lobster-claw";
                            return "Lobster Clasp";

                        case "Spring Ring":
                            if (productType == "jewelryset") return "Spring Ring Clasp";
                            if (productType == "fineother") return "spring-ring";
                            if (productType == "watches") return "spring-ring-clasp";
                            return "Spring Ring";

                        case "Toggle":
                            if (productType == "jewelryset") return "Toggle Clasp";
                            if (productType == "fineother") return "toggle";
                            if (productType == "watches") return "toggle-clasp";
                            return "Toggle";

                        case "Magnetic Clasp":
                            if (productType == "jewelryset") return "Magnetic Clasp";
                            if (productType == "fineother") return "magnetic";
                            return "Magnetic";

                        case "Fold Over Catch":
                            if (productType == "jewelryset") return "Fold Over Clasp";
                            if (productType == "fineother") return "fold-over";
                            if (productType == "watches") return "fold-over-clasp";
                            return "Fold Over";
                    }
                    return string.Empty;
                }

            }
            return null;
        }

        public static string GetValueStoneId(string itemType, string listOfSpecsText, string categories)
        {
            //string productType = GetFeedProductType(itemType, categories);
            //if (productType == "ring" || productType == "charm")
            //{
            //    return string.Empty;
            //}
            //List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
            //if (GemStones != null && GemStones.Count > 0)
            //{
            //    return "1";
            //    //string stoneId = "";
            //    //for (int i = 0; i < GemStones.Count; i++)
            //    //{
            //    //    if (i == 0)
            //    //    {
            //    //        stoneId = stoneId + (i + 1);
            //    //    }
            //    //    else
            //    //    {
            //    //        stoneId = stoneId + ", " + (i + 1);
            //    //    }
            //    //}
            //    //return stoneId;
            //}
            return null;
        }

        public static string GetValueStoneCreation(string item_Type, string categories, string listOfSpecsText, int stone_creationNumber)
        {
            List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "ring" || productType == "charm")
            {
                return null;
            }
            if (GemStones != null && GemStones.Count > 0)
            {
                var creation = GemStones.Select(p => p.CreationMethod).ToList();

                if (creation == null || creation.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    string creations = string.Empty;

                    if (stone_creationNumber == 0)
                    {
                        var creationString = creation.Where(b => !string.IsNullOrEmpty(b)).ToList();
                        return string.Join(", ", creationString);
                    }
                    else
                    {
                        for (int i = 0; i < creation.Count; i++)
                        {
                            if (creation[i] == null) return null;
                        }

                        if (stone_creationNumber == 1) if (creation[0].ToLower() == "cultured") return null;
                        if (stone_creationNumber == 2) if (creation.Count > 1) if (creation[1].ToLower() == "cultured") return null;
                        if (stone_creationNumber == 3) if (creation.Count > 2) if (creation[2].ToLower() == "cultured") return null;

                        if (productType == "ring" || productType == "charm")
                        {
                            if (stone_creationNumber == 1) return creation[0];
                            if (stone_creationNumber == 2) return creation.Count > 1 ? creation[1] : null;
                            if (stone_creationNumber == 3) return creation.Count > 2 ? creation[2] : null;
                        }
                        else if (productType == "fineother")
                        {
                            if (stone_creationNumber == 1) return creation[0].ToLower();
                            if (stone_creationNumber == 2) return creation.Count > 1 ? creation[1].ToLower() : null;
                            if (stone_creationNumber == 3) return creation.Count > 2 ? creation[2].ToLower() : null;
                        }
                    }

                }
            }
            return null;
        }

        public static string GetValueStoneShapes(string item_Type, string categories, string listOfSpecsText, int stone_shapeNumber)
        {
            if (stone_shapeNumber == 0)
            {
                return null;
            }
            List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "necklace")
            {
                var stoneTypeName = string.Empty;
                if (GemStones != null && GemStones.Count > 0)
                {
                    var shapes = GemStones.Select(p => p.Shape).ToList();

                    if (shapes == null || shapes.Count == 0)
                    {
                        if (stone_shapeNumber == 1) return "Marquise";
                    }
                    else
                    {
                        //if (stone_shapeNumber == 0)
                        //{
                        //    var shapesString = shapes.Where(b => !string.IsNullOrEmpty(b)).ToList();
                        //    return string.Join(", ", shapesString);
                        //}

                        if (stone_shapeNumber == 1)
                        {
                            if (shapes[0] == "Cushion") return "Cushion";
                            if (shapes[0] == "Octagon / Emerald" || shapes[0] == "Octagon") return "Octagon";
                            if (shapes[0] == "Near Round") return "Round";
                            if (shapes[0] == "Rice" || shapes[0] == "Teardrop" || shapes[0] == "Trillion"
                                || shapes[0] == "Button" || shapes[0] == "TBD" || shapes[0] == "Baroque") return "Marquise";
                            return "Marquise";
                        }
                        if (stone_shapeNumber == 2)
                        {
                            if (shapes.Count > 1)
                            {
                                if (shapes[1] == "Cushion") return "Cushion";
                                if (shapes[1] == "Octagon / Emerald" || shapes[1] == "Octagon") return "Octagon";
                                if (shapes[1] == "Near Round") return "Round";
                                if (shapes[1] == "Rice" || shapes[1] == "Teardrop" || shapes[1] == "Trillion"
                                    || shapes[1] == "Button" || shapes[1] == "TBD" || shapes[1] == "Baroque") return "Marquise";
                                stoneTypeName = shapes[1];
                            }
                        }
                        if (stone_shapeNumber == 3)
                        {
                            if (shapes.Count > 2)
                            {
                                if (shapes[2] == "Cushion") return "Cushion";
                                if (shapes[2] == "Near Round") return "Round";
                                if (shapes[2] == "Octagon / Emerald" || shapes[2] == "Octagon") return "Octagon";
                                if (shapes[2] == "Rice" || shapes[2] == "Teardrop" || shapes[2] == "Trillion"
                                 || shapes[2] == "Button" || shapes[2] == "TBD" || shapes[2] == "Baroque") return "Marquise";
                                stoneTypeName = shapes[2];
                            }
                        }

                    }
                }
                else
                {
                    if (stone_shapeNumber == 1)
                    {
                        return "Marquise";
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            if (productType == "jewelryset" || productType == "ring" || productType == "fineother")
            {
                return "";
            }

            if (productType == "bracelet" || productType == "necklace" || productType == "charm")
            {
                var stoneTypeName = string.Empty;
                if (GemStones != null && GemStones.Count > 0)
                {
                    var shapes = GemStones.Select(p => p.Shape).ToList();

                    if (shapes == null || shapes.Count == 0)
                    {
                        if (stone_shapeNumber == 1) return "Marquise";
                    }
                    else
                    {
                        //if (stone_shapeNumber == 0)
                        //{
                        //    var shapesString = shapes.Where(b => !string.IsNullOrEmpty(b)).ToList();
                        //    return string.Join(", ", shapesString);
                        //}

                        if (stone_shapeNumber == 1)
                        {
                            if (shapes[0] == "Cushion") return "Cushion";
                            if (shapes[0] == "Octagon / Emerald" || shapes[0] == "Octagon") return "Octagon";
                            if (shapes[0] == "Near Round") return "Round";
                            if (shapes[0] == "Rice" || shapes[0] == "Teardrop" || shapes[0] == "Trillion"
                                || shapes[0] == "Button" || shapes[0] == "TBD" || shapes[0] == "Baroque") return "Marquise";
                            stoneTypeName = shapes[0];
                        }
                        if (stone_shapeNumber == 2)
                        {
                            if (shapes.Count > 1)
                            {
                                if (shapes[1] == "Cushion") return "Cushion";
                                if (shapes[1] == "Octagon / Emerald" || shapes[1] == "Octagon") return "Octagon";
                                if (shapes[1] == "Near Round") return "Round";
                                if (shapes[1] == "Rice" || shapes[1] == "Teardrop" || shapes[1] == "Trillion"
                                    || shapes[1] == "Button" || shapes[1] == "TBD" || shapes[1] == "Baroque") return "Marquise";
                                stoneTypeName = shapes[1];
                            }
                        }
                        if (stone_shapeNumber == 3)
                        {
                            if (shapes.Count > 2)
                            {
                                if (shapes[2] == "Cushion") return "Cushion";
                                if (shapes[2] == "Near Round") return "Round";
                                if (shapes[2] == "Octagon / Emerald" || shapes[2] == "Octagon") return "Octagon";
                                if (shapes[2] == "Rice" || shapes[2] == "Teardrop" || shapes[2] == "Trillion"
                                 || shapes[2] == "Button" || shapes[2] == "TBD" || shapes[2] == "Baroque") return "Marquise";
                                stoneTypeName = shapes[2];
                            }
                        }

                    }
                }
                else
                {
                    if (stone_shapeNumber == 1)
                    {
                        return "Marquise";
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            if (productType == "ring" || productType == "cufflink" ||
                productType == "apparelpin" ||
                productType == "piercingjewelry")
            {
                if (GemStones != null && GemStones.Count > 0)
                {
                    var shapes = GemStones.Select(p => p.Shape).ToList();

                    if (shapes == null || shapes.Count == 0)
                    {
                        if (stone_shapeNumber == 1) return null;
                    }
                    else
                    {
                        if (stone_shapeNumber == 0)
                        {
                            var shapesString = shapes.Where(b => !string.IsNullOrEmpty(b)).ToList();
                            return string.Join(", ", shapesString);
                        }

                        if (stone_shapeNumber == 1)
                        {
                            if (shapes[0] == "Cushion") return "Cushion Cut";
                            if (shapes[0] == "Octagon / Emerald" || shapes[0] == "Octagon") return "Octagonal";
                            if (shapes[0] == "Near Round") return "Round";
                            if (shapes[0] == "Rice" || shapes[0] == "Teardrop" || shapes[0] == "Trillion"
                                || shapes[0] == "Button" || shapes[0] == "TBD" || shapes[0] == "Baroque") return "Other Shape";
                            return shapes[0];
                        }
                        if (stone_shapeNumber == 2)
                        {
                            if (shapes.Count > 1)
                            {
                                if (shapes[1] == "Cushion") return "Cushion Cut";
                                if (shapes[1] == "Octagon / Emerald" || shapes[1] == "Octagon") return "Octagonal";
                                if (shapes[1] == "Near Round") return "Round";
                                if (shapes[1] == "Rice" || shapes[1] == "Teardrop" || shapes[1] == "Trillion"
                                    || shapes[1] == "Button" || shapes[1] == "TBD" || shapes[1] == "Baroque") return "Other Shape";
                                return shapes[1];
                            }
                        }
                        if (stone_shapeNumber == 3)
                        {
                            if (shapes.Count > 2)
                            {
                                if (shapes[2] == "Cushion") return "Cushion Cut";
                                if (shapes[2] == "Near Round") return "Round";
                                if (shapes[2] == "Octagon / Emerald" || shapes[2] == "Octagon") return "Octagonal";
                                if (shapes[2] == "Rice" || shapes[2] == "Teardrop" || shapes[2] == "Trillion"
                                 || shapes[2] == "Button" || shapes[2] == "TBD" || shapes[2] == "Baroque") return "Other Shape";
                                return shapes[2];
                            }
                        }
                    }
                }
                else
                {
                    if (stone_shapeNumber == 1)
                    {
                        return "Other Shape";
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            if (productType == "fineother")
            {
                return "other-shape";
            }

            return null;
        }

        public static string GetValueStoneClarity(string item_Type, string categories, string listOfSpecsText, int stone_clarityNumber)
        {
            List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
            string productType = GetFeedProductType(item_Type, categories);

            if (productType == "ring"
                || productType == "necklace"
                || productType == "earring"
                || productType == "charm"
                || productType == "bracelet"
                )
            {
                return null;
            }
            if (GemStones != null && GemStones.Count > 0)
            {
                var clarity = GemStones.Select(p => p.Clarity).ToList();

                if (clarity == null || clarity.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    for (int i = 0; i < clarity.Count; i++)
                    {
                        if (clarity[i] == null)
                        {
                            clarity[i] = null;
                        }
                        else if (clarity[i].Contains("/"))
                        {
                            var splitClarity = clarity[i].Replace("/", "-");
                            if (splitClarity.Contains("(VS)"))
                            {
                                splitClarity = splitClarity.Replace(" (VS)", "");
                            }
                            else if (splitClarity.Contains("(AAA)"))
                            {
                                splitClarity = splitClarity.Replace(" (AAA)", "");
                            }
                            clarity[i] = splitClarity;
                        }
                        else
                        {
                            var splitClarity = clarity[i].Split(" ");
                            clarity[i] = splitClarity[0];
                        }

                        if (clarity[i] == ("SI2-SI3")) return string.Empty;
                    }
                    if (stone_clarityNumber == 0)
                    {
                        var clarityString = clarity.Where(b => !string.IsNullOrEmpty(b)).ToList();
                        return string.Join(", ", clarityString);
                    }
                    else
                    {
                        if (stone_clarityNumber == 1) return clarity[0];
                        if (stone_clarityNumber == 2) return clarity.Count > 1 ? clarity[1] : null;
                        if (stone_clarityNumber == 3) return clarity.Count > 2 ? clarity[2] : null;
                    }
                }
            }
            return null;
        }

        public static string GetValueCollectionName(string item_Type, string categories)
        {
            //string productType = GetFeedProductType(item_Type, categories);

            //if(productType == "ring" || productType == "charm")
            //{
            //    return null;
            //int currentMonth = DateTime.Now.Month;
            //switch (currentMonth)
            //{
            //    case 1:
            //    case 2:
            //    case 3:
            //        return "Spring";
            //    case 4:
            //    case 5:
            //    case 6:
            //        return "Summer";
            //    case 7:
            //    case 8:
            //    case 9:
            //        return "Fall";
            //    case 10:
            //    case 11:
            //    case 12:
            //        return "Winter";
            //}
            //}
            return null;
        }

        public static string GetGemType(string listOfSpecsText, int gemStoneNumber)
        {
            List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            if (GemStones != null && GemStones.Count > 0 && !string.IsNullOrEmpty(GemStones[0].GemType))
            {
                var gemstone = GemStones.Select(p => p.GemType).ToList();

                if (gemstone == null || gemstone.Count == 0)
                {
                    return "No Gemstone";
                }
                else
                {
                    if (gemStoneNumber == 0)
                    {
                        string stoneType = string.Join(", ", gemstone);
                        return textInfo.ToTitleCase(stoneType.ToLower());
                    }
                    if (gemStoneNumber == 1) return textInfo.ToTitleCase(gemstone[0].ToLower());
                    if (gemStoneNumber == 2) return gemstone.Count > 1 ? textInfo.ToTitleCase(gemstone[1]) : "No Gemstone";
                    if (gemStoneNumber == 3) return gemstone.Count > 2 ? textInfo.ToTitleCase(gemstone[2]) : "No Gemstone";
                }
            }

            return "No Gemstone";
        }

        public static string GetValueNumberStoneOrPearl(string listOfSpecsText, bool isStone, bool islisted)
        {
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);
            double quantityStone = 0;
            double quantityPearl = 0;

            if (islisted)
            {
                List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
                if (GemStones != null && GemStones.Count > 0)
                {
                    var quantity = GemStones.Select(p => p.Quantity).FirstOrDefault();
                    return quantity.ToString();
                }
            }
            else
            {
                for (int i = 1; i < 13; i++)
                {
                    if (listOfSpecs.ContainsKey("Stone Type_" + i))
                    {
                        if (listOfSpecs["Stone Type_" + i] == "Mother of Pearl"
                            || listOfSpecs["Stone Type_" + i] == "Simulated Pearl"
                            || listOfSpecs["Stone Type_" + i] == "Pearl")
                        {
                            if (listOfSpecs.ContainsKey("Stone Quantity_" + i))
                            {
                                if (listOfSpecs["Stone Quantity_" + i].Contains("to"))
                                {
                                    string[] split = listOfSpecs["Stone Quantity_" + i].Split("to");
                                    listOfSpecs["Stone Quantity_" + i] = split[0];
                                }
                                quantityPearl = quantityPearl + double.Parse(listOfSpecs["Stone Quantity_" + i]);
                            }
                        }
                        else
                        {
                            if (listOfSpecs.ContainsKey("Stone Quantity_" + i))
                                quantityStone = quantityStone + double.Parse(listOfSpecs["Stone Quantity_" + i]);
                        }
                    }
                }
            }
            if (quantityStone.ToString().Contains(".") || quantityStone == 0) return null;
            if (isStone)
            {
                return quantityStone.ToString();
            }
            else
            {
                return quantityPearl == 0 ? null : quantityPearl.ToString();
            }
        }

        public static string GetValueStoneTreatment(string item_Type, string categories, string listOfSpecsText, int stone_treatmentNumber)
        {
            string productType = GetFeedProductType(item_Type, categories);
            List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
            if (productType == "ring" || productType == "charm")
            {
                return null;
            }
            if (GemStones != null && GemStones.Count > 0)
            {
                var treatment = GemStones.Select(p => p.TreatmentMethod).ToList();

                if (treatment == null || treatment.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    if (stone_treatmentNumber == 0)
                    {
                        var treatmentString = treatment.Where(b => !string.IsNullOrEmpty(b)).ToList();
                        return string.Join(", ", treatmentString);
                    }
                    else if (productType == "fineother")
                    {
                        for (int i = 0; i < treatment.Count; i++)
                        {
                            if (treatment[i] == null) return null;
                        }

                        if (stone_treatmentNumber == 1) return treatment[0].ToLower();
                        if (stone_treatmentNumber == 2) return treatment.Count > 1 ? treatment[1].ToLower() : null;
                        if (stone_treatmentNumber == 3) return treatment.Count > 2 ? treatment[2].ToLower() : null;
                    }
                }
            }
            return null;
        }

        public static string GetValueInitialCharacter(string description, string item)
        {
            string itemName = GenerateItemName(description, item);
            string[] itemSplit = itemName.Split(" ");

            string initialCharacter = "";
            try
            {
                for (int i = 0; i < itemSplit.Count(); i++)
                {
                    string firstChars = itemSplit[i].Substring(0, 1);
                    if (i == 0)
                    {
                        initialCharacter = firstChars;
                    }
                    else
                    {
                        initialCharacter = initialCharacter + ", " + firstChars;
                    }
                }

                return initialCharacter.ToUpper();
            }
            catch { return null; }
        }

        public static string GetValueMetalStamp(string item_Type, string categories, string listOfSpecsText, bool isMetals)
        {
            Dictionary<string, string> listOfSpec = GetValueListOfSpecsText(listOfSpecsText);
            string productType = GetFeedProductType(item_Type, categories);
            if (isMetals)
            {
                if (listOfSpec.ContainsKey(" Primary - Purity"))
                    return listOfSpec[" Primary - Purity"];
                else if (listOfSpec.ContainsKey("Primary - Purity"))
                    return listOfSpec["Primary - Purity"];
            }
            else if (productType == "earring" || productType == "charm" || productType == "ring")
            {
                if (listOfSpec.ContainsKey(" Primary - Purity"))
                    return listOfSpec[" Primary - Purity"] == "925" ? "0.925" : listOfSpec[" Primary - Purity"];
                else if (listOfSpec.ContainsKey("Primary - Purity"))
                    return listOfSpec["Primary - Purity"] == "925" ? "0.925" : listOfSpec["Primary - Purity"];
            }
            return null;
        }

        public static string GetValueChainLength(string listOfSpecsText, bool isUOM)
        {
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);
            if (listOfSpecs.ContainsKey("Chain Length"))
            {
                var lengthSplit = listOfSpecs["Chain Length"].Split(' ');
                if (!isUOM)
                {
                    if (Regex.IsMatch(lengthSplit[0], @"[a-zA-Z]")) return null;

                    if (lengthSplit[0].Contains("-"))
                    {
                        var split = lengthSplit[0].Split("-");
                        lengthSplit[0] = split[0];
                    }
                    decimal b = decimal.Parse(lengthSplit[0]);
                    return b.ToString();
                }
                else
                {
                    if (lengthSplit.Length == 2)
                        return lengthSplit[1].ToUpper();
                }
            }
            return null;
        }

        public static string GetValueStoneCuts(string listOfSpecsText, int stone_cutNumber)
        {
            List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
            if (GemStones != null && GemStones.Count > 0)
            {
                var cuts = GemStones.Select(p => p.StoneCut).ToList();

                if (cuts == null || cuts.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    if (stone_cutNumber == 0)
                    {
                        var cutsString = cuts.Where(b => !string.IsNullOrEmpty(b)).ToList();
                        return string.Join(", ", cutsString);
                    }
                    if (stone_cutNumber == 1) return cuts[0];
                    if (stone_cutNumber == 2) return cuts.Count > 1 ? cuts[1] : null;
                    if (stone_cutNumber == 3) return cuts.Count > 2 ? cuts[2] : null;
                }
            }
            return null;
        }

        public static string GetValueChainType(string item_Type, string categories, string listOfSpecsText)
        {
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);
            string productType = GetFeedProductType(item_Type, categories);
            if (listOfSpecs.ContainsKey("Chain Type"))
            {
                if (listOfSpecs["Chain Type"] == "Other Chains" || listOfSpecs["Chain Type"] == "Ropa")
                    return null;

                if (productType == "bracelet")
                {
                    if (qgoldDataDictionary.ChainTypesDictionary2.ContainsKey(listOfSpecs["Chain Type"]))
                        return qgoldDataDictionary.ChainTypesDictionary2[listOfSpecs["Chain Type"]];
                    return null;
                }
                if (productType == "necklace")
                {
                    if (qgoldDataDictionary.ChainTypesDictionary3.ContainsKey(listOfSpecs["Chain Type"]))
                    {
                        var chainType = qgoldDataDictionary.ChainTypesDictionary3[listOfSpecs["Chain Type"]];

                        return string.IsNullOrWhiteSpace(chainType) ? "Snake" : chainType;
                    }
                    return "Snake";
                }
                if (productType == "fineother")
                {
                    if (qgoldDataDictionary.ChainTypesDictionary4.ContainsKey(listOfSpecs["Chain Type"]))
                        return qgoldDataDictionary.ChainTypesDictionary4[listOfSpecs["Chain Type"]];
                    return null;
                }
                else
                {
                    if (qgoldDataDictionary.ChainTypesDictionary1.ContainsKey(listOfSpecs["Chain Type"]))
                        return qgoldDataDictionary.ChainTypesDictionary1[listOfSpecs["Chain Type"]];
                    return null;
                }
            }
            else if (productType == "necklace")
            {
                return "Snake";
            }
            return null;
        }

        public static string GetValueDropLength(string item_Type, string categories, string listOfSpecsText, bool isUOM)
        {
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "earring")
            {
                string lengthOrUom = GetLengthInListOfSpecs(listOfSpecsText, isUOM);
                if (isUOM)
                {
                    if (lengthOrUom == "MM") return "Millimeters";
                    if (lengthOrUom == "CM") return "Centimeters";
                    if (lengthOrUom == "IN") return "Inches";
                }
                else
                {
                    return lengthOrUom;
                }
            }
            return null;
        }

        public static string GetValueBackingType(string item_Type, string categories, string listOfSpecsText)
        {
            string productType = GetFeedProductType(item_Type, categories);
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);

            if (productType == "earring")
            {
                if (listOfSpecs.ContainsKey("Earring Closure"))
                {
                    if (listOfSpecs["Earring Closure"] == "Leverback") return "Lever Back";
                    if (listOfSpecs["Earring Closure"] == "Post & Push Back") return "Push Back";
                    if (listOfSpecs["Earring Closure"] == "French Wire") return "French Back";
                    if (listOfSpecs["Earring Closure"] == "Hinged") return "Hinged Hoop";
                }
                return "Hinged Hoop";
            }
            return null;
        }

        public static string GetValueWheelType(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);

            if (productType == "accessory")
            {
                return "Rollerboard";
            }
            return null;
        }

        public static string GetValueSubjectMatter(string item_Type, string categories, int subjectMatterNumber)
        {
            string productType = GetFeedProductType(item_Type, categories);

            if (productType == "watches")
            {
                if (subjectMatterNumber == 1) return "Autumn";
                if (subjectMatterNumber == 2) return "Winter";
                if (subjectMatterNumber == 3) return "Summer";
                if (subjectMatterNumber == 4) return "Spring";
                if (subjectMatterNumber == 5) return "Mothers Day";
            }
            return null;
        }

        //public static string GetValueBandColor(string item_Type, string categories, string listOfSpecsText)
        //{
        //    string productType = GetFeedProductType(item_Type, categories);
        //    if (productType == "watches")
        //    {
        //        return GetValueColor(listOfSpecsText).ToLower();
        //    }

        //    return null;
        //}

        public static string GetValueCaseMaterialType(string item_Type, string categories, string listOfSpecsText)
        {
            string productType = GetFeedProductType(item_Type, categories);
            Dictionary<string, string> listOfSpec = GetValueListOfSpecsText(listOfSpecsText);

            if (productType == "watches")
            {
                if (listOfSpec.ContainsKey("Case Material"))
                {
                    if (listOfSpec["Case Material"] == "Brass") return "brass";
                    if (listOfSpec["Case Material"] == "Plastic") return "plastic";
                    if (listOfSpec["Case Material"] == "Alloy") return "Alloy Steel";
                    return "metal";
                }
            }

            return null;
        }

        public static string GetValueWaterResistance(string item_Type, string categories, string listOfSpecsText, bool isOUM)
        {
            string productType = GetFeedProductType(item_Type, categories);
            Dictionary<string, string> listOfSpec = GetValueListOfSpecsText(listOfSpecsText);

            if (productType == "watches")
            {
                if (listOfSpec.ContainsKey("Water Resistant"))
                {
                    string[] waterResistant = listOfSpec["Water Resistant"].Split(" ");

                    if (waterResistant.Length > 2)
                    {
                        if (!isOUM)
                        {
                            return waterResistant[0];
                        }
                        else
                        {
                            return waterResistant[1] == "ATM" ? "IN" : "";
                        }
                    }
                }
            }

            return null;
        }
        #endregion

        #region Product Enrichment
        public static string GetValuePearlType(string item_Type, string categories, string listOfSpecsText)
        {
            string productType = GetFeedProductType(item_Type, categories);
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);

            if (productType == "necklace" || productType == "fineother")
            {
                for (int i = 1; i < 8; i++)
                {
                    if (listOfSpecs.ContainsKey("Pearl Type_" + i))
                    {
                        if (listOfSpecs["Pearl Type_" + i] == "Saltwater") return productType == "necklace" ? "Saltwater Pearl" : "NA";
                        if (listOfSpecs["Pearl Type_" + i] == "Akoya") return productType == "necklace" ? "Akoya Cultured Pearl" : "akoya-cultured";
                        if (listOfSpecs["Pearl Type_" + i] == "Freshwater") return productType == "necklace" ? "Freshwater Cultured Pearl" : "freshwater-cultured";
                        if (listOfSpecs["Pearl Type_" + i] == "Tahitian") return productType == "necklace" ? "Tahitian Cultured Pearl" : "tahitian-cultured";
                        if (listOfSpecs["Pearl Type_" + i] == "Glass / Simulated") return productType == "necklace" ? "Simulated Pearl" : "simulated-pearl";
                    }
                }
                if (productType == "necklace") return null;
                return "NA";
            }
            return null;
        }

        public static string GetValuePearlUniformity(string listOfSpecsText)
        {
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);
            for (int i = 1; i < 8; i++)
            {
                if (listOfSpecs.ContainsKey("Pearl Type_" + i))
                {
                    if (listOfSpecs["Pearl Type_" + i] != null) return "Good to Very Good";
                }
            }
            return null;
        }

        public static string GetValuePearlShape(string item_Type, string categories, string listOfSpecsText)
        {
            string productType = GetFeedProductType(item_Type, categories);
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);
            if (productType == "necklace" || productType == "fineother")
            {
                for (int i = 1; i < 3; i++)
                {
                    if (listOfSpecs.ContainsKey("Stone Type_" + i))
                    {
                        if (listOfSpecs["Stone Type_" + i] == "Mother of Pearl"
                            || listOfSpecs["Stone Type_" + i] == "Simulated Pearl"
                            || listOfSpecs["Stone Type_" + i] == "Pearl")
                        {
                            if (!listOfSpecs.ContainsKey("Stone Shape_" + i)) return null;

                            if (listOfSpecs["Stone Shape_" + i] == "Near Round") return productType == "necklace" ? "Near-Round" : null;
                            if (listOfSpecs["Stone Shape_" + i] == "Rice") return productType == "necklace" ? "Rice" : null;
                            if (listOfSpecs["Stone Shape_" + i] == "Baroque") return productType == "necklace" ? "Baroque" : "baroque";
                            if (listOfSpecs["Stone Shape_" + i] == "Semi Baroque") return productType == "necklace" ? "Semi-Baroque" : "semi-Baroque";
                            if (listOfSpecs["Stone Shape_" + i] == "Round") return productType == "necklace" ? "Round" : "round";
                            if (listOfSpecs["Stone Shape_" + i] == "Oval") return productType == "necklace" ? "Oval" : "oval";
                            if (listOfSpecs["Stone Shape_" + i] == "Button") return productType == "necklace" ? "Button" : "button";
                            if (listOfSpecs["Stone Shape_" + i] == "Potato") return productType == "necklace" ? "Potato" : "potato";
                            if (listOfSpecs["Stone Shape_" + i] == "Coin") return productType == "necklace" ? "Coin" : "coin";
                            if (listOfSpecs["Stone Shape_" + i] == "Semi Round") return productType == "necklace" ? "Semi-Round" : "semi-Round";
                            if (listOfSpecs["Stone Shape_" + i] == "Circle Baroque") return productType == "necklace" ? "Circle Baroque" : null;
                            if (listOfSpecs["Stone Shape_" + i] == "Heart") return productType == "necklace" ? "Heart" : null;
                        }
                    }
                }
            }
            return null;
        }

        public static string GetValuePearlSucface(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);

            if (productType == "bracelet")
            {
                return "Moderate";
            }
            else if (productType == "necklace" || productType == "fineother")
            {
                return "Moderately Blemished";
            }
            return null;
        }

        public static string GetValuePearLuster(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);

            if (productType == "fineother")
            {
                return "high-luster";
            }
            else if (productType == "necklace" || productType == "bracelet")
            {
                return "High";
            }
            return null;
        }

        public static string GetValuePearlColor(string item_Type, string categories, string listOfSpecsText)
        {
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);
            string productType = GetFeedProductType(item_Type, categories);
            if (productType == "fineother")
            {
                for (int i = 1; i < 3; i++)
                {
                    if (listOfSpecs.ContainsKey("Stone Type_" + i))
                    {
                        if (listOfSpecs["Stone Type_" + i] == "Mother of Pearl"
                            || listOfSpecs["Stone Type_" + i] == "Simulated Pearl"
                            || listOfSpecs["Stone Type_" + i] == "Pearl")
                        {
                            if (listOfSpecs.ContainsKey("Stone Color_" + i)) return listOfSpecs["Stone Color_" + i];

                        }
                    }
                }
                return "other-color";
            }

            return null;
        }

        public static string GetValueFaricWash(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);

            if (productType == "keychain" || productType == "accessory")
            {
                return "Medium";
            }

            return null;
        }

        public static string GetValueBandSize(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);

            if (productType == "watches")
            {
                return "unisex";
            }

            return null;
        }

        public static string GetValueShelfLifeUnit(string item_Type, string categories)
        {
            string productType = GetFeedProductType(item_Type, categories);

            if (productType == "jewelryfinding")
            {
                return "Days";
            }
            return null;
        }

        public static string GetValueDialColor(string item_Type, string categories, string listOfSpecsText)
        {
            string productType = GetFeedProductType(item_Type, categories);
            Dictionary<string, string> listOfSpec = GetValueListOfSpecsText(listOfSpecsText);

            if (productType == "watches")
            {
                if (listOfSpec.ContainsKey("Watch Dial Color"))
                {
                    if (listOfSpec["Watch Dial Color"].ToLower() == "champagne") return "yellow";
                    if (listOfSpec["Watch Dial Color"].ToLower() == "multi color") return "Multicolor";
                    return listOfSpec["Watch Dial Color"].ToLower();
                }
            }
            return null;
        }
        #endregion

        #region Dimensions
        public static string GetValueTotalDiamondWeight(string listOfSpecsText, bool isUOM)
        {
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);

            string diamondUOM = null;
            decimal totalDiamondWeight = 0;
            for (int i = 1; i < 3; i++)
            {
                if (listOfSpecs.ContainsKey("Stone Type_" + i))
                {
                    if (listOfSpecs["Stone Type_" + i] == "Diamond")
                    {
                        if (!listOfSpecs.ContainsKey("Stone Weight_" + i)) return null;

                        string[] stoneSplit = listOfSpecs["Stone Weight_" + i].Split(' ');

                        if (Regex.IsMatch(stoneSplit[0], @"[a-zA-Z]")) return null;
                        totalDiamondWeight = totalDiamondWeight + decimal.Parse(stoneSplit[0]);

                        if (stoneSplit.Length > 2)
                            diamondUOM = stoneSplit[1] == "ct" ? stoneSplit[1] = "CARATS" : stoneSplit[1] = null;
                        else
                            diamondUOM = null;
                    }
                }
            }
            if (isUOM)
            {
                return diamondUOM;
            }
            else
            {
                return totalDiamondWeight == 0 ? null : totalDiamondWeight.ToString();
            }
        }

        public static string GetValueStoneWeight(string listOfSpecsText, int stone_weightNumber)
        {
            List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
            if (GemStones != null && GemStones.Count > 0)
            {
                var weight = GemStones.Select(p => p.Weight).ToList();

                if (weight == null || weight.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    if (stone_weightNumber == 1) return weight[0].ToString();
                    if (stone_weightNumber == 2) return weight.Count > 1 ? weight[1].ToString() : null;
                    if (stone_weightNumber == 3) return weight.Count > 2 ? weight[2].ToString() : null;
                }
            }
            return null;
        }
        public static string GetValueTheme(string feed_product_type)
        {
            if (feed_product_type == "accessory")
            {
                return "Occupational/Professional";
            }
            else
            {
                return "Holidays";
            }

        }

        public static string GetValueStoneWeightOUM(string listOfSpecsText, int stone_weightOUMNumber)
        {
            List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
            if (GemStones != null && GemStones.Count > 0)
            {
                var weightOUM = GemStones.Select(p => p.WeightUOM).ToList();

                if (weightOUM == null || weightOUM.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    for (int i = 0; i < weightOUM.Count; i++)
                    {
                        if (weightOUM[i] == "CT")
                            weightOUM[i] = "CARATS";
                    }


                    if (stone_weightOUMNumber == 1)
                    {
                        if (weightOUM[0] == "PT") return null;
                        return weightOUM[0];
                    }
                    if (stone_weightOUMNumber == 2)
                    {
                        if (weightOUM.Count > 1)
                        {
                            if (weightOUM[1] == "PT") return null;
                            return weightOUM[1];
                        }
                        return null;
                    }
                    if (stone_weightOUMNumber == 3)
                    {
                        if (weightOUM.Count > 2)
                        {
                            if (weightOUM[2] == "PT") return null;
                            return weightOUM[2];
                        }
                        return null;
                    }
                }
            }
            return null;
        }

        public static string GetValueTotalGemWeight(string item_Type, string categories, string listOfSpecsText)
        {
            string productType = GetFeedProductType(item_Type, categories);

            if (productType == "bracelet" || productType == "necklace")
            {
                return GetWeightUOM(listOfSpecsText, "CT");
            }
            return null;
        }

        public static string GetWeightUOM(string listOfSpecsText, string input)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["CT"] = "CARATS";
            dict["GR"] = "GR";
            dict["GM"] = "GR";
            dict["LBS"] = "LB";

            if (string.IsNullOrEmpty(input))
            {
                return dict[WeightInListOfSpecs(listOfSpecsText).ToUpper()];
            }
            else
            {
                return dict[input.ToUpper()];
            }
        }

        public static string GetValueItemShape(string item_Type, string categories, string listOfSpecsText)
        {
            string productType = GetFeedProductType(item_Type, categories);
            List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);
            if (productType == "charm" || productType == "watches" || productType == "keychain")
            {
                var shapes = GemStones.Select(p => p.Shape).ToList();
                if (GemStones != null && GemStones.Count > 0)
                {
                    foreach (var shape in shapes)
                    {
                        if (shape != null && shape != string.Empty)
                        {
                            if (shape == "Circle")
                            {
                                if (productType == "charm") return "Circle";
                            }
                            if (shape == "Oval")
                            {
                                if (productType == "charm" || productType == "keychain") return "Oval";
                                if (productType == "watches") return "oval";
                            }
                            if (shape == "Triangle")
                            {
                                if (productType == "charm" || productType == "keychain") return "Triangle";
                            }
                            if (shape == "Square")
                            {
                                if (productType == "charm" || productType == "keychain") return "Square";
                                if (productType == "watches") return "square";
                            }
                        }
                    }
                }
                if (productType == "watches")
                {
                    return "round";
                }
            }
            return null;
        }

        public static string GetValueItemDisplayWidthOUM(string item_Type, string categories, string listOfSpecsText)
        {
            string productType = GetFeedProductType(item_Type, categories);

            if (productType == "ring" || productType == "fineother")
            {
                return GetWidthInListOfSpecs(listOfSpecsText, true);
            }
            return null;
        }

        public static string GetValueTotalMetalWeightUOM(string item_Type, string categories, string listOfSpecsText)
        {
            string productType = GetFeedProductType(item_Type, categories);

            if (productType == "ring" || productType == "fineother")
            {
                return GetWeightUOM(listOfSpecsText, string.Empty);
            }
            return null;
        }

        public static string GetValueStoneOrWidthLength(string listOfSpecsText, int stone_lengthOrWidthNumber, bool isLength, bool isOUM)
        {
            List<JewelryGemStone> GemStones = GetGemStones(listOfSpecsText);

            if (GemStones != null && GemStones.Count > 0)
            {
                var lengthOrWidth = GemStones.Select(p => p.StoneSizeText).ToList();

                if (lengthOrWidth == null || lengthOrWidth.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    for (int i = 0; i < lengthOrWidth.Count; i++)
                    {
                        if (lengthOrWidth[i] == null || lengthOrWidth[i].ToLower().Contains("(range)")) return null;

                        if (string.IsNullOrWhiteSpace(lengthOrWidth[i]))
                        {
                            string number = Regex.Match(lengthOrWidth[i], @"\d+").Value;
                            string text = Regex.Match(lengthOrWidth[i], @"\D+").Value;

                            lengthOrWidth[i] = number + " " + text;
                        }

                        var lengthOrWidthSplit = lengthOrWidth[i].Split(' ');
                        if (isOUM)
                        {
                            int checkNumber;
                            decimal checkNumberDot;
                            if (lengthOrWidthSplit.Length > 3)
                            {
                                if (int.TryParse(lengthOrWidthSplit[3], out checkNumber) || decimal.TryParse(lengthOrWidthSplit[3], out checkNumberDot)
                                    || lengthOrWidthSplit[3].ToUpper().Contains("X") || lengthOrWidthSplit[3].ToUpper().Contains(",")
                                    || lengthOrWidthSplit[3].ToUpper().Contains("TO"))
                                    return null;

                                if (stone_lengthOrWidthNumber == 1) return lengthOrWidthSplit[3].ToUpper();
                                if (stone_lengthOrWidthNumber == 2 && i == 1) return lengthOrWidth.Count > 1 && i == 1 ? lengthOrWidthSplit[3].ToUpper() : null;
                                if (stone_lengthOrWidthNumber == 3 && i == 2) return lengthOrWidth.Count > 2 && i == 2 ? lengthOrWidthSplit[3].ToUpper() : null;
                            }
                            else if (lengthOrWidthSplit.Length > 2)
                            {
                                if (int.TryParse(lengthOrWidthSplit[2], out checkNumber) || decimal.TryParse(lengthOrWidthSplit[2], out checkNumberDot)
                                    || lengthOrWidthSplit[2].ToUpper().Contains("X") || lengthOrWidthSplit[2].ToUpper().Contains(",")
                                    || lengthOrWidthSplit[2].ToUpper().Contains("TO"))
                                    return null;

                                if (stone_lengthOrWidthNumber == 1) return lengthOrWidthSplit[2].ToUpper();
                                if (stone_lengthOrWidthNumber == 2 && i == 1) return lengthOrWidth.Count > 1 && i == 1 ? lengthOrWidthSplit[2].ToUpper() : null;
                                if (stone_lengthOrWidthNumber == 3 && i == 2) return lengthOrWidth.Count > 2 && i == 2 ? lengthOrWidthSplit[2].ToUpper() : null;
                            }
                            else if (lengthOrWidthSplit.Length > 1)
                            {
                                if (int.TryParse(lengthOrWidthSplit[1], out checkNumber) || decimal.TryParse(lengthOrWidthSplit[1], out checkNumberDot)
                                   || lengthOrWidthSplit[1].ToUpper().Contains("X") || lengthOrWidthSplit[1].ToUpper().Contains(",")
                                   || lengthOrWidthSplit[1].ToUpper().Contains("TO"))
                                    return null;

                                if (stone_lengthOrWidthNumber == 1) return lengthOrWidthSplit[1].ToUpper();
                                if (stone_lengthOrWidthNumber == 2 && i == 1) return lengthOrWidth.Count > 1 && i == 1 ? lengthOrWidthSplit[1].ToUpper() : null;
                                if (stone_lengthOrWidthNumber == 3 && i == 2) return lengthOrWidth.Count > 2 && i == 2 ? lengthOrWidthSplit[1].ToUpper() : null;
                            }
                        }
                        else
                        {

                            if (isLength == true)
                            {
                                if (lengthOrWidthSplit[0].Contains("x"))
                                {
                                    var getWidth = lengthOrWidthSplit[0].Split("x");
                                    lengthOrWidthSplit[0] = getWidth[0];
                                }

                                if (stone_lengthOrWidthNumber == 1)
                                {
                                    if (lengthOrWidthSplit.Length > 1)
                                    {
                                        if (lengthOrWidthSplit[1].Contains("to")) return lengthOrWidthSplit[0].ToString();
                                    }

                                    return lengthOrWidthSplit[0].ToString();
                                }
                                if (stone_lengthOrWidthNumber == 2 && i == 1)
                                {
                                    if (lengthOrWidthSplit.Length > 1)
                                    {
                                        if (lengthOrWidthSplit[1].Contains("to")) return lengthOrWidthSplit[0].ToString();
                                    }
                                    return lengthOrWidth.Count > 1 && i == 1 ? lengthOrWidthSplit[0].ToString() : null;
                                }
                                if (stone_lengthOrWidthNumber == 3 && i == 2)
                                {
                                    if (lengthOrWidthSplit.Length > 1)
                                    {
                                        if (lengthOrWidthSplit[1].Contains("to")) return lengthOrWidthSplit[0].ToString();
                                    }
                                    return lengthOrWidth.Count > 2 && i == 2 ? lengthOrWidthSplit[0].ToString() : null;
                                }
                            }
                            else
                            {
                                if (lengthOrWidthSplit[0].Contains("x"))
                                {
                                    var getWidth = lengthOrWidthSplit[0].Split("x");
                                    lengthOrWidthSplit[0] = getWidth[1];
                                }

                                if (lengthOrWidthSplit.Length > 2)
                                {
                                    if (stone_lengthOrWidthNumber == 1)
                                    {
                                        if (lengthOrWidthSplit[1].Contains("to")) return lengthOrWidthSplit[0].ToString();
                                        return lengthOrWidthSplit[2].ToString();
                                    }
                                    if (stone_lengthOrWidthNumber == 2 && i == 1)
                                    {
                                        if (lengthOrWidthSplit[1].Contains("to")) return lengthOrWidthSplit[0].ToString();
                                        return lengthOrWidth.Count > 1 && i == 1 ? lengthOrWidthSplit[2].ToString() : null;
                                    }
                                    if (stone_lengthOrWidthNumber == 3 && i == 2)
                                    {
                                        if (lengthOrWidthSplit[1].Contains("to")) return lengthOrWidthSplit[0].ToString();
                                        return lengthOrWidth.Count > 2 && i == 2 ? lengthOrWidthSplit[2].ToString() : null;
                                    }
                                }
                                else
                                {
                                    if (stone_lengthOrWidthNumber == 1) return lengthOrWidthSplit[0].ToString();
                                    if (stone_lengthOrWidthNumber == 2 && i == 1) return lengthOrWidth.Count > 1 && i == 1 ? lengthOrWidthSplit[0].ToString() : null;
                                    if (stone_lengthOrWidthNumber == 3 && i == 2) return lengthOrWidth.Count > 2 && i == 2 ? lengthOrWidthSplit[0].ToString() : null;
                                }


                            }
                        }
                    }

                }
            }

            return null;
        }
        #endregion

        public static string GetLengthInListOfSpecs(string listOfSpecsText,
            bool isUOM,
            string size = "",
            string length = "")
        {
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);
            if (listOfSpecs.ContainsKey("Length of Item"))
            {
                var lengthSplit = listOfSpecs["Length of Item"].Split(' ');
                if (!isUOM)
                {
                    //if (lengthSplit.Length > 2) ;

                    //    return lengthSplit[0] + ", " + lengthSplit[2]; 
                    if (lengthSplit[0].Contains("-"))
                    {
                        var lenghtStrikethrough = lengthSplit[0].Split("-");
                        return lenghtStrikethrough[1];
                    }

                    if (lengthSplit[0].Contains("."))
                    {
                        //var lenghtDot = lengthSplit[0].Replace(".", "");
                        return lengthSplit[0];
                    }

                    if (lengthSplit[0].Contains(","))
                    {
                        //var lenghtDot = lengthSplit[0].Replace(",", "");
                        return lengthSplit[0];
                    }

                    if (lengthSplit[0].Any(char.IsLetter)) return null;
                    return string.IsNullOrEmpty(lengthSplit[0]) ? "0.25" : lengthSplit[0];
                }
                else
                {
                    if (lengthSplit.Length == 2)
                    {
                        if (lengthSplit[1] == "in") return "IN";
                        if (lengthSplit[1] == "mm") return "MM";
                        if (lengthSplit[1] == "cm") return "CM";
                    }

                    if (lengthSplit.Length > 2)
                    {
                        try
                        {
                            if (lengthSplit[3] == "in") return "IN";
                            if (lengthSplit[3] == "mm") return "MM";
                            if (lengthSplit[3] == "cm") return "CM";
                        }
                        catch
                        {
                            if (lengthSplit[1] == "in") return "IN";
                            if (lengthSplit[1] == "mm") return "MM";
                            if (lengthSplit[1] == "cm") return "CM";
                        }
                    }
                    return "IN";
                }
            }
            else if (listOfSpecs.ContainsKey("Ring Top Length"))
            {
                var lengthSplit = listOfSpecs["Ring Top Length"].Split(' ');
                if (!isUOM)
                {
                    //if (lengthSplit.Length > 2) ;

                    //    return lengthSplit[0] + ", " + lengthSplit[2]; 
                    if (lengthSplit[0].Contains("-"))
                    {
                        var lenghtStrikethrough = lengthSplit[0].Split("-");
                        return lenghtStrikethrough[1];
                    }

                    if (lengthSplit[0].Contains("."))
                    {
                        //var lenghtDot = lengthSplit[0].Replace(".", "");
                        return lengthSplit[0];
                    }

                    if (lengthSplit[0].Contains(","))
                    {
                        //var lenghtDot = lengthSplit[0].Replace(",", "");
                        return lengthSplit[0];
                    }

                    if (lengthSplit[0].Any(char.IsLetter)) return null;
                    return string.IsNullOrEmpty(lengthSplit[0]) ? "0.25" : lengthSplit[0];
                }
                else
                {
                    if (lengthSplit.Length == 2)
                    {
                        if (lengthSplit[1] == "in") return "IN";
                        if (lengthSplit[1] == "mm") return "MM";
                        if (lengthSplit[1] == "cm") return "CM";
                    }

                    if (lengthSplit.Length > 2)
                    {
                        try
                        {
                            if (lengthSplit[3] == "in") return "IN";
                            if (lengthSplit[3] == "mm") return "MM";
                            if (lengthSplit[3] == "cm") return "CM";
                        }
                        catch
                        {
                            if (lengthSplit[1] == "in") return "IN";
                            if (lengthSplit[1] == "mm") return "MM";
                            if (lengthSplit[1] == "cm") return "CM";
                        }
                    }
                    return "IN";
                }
            }

            if (size != "")
            {
                return size;
            }

            if (length != "")
            {
                return length;
            }
            return isUOM == true ? "MM" : "";
        }
        private static List<JewelryGemStone> GetGemStones(string listOfSpecsText)
        {
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);
            List<JewelryGemStone> gemstones = new();

            for (int i = 1; i < 10; i++)
            {
                if (listOfSpecs.ContainsKey("Stone Type_" + i))
                {
                    JewelryGemStone stone = new();
                    stone.GemType = listOfSpecs["Stone Type_" + i];
                    stone.StoneCut = listOfSpecs.ContainsKey("Stone Cut_" + i) ? listOfSpecs["Stone Cut_" + i] : null;
                    stone.Color = listOfSpecs.ContainsKey("Stone Color_" + i) ? listOfSpecs["Stone Color_" + i] : null;
                    stone.Clarity = listOfSpecs.ContainsKey("Stone Clarity_" + i) ? listOfSpecs["Stone Clarity_" + i] : null;

                    var Shape = listOfSpecs.ContainsKey("Stone Shape_" + i) ? listOfSpecs["Stone Shape_" + i] : null;
                    if (Shape == null && stone.GemType == "Pearl")
                    {
                        Shape = "Round";
                    }
                    if (string.IsNullOrEmpty(Shape)) stone.Shape = "Other Shape";
                    else
                        stone.Shape = Shape; /*this.qgoldDataDictionary.StoneShapesDictionary.ContainsKey(Shape) ? this.qgoldDataDictionary.StoneShapesDictionary[Shape] : "other-shape"; */

                    stone.CreationMethod = listOfSpecs.ContainsKey("Stone Creation Method_" + i) ? listOfSpecs["Stone Creation Method_" + i] : null;
                    stone.TreatmentMethod = listOfSpecs.ContainsKey("Stone Treatment_" + i) ? listOfSpecs["Stone Treatment_" + i] : null;
                    int q = 0;
                    if (listOfSpecs.ContainsKey("Stone Quantity_" + i))
                        int.TryParse(listOfSpecs["Stone Quantity_" + i], out q);
                    stone.Quantity = q;

                    stone.Setting = listOfSpecs.ContainsKey("Stone Setting_" + i) ? listOfSpecs["Stone Setting_" + i] : null;

                    if (listOfSpecs.ContainsKey("Stone Size_" + i))
                    {
                        var spec = listOfSpecs["Stone Size_" + i];
                        stone.LengthUOM = "mm";

                        stone.StoneSizeText = spec;
                        var pair = spec.Split(' ');
                        if (pair.Length == 2)
                        {
                            decimal num;
                            bool isNum = decimal.TryParse(pair[0], out num);
                            if (isNum)
                                stone.Width = Decimal.Parse(pair[0]);
                            else
                            {
                                if (!string.IsNullOrEmpty(pair[0]))
                                {
                                    if (pair[0].Contains('x'))
                                    {
                                        var wl = pair[0].Split('x');
                                        if (wl[0] == "")
                                            stone.Width = decimal.Parse("0");
                                        else
                                            stone.Width = decimal.Parse(wl[0]);

                                        if (wl[1] == "")
                                            stone.Length = decimal.Parse("0");
                                        else
                                            stone.Length = decimal.Parse(wl[1]);
                                    }
                                    else
                                    {
                                        if (pair[0].Contains('-'))
                                        {

                                            var wl = pair[0].Split('-');
                                            decimal num1, num2;
                                            bool isNum1 = decimal.TryParse(wl[0], out num1);
                                            bool isNum2 = decimal.TryParse(wl[1], out num2);
                                            if (isNum1 && isNum2)
                                            {
                                                stone.Width = (num1 + num2) / 2;
                                                stone.Length = (num1 + num2) / 2;

                                            }
                                            else
                                            {

                                            }
                                        }
                                        else if (pair[0].Contains(','))
                                        {
                                            var sp = pair[0].Split(',');

                                            stone.Height = decimal.Parse(sp[0]);
                                            stone.Width = decimal.Parse(sp[1]);
                                            stone.Length = decimal.Parse(sp[2]);


                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                            }
                            stone.LengthUOM = pair[0].ToUpper();

                        }
                        else
                        {
                            if (pair.Length == 4)
                            {
                                if (pair[1] == "x")
                                {
                                    stone.Width = decimal.Parse(pair[0].Trim(','));
                                    stone.Length = decimal.Parse(pair[2].Trim(','));

                                }
                                else
                                {
                                    if (pair[1] == "to")
                                    {
                                        stone.StoneSizeText = spec;
                                    }
                                    else
                                    {

                                        stone.LengthUOM = pair[3];
                                    }

                                }
                                stone.LengthUOM = pair[3];

                            }
                            else
                            {
                                if (pair.Length == 3)
                                {
                                    if (pair[1] == "x")
                                    {
                                        stone.Width = decimal.Parse(pair[0].Trim(','));
                                        stone.Length = decimal.Parse(pair[2].Trim(','));
                                    }
                                    else
                                    {
                                        decimal num;
                                        bool isNum = decimal.TryParse(pair[0].Trim(','), out num);

                                        if (isNum)
                                        {
                                            if (pair[1] == "to")
                                            {
                                                stone.Width = decimal.Parse(pair[0].Trim(','));
                                                stone.Length = decimal.Parse(pair[2].Trim(','));
                                                stone.LengthUOM = pair[2];
                                            }
                                            else
                                            {
                                                if (Regex.IsMatch(pair[1], @"\d"))
                                                {
                                                    pair[1] = pair[1].Replace("to", "");
                                                }
                                                stone.Width = decimal.Parse(pair[0].Trim(','));
                                                stone.Length = decimal.Parse(pair[1].Trim(','));
                                                stone.LengthUOM = pair[2];
                                            }

                                        }



                                        else
                                        {
                                            if (pair[2] == "(Range)")
                                            {
                                                if (pair[0].Contains('-'))
                                                {

                                                    var wl = pair[0].Split('-');
                                                    decimal num1, num2;
                                                    bool isNum1 = decimal.TryParse(wl[0], out num1);
                                                    bool isNum2 = decimal.TryParse(wl[1], out num2);
                                                    if (isNum1 && isNum2)
                                                    {
                                                        stone.Width = (num1 + num2) / 2;
                                                        stone.Length = (num1 + num2) / 2;

                                                    }
                                                    else
                                                    {

                                                    }
                                                }
                                            }
                                            else
                                            {

                                            }
                                        }

                                    }

                                }
                                else if (pair.Length == 1)
                                {
                                    decimal num;
                                    bool isNum = decimal.TryParse(pair[0], out num);
                                    if (isNum)
                                    {
                                        stone.Width = num;
                                        stone.LengthUOM = "mm";
                                    }
                                    else
                                    {
                                        isNum = decimal.TryParse(pair[0].TrimEnd('m'), out num);
                                        if (isNum)
                                        {
                                            stone.Width = num;
                                            stone.LengthUOM = "mm";
                                        }
                                        else
                                        {
                                            string p = pair[0].ToLower();
                                            if (p.Contains('x'))
                                            {
                                                var sp = p.Split('x');
                                                decimal num1, num2;
                                                bool isNum1 = decimal.TryParse(sp[0], out num1);
                                                bool isNum2 = decimal.TryParse(sp[0], out num2);
                                                if (isNum1 && isNum2)
                                                {
                                                    stone.Width = Math.Min(num1, num2);
                                                    stone.Length = Math.Max(num1, num2);
                                                    stone.LengthUOM = "mm";
                                                }
                                                else
                                                {

                                                }

                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                }
                                else if (pair.Length == 5)
                                {

                                    stone.LengthUOM = pair[3];
                                }
                                else if (pair.Length == 6)
                                {
                                    try
                                    {
                                        stone.Height = decimal.Parse(pair[0].Trim(','));
                                        stone.Width = decimal.Parse(pair[2].Trim(','));
                                        stone.Length = decimal.Parse(pair[4].Trim(','));
                                        stone.LengthUOM = pair[5];
                                    }
                                    catch
                                    {
                                        if (stone.Width == 0) stone.Width = decimal.Parse(pair[3].Trim(','));
                                    }

                                }
                                else
                                {

                                }

                            }
                        }
                    }

                    if (listOfSpecs.ContainsKey("Stone Weight_" + i))
                    {
                        var spec = listOfSpecs["Stone Weight_" + i];
                        var pair = spec.Split(' ');
                        if (pair.Length == 2)
                        {
                            stone.Weight = Decimal.Parse(pair[0]);
                            stone.WeightUOM = pair[1].ToUpper();
                        }
                    }
                    if (qgoldDataDictionary.GemTypesDictionary.ContainsKey(stone.GemType))
                    {
                        stone.GemType = qgoldDataDictionary.GemTypesDictionary[stone.GemType];
                        gemstones.Add(stone);
                    }
                }
            }

            return gemstones;
        }

        private static Dictionary<string, string> GetValueListOfSpecsText(string listOfSpecsText)
        {
            Dictionary<string, string> listOfSpecsTextDictionary = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(listOfSpecsText))
            {
                string[] listArray = listOfSpecsText.Split('|');
                foreach (string s in listArray)
                {
                    //string[] myKeyValuePair = s.Split(':');
                    string pattern = @":([^:]+)$";

                    Match match = Regex.Match(s, pattern);

                    if (match.Success)
                    {
                        var textBefore = s.Replace($":{match.Groups[1].Value}", "");
                        listOfSpecsTextDictionary.Add(textBefore, match.Groups[1].Value);
                    }
                }
                return listOfSpecsTextDictionary;
            }
            return new Dictionary<string, string>();
        }

        private static string SetBulletPoint(string listOfSpecsText, int bulletPointNumber)
        {
            string bulletPoint1 = "";
            string bulletPoint2 = "";
            string endBulletPoint2 = "Material quality is genuine fine-jewelry standard guarantee.";

            foreach (var spec in GetValueListOfSpecsText(listOfSpecsText))
            {
                var keyandValue = " " + spec.Key + ": " + spec.Value;

                if (bulletPoint1.Length + keyandValue.Length < 250)
                {
                    bulletPoint1 += keyandValue;
                }
                else if (bulletPoint2.Length + keyandValue.Length < 250)
                {
                    bulletPoint2 += keyandValue;
                }
            }

            if ((!string.IsNullOrEmpty(bulletPoint2)) && (bulletPoint2.Length + endBulletPoint2.Length < 250))
                bulletPoint2 = bulletPoint2 + ". " + endBulletPoint2;

            return bulletPointNumber == 1 ? bulletPoint1 : bulletPoint2;
        }

        private static string WeightInListOfSpecs(string listOfSpecsText)
        {
            Dictionary<string, string> listOfSpecs = GetValueListOfSpecsText(listOfSpecsText);

            return listOfSpecs.ContainsKey("Item Weight U/M") ? listOfSpecs["Item Weight U/M"].ToUpper() : "GR";
        }

        public static string GetWebsiteShippingWeight(string item_display_weight)
        {
            if (!string.IsNullOrWhiteSpace(item_display_weight))
            {
                return Math.Round((double.Parse(item_display_weight) + 28.3495231), 2, MidpointRounding.ToPositiveInfinity).ToString();
            }
            return "40";
        }

        public static string ImageStandard(string image)
        {
            if (string.IsNullOrEmpty(image)) { return string.Empty; }

            int questionMarkIndex = image.IndexOf('?');
            if (questionMarkIndex != -1)
            {
                string baseUrl = image.Substring(0, questionMarkIndex);
                string queryString = image.Substring(questionMarkIndex + 1);

                string[] parameters = queryString.Split('&');
                string newQueryString = string.Empty;

                foreach (string parameter in parameters)
                {
                    string[] parts = parameter.Split('=');
                    if (parts.Length == 2)
                    {
                        string key = parts[0];
                        string value = parts[1];

                        if (key.Equals("w", StringComparison.OrdinalIgnoreCase))
                        {
                            value = "400";
                        }
                        else if (key.Equals("h", StringComparison.OrdinalIgnoreCase))
                        {
                            value = "400";
                        }

                        newQueryString += $"{key}={value}&";
                    }
                }

                if (!string.IsNullOrEmpty(newQueryString))
                {
                    newQueryString = newQueryString.TrimEnd('&');
                    return $"{baseUrl}?{newQueryString}";
                }
            }

            return string.Empty;

        }

        public static string GetSizeName(string typeProduct, string size, string ctWeight)
        {
            if (typeProduct == "earring" && !string.IsNullOrWhiteSpace(ctWeight))
            {
                return $"{ctWeight} carats";
            }
            return string.IsNullOrEmpty(size) ? "None" : size;
        }
    }
}
