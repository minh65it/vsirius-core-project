namespace PhillyLuxEcommerce.ViewModels
{
    public class OrderVM
    {
        public OrderInformationVM OrderInformation { get; set; }
        public OrderCartVM OrderCart { get; set; }

        public string MessageCardNumber { get; set; }
        public string MessageExpiredDate { get; set; }
        public string MessageGeneral { get; set; }
    }

}
