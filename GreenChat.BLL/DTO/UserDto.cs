using System;
using System.Collections.Generic;
using System.Text;
using GreenChat.DAL.Models;

namespace GreenChat.BLL.DTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }        
        public bool RememberMe { get; set; }
        public ApplicationUser User{ get; set; }
    }
}
