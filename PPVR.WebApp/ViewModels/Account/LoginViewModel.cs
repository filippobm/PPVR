using PPVR.WebApp.Resources;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.Account
{
    public class LoginViewModel
    {
        [Display(Name = nameof(Labels.Email), ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.UsuarioEmailNotNull))]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = nameof(Labels.Senha), ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.UsuarioSenhaNotNull))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = nameof(Labels.Lembrar), ResourceType = typeof(Labels))]
        public bool RememberMe { get; set; }
    }
}