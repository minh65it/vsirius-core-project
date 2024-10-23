namespace IdentityApi.Dtos
{
    public class FacebookUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public PictureData Picture { get; set; }
    }

    public class PictureData
    {
        public Picture Data { get; set; }
    }

    public class Picture
    {
        public string Url { get; set; }
    }
}
