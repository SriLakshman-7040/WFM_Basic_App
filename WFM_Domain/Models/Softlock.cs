using System;
using System.Collections.Generic;

namespace WFM_Domain.Models
{
    public partial class Softlock
    {
        public int? EmployeeId { get; set; }
        public string? Manager { get; set; }
        public DateTime? ReqDate { get; set; }
        public string? Status { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int LockId { get; set; }
        public string? RequestMessage { get; set; }
        public string? WfmRemark { get; set; }
        public string? ManagerStatus { get; set; }
        public string? ManagerStatusComment { get; set; }
        public DateTime? MgrLastUpdate { get; set; }
    }
}
