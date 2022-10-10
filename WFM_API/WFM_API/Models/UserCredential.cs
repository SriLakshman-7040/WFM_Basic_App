using System.ComponentModel.DataAnnotations;

namespace WFM_API.Models
{
    public class UserCredential
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
