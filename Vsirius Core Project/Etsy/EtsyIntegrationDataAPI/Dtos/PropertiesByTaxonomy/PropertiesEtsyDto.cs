namespace EtsyIntegrationDataAPI.Dtos.PropertiesEtsy
{
    public class PropertiesEtsyDto
    {
        public int count { get; set; }
        public List<PropertiesEtsy> results { get; set; }
    }
    public class PropertiesEtsy
    {
        public long property_id { get; set; }
        public string name { get; set; }
        public string display_name { get; set; }
        public List<Scale> scales { get; set; }
        public bool is_required { get; set; }
        public bool supports_attributes { get; set; }
        public bool supports_variations { get; set; }
        public bool is_multivalued { get; set; }
        public int? max_values_allowed { get; set; }
        public List<PossibleValue> possible_values { get; set; }
        public List<SelectedValue> selected_values { get; set; }
    }
    public class Scale
    {
        public int scale_id { get; set; }
        public string display_name { get; set; }
        public string description { get; set; }
    }

    public class PossibleValue
    {
        public long value_id { get; set; }
        public string name { get; set; }
        public int? scale_id { get; set; }
        public List<int> equal_to { get; set; }
    }

    public class SelectedValue
    {
        public long value_id { get; set; }
        public string name { get; set; }
        public int? scale_id { get; set; }
        public List<int> equal_to { get; set; }
    }
}
