using System;
using System.Collections.Generic;

namespace StullerApiClient.Models
{
    [Serializable]
    public class Type
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public List<Font> Fonts { get; set; }
        public List<Finish> Finishes { get; set; }
        public List<Pattern> Patterns { get; set; }
        public List<FillOption> FillOptions { get; set; }
        public Labor Labor { get; set; }
    }

  


}
