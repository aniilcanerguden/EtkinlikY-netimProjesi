using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dtos
{
    public class UserRegisterViewModel
    {
        public string EMail { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
        public string Role { get; set; }
    }
}
