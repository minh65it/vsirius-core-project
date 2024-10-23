using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VsiriusDataModel.DataModel
{
    public interface IPageView
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; set; }
        int Day { get; set; }
        int Month { get; set; }
        int Year { get; set; }
        string PageUrl { get; set; }
        string PageTitle { get; set; }
        DateTime LastView { get; set; }
        DateTime FirstView { get; set; }
        int DateViewCount { get; set; }
        int TotalViews { get; set; }
    }

    public class PageView : IPageView
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string PageUrl { get; set; }
        public string PageParam { get; set; }
        public string PageTitle { get; set; }
        public DateTime LastView { get; set; }
        public DateTime FirstView { get; set; }
        public int DateViewCount { get; set; }
        public int TotalViews { get; set; }
    }
}
