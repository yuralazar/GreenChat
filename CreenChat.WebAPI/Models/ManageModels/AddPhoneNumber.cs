using System.ComponentModel.DataAnnotations;

namespace CreenChat.WebAPI.Models.ManageModels
{
    public class AddPhoneNumber
    {
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
