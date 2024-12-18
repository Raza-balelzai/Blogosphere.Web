﻿namespace Blogosphere.Web.Models.DTOs
{
    public class UsersViewModel
    {
        public List<User> Users { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool AdminRoleCheckBox { get; set; }
    }
}
