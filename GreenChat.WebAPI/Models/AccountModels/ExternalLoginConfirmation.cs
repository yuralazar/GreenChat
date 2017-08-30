using System.ComponentModel.DataAnnotations;

namespace GreenChat.WebAPI.Models.AccountModels
{
    public class ExternalLoginConfirmation
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
