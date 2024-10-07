using System;
using System.Collections.Generic;

namespace StullerApiClient.Models
{
    [Serializable]
    public class DescriptiveElementGroup
    {
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public List<DescriptiveElement> DescriptiveElements { get; set; }
    }

  


}
