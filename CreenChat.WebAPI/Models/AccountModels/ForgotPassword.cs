using System.ComponentModel.DataAnnotations;

namespace GreenChat.WebAPI.Models.AccountModels
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
