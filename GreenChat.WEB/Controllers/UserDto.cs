﻿namespace GreenChat.Client_WEB.Controllers
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }        
        public bool RememberMe { get; set; }        
    }
}