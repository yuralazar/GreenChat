using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GreenChat.WebAPI.Models.AccountModels
{
    public class SendCode
    {
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}
