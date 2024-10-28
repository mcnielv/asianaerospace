using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Web.Model.Account
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
    }
}
