using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EditablePrototype.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool CanEditHome { get; set; }
        public bool CanEditAboutUs { get; set; }
        public bool CanEditContacts { get; set; }
        public bool IsAdmin { get; set; }
    }
}
