using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsiriusDataModel.DataModel
{
    interface INotifications
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Content { get; set; }

        string CreateBy { get; set; }

        string UpdateBy { get; set; }

        DateTime UpdateTime { get; set; }

        DateTime CreateTime { get; set; }

        bool isAll { get; set; }
    }

    public class Notifications : INotifications
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }

        public bool isAll { get; set; }
    }
}
