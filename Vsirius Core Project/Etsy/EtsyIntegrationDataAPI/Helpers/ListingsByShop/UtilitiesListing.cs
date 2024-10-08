namespace EtsyIntegrationDataAPI.Helpers.ListingsByShop
{
    public static class UtilitiesListing
    {
        public static async Task<byte[]> GetImageBytesFromUrlAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            }
        }
    }
}
