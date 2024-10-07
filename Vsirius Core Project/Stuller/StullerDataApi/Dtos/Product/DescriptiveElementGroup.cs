using System;
using System.Collections.Generic;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class DescriptiveElementGroupDto
    {
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public List<DescriptiveElementDto> DescriptiveElements { get; set; }
    }

  


}
