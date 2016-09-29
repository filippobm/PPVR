using PPVR.WebApp.Resources;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.Ideologia
{
    public class IdeologiaPartidoViewModel
    {
        public byte PartidoId { get; set; }

        [Display(Name = nameof(Labels.PartidoNome), ResourceType = typeof(Labels))]
        public string Nome { get; set; }

        [Display(Name = nameof(Labels.PartidoSigla), ResourceType = typeof(Labels))]
        public string Sigla { get; set; }
    }
}