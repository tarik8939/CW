using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CW.ViewModels
{
    public class LoginModel
    {
        [RegularExpression("^[-\\w.]+@([A-z0-9][-A-z0-9]+\\.)+[A-z]{2,4}$",ErrorMessage = "Please, write email correctly")]
        [Required(ErrorMessage = "Field 'Email' must be set")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field 'Password' must be set")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
