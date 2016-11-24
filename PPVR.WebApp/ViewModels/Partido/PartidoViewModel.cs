using PPVR.WebApp.Resources;
using PPVR.WebApp.ViewModels.Ideologia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.Partido
{
    public class PartidoViewModel
    {
        [Key]
        public byte PartidoId { get; set; }

        [Display(Name = nameof(Labels.PartidoNome), ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.PartidoNomeNotNull))]
        [MaxLength(60, ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.PartidoNomeInvalidLength))]
        public string Nome { get; set; }

        [Display(Name = nameof(Labels.PartidoSigla), ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.PartidoSiglaNotNull))]
        [MaxLength(10, ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.PartidoSiglaInvalidLength))]
        public string Sigla { get; set; }

        [Display(Name = nameof(Labels.PartidoNumeroEleitoral), ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.PartidoNumeroEleitoralNotNull))]
        [Range(10, 99, ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.PartidoNumeroEleitoralInvalidRange))]
        public byte NumeroEleitoral { get; set; }

        [Display(Name = nameof(Labels.PartidoEspectroPolitico), ResourceType = typeof(Labels))]
        public EspectroPoliticoViewModel EspectroPolitico { get; set; }

        [Display(Name = nameof(Labels.PartidoQtdeCandidatosAssociados), ResourceType = typeof(Labels))]
        public int QtdeCandidatosAssociados { get; set; }

        [Display(Name = nameof(Labels.Ativo), ResourceType = typeof(Labels))]
        [Required]
        public bool Enabled { get; set; }

        [Display(Name = nameof(Labels.CreatedAt), ResourceType = typeof(Labels))]
        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = nameof(Labels.UpdatedAt), ResourceType = typeof(Labels))]
        [ScaffoldColumn(false)]
        public DateTime? UpdatedAt { get; set; }

        public List<IdeologiaViewModel> Ideologias { get; set; }
    }
}