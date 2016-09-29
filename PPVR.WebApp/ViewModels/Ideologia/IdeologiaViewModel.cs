using PPVR.WebApp.Resources;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.Ideologia
{
    public class IdeologiaViewModel
    {
        [Key]
        public short IdeologiaId { get; set; }

        [Display(Name = nameof(Labels.IdeologiaNome), ResourceType = typeof(Labels))]
        public string Nome { get; set; }

        [Display(Name = nameof(Labels.Ativo), ResourceType = typeof(Labels))]
        public bool Enabled { get; set; }
    }
}