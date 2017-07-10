using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Authentication;

namespace GreenChat.Client_WEB.Models.ManageViewModels
{
    public class ManageLoginsViewModel
    {
        //public IList<UserLoginInfo> CurrentLogins { get; set; }

        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }
}
