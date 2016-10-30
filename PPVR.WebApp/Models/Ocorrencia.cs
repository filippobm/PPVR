using System;

namespace PPVR.WebApp.Models
{
    public class Ocorrencia
    {
        #region Properties

        public long OcorrenciaId { get; set; }
        public int? CandidatoId { get; set; }
        public long? EnderecoId { get; set; }
        public int TipoPropagandaId { get; set; }
        public byte[] Foto { get; set; }
        public bool Verificada { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Candidato Candidato { get; set; }
        public virtual Endereco Endereco { get; set; }
        public virtual TipoPropaganda TipoPropaganda { get; set; }

        #endregion
    }
}