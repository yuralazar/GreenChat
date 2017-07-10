using System.ComponentModel.DataAnnotations;

namespace CreenChat.WebAPI.Models.AccountModels
{
    public class ExternalLoginConfirmation
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
