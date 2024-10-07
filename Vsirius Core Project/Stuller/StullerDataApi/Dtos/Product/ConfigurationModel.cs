using System;
using System.Collections.Generic;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class ConfigurationModelDto
    {
        public int Id { get; set; }
        public bool IsPegHeadable { get; set; }
        public List<RingSizeOptionDto> RingSizeOptions { get; set; }
        public List<SettingOptionDto> SettingOptions { get; set; }
        public List<EngravingOptionDto> EngravingOptions { get; set; }
        public List<MonogramOptionDto> MonogramOptions { get; set; }
        public List<MonogramChainOptionDto> MonogramChainOptions { get; set; }
    }

  


}
