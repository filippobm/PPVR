using PPVR.WebApp.Utils;
using Resources;
using System;

namespace PPVR.WebApp.Models
{
    public class Eleicao
    {
        #region Private Fields

        private string _descricao;

        #endregion

        #region Properties

        public short EleicaoId { get; set; }

        /// <summary>
        ///     Ano da eleição.
        /// </summary>
        public byte Ano { get; set; }

        /// <summary>
        ///     Número do turno.
        /// </summary>
        public byte Turno { get; set; }

        /// <summary>
        ///     Descrição da eleição.
        /// </summary>
        public string Descricao
        {
            get { return _descricao; }
            set
            {
                AssertionConcern.AssertArgumentNotNull(value, ValidationErrorMessage.EleicaoDescricaoNotNull);
                AssertionConcern.AssertArgumentLength(value, 1, 255,
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