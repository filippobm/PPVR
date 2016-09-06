using PPVR.WebApp.Utils;
using Resources;
using System;

namespace PPVR.WebApp.Models
{
    public class Eleicao
    {
        #region Private Fields

        private string _descricao;
        //private string _unidadeFederacaoSigla;
        //private string _unidadeEleitoralSigla;
        //private string _unidadeEleitoralDescricao;

        #endregion

        #region Properties

        public short EleicaoId { get; set; }

        /// <summary>
        /// Ano da eleição.
        /// </summary>
        public int Ano { get; set; }

        /// <summary>
        /// Número do turno.
        /// </summary>
        public int Turno { get; set; }

        /// <summary>
        /// Descrição da eleição.
        /// </summary>
        public string Descricao
        {
            get { return _descricao; }
            set
            {
                AssertionConcern.AssertArgumentNotNull(value, ValidationErrorMessage.EleicaoDescricaoNotNull);
                AssertionConcern.AssertArgumentLength(value, 1, 60, ValidationErrorMessage.EleicaoDescricaoInvalidLength);

                _descricao = value;
            }
        }

        /// <summary>
        /// Sigla da Unidade da Federação em que ocorreu a eleição.
        /// </summary>
        public string UnidadeFederacaoSigla { get; set; }

        /// <summary>
        /// Sigla da Unidade Eleitoral (Em caso de eleição majoritária é a sigla da UF
        /// que o candidato concorre (texto) e em caso de eleição municipal é o código
        /// TSE do município (número)). Assume os valores especiais BR, ZZ e VT para
        /// designar, respectivamente, o Brasil, Exterior e Voto em Trânsito.
        /// </summary>
        public string UnidadeEleitoralSigla { get; set; }

        /// <summary>
        /// Descrição da Unidade Eleitoral.
        /// </summary>
        public string UnidadeEleitoralDescricao { get; set; }

        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        #endregion
    }
}