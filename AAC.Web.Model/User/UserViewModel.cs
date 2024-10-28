using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAC.Web.Model;

namespace AAC.Web.Model.User
{
    public class UserViewModel
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string SSS { get; set; }
        public string TIN { get; set; }
        public string RoleName { get; set; }
        public string Status { get; set; }

        public List<Role.RoleViewModel> RoleViewModels { get; set; }
    }
}
