using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelValidation.Models
{
    public class User
    {
        [Required]
        [StringLength(10,ErrorMessage ="İsim alanı en fazla 10 karakter olmalıdır."),MinLength(4)]
        public string firstname { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Soyisim alanı en fazla 10 karakter olmalıdır."), MinLength(4)]
        public string lastname { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [Phone]
        public string phone { get; set; }

        [Url]
        public string facebookURL { get; set; }

        [CreditCard]
        public string creditCard { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Şifre alanı en fazla 10 karakter olmalıdır."), MinLength(4)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[*.!@$%^&(){}[]:;<>,.?/~_+-=|\]).{4,10}$")]
        public string password { get; set; }

        [Required]
        [Compare("password")]
        public string passwordConfirm { get; set; }
    }
}