using System.ComponentModel.DataAnnotations;

namespace GreenChat.Client_WEB.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
