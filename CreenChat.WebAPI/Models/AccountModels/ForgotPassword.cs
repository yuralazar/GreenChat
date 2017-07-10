using System.ComponentModel.DataAnnotations;

namespace CreenChat.WebAPI.Models.AccountModels
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
