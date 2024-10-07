namespace StullerDataApi.Utilities.Mapping
{
    public class Stuller_Data_Maps_2
    {
        public string[] PearlTypeNecklaceList { get; set; } = { "Saltwater Pearl", "South Sea Cultured Pearl", "Akoya Cultured Pearl", "Natural Pearl", "Black Pearl", "Conch Pearl", "Blister Pearl", "Fireballs Pearl", "Scallop Pearl", "Coin Pearl", "Freshwater Cultured Pearl", "Blue Mussel Pearl", "Mabe Cultured Pearl", "Biwa Pearl", "Seed Pearl", "Keshi Cultured Pearl", "Basra Pearl", "Cassis Pearl", "Tahitian Cultured Pearl", "Edison Pearl", "Simulated Pearl", "Melo Pearl", "Quahog Pearl", "Abalone Pearl", };
        public string[] PearlShapeNecklaceList { get; set; } = { "Heart", "Circle Baroque", "Drop", "Semi-Round", "Coin", "Potato", "Near-Round", "Button", "Oval", "Round", "Semi-Baroque", "Baroque", "Rice", };
        public Dictionary<string, string> NumberOfPearlDictionary { get; set; }
        public Stuller_Data_Maps_2()
        {
            BuildNumberOfPearlDictionary();
        }
        private void BuildNumberOfPearlDictionary()
        {
            NumberOfPearlDictionary = new Dictionary<string, string>();
            NumberOfPearlDictionary["Two-Stone"] = "2";
            NumberOfPearlDictionary["Three-Stone"] = "3";
            NumberOfPearlDictionary["Four-Stone"] = "4";
            NumberOfPearlDictionary["Five-Stone"] = "5";
            NumberOfPearlDictionary["Six-Stone"] = "6";
            NumberOfPearlDictionary["Seven-Stone"] = "7";
            NumberOfPearlDictionary["Eight-Stone"] = "8";
            NumberOfPearlDictionary["Nine-Stone"] = "9";
        }
    }
}
