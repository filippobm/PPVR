using PPVR.WebApp.Resources;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.Candidato
{
    public class GastoCandidatoViewModel
    {
        public int TipoPropagandaId { get; set; }

        [Display(Name = nameof(Labels.TipoPropagandaDescricao), ResourceType = typeof(Labels))]
        public string TipoPropagandaDescricao { get; set; }

        [Display(Name = nameof(Labels.TipoPropagandaValorMedio), ResourceType = typeof(Labels))]
        public decimal ValorMedio { get; set; }

        [Display(Name = nameof(Labels.QtdeOcorrencias), ResourceType = typeof(Labels))]
        public int QtdeOcorrencias { get; set; }

        [Display(Name = nameof(Labels.TotalGasto), ResourceType = typeof(Labels))]
        public decimal TotalGasto => ValorMedio * QtdeOcorrencias;
    }
}