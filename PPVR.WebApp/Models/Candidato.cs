using PPVR.WebApp.Resources;
using PPVR.WebApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PPVR.WebApp.Models
{
    public class Candidato
    {
        #region Private Fields

        private string _descricaoUnidadeEleitoral;
        private string _nome;
        private string _nomeUrna;
        private string _siglaUnidadeFederacao;
        private string _siglaUnidadeEleitoral;

        #endregion

        #region Properties

        public int CandidatoId { get; set; }
        public byte PartidoId { get; set; }
        public short EleicaoId { get; set; }

        public string Nome
        {
            get { return _nome; }
            set
            {
                AssertionConcern.AssertArgumentNotNull(value,
                    ValidationErrorMessage.CandidatoNomeNotNull);

                AssertionConcern.AssertArgumentLength(value, 1, 100,
                    ValidationErrorMessage.CandidatoNomeInvalidLength);

                _nome = value;
            }
        }

        public string NomeUrna
        {
            get { return _nomeUrna; }
            set
            {
                AssertionConcern.AssertArgumentNotNull(value,
                    ValidationErrorMessage.CandidatoNomeUrnaNotNull);

                AssertionConcern.AssertArgumentLength(value, 1, 60,
                    ValidationErrorMessage.CandidatoNomeUrnaInvalidLength);

                _nomeUrna = value;
            }
        }

        public CargoEletivo CargoEletivo { get; private set; }
        public int NumeroEleitoral { get; private set; }

        /// <summary>
        ///     Sigla da Unidade da Federação da Eleição à qual o Candidato está participando.
        /// </summary>
        public string SiglaUnidadeFederacao
        {
            get { return _siglaUnidadeFederacao; }
            set
            {
                if (_siglaUnidadeFederacao != null)
                    AssertionConcern.AssertStateTrue(Endereco.EstadosBrasileiros.Contains(value),
                        ValidationErrorMessage.CandidatoSiglaUnidadeFederacaoInvalidValue);

                _siglaUnidadeFederacao = value;
            }
        }

        /// <summary>
        ///     Sigla da Unidade Eleitoral da Eleição à qual o Candidato está participando.
        ///     (Em caso de eleição majoritária é a sigla da UF que o candidato concorre (texto)
        ///     e em caso de eleição municipal é o código TSE do município (número)).
        ///     Assume os valores especiais BR, ZZ e VT para designar, respectivamente,
        ///     o Brasil, Exterior e Voto em Trânsito.
        /// </summary>
        public string SiglaUnidadeEleitoral
        {
            get { return _siglaUnidadeEleitoral; }
            set
            {
                if (_siglaUnidadeEleitoral != null)
                    AssertionConcern.AssertArgumentLength(value, 1, 60,
                        ValidationErrorMessage.CandidatoSiglaUnidadeEleitoralInvalidLength);

                _siglaUnidadeEleitoral = value;
            }
        }

        /// <summary>
        ///     Descrição da Unidade Eleitoral da Eleição à qual o Candidato está participando.
        /// </summary>
        public string DescricaoUnidadeEleitoral
        {
            get { return _descricaoUnidadeEleitoral; }
            set
            {
                if (_descricaoUnidadeEleitoral != null)
                    AssertionConcern.AssertArgumentLength(value, 1, 255,
                        ValidationErrorMessage.CandidatoDescricaoUnidadeEleitoralInvalidLength);

                _descricaoUnidadeEleitoral = value;
            }
        }

        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Partido Partido { get; set; }
        public virtual Eleicao Eleicao { get; set; }
        public virtual ICollection<Ocorrencia> Ocorrencias { get; set; }

        #endregion

        #region Methods

        public void SetNumeroEleitoral(CargoEletivo cargoEletivo, int numeroEleitoral)
        {
            switch (cargoEletivo)
            {
                case CargoEletivo.Presidente:
                    AssertionConcern.AssertArgumentRange(numeroEleitoral, 10, 99,
                        ValidationErrorMessage.CandidatoNumeroEleitoralPresidenteInvalidRange);
                    break;
                case CargoEletivo.Governador:
                    AssertionConcern.AssertArgumentRange(numeroEleitoral, 10, 99,
                        ValidationErrorMessage.CandidatoNumeroEleitoralGovernadorInvalidRange);
                    break;
                case CargoEletivo.Prefeito:
                    AssertionConcern.AssertArgumentRange(numeroEleitoral, 10, 99,
                        ValidationErrorMessage.CandidatoNumeroEleitoralPrefeitoInvalidRange);
                    break;
                case CargoEletivo.Senador:
                    AssertionConcern.AssertArgumentRange(numeroEleitoral, 10, 999,
                        ValidationErrorMessage.CandidatoNumeroEleitoralSenadorInvalidRange);
                    break;
                case CargoEletivo.DeputadoFederal:
                    AssertionConcern.AssertArgumentRange(numeroEleitoral, 10, 99999,
                        ValidationErrorMessage.CandidatoNumeroEleitoralDeputadoFederalInvalidRange);
                    break;
                case CargoEletivo.DeputadoEstadual:
                    AssertionConcern.AssertArgumentRange(numeroEleitoral, 10, 99999,
                        ValidationErrorMessage.CandidatoNumeroEleitoralDeputadoEstadualInvalidRange);
                    break;
                case CargoEletivo.Vereador:
                    AssertionConcern.AssertArgumentRange(numeroEleitoral, 10, 99999,
                        ValidationErrorMessage.CandidatoNumeroEleitoralVereadorInvalidRange);
                    break;
            }
            CargoEletivo = cargoEletivo;
            NumeroEleitoral = numeroEleitoral;
        }

        #endregion
    }
}