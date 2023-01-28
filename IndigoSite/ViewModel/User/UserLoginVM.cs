using System.ComponentModel.DataAnnotations;

namespace IndigoSite.ViewModel
{
    public class UserLoginVM
    {
        [Required]
        public string NameOrUsername { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public bool IsPersistense { get; set; }
    }
}
