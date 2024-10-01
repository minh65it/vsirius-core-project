namespace CSRApi.Dtos
{
    public class NotificationDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string CreateBy { get; set; }

        public string UpdateBy { get; set; }

        public DateTime UpdateTime { get; set; }

        public DateTime CreateTime { get; set; }

        public bool isAll { get; set; }
        public bool isView { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<string> Users { get; set; }
    }
}
