using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Siimple_Back_End.ViewModels
{
    public class EditUserVM
    {
        [StringLength(maximumLength: 15)]
        public string FirstName { get; set; }
        [StringLength(maximumLength: 25)]
        public string LastName { get; set; }
        [StringLength(maximumLength: 25)]
        public string Username { get; set; }
        [StringLength(maximumLength: 25), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password), Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}
