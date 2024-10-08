namespace EtsyIntegrationDataAPI.Dtos.AuthenticationEtsyDto
{
    public class ConnectEtsyDto
    {
        public string Response_type { get; set; } = "code";
        public string Client_id { get; set; }
        public string Redirect_uri { get; set; }
        public string Scope { get; set; }
        public string State { get; set; }
        public string Code_challenge { get; set; }
        public string Code_challenge_method { get; set; } = "S256";
    }
    public enum Scope
    {
        all_scopes,
        address_r,
        address_w,
        billing_r,
        cart_r,
        cart_w,
        email_r,
        favorites_r,
        favorites_w,
        feedback_r,
        listings_d,
        listings_r,
        listings_w,
        profile_r,
        profile_w,
        recommend_r,
        recommend_w,
        shops_r,
        shops_w,
        transactions_r,
        transactions_w
    }
}
