using PPVR.WebApp.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.TipoPropaganda
{
    public class TipoPropagandaViewModel
    {
        [Key]
        public int TipoPropagandaId { get; set; }

        [Display(Name = nameof(Labels.TipoPropagandaDescricao), ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.TipoPropagandaDescricaoNotNull))]
        [MaxLength(30, ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.TipoPropagandaDescricaoInvalidLength))]
        public string Descricao { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.TipoPropagandaValorMedioNotNull))]
        [Display(Name = nameof(Labels.TipoPropagandaValorMedio), ResourceType = typeof(Labels))]
        public decimal ValorMedio { get; set; }


        [Display(Name = nameof(Labels.Ativo), ResourceType = typeof(Labels))]
        public bool Enabled { get; set; }

        [Display(Name = nameof(Labels.CreatedAt), ResourceType = typeof(Labels))]
        public DateTime CreatedAt { get; set; }

        [Display(Name = nameof(Labels.UpdatedAt), ResourceType = typeof(Labels))]
        public DateTime? UpdatedAt { get; set; }
    }
}