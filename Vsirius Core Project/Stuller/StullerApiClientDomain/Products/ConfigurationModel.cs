using System;
using System.Collections.Generic;

namespace StullerApiClient.Models
{
    [Serializable]
    public class ConfigurationModel
    {
        public int Id { get; set; }
        public bool IsPegHeadable { get; set; }
        public List<RingSizeOption> RingSizeOptions { get; set; }
        public List<SettingOption> SettingOptions { get; set; }
        public List<EngravingOption> EngravingOptions { get; set; }
        public List<MonogramOption> MonogramOptions { get; set; }
        public List<MonogramChainOption> MonogramChainOptions { get; set; }
    }

  


}
