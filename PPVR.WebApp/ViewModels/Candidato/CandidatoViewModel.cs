using PPVR.WebApp.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.Candidato
{
    public class CandidatoViewModel
    {
        [Key]
        public int CandidatoId { get; set; }

        public string Partido { get; set; }

        [Display(Name = nameof(Labels.Partido), ResourceType = typeof(Labels))]
        public string PartidoSigla { get; set; }

        [Display(Name = nameof(Labels.CandidatoNome), ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.CandidatoNomeNotNull))]
        [MaxLength(100, ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.CandidatoNomeInvalidLength))]
        public string Nome { get; set; }

        [Display(Name = nameof(Labels.CandidatoNomeUrna), ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.CandidatoNomeUrnaNotNull))]
        [MaxLength(60, ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.CandidatoNomeUrnaInvalidLength))]
        public string NomeUrna { get; set; }

        [Display(Name = nameof(Labels.CandidatoNumeroEleitoral), ResourceType = typeof(Labels))]
        public int NumeroEleitoral { get; set; }

        [Display(Name = nameof(Labels.CandidatoUnidadeFederacao), ResourceType = typeof(Labels))]
        public string UnidadeFederacao { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.CandidatoDescricaoUnidadeEleitoralInvalidLength))]
        [Display(Name = nameof(Labels.CandidatoUnidadeEleitoral), ResourceType = typeof(Labels))]
        public string UnidadeEleitoral { get; set; }

        [Display(Name = nameof(Labels.CreatedAt), ResourceType = typeof(Labels))]
        public DateTime CreatedAt { get; set; }

        [Display(Name = nameof(Labels.UpdatedAt), ResourceType = typeof(Labels))]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = nameof(Labels.Ativo), ResourceType = typeof(Labels))]
        public bool Enabled { get; set; }

        public List<GastoCandidatoViewModel> Gastos { get; set; }
    }
}