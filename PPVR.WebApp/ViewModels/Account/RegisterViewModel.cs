using PPVR.WebApp.Resources;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Display(Name = nameof(Labels.Email), ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.UsuarioEmailNotNull))]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = nameof(Labels.Senha), ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.UsuarioSenhaNotNull))]
        [StringLength(30, ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.UsuarioSenhaMinimumLength), MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = nameof(Labels.SenhaConfirmar), ResourceType = typeof(Labels))]
        [Compare("Password", ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.UsuarioSenhaCompareSenhaConfirmacao))]
        public string ConfirmPassword { get; set; }
    }
}