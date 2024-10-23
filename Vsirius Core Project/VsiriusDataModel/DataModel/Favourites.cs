
namespace VsiriusDataModel.DataModel
{
    interface IFavourites
    {
        Guid Id { get; set; }
        string UserId { get; set; }
        string ProductId { get; set; }

        DateTime CreatedDate { get; set; }
    }
    public class Favourites : IFavourites
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public DateTime CreatedDate { get ; set; }
    }
}
