using PPVR.WebApp.Resources;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.TipoPropaganda
{
    public class TipoPropagandaViewModel
    {
        [Key]
        public short TipoPropagandaId { get; set; }

        [Display(Name = nameof(Labels.TipoPropagandaDescricao), ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.TipoPropagandaDescricaoNotNull))]
        [MaxLength(30, ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.TipoPropagandaDescricaoInvalidLength))]
        public string Descricao { get; set; }

        [Display(Name = nameof(Labels.TipoPropagandaValorMedio), ResourceType = typeof(Labels))]
        public decimal ValorMedio { get; set; }
    }
}