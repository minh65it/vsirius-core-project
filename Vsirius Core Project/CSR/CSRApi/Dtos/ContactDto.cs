namespace CSRApi.Dtos
{
    public class ContactDto
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
        public string EmployeeName { get; set; }
    }
}
