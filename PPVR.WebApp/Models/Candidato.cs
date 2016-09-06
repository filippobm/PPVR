using PPVR.WebApp.Utils;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PPVR.WebApp.Models
{
    public class Candidato
    {
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

        #region Private Fields

        private string _nome;
        private string _cidade;
        private string _estado;

        #endregion

        #region Properties

        public int CandidatoId { get; set; }
        public byte PartidoId { get; set; }

        public string Nome
        {
            get { return _nome; }
            set
            {
                AssertionConcern.AssertArgumentNotNull(value,
                    ValidationErrorMessage.CandidatoNomeNotNull);

                AssertionConcern.AssertArgumentLength(value, 1, 60,
                    ValidationErrorMessage.CandidatoNomeInvalidLength);
                _nome = value;
            }
        }

        public CargoEletivo CargoEletivo { get; private set; }
        public int NumeroEleitoral { get; private set; }

        public string Estado
        {
            get { return _estado; }
            set
            {
                if (_estado != null)
                    AssertionConcern.AssertStateTrue(Endereco.EstadosBrasileiros.Contains(value),
                        ValidationErrorMessage.EnderecoEstadoInvalidValue);
                _estado = value;
            }
        }

        public string Cidade
        {
            get { return _cidade; }
            set
            {
                if (_cidade != null)
                    AssertionConcern.AssertArgumentLength(value, 1, 60,
                        ValidationErrorMessage.EnderecoCidadeInvalidLength);
                _cidade = value;
            }
        }

        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Partido Partido { get; set; }
        public virtual ICollection<Ocorrencia> Ocorrencias { get; set; }

        #endregion
    }
}