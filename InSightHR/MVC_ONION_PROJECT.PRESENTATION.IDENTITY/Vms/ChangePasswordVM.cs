using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Vms
{
    public class ChangePasswordVM
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "Eski şifre alanı boş bırakılamaz.")]
        [Display(Name = "Eski Şifre")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre alanı boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Yeni şifreler uyuşmuyor.")]
        [Display(Name = "Yeni Şifre Tekrar")]
        public string ConfirmNewPassword { get; set; }
    }
}
