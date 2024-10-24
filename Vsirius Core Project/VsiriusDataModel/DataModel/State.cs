using System.ComponentModel.DataAnnotations;

namespace VsiriusDataModel.DataModel
{
    public interface IState
    {
        [Key]
        string? Id { get; set; }
        string? Name { get; set; }
        string? Description { get; set; }
    }
    public class State : IState
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
