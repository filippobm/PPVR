using PPVR.WebApp.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.Ideologia
{
    public class IdeologiaDetailsViewModel
    {
        public IdeologiaViewModel Ideologia { get; set; }

        [Display(Name = nameof(Labels.IdeologiaQtdePartidosAssociados), ResourceType = typeof(Labels))]
        public int QtdePartidosAssociados { get; set; }

        public IList<IdeologiaPartidoViewModel> Partidos { get; set; }
    }
}