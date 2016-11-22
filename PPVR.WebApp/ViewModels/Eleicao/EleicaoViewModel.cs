using PPVR.WebApp.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.Eleicao
{
    public class EleicaoViewModel
    {
        [Key]
        public short EleicaoId { get; set; }

        [Display(Name = nameof(Labels.EleicaoDescricao), ResourceType = typeof(Labels))]
        public string Descricao { get; set; }

        [Display(Name = nameof(Labels.EleicaoAno), ResourceType = typeof(Labels))]
        public int Ano { get; set; }

        [Display(Name = nameof(Labels.EleicaoTurno), ResourceType = typeof(Labels))]
        public byte Turno { get; set; }

        [Display(Name = nameof(Labels.CreatedAt), ResourceType = typeof(Labels))]
        public DateTime CreatedAt { get; set; }

        [Display(Name = nameof(Labels.UpdatedAt), ResourceType = typeof(Labels))]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = nameof(Labels.Ativo), ResourceType = typeof(Labels))]
        public bool Enabled { get; set; }
    }
}