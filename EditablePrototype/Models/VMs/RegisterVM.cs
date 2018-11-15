using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EditablePrototype.Models.VMs
{
    public class RegisterVM
    {
        [Required]
        [StringLength(20,MinimumLength = 5)]
        public string Username { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Message { get; set; }
    }
}
