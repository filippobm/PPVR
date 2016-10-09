using PPVR.WebApp.Resources;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.Ideologia
{
    public class IdeologiaViewModel
    {
        [Key]
        public short IdeologiaId { get; set; }

        [Display(Name = nameof(Labels.IdeologiaNome), ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.IdeologiaNomeNotNull))]
        [MaxLength(30, ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.IdeologiaNomeInvalidLength))]
        public string Nome { get; set; }

        [Display(Name = nameof(Labels.Ativo), ResourceType = typeof(Labels))]
        public bool Enabled { get; set; }
    }
}