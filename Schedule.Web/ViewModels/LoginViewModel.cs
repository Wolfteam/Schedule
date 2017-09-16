using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="El nombre de usuario no puede estar en blanco.")]
        [MinLength(4, ErrorMessage = "El usuario debe contener minimo 4 caracteres.")]
        [MaxLength(10, ErrorMessage = "El usuario debe contener maximo 10 caracteres.")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "La clave de usuario no puede estar en blanco.")]
        [MinLength(4, ErrorMessage = "La clave debe contener minimo 4 caracteres.")]
        [MaxLength(10, ErrorMessage = "La clave debe contener maximo 10 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
