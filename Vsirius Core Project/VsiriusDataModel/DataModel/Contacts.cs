
namespace VsiriusDataModel.DataModel
{
    interface IContacts
    {
        Guid Id { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        string FullName { get; set; }

        string Content { get; set; }

        DateTime CreateTime { get; set; }
        DateTime UpdateTime { get; set; }

        bool Status { get; set; }
        string EmployeeId { get; set; }

    }

    public class Contacts : IContacts
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public bool Status { get; set; }
        public string EmployeeId { get; set; }
    }
}
