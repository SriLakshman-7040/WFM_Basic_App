using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFM_Domain.ViewModel
{
    public class EmployeeWithSkills
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Manager { get; set; }
        public string Wfm_Manager { get; set; }
        public string Email { get; set; }
        public string LockStatus { get; set; }
        public decimal? Experience { get; set; }
        [NotMapped]
        public List<string> Skills { get; set; }
    }
}
