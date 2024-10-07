namespace WalmartIntegrationDataApi.Common.Mappers
{
    public static class DataMaps
    {
        public static List<string> DiameterUnitOfMeasureList = GetDiameterUnitOfMeasureList();
        private static List<string> GetDiameterUnitOfMeasureList()
        {
            if (DiameterUnitOfMeasureList == null)
            {
                return
                    new List<string>()
                    {
                    "in",
                    "ft",
                    "mm",
                    "cm"
                    };
            }
            return DiameterUnitOfMeasureList;
        }


        public static List<string> ChainTypeList = GetChainTypeList();
        private static List<string> GetChainTypeList()
        {
            if (ChainTypeList == null)
            {
                return new List<string>()
                {
                    "Anchor",
                    "Ball",
                    "Bar",
                    "Basketweave",
                    "Bead",
                    "Belcher",
                    "Beveled Herringbone",
                    "Bismark",
                    "Boston Link",
                    "Bow-Tie Figaro",
                    "Box",
                    "Byzantine",
                    "C-Chain",
                    "Cable",
                    "Cable Wire",
                    "Cascade",
                    "Cobra",
                    "Cord",
                    "Coreana",
                    "Crisscross",
                    "Cuban Link",
                    "Curb",
                    "Diamond Cut Figaro",
                    "Double Cable",
                    "Double Curb",
                    "Draw Flat Cable",
                    "Elongated Cable",
                    "Fancy Double Link",
                    "Figaro",
                    "Flat Figaro",
                    "Flat Gooseneck",
                    "Franco",
                    "French Rope",
                    "Half Round Gooseneck",
                    "Heart",
                    "Herringbone",
                    "Hexagon",
                    "Hollow Mariner",
                    "Lace",
                    "Link",
                    "Mariner",
                    "Mesh",
                    "Mirror",
                    "Omega",
                    "Open Cable",
                    "Oval Curb",
                    "Palma",
                    "Parallel Curb",
                    "Popcorn",
                    "Puzzle",
                    "Raspberry",
                    "Rolo",
                    "Rope",
                    "Round Omega",
                    "San Marco",
                    "Serpentine",
                    "Side-by-Side",
                    "Singapore",
                    "Snake",
                    "Stocking Mesh",
                    "Strand",
                    "Tornado",
                    "Track",
                    "Triple Rope",
                    "Twister Rope",
                    "Twister Serpentine",
                    "Wheat",
                    "Wire Cable",
                    "Other",
                };
            }
            return ChainTypeList;
        }


        public static List<string> GemTypeList = GetGemTypeList();
        private static List<string> GetGemTypeList()
        {
            if (GemTypeList == null)
            {
                return new List<string>()
                {
                    "Kunzite",
                    "Lapis Lazuli",
                    "Malachite",
                    "Moissanite",
                    "Moonstone",
                    "Morganite",
                    "Mother of Pearl",
                    "Onyx",
                    "Opal",
                    "Other Stone",
                    "Pearl",
                    "Peridot",
                    "Quartz",
                    "Rhodolite",
                    "Rhodonite",
                    "Rose Quartz",
                    "Ruby",
                    "Sapphire",
                    "Sodalite",
                    "Spinel",
                    "Sunstone",
                    "Tanzanite",
                    "Tiger's Eye",
                    "Topaz",
                    "Tourmaline",
                    "Tsavorite",
                    "Turquoise",
                    "White Sapphire",
                    "Zircon",
                };
            }
            return GemTypeList;
        }


        public static List<string> GemTypeList1 = GetGemTypeList1();
        private static List<string> GetGemTypeList1()
        {
            if (GemTypeList1 == null)
            {
                return new List<string>()
                {
                "Goldstone",
                "Stabilized Turquoise",
                "Amazonite",
                "Red Agate",
                "Quartzite",
                "Aquamarine",
                "Beryl",
                "Created Peridot",
                "Iolite",
                "Caribbean Amber",
                "Created Tanzanite",
                "Prehnite",
                "Ammolite",
                "Pezzottaite",
                "Sodalite",
                "Created Amethyst",
                "Black Opal",
                "Gaspeite",
                "Created Emerald",
                "Tiger Eye",
                "Howlite",
                "Created Citrine",
                "Celestite",
                "Jade",
                "Jadeite",
                "Fluorite",
                "Mint Tourmaline",
                "Iron Pyrite",
                "Pink Topaz",
                "Yellow Topaz",
                "White Sapphire",
                "Goshenite",
                "Fire Citrine",
                "Cubic Zirconia",
                "Sapphire",
                "Padparadscha",
                "Petalite",
                "Hiddenite",
                "Tashmarine Diopside",
                "Lavender Chalcedony",
                "Andalusite",
                "Drusy Quartz",
                "Nephrite Jade",
                "Quartz",
                "Lapis Lazuli",
                "Jasper",
                "Moissanite",
                "Zandrite",
                "Grossular Garnet",
                "Jet",
                "Created Blue Sapphire",
                "Ethiopian Opal",
                "Yellow Sapphire",
                "Peridot",
                "Green Sapphire",
                "Red Diamond",
                "Created Alexandrite",
                "Marcasite",
                "Chrome Tourmaline",
                "Sphene",
                "Amethyst",
                "Created Sapphire",
                "Champagne Diamond",
                "Lava Rock",
                "Hickoryite",
                "Pink Tourmaline",
                "Heliodor",
                "Zoisite",
                "Brazanite",
                "Obsidian",
                "Green Chalcedony",
                "Moonstone",
                "Medusa Quartz",
                "Pearl",
                "Scapolite",
                "Blue Sapphire",
                "Created Yellow Sapphire",
                "Zircon Gemstone",
                "Tanzanite",
                "Purple Jade",
                "White Topaz",
                "Rhodonite",
                "London Blue Topaz",
                "Malachite",
                "Crystal",
                "Rubellite Tourmaline",
                "Diaspore",
                "Created Spinel",
                "Lemon Amber",
                "Tsavorite",
                "Cherry Amber",
                "Almandine Garnet",
                "Inclusion Citrine",
                "Almandite Garnet",
                "Andradite Garnet",
                "Granite",
                "Spessartite Garnet",
                "White Diamond",
                "Created Morganite",
                "Rhodolite Garnet",
                "Amblygonite",
                "Sandstone",
                "Dyed Howlite",
                "Aventurine",
                "Lemon Quartz",
                "Mandarin Garnet",
                "Pink Chalcedony",
                "Sea Blue Chalcedony",
                "Peruvian Opal",
                "Created Garnet",
                "Watermelon Tourmaline",
                "Diamond",
                "Green Quartz",
                "Rhodochrosite",
                "Botswana Agate",
                "Blue Topaz",
                "Created Turquoise",
                "Topaz",
                "Boulder Opal",
                "Multi Color Amber",
                "Green Amber",
                "Alexandrite",
                "Demantoid Garnet",
                "Amber",
                "Seraphinite",
                "Larimar",
                "Kyanite",
                "Rutilated Quartz",
                "Star Ruby",
                "Sphalerite",
                "Created White Sapphire",
                "Blue Chalcedony",
                "Epidote",
                "Chrysocolla",
                "Hematite",
                "Orange Chalcedony",
                "Spectrolite",
                "Danburite",
                "Green Amethyst",
                "Swedish Slag",
                "Sardonyx",
                "Pink Sapphire",
                "Serpentine",
                "Butterscotch Amber",
                "Labradorite",
                "Golden Topaz",
                "Hessonite",
                "Charoite",
                "Morganite",
                "Pink Quartz",
                "Moldavite",
                "Yellow Scapolite",
                "Smoky Topaz",
                "Agate",
                "Star Sapphire",
                "Created Pink Sapphire",
                "Yellow Diamond",
                "Chalcedony",
                "Garnet",
                "Blue Diamond",
                "Created Zircon Gemstone",
                "Yttrium Aluminium Garnet",
                "Madeira Citrine",
                "Copper Sunstone",
                "Green Tourmaline",
                "Turquoise",
                "Blue Zircon",
                "Strontium Titanate",
                "Kunzite",
                "White Agate",
                "Paraiba Tourmaline",
                "Sugilite",
                "Carnelian",
                "Azurite",
                "Cats Eye",
                "Smoky Quartz",
                "Fuchsite",
                "Magnesite",
                "Blue Amber",
                "Spinel",
                "Ouro Verde Quartz",
                "Onyx",
                "Created Topaz",
                "Pink Amethyst",
                "Paraiba",
                "Created Pink Tourmaline",
                "Chrysoprase",
                "Orange Agate",
                "Blue Agate",
                "Imperial Topaz",
                "Ametrine",
                "Green Garnet",
                "Black Diamond",
                "Cinnabar",
                "Diopside",
                "Prasiolite Quartz",
                "Hauyne",
                "Created Padparadscha",
                "Lab Grown Diamond",
                "Paraiba Topaz",
                "Fire Opal",
                "Sunstone Feldspar",
                "Selenite",
                "Indicolite Tourmaline",
                "Chrome Diopside",
                "Brown Diamond",
                "Bloodstone",
                "Apatite",
                "Tourmaline",
                "Rose Quartz",
                "Rubelite",
                "Opal",
                "Blue Opal",
                "Created Aquamarine",
                "Cream Amber",
                "Pyrope Garnet",
                "Pink Diamond",
                "Emerald",
                "Citrine",
                "Purple Spinel",
                "Ruby",
                "Indicolite",
                "Honey Amber",
                "Compressed Turquoise",
                "Dumortierite",
                "Gypsum",
                "Rose De France",
                "Created Opal",
                "Calcite",
                "Created Quartz",
                "Corundum",
                "Coral",
                "Triplet Opal",
                "Chrysoberyl",
                "Created Ruby",
                "Swiss Blue Topaz",
                "No Gemstone",
                "Goldstone",
                "Created Cubic Zirconia",
                "Created Pearl",
                "Created Diamond",
                "Heliotrope",
                "Rhinestone",
                "Created Coral",
                "Created Moissanite",
                "Agate",
                "agate",
                "alexandrite",
                "almandite-garnet",
                "amazonite",
                "amblygonite",
                "amethyst",
                "ametrine",
                "ammolite",
                "andalusite",
                "andradite-garnet",
                "apatite",
                "aquamarine",
                "aventurine",
                "azurite",
                "beryl",
                "black-diamond",
                "black-opal",
                "bloodstone",
                "blue-chalcedony",
                "blue-diamond",
                "blue-sapphire",
                "blue-topaz",
                "blue-zircon",
                "boulder-opal",
                "brazanite",
                "brown-diamond",
                "carnelian",
                "cats-eye",
                "chalcedony",
                "champagne-diamond",
                "chrome-diopside",
                "chrysoberyl",
                "chrysoprase",
                "cinnabar",
                "citrine",
                "copper-sunstone",
                "created-alexandrite",
                "created-emerald",
                "created-opal",
                "created-ruby",
                "created-sapphire",
                "cubic-zirconia",
                "danburite",
                "demantoid-garnet",
                "diamond",
                "diaspore",
                "diopside",
                "drusy-quartz",
                "emerald",
                "fire-citrine",
                "fire-opal",
                "fluorite",
                "garnet",
                "golden-topaz",
                "goshenite",
                "green-chalcedony",
                "green-garnet",
                "green-quartz",
                "green-sapphire",
                "green-tourmaline",
                "grossular-garnet",
                "hauyne",
                "heliodor",
                "hessonite",
                "hiddenite",
                "howlite",
                "imperial-topaz",
                "indicolite",
                "indicolite-tourmaline",
                "iolite",
                "iron-pyrite",
                "jade",
                "jasper",
                "jet",
                "kunzite",
                "kyanite",
                "labradorite",
                "lapis-lazuli",
                "lavender-chalcedony",
                "lemon-quartz",
                "london-blue-topaz",
                "madeira-citrine",
                "malachite",
                "mandarin-garnet",
                "marcasite",
                "medusa-quartz",
                "mint-tourmaline",
                "moissanite",
                "moldovite",
                "moonstone",
                "morganite",
                "multi",
                "mystic-topaz",
                "onyx",
                "opal",
                "orange-chalcedony",
                "ouro-verde-quartz",
                "padparadscha",
                "paraiba-topaz",
                "paraiba-tourmaline",
                "peridot",
                "petalite",
                "pezzottaite",
                "pink-chalcedony",
                "pink-diamond",
                "pink-quartz",
                "pink-sapphire",
                "pink-topaz",
                "pink-tourmaline",
                "prasiolite-quartz",
                "prehnite",
                "purple-spinel",
                "pyrope-garnet",
                "quartz",
                "red-diamond",
                "rhodochrosite",
                "rhodolite-garnet",
                "rose-quartz",
                "rubellite-tourmaline",
                "ruby",
                "rutilated-quartz",
                "sapphire",
                "sardonyx",
                "scapolite",
                "serpentine",
                "shanseres",
                "smoky-quartz",
                "smoky-topaz",
                "sodalite",
                "spessartite-garnet",
                "sphalerite",
                "sphene",
                "spinel",
                "star-sapphire",
                "strontium-titanate",
                "sunstone-feldspar",
                "swiss-blue-topaz",
                "tanzanite",
                "tashmarine-diopside",
                "tiger-eye",
                "topaz",
                "tourmaline",
                "tsavorite",
                "turquoise",
                "watermelon-tourmaline",
                "white-diamond",
                "white-topaz",
                "yellow-diamond",
                "yellow-scapolite",
                "yellow-topaz",
                "yttrium-aluminium-garnet",
                "zandrite",
                "zircon-gemstone",
                "zoisite",
                "Alexandrite",
                "Shungite",
                "Zircon",
                "Andesine",
                "Tigers Eye",
                "Simulated Pearl",
                "Onynx",
                "No Gemstone",
                };
            }
            return GemTypeList1;
        }


        public static List<string> BackFindingList = GetBackFindingList();
        private static List<string> GetBackFindingList()
        {
            if (BackFindingList == null)
            {
                return new()
                {
                    "Click Top","Clip-On","Clutchless","Continuous Hoop","Endless","Euro Wire","Fishhook",
                    "French Wire","Hinged","Hoop","La Pousette","Latch Back","Lever Back","Omega Back",
                    "Other","Post With Friction Back","Post With Snap Down","Push Back","Screw Back",
                    "Shepherds Hook","Thread Through"
                };
            }
            return BackFindingList;
        }


        public static List<string> BraceletList = GetBraceletList();

        private static List<string> GetBraceletList()
        {
            if (BraceletList == null)
            {
                return new()
                {
                    "Bangle",
                    "Bead",
                    "Cable",
                    "Cape Cod",
                    "Chain",
                    "Charm",
                    "Cord",
                    "Cuff",
                    "ID Bracelets",
                    "Link",
                    "Multi-Strand",
                    "Other",
                    "Stackable",
                    "Stretch",
                    "Tennis Bracelets",
                    "Wrap",
                };
            }
            return BraceletList;
        }


        public static List<string> StoneClarityList = GetStoneClarityList();
        private static List<string> GetStoneClarityList()
        {
            if (StoneClarityList == null)
            {
                return new()
                {
                    //"Slightly Included",
                    //"Very Slightly Included",
                    //"Included",
                    //"Internally Flawless",
                    //"Flawless",
                    //"Very Very Slightly Included",
                    "FL",
                    "I1",
                    "I2",
                    "I3",
                    "IF",
                    "SI1",
                    "SI2",
                    "VS1",
                    "VS2",
                    "VVS1",
                    "VVS2",
                    //"SI2-I1",
                    //"VVS1-VVS2",
                    //"I2-I3",
                    //"VS2-SI1",
                    //"VVS2-VS1",
                    //"I1-I2",
                    //"SI1-SI2",
                    //"VS1-VS2",
                    //"Slightly Included",
                    //"Very Very Slightly Included",
                };
            }
            return StoneClarityList;
        }


        public static List<string> ProductIdTypeList = GetProductIdTypeList();

        private static List<string> GetProductIdTypeList()
        {
            if (ProductIdTypeList == null)
            {
                return new()
                {
                    "EAN",
                    "GTIN",
                    "ISBN",
                    "UPC",
                    "ASIN"
                };
            }
            return ProductIdTypeList;
        }


        public static List<string> MaterialTypeList = GetMaterialTypeList();

        private static List<string> GetMaterialTypeList()
        {
            if (MaterialTypeList == null)
            {
                return new()
                {
                    "agarwood",
                    "alumide",
                    "bamboo",
                    "bodhi",
                    "ceramic",
                    "coral",
                    "crystal",
                    "enamel",
                    "epoxy",
                    "gemstone",
                    "glass",
                    "Gold Phoebe",
                    "horn",
                    "leather",
                    "metal",
                    "mineral-powder",
                    "mother-of-pearl",
                    "natural-fiber",
                    "paper",
                    "pearl",
                    "plaster",
                    "plastic",
                    "polyamide",
                    "resin",
                    "rhinestone",
                    "rosewood",
                    "rubber",
                    "sandalwood",
                    "shell",
                    "silicone",
                    "stone",
                    "synthetic-fiber",
                    "synthetic-resin",
                    "wood",
                };
            }
            return MaterialTypeList;
        }


        public static List<string> EarringTypeList = GetEarringTypeList();

        private static List<string> GetEarringTypeList()
        {
            if (EarringTypeList == null)
            {
                return new()
                {
                    "Chandelier",
                    "Dangling",
                    "Drop",
                    "Fake Gauge",
                    "Hoop",
                    "Stud",
                    "Teardrop",
                    "Threader",
                };
            }
            return EarringTypeList;
        }


        public static List<string> JewelrySettingList = GetJewelrySettingList();

        private static List<string> GetJewelrySettingList()
        {
            if (JewelrySettingList == null)
            {
                return new()
                {
                    "2-prong",
                    "3-prong",
                    "3-stone",
                    "4-prong",
                    "6-prong",
                    "bar",
                    "basket",
                    "bead-set",
                    "bezel",
                    "bypass",
                    "cathedral",
                    "channel",
                    "classic solitaire",
                    "cluster",
                    "cocktail",
                    "designer",
                    "eternity band",
                    "flat top",
                    "flush",
                    "freedom",
                    "guard",
                    "gypsy",
                    "half bezel",
                    "halo",
                    "hand-wired",
                    "illusion",
                    "invisible",
                    "other",
                    "pave",
                    "prongs",
                    "semimount",
                    "shared prong",
                    "side stone",
                    "solitaire",
                    "stackable",
                    "strung",
                    "tension mount",
                    "trellis",
                    "tricolor rolling",
                    "two-tone freedom",
                    "v-prong",
                    "waterfall",
                    "wrap",
                };
            }
            return JewelrySettingList;
        }


        public static List<string> ClaspList = GetClaspList();

        private static List<string> GetClaspList()
        {
            if (ClaspList == null)
            {
                return new()
                {
                    "Ball",
                    "Barrel Clasp",
                    "Box",
                    "Buckle",
                    "Button Fly",
                    "Button Toggle",
                    "Button-End",
                    "Concealed",
                    "D Ring",
                    "Elastic",
                    "Fishhook",
                    "Flat Solid Buckle",
                    "Fold Over",
                    "Hidden Safety Clasp",
                    "Hook & Eye Clasp",
                    "Hook & Loop Clasp",
                    "Invisible Double-Locking",
                    "Lobster Claw",
                    "Magnetic Clasp",
                    "No Clasp",
                    "Other",
                    "Push Button",
                    "S Hook",
                    "Screw",
                    "Side Lock",
                    "Snaps",
                    "Spring Ring",
                    "Toggle",
                };
            }
            return ClaspList;
        }


        public static List<string> NecklaceList = GetNecklaceList();

        private static List<string> GetNecklaceList()
        {
            if (NecklaceList == null)
            {
                return new()
                {
                    "Bib",
                    "Cascade",
                    "Chain",
                    "Choker",
                    "Collar",
                    "Initial",
                    "Lariat",
                    "Locket",
                    "Multi Strand",
                    "Other",
                    "Pendant",
                    "Sentiment & Symbols",
                    "Single Strand",
                    "Solitaire",
                    "Y-necklace",
                };
            }
            return NecklaceList;
        }


        public static List<string> StoneCutList = GetStoneCutList();

        private static List<string> GetStoneCutList()
        {
            if (StoneCutList == null)
            {
                return new() {
                "Asscher cut",
                "Baguette cut",
                "Cushion cut",
                "Emerald cut",
                "Fancy cut",
                "Heart cut",
                "Marquise cut",
                "Other cut",
                "Oval cut",
                "Pear cut",
                "Princess cut",
                "Radiant cut",
                "Round Brilliant cut",
                "Square cut",
                };
            }
            return StoneCutList;
        }


        public static List<string> StoneColorList = GetStoneColorList();

        private static List<string> GetStoneColorList()
        {
            if (StoneColorList == null)
            {
                return new()
                {
                    "D",
                    "D-E",
                    "D-F",
                    "E",
                    "E-F",
                    "F",
                    "F-G",
                    "G",
                    "G-H",
                    "H",
                    "H-I",
                    "I",
                    "I-J",
                    "J",
                    "J-K",
                    "K",
                    "K-L",
                    "L",
                    "L-M",
                    "M",
                    "N-R",
                    "Other",
                    "S-Z",
                };
            }
            return StoneColorList;
        }


        public static List<string> MetalTypeList = GetMetalTypeList();

        private static List<string> GetMetalTypeList()
        {
            if (MetalTypeList == null)
            {
                return new()
                {
                    "Black Gold",
                    "Black Gold-plated",
                    "Brass",
                    "Copper",
                    "Goldtone",
                    "Iron",
                    "Nickel",
                    "No Metal",
                    "Other Metal",
                    "Palladium",
                    "Pewter",
                    "Platinaire",
                    "Platinum",
                    "Rhodium",
                    "Rose Gold",
                    "Rose Gold-plated",
                    "Silver-plated",
                    "Silvertone",
                    "Stainless Steel",
                    "Sterling Silver",
                    "Titanium",
                    "Tri-Tone Gold",
                    "Tungsten",
                    "Two-Tone Gold",
                    "White Gold",
                    "White Gold-plated",
                    "Yellow Gold",
                    "Yellow Gold-plated",
                };
            }
            return MetalTypeList;
        }


        public static List<string> GemstoneShapeList = GetGemstoneShapeList();

        private static List<string> GetGemstoneShapeList()
        {
            if (GemstoneShapeList == null)
            {
                return new()
        {
            "Asscher",
            "Baguette",
            "Cushion",
            "Emerald",
            "Heart",
            "Marquise",
            "Oval",
            "Pear",
            "Princess",
            "Radiant",
            "Round",
        };
            }
            return GemstoneShapeList;
        }


        public static List<string> ColorCategoryList = GetColorCategoryList();

        private static List<string> GetColorCategoryList()
        {
            if (ColorCategoryList == null)
            {
                return new()
        {
            "Beige",
            "Black",
            "Blue",
            "Bronze",
            "Brown",
            "Clear",
            "Gold",
            "Gray",
            "Green",
            "Multi-color",
            "Off-White",
            "Orange",
            "Pink",
            "Purple",
            "Red",
            "Silver",
            "White",
            "Yellow",
        };
            }
            return ColorCategoryList;
        }


        public static Dictionary<string, double> ShippingWeightDictionary = GetShippingWeightDictionary();

        private static Dictionary<string, double> GetShippingWeightDictionary()
        {
            if (ShippingWeightDictionary == null)
            {
                return new(){
            {"LB", 1 },
            {"KG", 2.20462 },
            {"GR", 0.00220462 },
            {"Hundredths Pounds", 0.01 },
            {"MG" , 0.00000220462 },
            {"Tons", 2000 },
            {"OZ", 0.25 }
        };
            }
            return ShippingWeightDictionary;
        }


        public static Dictionary<string, string> SmallPartsWarningsDictionary = GetSmallPartsWarningsDictionary();

        private static Dictionary<string, string> GetSmallPartsWarningsDictionary()
        {
            if (SmallPartsWarningsDictionary == null)
            {
                return new(){
                    {"NoWarningApplicable", "0 - No warning applicable" },
                    {"ChokingHazardIsASmallBall","1 - Choking hazard is a small ball"},
                    {"ChokingHazardContainsSmallBall", "2 - Choking hazard contains small ball" },
                    {"ChokingHazardSmallParts", "3 - Choking hazard contains small parts" },
                    {"ChokingHazardBalloon" , "4 - Choking hazard balloon" },
                    {"ChokingHazardIsAMarble", "5 - Choking hazard is a marble" },
                    {"ContainsSmallMagnets", "6 - Choking hazard contains a marble" },
                    {"ChokingHazardContainsAMarble", "6 - Choking hazard contains a marble" }
                };
            }
            return SmallPartsWarningsDictionary;
        }


        public static Dictionary<string, string> AgeGroupDictionary = GetAgeGroupDictionary();

        private static Dictionary<string, string> GetAgeGroupDictionary()
        {
            if (AgeGroupDictionary == null)
            {
                return new(){
            {"Unisex Adult", "Adult" },
            {"Unisex Child", "Child" },
            {"Unisex Baby", "Toddler" },
            {"Baby Boys", "Toddler" },
            {"Baby Girls", "Toddler" },
            {"Men", "Adult" },
            {"Women", "Adult" },
            {"Boys", "Teen" },
            {"Girls", "Teen" },
        };
            }
            return AgeGroupDictionary;
        }


        public static Dictionary<string, string> ColorCategoryDictionary = GetColorCategoryDictionary();

        private static Dictionary<string, string> GetColorCategoryDictionary()
        {
            if (ColorCategoryDictionary == null)
            {
                return new(){
            {"Bronze", "Bronze" },
            {"Beige", "Beige" },
            {"Black","Black"},
            {"Blue", "Blue" },
            {"Brown" , "Brown" },
            {"Clear", "Clear" },
            {"Gold", "Gold" },
            {"Grey", "Gray" },
            {"Green", "Green" },
            {"Multi", "Multi-color" },
            {"Off-white", "Off-White" },
            {"Orange", "Orange" },
            {"Pink", "Pink" },
            {"Purple", "Purple" },
            {"Red", "Red" },
            {"Silver", "Silver" },
            {"White", "White" },
            {"Yellow", "Yellow" },
            {"Ivory", "" },
            {"Metallic","" },
            {"Turquoise","" },
        };
            }
            return ColorCategoryDictionary;
        }


        public static Dictionary<string, string> ChainTypeDictionary = GetChainTypeDictionary();

        private static Dictionary<string, string> GetChainTypeDictionary()
        {
            if (ChainTypeDictionary == null)
            {
                return new(){
            {"Anchor", "Anchor" },
            {"Ball","Ball"},
            {"Blue", "Bar" },
            {"Basket Weave" , "Basketweave" },
            {"Bead", "Bead" },
            {"Belcher", "Belcher" },
            {"Bevelled Herringbone", "Beveled Herringbone" },
            {"Bismark", "Bismark" },
            {"Boston Link", "Boston Link" },
            {"Box", "Box" },
            {"Bow Tie Figaro", "" },
            {"Byzantine", "Byzantine" },
            {"C-Chain", "C-Chain" },
            {"Cable", "Cable" },
            {"Cable Wire", "Cable Wire" },
            {"Cascade", "Cascade" },
            {"Cobra", "Cobra" },
            {"Curb","Curb" },
            {"Double Cable","Double Cable" },
            {"Double Curb","Double Curb" },
            {"Draw Flat Cable","Draw Flat Cable" },
            {"Elongated Cable","Elongated Cable" },
            {"Fancy Double Link","Fancy Double Link" },
            {"Fancy","Fancy Double Link" },
            {"Figaro","Figaro" },
            {"Flat Gooseneck","Flat Gooseneck" },
            {"Franco","Franco" },
            {"French Rope","French Rope" },
            {"Half Round Gooseneck","Half Round Gooseneck" },
            {"Heart","Heart" },
            {"Herringbone","Herringbone" },
            {"Hollow Mariner","Hollow Mariner" },
            {"Lace","Lace" },
            {"Link","Link" },
            {"Mariner","Mariner" },
            {"Mesh","Mesh" },
            {"Mirror","Mirror" },
            {"Omega","Omega" },
            {"Round Omega","Round Omega" },
            {"Open Cable","Open Cable" },
            {"Other Chain Type","Other" },
            {"Popcorn","Popcorn" },
            {"Puzzle","Puzzle" },
            {"Raspberry","Raspberry" },
            {"Rolo","Rolo" },
            {"Rope","Rope" },
            {"Serpentine","Serpentine" },
            {"Side By Side","Side-by-Side" },
            {"Singapore","Singapore" },
            {"Snake","Snake" },
            {"Spiga","Cuban Link" },
            {"Stocking Mesh","Stocking Mesh" },
            {"Strand","Strand" },
            {"Tornado","Tornado" },
            {"Track","Track" },
            {"Triple Rope","Triple Rope" },
            {"Twister Rope","Twister Rope" },
            {"Twister Serpentine","Twister Serpentine" },
            {"Wheat","Wheat" },
            {"Wire Cable","Wire Cable" },
            //{"", "Cord" },
            //{"Metallic","Crisscross" },
            //{"Metallic","Diamond Cut Figaro" },
            //{"Metallic","Flat Figaro" },
            //{"Metallic","Hexagon" },
            //{"Metallic","Oval Curb" },
            //{"Metallic","Palma" },
            //{"Metallic","Parallel Curb" },
            //{"Metallic","San Marco" },
        };
            }
            return ChainTypeDictionary;
        }


        public static Dictionary<string, string> JewelrySettingDictionary = GetJewelrySettingDictionary();

        private static Dictionary<string, string> GetJewelrySettingDictionary()
        {
            if (JewelrySettingDictionary == null)
            {
                return new(){
            {"2-Prong-setting", "2-prong" },
            {"3-Prong-Setting","3-prong"},
            {"4-Prong-Setting", "4-prong" },
            {"6-Prong-Setting" , "6-prong" },
            {"8-Prong-Setting" , "6-prong" },
            {"Band-Setting" , "" },
            {"Bar-Setting", "bar" },
            {"Bead-Set", "bead-set" },
            {"Bezel-Setting", "bezel" },
            {"Bypass-Setting", "bypass" },
            //{"", "basket" },
            //{"", "cathedral" },
            {"Channel-Setting", "channel" },
            {"Classic-Solitaire Setting", "classic solitaire" },
            {"Cluster-Setting", "cluster" },
            {"Cocktail-Setting", "cocktail" },
            {"Cup-Setting", "" },
            {"Designer-Setting", "designer" },
            {"Dome-Setting", "" },
            {"Eternity-Band-Setting", "eternity band" },
            {"Fishtail-Setting", "" },
            {"Flat-Top-Setting", "flat top" },
            {"Floating-Setting", "" },
            {"Flush-Setting","flush" },
            {"Freedom-Setting","freedom" },
            {"French-Setting","" },
            {"Full-Eternity-Setting","" },
            {"Guard-Setting","guard" },
            {"Gypsy-Setting","gypsy" },
            {"Half-Bezel-Setting","half bezel" },
            {"Half-Eternity-Setting","" },
            {"Halo-Setting","halo" },
            {"Illusion-Setting","illusion" },
            {"Invisible-Setting","invisible" },
            {"Micro-Pave-Setting","" },
            {"Milgrain-Accents-Setting","" },
            {"Other-Setting-Type","other" },
            {"Pavé-Setting","pave" },
            {"Prong-Setting","prongs" },
            {"Rub-Set-Setting","" },
            {"Semi-Mount-Setting","" },
            {"Shared-Prong-Setting","shared prong" },
            {"Side-Stone-Setting","side stone" },
            {"Solitaire-Setting","solitaire" },
            {"Split-Shank-Setting","" },
            {"Stackable-Setting","stackable" },
            {"Tension-Mount-Setting","tension mount" },
            {"Three-Stone-Setting","" },
            //{"","strung" },
            {"Trellis-Setting","trellis" },
            {"Tricolor-Rolling-Setting","tricolor rolling" },
            {"Two-Tone-Freedom-Setting","two-tone freedom" },
            {"V-Prong-Setting","v-prong" },
            {"Waterfall-Setting","waterfall" },
            {"Wrap-Setting","wrap" }
        };
            }
            return JewelrySettingDictionary;
        }


        public static Dictionary<string, string> GemDictionary = GetGemDictionary();

        private static Dictionary<string, string> GetGemDictionary()
        {
            if (GemDictionary == null)
            {
                return new(){
            {"Alexandrite", "Alexandrite" },
            {"Created Alexandrite", "Alexandrite" },
            {"amber","Amber"},
            {"Amber","Amber"},
            {"Multi Color Amber","Amber"},
            {"Lemon Amber","Amber"},
            {"Blue Amber","Amber"},
            {"Butterscotch Amber","Amber"},
            {"Green Amber","Amber"},
            {"Cherry Amber","Amber"},
            {"Caribbean Amber","Amber"},
            {"Cream Amber","Amber"},
            {"Honey Amber","Amber"},
            {"Amethyst", "Amethyst" },
            {"Green Amethyst", "Amethyst" },
            {"Created Amethyst", "Amethyst" },
            {"Pink Amethyst", "Amethyst" },
            {"ametrine" , "Ametrine" },
            {"Ametrine" , "Ametrine" },
            {"Aquamarine", "Aquamarine" },
            {"Created Aquamarine", "Aquamarine" },
            {"Beryl", "Beryl" },
            {"carnelian", "Carnelian" },
            {"Carnelian", "Carnelian" },
            {"Cats Eye", "Cat's Eye" },
            {"chalcedony", "Chalcedony" },
            {"Chalcedony", "Chalcedony" },
            {"Lavender Chalcedony", "Chalcedony" },
            {"Sea Blue Chalcedony", "Chalcedony" },
            {"Pink Chalcedony", "Chalcedony" },
            {"Blue Chalcedony", "Chalcedony" },
            {"Green Chalcedony", "Chalcedony" },
            {"Orange Chalcedony", "Chalcedony" },
            {"diopside", "Chrome Diopside" },
            {"Diopside", "Chrome Diopside" },
            {"Chrome Diopside", "Chrome Diopside" },
            {"Tashmarine Diopside", "Chrome Diopside" },
            {"Created Citrine", "Citrine" },
            {"Inclusion Citrine", "Citrine" },
            {"Fire Citrine", "Citrine" },
            {"Madeira Citrine", "Citrine" },
            {"Citrine", "Citrine" },
            {"Coral", "Coral" },
            {"Crystal", "Crystal" },
            {"Cubic Zirconia", "Cubic Zirconia" },
            {"Diamond", "Diamond" },
            {"Lab Grown Diamond", "Diamond" },
            {"Blue Diamond", "Diamond" },
            {"Yellow Diamond", "Diamond" },
            {"White Diamond", "Diamond" },
            {"Red Diamond", "Diamond" },
            {"Champagne Diamond", "Diamond" },
            {"Black Diamond", "Diamond" },
            {"Brown Diamond", "Diamond" },
            {"Pink Diamond", "Diamond" },
            {"Created Emerald", "Emerald" },
            {"Emerald", "Emerald" },
            {"garnet", "Garnet" },
            {"Garnet", "Garnet" },
            {"Created Garnet", "Garnet" },
            {"Mandarin Garnet", "Garnet" },
            {"Almandite Garnet", "Garnet" },
            {"Almandine Garnet", "Garnet" },
            {"Andradite Garnet", "Garnet" },
            {"Spessartite Garnet", "Garnet" },
            {"Rhodolite Garnet", "Garnet" },
            {"Grossular Garnet", "Garnet" },
            {"Demantoid Garnet", "Garnet" },
            {"Yttrium Aluminium Garnet", "Garnet" },
            {"Pyrope Garnet", "Garnet" },
            {"mandarin-garnet", "Garnet" },
            {"Green Garnet", "Garnet" },
            {"indicolite", "Iolite" },
            {"Indicolite", "Iolite" },
            {"Iolite", "Iolite" },
            {"Jade", "Jade" },
            {"Purple Jade", "Jade" },
            {"Nephrite Jade", "Jade" },
            {"Jadeite", "Jade" },
            {"kunzite", "Kunzite" },
            {"Kunzite", "Kunzite" },
            {"Lapis Lazuli", "Lapis Lazuli" },
            {"Malachite", "Malachite" },
            {"Moissanite", "Moissanite" },
            {"Moonstone", "Moonstone" },
            {"morganite", "Morganite" },
            {"Morganite", "Morganite" },
            {"Created Morganite", "Morganite" },
            //{"", "Mother of Pearl" },
            {"Pearl","Pearl" },
            {"onyx", "Onyx" },
            {"Onyx", "Onyx" },
            {"Sardonyx", "Onyx" },
            {"opal","Opal" },
            {"Created Opal","Opal" },
            {"Opal","Opal" },
            {"Boulder Opal","Opal" },
            {"Peruvian Opal", "Opal" },
            {"Fire Opal", "Opal" },
            {"Ethiopian Opal", "Opal" },
            {"Triplet Opal","Opal" },
            {"boulder-opal","Opal" },
            {"Black Opal","Opal" },
            {"Blue Opal","Opal" },
            {"Created Peridot","Peridot" },
            {"Peridot","Peridot" },
            {"Quartz","Quartz" },
            {"Created Quartz","Quartz" },
            {"Smoky Quartz","Quartz" },
            {"Rutilated Quartz","Quartz" },
            {"Quartzite","Quartz" },
            {"Lemon Quartz","Quartz" },
            {"Pink Quartz","Quartz" },
            {"Medusa Quartz","Quartz" },
            {"Green Quartz","Quartz" },
            {"Drusy Quartz","Quartz" },
            {"smoky-quartz","Quartz" },
            {"Ouro Verde Quartz","Quartz" },
            {"Prasiolite Quartz","Quartz" },
            {"Rose Quartz","Rose Quartz" },
            //{"","Rhodolite" },
            {"Rhodonite","Rhodonite" },
            {"ruby","Ruby" },
            {"Created Ruby","Ruby" },
            {"Ruby","Ruby" },
            {"Sapphire","Sapphire" },
            {"Star Sapphire","Sapphire" },
            {"Created White Sapphire","Sapphire" },
            {"Created Yellow Sapphire","Sapphire" },
            {"Blue Sapphire","Sapphire" },
            {"Created Sapphire","Sapphire" },
            {"Created Blue Sapphire","Sapphire" },
            {"Yellow Sapphire","Sapphire" },
            {"Green Sapphire","Sapphire" },
            {"Pink Sapphire","Sapphire" },
            {"Created Pink Sapphire","Sapphire" },
            {"Star Ruby","Ruby" },
            {"Sodalite","Sodalite" },
            {"spinel","Spinel" },
            {"Purple Spinel","Spinel" },
            {"Spinel","Spinel" },
            {"Created Spinel","Spinel" },
            {"Sunstone Feldspar","Sunstone" },
            {"Copper Sunstone","Sunstone" },
            {"Created Tanzanite","Tanzanite" },
            {"Tanzanite","Tanzanite" },
            {"Tiger Eye","Tiger's Eye" },
            {"topaz","Topaz" },
            {"Topaz","Topaz" },
            {"Created Topaz","Topaz" },
            {"Smoky Topaz","Topaz" },
            {"Paraiba Topaz","Topaz" },
            {"Blue Topaz","Topaz" },
            {"Yellow Topaz","Topaz" },
            {"White Topaz","Topaz" },
            {"Pink Topaz","Topaz" },
            {"Golden Topaz","Topaz" },
            {"London Blue Topaz","Topaz" },
            {"Imperial Topaz","Topaz" },
            {"Swiss Blue Topaz","Topaz" },
            {"mystic-topaz","Topaz" },
            {"blue-topaz","Topaz" },
            {"tourmaline", "Tourmaline" },
            {"Tourmaline", "Tourmaline" },
            {"Created Pink Tourmaline", "Tourmaline" },
            {"Rubellite Tourmaline","Tourmaline" },
            {"Pink Tourmaline","Tourmaline" },
            {"Watermelon Tourmaline", "Tourmaline" },
            {"Green Tourmaline","Tourmaline" },
            {"Chrome Tourmaline", "Tourmaline" },
            {"Mint Tourmaline","Tourmaline" },
            {"Paraiba Tourmaline","Tourmaline" },
            {"Indicolite Tourmaline", "Tourmaline" },
            {"Tsavorite","Tsavorite" },
            {"turquoise","Turquoise" },
            {"Turquoise","Turquoise" },
            {"Created Turquoise","Turquoise" },
            {"Compressed Turquoise","Turquoise" },
            {"Stabilized Turquoise","Turquoise" },
            {"White Sapphire","White Sapphire" },
            {"blue-zircon","Zircon" },
            {"Blue Zircon","Zircon" },
            {"Zircon Gemstone","Zircon" },
            {"Created Zircon Gemstone","Zircon" },

            {"agate","Other Stone" },
            {"Rubelite","Other Stone" },
            {"Orange Agate","Other Stone" },
            {"Paraiba","Other Stone" },
            {"Azurite","Other Stone" },
            {"Agate","Other Stone" },
            {"White Agate","Other Stone" },
            {"not known","Other Stone" },
            {"not-applicable","Other Stone" },
            {"orange-agate","Other Stone" },
            {"Amazonite","Other Stone" },
            {"botswana-agate","Other Stone" },
            {"calcite","Other Stone" },
            {"Calcite","Other Stone" },
            {"bloodstone","Other Stone" },
            {"Bloodstone","Other Stone" },
            {"Blue Agate","Other Stone" },
            {"Fluorite","Other Stone" },
            {"Obsidian","Other Stone" },
            {"Celestite","Other Stone" },
            {"charoite","Other Stone" },
            {"Charoite","Other Stone" },
            {"chrysoberyl","Other Stone" },
            {"Chrysoberyl","Other Stone" },
            {"chrysocolla","Other Stone" },
            {"Chrysocolla","Other Stone" },
            {"chrysoprase","Other Stone" },
            {"Chrysoprase","Other Stone" },
            {"cinnabar","Other Stone" },
            {"Cinnabar","Other Stone" },
            {"corundum","Other Stone" },
            {"Corundum","Other Stone" },
            {"danburite","Other Stone" },
            {"Danburite","Other Stone" },
            {"Padparadscha","Other Stone" },
            {"Created Padparadscha","Other Stone" },
            {"Diaspore","Other Stone" },
            {"dumortierite","Other Stone" },
            {"Dumortierite","Other Stone" },
            {"Dyed Howlite","Other Stone" },
            {"epidote","Other Stone" },
            {"Epidote","Other Stone" },
            {"fuchsite","Other Stone" },
            {"Fuchsite","Other Stone" },
            {"Gaspeite","Other Stone" },
            {"Goshenite","Other Stone" },
            {"Petalite","Other Stone" },
            {"gypsum","Other Stone" },
            {"Gypsum","Other Stone" },
            {"hauyne","Other Stone" },
            {"Hauyne","Other Stone" },
            {"Heliodor","Other Stone" },
            {"hematite","Other Stone" },
            {"Hematite","Other Stone" },
            {"hessonite","Other Stone" },
            {"Hessonite","Other Stone" },
            {"Hickoryite","Other Stone" },
            {"Hiddenite","Other Stone" },
            {"Howlite","Other Stone" },
            {"Iron Pyrite","Other Stone" },
            {"Jasper","Other Stone" },
            {"Jet","Other Stone" },
            {"labradorite","Other Stone" },
            {"Labradorite","Other Stone" },
            {"moldavite","Other Stone" },
            {"Moldavite","Other Stone" },
            {"magnesite","Other Stone" },
            {"Magnesite","Other Stone" },
            {"Marcasite","Other Stone" },
            {"Pezzottaite","Other Stone" },
            {"Prehnite","Other Stone" },
            {"Red Agate","Other Stone" },
            {"Rhodochrosite","Other Stone" },
            {"Rose De France","Other Stone" },
            {"sardonyx","Other Stone" },
            {"Scapolite","Other Stone" },
            {"Yellow Scapolite","Other Stone" },
            {"selenite","Other Stone" },
            {"Selenite","Other Stone" },
            {"white-agate","Other Stone" },
            {"serpentine","Other Stone" },
            {"Serpentine","Other Stone" },
            {"spectrolite","Other Stone" },
            {"Spectrolite","Other Stone" },
            {"Sphene","Other Stone" },
            {"Strontium Titanate","Other Stone" },
            {"Swedish Slag","Other Stone" },
            {"Zandrite","Other Stone" },
            {"zircon-gemstone","Other Stone" },
            {"Zoisite","Other Stone" },
            {"sugilite","Other Stone" },
            {"Sugilite","Other Stone" },
            {"Lava Rock","Other Stone" },
            {"seraphinite","Other Stone" },


            {"Apatite","Other Stone" },
            {"Sphalerite","Other Stone" },
            {"Kyanite","Other Stone" },
            {"Larimar","Other Stone" },
            {"Seraphinite","Other Stone" },
            {"Botswana Agate","Other Stone" },
            {"Aventurine","Other Stone" },
            {"Granite","Other Stone" },
            {"Sandstone","Other Stone" },
            {"Ammolite","Other Stone" },
            {"Amblygonite","Other Stone" },
            {"Brazanite","Other Stone" },
            {"Andalusite","Other Stone" },
            {"No Gemstone","Other Stone" },


        };
            }
            return GemDictionary;
        }


        public static Dictionary<string, string> StoneClarityDictionary = GetStoneClarityDictionary();

        private static Dictionary<string, string> GetStoneClarityDictionary()
        {
            if (StoneClarityDictionary == null)
            {
                return new(){
            {"FL", "FL" },
            {"I1-I2", "I1"},
            {"I1", "I1"},
            {"I2-I3", "I2" },
            {"I2", "I2" },
            {"I3", "I3" },
            {"IF" , "IF" },
            //{"", "Other" },
            {"SI1-SI2", "SI1" },
            {"SI1", "SI1" },
            {"SI2-I1", "SI2" },
            {"SI2", "SI2" },
            {"VS1-VS2", "VS1" },
            {"VS1", "VS1" },
            {"VS2-SI1", "VS2" },
            {"VS2", "VS2" },
            {"VVS1-VVS2", "VVS1" },
            {"VVS1", "VVS1" },
            {"VVS2", "VVS2" },
            {"VVS2-VS1", "VVS2" },
        };
            }
            return StoneClarityDictionary;
        }


        public static Dictionary<string, string> StoneColorDictionary = GetStoneColorDictionary();

        private static Dictionary<string, string> GetStoneColorDictionary()
        {
            if (StoneColorDictionary == null)
            {
                return
        new()
        {
            {"K","K" },
            {"K-L","K-L" },
            {"L","L" },
            {"L-M","L-M" },
            {"M","M" },
            //{"","N-R" },
            //{"","S-Z" },
            {"D","Other" },
            {"E","Other" },
            {"F","Other" },
            {"G","Other" },
            {"H","Other" },
            {"I","Other" },
            {"J","Other" },
            {"Y-Z","Other" },
            {"X-Y","Other" },
            {"W-X","Other" },
            {"V-W","Other" },
            {"U-V","Other" },
            {"T-U","Other" },
            {"S-T","Other" },
            {"R-S","Other" },
            {"Q-R","Other" },
            {"D-E","Other" },
            {"E-F","Other" },
            {"F-G","Other" },
            {"G-H","Other" },
            {"H-I","Other" },
            {"I-J","Other" },
            {"J-K","Other" },
            {"M-N","Other" },
            {"N-O","Other" },
            {"O-P","Other" },
            {"P-Q","Other" },
            {"beige","Other"},
            {"black","Other"},
            {"blue","Other"},
            {"blue-green","Other"},
            {"blue-violet","Other"},
            {"brown","Other"},
            {"champagne","Other"},
            {"clear","Other"},
            {"color-changing","Other"},
            {"gold","Other"},
            {"green","Other"},
            {"green-blue","Other"},
            {"green-yellow","Other"},
            {"gray","Other"},
            {"metallic","Other"},
            {"multiple-colors","Other"},
            {"cream","Other"},
            {"orange","Other"},
            {"orange-red","Other"},
            {"orange-yellow","Other"},
            {"pink","Other"},
            {"red","Other"},
            {"red-orange","Other"},
            {"red-violet","Other"},
            {"silver","Other"},
            {"violet","Other"},
            {"violet-blue","Other"},
            {"violet-red","Other"},
            {"white","Other"},
            {"yellow","Other"},
            {"yellow-green","Other"},
            {"yellow-orange","Other"},
            {"other-color","Other"},
        };
            }
            return StoneColorDictionary;
        }

        public static Dictionary<string, string> StoneCutDictionary = GetStoneCutDictionary();

        private static Dictionary<string, string> GetStoneCutDictionary()
        {
            if (StoneCutDictionary == null)
            {
                return new()
        {
            //{"Radiant cut",""},
            //{"Round Brilliant cut",""},
            //{"Square cut",""},
            //{"Princess cut",""},
            //{"Pear cut",""},
            //{"Oval cut",""},
            //{"Marquise cut",""},
            //{"Heart cut",""},
            //{"Fancy cut",""},
            //{"Emerald cut",""},
            //{"Cushion cut",""},
            //{"Baguette cut",""},
            //{"Asscher cut",""},
            {"ideal-cut","Other cut" },
            {"very-good-cut","Other cut" },
            {"good-cut","Other cut" },
            {"fair-cut","Other cut" },
            {"poor-cut","Other cut" },
            {"cabochon","Other cut" },
            {"rose-cut","Other cut" },
            {"rough-cut","Other cut" },
            {"uncut","Other cut" },
            {"checkerboard-cut","Other cut" },
            {"fantasy-cut","Fancy cut" },
            {"mixed-cut","Other cut" },
        };
            }
            return StoneCutDictionary;
        }


        public static Dictionary<string, string> StoneShapeDictionary = GetStoneShapeDictionary();

        private static Dictionary<string, string> GetStoneShapeDictionary()
        {
            if (StoneShapeDictionary == null)
            {
                return new()
                {
                    {"asscher-shape","Asscher"},
                    {"baguette-shape","Baguette"},
                    {"cushion-cut","Cushion"},
                    {"emerald-shape","Emerald"},
                    {"heart-shape","Heart"},
                    {"marquise-shape","Marquise"},
                    {"pear-shape","Pear"},
                    {"princess-shape","Princess"},
                    {"oval-shape","Oval"},
                    {"radiant-shape","Radiant"},
                    {"round-shape","Round"},
                    {"round-brilliant-shape","Round"},
                    {"bullet-shape","Baguette"},
                    {"half-moon-shape",""},
                    {"other-shape",""},
                    {"rectangular-shape",""},
                    {"special-cut-shape",""},
                    {"square-shape",""},
                    {"trapezoid-shape",""},
                    {"triangle-shape",""},
                    {"irregular-shape",""},
                    {"nugget-shape",""},
                };
            }
            return StoneShapeDictionary;
        }


        public static Dictionary<string, string> ClaspDictionary = GetClaspDictionary();

        private static Dictionary<string, string> GetClaspDictionary()
        {
            if (ClaspDictionary == null)
            {
                return new()
        {
            {"box-with-tongue-and-safety" ,"Box" },
            {"box-with-tongue" ,"Box" },
            {"buckles" ,"Buckle" },
            {"Flat Solid Buckle" ,"Buckle" },
            {"double-locking-fold-over" ,"Fold Over" },
            {"double-safety" ,"Invisible Double-Locking" },
            {"easy-hook" ,"S Hook" },
            {"fancy" , "Elastic" },
            {"fold-over" ,"Fold Over" },
            {"hidden-safety-clasp" ,"Hidden Safety Clasp" },
            {"invisible-double-locking" ,"Invisible Double-Locking" },
            {"leather-deployment-buckles" ,"Buckle" },
            {"lobster-claw","Lobster Claw" },
            {"magnetic","Magnetic Clasp" },
            {"multistrand-box","Box" },
            {"pin-stem-with-safety","Other" },
            {"post-and-nut","Other" },
            {"push-button","Other" },
            {"screw-barrel","Screw" },
            {"screw-barrel-with-safety","Screw" },
            {"single-locking-fold-over","Fold Over" },
            {"single-strand-fishhook","Fishhook" },
            {"snap","Snaps" },
            {"spring-ring","Spring Ring" },
            {"spring-rings-with-terminals","Spring Ring" },
            {"toggle","Toggle" },
            {"other-clasp-type","Other" },
            {"no-clasp-type","No Clasp" },
        };
            }
            return ClaspDictionary;
        }


        public static Dictionary<string, string> SeasonDictionary = GetSeasonDictionary();

        private static Dictionary<string, string> GetSeasonDictionary()
        {
            if (SeasonDictionary == null)
            {
                return new()
                {
                    { "All", "0- Basic" },
                    { "Spring", "1- Spring" },
                    { "Summner", "2- Summer" },
                    { "Fall", "3- BTS/Fall" },
                    { "Winter", "4- Winter" },
                };
            }
            return SeasonDictionary;
        }


        public static Dictionary<string, string> MetalTypeDictionary = GetMetalTypeDictionary();

        private static Dictionary<string, string> GetMetalTypeDictionary()
        {
            if (MetalTypeDictionary == null)
            {
                return new(){
            {"yellow-gold", "Yellow Gold" },
            {"yellow-gold-and-sterling-silver","Yellow Gold-plated" },
            {"yellow-gold-flashed-silver","Yellow Gold-plated" },
            {"yellow-gold-plated-base","Yellow Gold-plated" },
            {"yellow-gold-plated-brass","Yellow Gold-plated" },
            {"yellow-gold-plated-bronze","Yellow Gold-plated" },
            {"yellow-gold-plated-silver","Yellow Gold-plated" },
            {"yellow-gold-plated-stainless-steel","Yellow Gold-plated" },
            {"yellow-gold-plated-titanium","Yellow Gold-plated" },
            {"white-gold","White Gold"},
            {"white-and-yellow-gold","White Gold"},
            {"white-gold-and-sterling-silver","White Gold-plated"},
            {"white-gold-plated-base","White Gold-plated"},
            {"white-gold-plated-brass","White Gold-plated"},
            {"white-gold-plated-silver","White Gold-plated"},
            {"tungsten", "Tungsten" },
            {"tri-color-gold", "Tri-Tone Gold" },
            {"titanium" , "Titanium" },
            {"titanium-and-stainless-steel","Titanium"},
            {"sterling-silver", "Sterling Silver" },
            {"stainless-steel","Stainless Steel" },
            {"silver-plated-titanium","Silver-plated"},
            {"silver-plated-stainless-steel","Silver-plated"},
            {"silver-plated-brass","Silver-plated"},
            {"silver-plated-bronze","Silver-plated"},
            {"silver-plated-base","Silver-plated"},
            {"silver-and-gold-plated-base","Silver-plated"},
            {"rose-gold","Rose Gold" },
            {"rose-and-yellow-gold","Rose Gold" },
            {"rose-and-white-gold","Rose Gold" },
            {"rose-gold-plated-titanium","Rose Gold-plated" },
            {"rose-gold-plated-stainless-steel","Rose Gold-plated" },
            {"rose-gold-plated-silver","Rose Gold-plated" },
            {"rose-gold-plated-bronze","Rose Gold-plated" },
            {"rose-gold-plated-brass","Rose Gold-plated" },
            {"rose-gold-plated-base","Rose Gold-plated" },
            {"rose-gold-flashed-silver","Rose Gold-plated" },
            {"rose-gold-and-sterling-silver","Rose Gold-plated" },
            {"rhodium-flashed-silver","Rhodium" },
            {"rhodium-plated-base-metal","Rhodium" },
            {"rhodium-plated-brass","Rhodium" },
            {"rhodium-plated-bronze","Rhodium" },
            {"rhodium-plated-gold","Rhodium" },
            {"rhodium-plated-gold-and-silver","Rhodium" },
            {"rhodium-plated-silver","Rhodium" },
            {"rhodium-plated-stainless-steel","Rhodium" },
            {"rhodium-plated-titanium","Rhodium" },
            {"rhodium-plated-white-gold","Rhodium" },
            {"rhodium-plated-yellow-gold","Rhodium" },
            {"platinum","Platinum"},
            {"platinum-and-gold-plated-base","Platinum"},
            {"platinum-and-sterling-silver","Platinum"},
            {"platinum-and-white-gold","Platinum"},
            {"platinum-and-yellow-gold","Platinum"},
            {"platinum-flashed-silver","Platinum"},
            {"platinum-plated-base","Platinum"},
            {"platinum-plated-brass","Platinum"},
            {"platinum-plated-bronze","Platinum"},
            {"platinum-plated-gold","Platinum"},
            {"platinum-plated-silver","Platinum"},
            {"platinum-plated-stainless-steel","Platinum"},
            {"platinum-plated-titanium","Platinum"},
            {"pewter","Pewter" },
            {"palladium","Palladium" },
            {"NA","Other Metal" },
            {"no-metal-type","No Metal" },
            {"nickel","Nickel" },
            //{"","Iron" },
            {"gold","Goldtone" },
            {"gold-filled","Goldtone" },
            {"gold-filled-silver","Goldtone" },
            {"gold-plated-base","Goldtone" },
            {"gold-plated-brass","Goldtone" },
            {"gold-plated-bronze","Goldtone" },
            {"gold-plated-copper","Goldtone" },
            {"gold-plated-silver","Goldtone" },
            {"gold-plated-stainless-steel","Goldtone" },
            {"gold-plated-titanium","Goldtone" },
            {"copper","Copper" },
            {"brass","Brass" },
            {"cobalt","" },
            {"chocolate-gold","" },
            {"bronze","" },
            {"bonded-gold-and-silver","" },
            {"black-rhodium-plated-silver","" },
            {"base","" },
            {"argentium-silver","" },
            {"argentium-plated-titanium","" },
            {"argentium-plated-stainless-steel","" },
            {"argentium-plated-silver","" },
            {"argentium-plated-bronze","" },
            {"argentium-plated-brass","" },
            {"argentium-plated-base","" },
            {"alloy","" },
        };
            }
            return MetalTypeDictionary;
        }


    }
}
