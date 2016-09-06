using PPVR.WebApp.Utils;
using Resources;
using System.Collections.Generic;
using System.Linq;

namespace PPVR.WebApp.Models
{
    public class Endereco
    {
        public static readonly string[] EstadosBrasileiros =
        {
            "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO",
            "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI",
            "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
        };

        #region Private Fields        

        private string _estado;
        private string _cidade;
        private string _bairro;
        private string _cep;
        private string _enderecoFormatado;

        #endregion

        #region Properties

        public long EnderecoId { get; set; }

        public string Estado
        {
            get { return _estado; }
            set
            {
                if (_estado != null)
                    AssertionConcern.AssertStateTrue(EstadosBrasileiros.Contains(value),
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

        public string Bairro
        {
            get { return _bairro; }
            set
            {
                if (_bairro != null)
                    AssertionConcern.AssertArgumentLength(value, 1, 60,
                        ValidationErrorMessage.EnderecoBairroInvalidLength);
                _bairro = value;
            }
        }

        public string CEP
        {
            get { return _cep; }
            set
            {
                if (_cep != null)
                    AssertionConcern.AssertArgumentLength(value, 8, 9,
                        ValidationErrorMessage.EnderecoCEPInvalidLength);
                _cep = value;
            }
        }

        public string EnderecoFormatado
        {
            get { return _enderecoFormatado; }
            set
            {
                if (_enderecoFormatado != null)
                    AssertionConcern.AssertArgumentLength(value, 1, 255,
                        ValidationErrorMessage.EnderecoFormatadoInvalidLength);
                _enderecoFormatado = value;
            }
        }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<Ocorrencia> Ocorrencias { get; set; }

        #endregion
    }
}