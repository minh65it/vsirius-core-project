using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VsiriusDataModel.DataModel
{
    public interface IDrivenHtmlPages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; set; } 
        string Title { get; set; }
        string Url { get; set; }
        string Description { get; set; }
        string CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        string UpdatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        bool IsShow { get; set; }
        bool IsDelete { get; set; }

    }
    public class DrivenHtmlPages : IDrivenHtmlPages
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsShow { get; set; }
        public bool IsDelete { get; set; }
    }
}
