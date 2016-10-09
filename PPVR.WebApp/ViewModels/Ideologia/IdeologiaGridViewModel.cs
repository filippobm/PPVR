using PPVR.WebApp.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.Ideologia
{
    public class IdeologiaGridViewModel
    {
        public IdeologiaViewModel Ideologia { get; set; }

        [Display(Name = nameof(Labels.IdeologiaQtdePartidosAssociados), ResourceType = typeof(Labels))]
        public int QtdePartidosAssociados { get; set; }

        [Display(Name = nameof(Labels.CreatedAt), ResourceType = typeof(Labels))]
        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = nameof(Labels.UpdatedAt), ResourceType = typeof(Labels))]
        [ScaffoldColumn(false)]
        public DateTime? UpdatedAt { get; set; }
    }
}