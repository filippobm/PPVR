using PPVR.WebApp.Resources;
using PPVR.WebApp.Utils;
using System;
using System.Collections.Generic;

namespace PPVR.WebApp.Models
{
    public class Eleicao
    {
        #region Private Fields

        private string _descricao;

        #endregion

        #region Navigation properties

        public virtual ICollection<Candidato> Candidatos { get; set; }

        #endregion

        #region Properties

        public short EleicaoId { get; set; }

        // Ano da Eleição
        public int Ano { get; set; }

        // Número do Turno
        public byte Turno { get; set; }

        // Descrição da Eleição
        public string Descricao
        {
            get { return _descricao; }
            set
            {
                AssertionConcern.AssertArgumentNotNull(value, ValidationErrorMessage.EleicaoDescricaoNotNull);
                AssertionConcern.AssertArgumentLength(value, 1, 100,
                    ValidationErrorMessage.EleicaoDescricaoInvalidLength);

                _descricao = value;
            }
        }

        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        #endregion
    }
}