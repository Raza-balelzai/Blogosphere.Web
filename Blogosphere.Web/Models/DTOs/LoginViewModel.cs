using System.ComponentModel.DataAnnotations;

namespace Blogosphere.Web.Models.DTOs
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
