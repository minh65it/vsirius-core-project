using System.ComponentModel.DataAnnotations;


namespace EbayApi.Dtos
{
    public class ProductDto
    {
        [MaxLength(50, ErrorMessage = "Max Length: 50"), Required(ErrorMessage = "Required")] public string Sku { get; set; }
        [MaxLength(80), Required] public string Tittle { get; set; }
        [Required] public int Quantity { get; set; }
        [Required] public decimal Price { get; set; }
        [MaxLength(800), Required] public string Description { get; set; }
        [MaxLength(100), Required] public string Brand { get; set; }
        [MaxLength(100), Required] public string Category { get; set; }
        [MaxLength(100), Required] public string ProductType { get; set; }
        [MaxLength(100), Required] public string ItemStyle { get; set; }
        [MaxLength(100), Required] public string Material { get; set; }
        [MaxLength(4000), Required] public string MainImageUrl { get; set; }
        [MaxLength(4000)] public string Image1Url { get; set; }
        [MaxLength(4000)] public string Image2Url { get; set; }
        [MaxLength(4000)] public string Image3Url { get; set; }
        [MaxLength(4000)] public string Image4Url { get; set; }
        [MaxLength(4000)] public string Image5Url { get; set; }
        [MaxLength(4000)] public string Image6Url { get; set; }
        [MaxLength(4000)] public string Image7Url { get; set; }
        [MaxLength(4000)] public string Image8Url { get; set; }
        [MaxLength(4000)] public string Image9Url { get; set; }
        [MaxLength(100)] public string ItemLength { get; set; }
        [MaxLength(100)] public string Origin { get; set; }
        [MaxLength(100)] public string ItemSize { get; set; }
        [MaxLength(100)] public string ItemColor { get; set; }
        [MaxLength(100)] public string PrimaryStoneType { get; set; }
        [MaxLength(100)] public string PrimaryStoneSize { get; set; }
        [MaxLength(100)] public string PrimaryStoneShape { get; set; }
        [MaxLength(100)] public string ItemWeight { get; set; }
        [MaxLength(100)] public string ChainType { get; set; }
        [Required] public string CreatedBy { get; set; }
        public string Note { get; set; }
    }

}