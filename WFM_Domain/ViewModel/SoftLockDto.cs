using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFM_Domain.ViewModel
{
    public class SoftLockDto
    {
        public int EmployeeID { get; set; }
        public string? Manager { get; set; }
        public DateTime ReqDate { get; set; }
        public string? Status { get; set; }
        //public DateTime LastUpdated { get; set; }
        //public int LockID { get; set; }
        public string? RequestMessage { get; set; }
        //public string? Wfm_Remark { get; set; }
        //public string? ManagerStatus { get; set; }
        //public string? ManagerStatusComment { get; set; }
        //public DateTime MgrLastUpdate { get; set; }
    }
}
