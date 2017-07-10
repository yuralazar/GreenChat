using System;
using System.Collections.Generic;
using System.Text;

namespace GreenChat.BLL.Services
{
    public class UserService
    {
        public UserService(SignInManagerDto signInManager, UserManagerDto userManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
        }

        public SignInManagerDto SignInManager { get; }
        public UserManagerDto UserManager { get; }
    }
}
