using System.ComponentModel.DataAnnotations;

namespace GreenChat.WebAPI.Models.ManageModels
{
    public class AddPhoneNumber
    {
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
