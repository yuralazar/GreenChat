using System.ComponentModel.DataAnnotations;

namespace CreenChat.WebAPI.Models.ManageModels
{
    public class VerifyPhoneNumber
    {
        [Required]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
