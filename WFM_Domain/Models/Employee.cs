using System;
using System.Collections.Generic;

namespace WFM_Domain.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? Manager { get; set; }
        public string? WfmManager { get; set; }
        public string? Email { get; set; }
        public string? LockStatus { get; set; }
        public decimal? Experience { get; set; }
        public int? ProfileId { get; set; }

        public ICollection<SkillMap> SkillMaps { get; set; } = new HashSet<SkillMap>();
    }
}
