using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WFM_Domain.Models
{
    public partial class Skill
    {
        public int SkillId { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public ICollection<SkillMap> SkillMaps { get; set; } = new HashSet<SkillMap>();
    }
}
