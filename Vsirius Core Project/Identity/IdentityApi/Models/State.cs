namespace IdentityApi.Models
{
    public class State : VsiriusDataModel.DataModel.IState
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
