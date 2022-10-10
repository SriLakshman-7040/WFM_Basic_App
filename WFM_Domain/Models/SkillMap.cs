using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WFM_Domain.Models
{
    public partial class SkillMap
    {
        public int? Employeeid { get; set; }
        public int? Skillid { get; set; }

        [JsonIgnore]
        public virtual Employee? Employee { get; set; }
        [JsonIgnore]
        public virtual Skill? Skill { get; set; }
    }
}
