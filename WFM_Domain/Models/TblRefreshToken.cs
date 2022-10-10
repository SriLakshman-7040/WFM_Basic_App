using System.ComponentModel.DataAnnotations;

namespace WFM_Domain.Models
{
    
    public partial class TblRefreshToken
    {
        [Key]
        public int Sno { get; set; }
        public string? UserName { get; set; }
        public string? TokenId { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsActive { get; set; }
    }
}
