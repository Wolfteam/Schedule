using System.ComponentModel.DataAnnotations;

namespace Schedule.Entities
{
    public class ChangePasswordDTO
    {
        [Required]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "La longitud de caracteres de la password actual no se encuentra entre 8 - 255 caracteres")]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "La longitud de caracteres de la nueva password no se encuentra entre 8 - 255 caracteres")]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "La longitud de caracteres de la nueva password actual no se encuentra entre 8 - 255 caracteres")]
        public string NewPasswordConfirmation { get; set; }
    }
}
