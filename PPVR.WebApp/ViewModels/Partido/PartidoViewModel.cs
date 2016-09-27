using System;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.Partido
{
    public class PartidoViewModel
    {
        [Key]
        public byte PartidoId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ValidationErrorMessage), ErrorMessageResourceName = "PartidoNomeNotNull")]
        [MaxLength(60, ErrorMessageResourceType = typeof(Resources.ValidationErrorMessage), ErrorMessageResourceName = "PartidoNomeInvalidLength")]
        public string Nome { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ValidationErrorMessage), ErrorMessageResourceName = "PartidoSiglaNotNull")]
        [MaxLength(10, ErrorMessageResourceType = typeof(Resources.ValidationErrorMessage), ErrorMessageResourceName = "PartidoSiglaInvalidLength")]
        public string Sigla { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ValidationErrorMessage), ErrorMessageResourceName = "PartidoNumeroEleitoralNotNull")]
        [Range(10, 99, ErrorMessageResourceType = typeof(Resources.ValidationErrorMessage), ErrorMessageResourceName = "PartidoNumeroEleitoralInvalidRange")]
        public byte NumeroEleitoral { get; set; }

        //[Range(1, 11, ErrorMessageResourceType = typeof(Resources.ValidationErrorMessage), ErrorMessageResourceName = "PartidoEspectroPoliticoInvalidValue")]
        //public EspectroPoliticoViewModel EspectroPolitico { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Display(Name = "CreatedAt", ResourceType = typeof(Resources.Labels))]
        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "UpdatedAt", ResourceType = typeof(Resources.Labels))]
        [ScaffoldColumn(false)]
        public DateTime? UpdatedAt { get; set; }
    }
}