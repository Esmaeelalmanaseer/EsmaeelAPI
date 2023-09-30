using System;
using System.Collections.Generic;

namespace EsmaeelAPI.Models
{
    public partial class UserInfo
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string EmailName { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
