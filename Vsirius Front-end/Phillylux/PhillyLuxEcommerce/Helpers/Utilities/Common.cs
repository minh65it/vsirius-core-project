using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.ViewModels;
using System.Text.RegularExpressions;

namespace PhillyLuxEcommerce.Helpers.Utilities
{
    public static class Common
    {
        public static bool IsValidEmail(string email)
        {
            // Define a regular expression pattern for email validation
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            // Create a Regex object with the pattern
            Regex regex = new Regex(pattern);

            // Use the IsMatch method to check if the email matches the pattern
            return regex.IsMatch(email);
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            Regex regex = new Regex("^[0-9]{10}$");
            string phoneNumberWithoutSpaces = Regex.Replace(phoneNumber, @"\s", "");
            return regex.IsMatch(phoneNumberWithoutSpaces);
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

        public static List<HomeVMFilter> categoryMenus = new();
        public static List<State> lstStateUSA = GetListStateUSA();
        public static List<State> lstStateCanada = GetListStateCanada();
        public static List<Country> lstCountry = GetListCountry();

        private static List<Country> GetListCountry()
        {
            if (lstCountry == null)
            {
                var lstCountry = new List<Country>
                {
                    new Country() { Title = "United States", Key = "United States" },
                    new Country() { Title = "Canada", Key = "Canada" }
                };
                return lstCountry;
            }
            return lstCountry;
        }

        //private static List<CategoryMenu> GetListCategoryMenu()
        //{
        //    if (categoryMenus == null)
        //    {
        //        var lstCategoryMenu = new List<CategoryMenu>();
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Ring", Key = "ring", Position = 1 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Earring", Key = "earring", Position = 2 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Bracelet", Key = "bracelet", Position = 3 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Necklace", Key = "necklace", Position = 4 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Keychain", Key = "keychain", Position = 5 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Cufflink", Key = "cufflink", Position = 6 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Wallet", Key = "wallet", Position = 7 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Watches", Key = "watches", Position = 8 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Watchband", Key = "watchband", Position = 9 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Apparel", Key = "apparel", Position = 10 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Apparel Pin", Key = "apparelpin", Position = 11 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Apparel Belt", Key = "apparelbelt", Position = 12 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Necktie", Key = "necktie", Position = 13 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Charm", Key = "charm", Position = 14 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Fine Ring", Key = "finering", Position = 15 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Fine Other", Key = "fineother", Position = 16 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Accessory", Key = "accessory", Position = 17 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Jewelry", Key = "jewelry", Position = 18 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Jewelry Set", Key = "jewelryset", Position = 19 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Guild Jewelry", Key = "guildjewelry", Position = 20 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Fine Earring", Key = "fineearring", Position = 21 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Fine Necklace Bracelet Anklet", Key = "finenecklacebraceletanklet", Position = 22 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Coin Purse Pouch", Key = "coinpursepouch", Position = 23 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Jewelry Finding", Key = "jewelryfinding", Position = 24 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Piercing Jewelry", Key = "piercingjewelry", Position = 25 });
        //        lstCategoryMenu.Add(new CategoryMenu() { Title = "Jewelry Storage", Key = "jewelrystorage", Position = 26 });
        //        return lstCategoryMenu;
        //    }
        //    return new List<CategoryMenu>();
        //}

        private static List<State> GetListStateUSA()
        {
            if (lstStateUSA == null)
            {
                var lstState = new List<State>
                {
                    new State() { Title = "Alabama", Key = "AL" },
                    new State() { Title = "Alaska", Key = "AK" },
                    new State() { Title = "Arizona", Key = "AZ" },
                    new State() { Title = "Arkansas", Key = "AR" },
                    new State() { Title = "California", Key = "CA" },
                    new State() { Title = "Colorado", Key = "CO" },
                    new State() { Title = "Connecticut", Key = "CT" },
                    new State() { Title = "Delaware", Key = "DE" },
                    new State() { Title = "Florida", Key = "FL" },
                    new State() { Title = "Georgia", Key = "GA" },
                    new State() { Title = "Hawaii", Key = "HI" },
                    new State() { Title = "Idaho", Key = "ID" },
                    new State() { Title = "Illinois", Key = "IL" },
                    new State() { Title = "Indiana", Key = "IN" },
                    new State() { Title = "Iowa", Key = "IA" },
                    new State() { Title = "Kansas", Key = "KS" },
                    new State() { Title = "Kentucky", Key = "KY" },
                    new State() { Title = "Louisiana", Key = "LA" },
                    new State() { Title = "Maine", Key = "ME" },
                    new State() { Title = "Maryland", Key = "MD" },
                    new State() { Title = "Massachusetts", Key = "MA" },
                    new State() { Title = "Michigan", Key = "MI" },
                    new State() { Title = "Minnesota", Key = "MN" },
                    new State() { Title = "Mississippi", Key = "MS" },
                    new State() { Title = "Missouri", Key = "MO" },
                    new State() { Title = "Montana", Key = "MT" },
                    new State() { Title = "Nebraska", Key = "NE" },
                    new State() { Title = "Nevada", Key = "NV" },
                    new State() { Title = "New Hampshire", Key = "NH" },
                    new State() { Title = "New Jersey", Key = "NJ" },
                    new State() { Title = "New Mexico", Key = "NM" },
                    new State() { Title = "New York", Key = "NY" },
                    new State() { Title = "North Carolina", Key = "NC" },
                    new State() { Title = "North Dakota", Key = "ND" },
                    new State() { Title = "Ohio", Key = "OH" },
                    new State() { Title = "Oklahoma", Key = "OK" },
                    new State() { Title = "Oregon", Key = "OR" },
                    new State() { Title = "Pennsylvania", Key = "PA" },
                    new State() { Title = "Rhode Island", Key = "RI" },
                    new State() { Title = "South Carolina", Key = "SC" },
                    new State() { Title = "South Dakota", Key = "SD" },
                    new State() { Title = "Tennessee", Key = "TN" },
                    new State() { Title = "Texas", Key = "TX" },
                    new State() { Title = "Utah", Key = "UT" },
                    new State() { Title = "Vermont", Key = "VT" },
                    new State() { Title = "Virginia", Key = "VA" },
                    new State() { Title = "Washington", Key = "WA" },
                    new State() { Title = "West Virginia", Key = "WV" },
                    new State() { Title = "Wisconsin", Key = "WI" },
                    new State() { Title = "Wyoming", Key = "WY" }
                };
                return lstState;
            }
            return lstStateUSA;
        }

        public static List<State> GetListStateCanada()
        {
            if (lstStateCanada == null)
            {
                var lstState = new List<State>
                {
                    new State() { Title = "Alberta", Key = "AB" },
                    new State() { Title = "British Columbia", Key = "BC" },
                    new State() { Title = "Manitoba", Key = "MB" },
                    new State() { Title = "New Brunswick", Key = "NB" },
                    new State() { Title = "Newfoundland and Labrador", Key = "NL" },
                    new State() { Title = "Northwest Territories", Key = "NT" },
                    new State() { Title = "Nova Scotia", Key = "NS" },
                    new State() { Title = "Nunavut", Key = "NU" },
                    new State() { Title = "Ontario", Key = "ON" },
                    new State() { Title = "Prince Edward Island", Key = "PE" },
                    new State() { Title = "Quebec", Key = "QC" },
                    new State() { Title = "Saskatchewan", Key = "SK" },
                    new State() { Title = "Yukon", Key = "YT" }
                };
                return lstState;
            }
            return lstStateCanada;
        }
    }

    public enum Payment
    {
        Cash = 0,
        ChuyenKhoan = 1,
    }

    public enum StatusOrder
    {
        Pending = 1,
        Approved = 2
    }
}
