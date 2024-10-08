using System.ComponentModel.DataAnnotations;

namespace EtsyIntegrationDataAPI.Models
{
    public class Taxonomy
    {
        [Key]
        public long id { get; set; }
        public int level { get; set; }
        public string name { get; set; }
        public int? parent_id { get; set; }
        public List<Taxonomy>? children { get; set; }
        public string? full_path_taxonomy_ids { get; set; }
    }
}
