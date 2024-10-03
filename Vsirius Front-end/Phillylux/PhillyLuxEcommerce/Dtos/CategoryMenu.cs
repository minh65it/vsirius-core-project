namespace PhillyLuxEcommerce.Dtos
{
    public class Dropdown
    {
        public required string Key { get; set; }
        public required string Title { get; set; }
        public int Position { get; set; }
    }

    public class State : Dropdown
    {

    }

    public class Country : Dropdown
    {

    }
}
