namespace PPVR.WebApp.Models
{
    public enum CargoEletivo : byte
    {
        Presidente = 1,
        Governador = 2,
        Prefeito = 3,
        Senador = 4,
        DeputadoFederal = 5,
        DeputadoEstadual = 6,
        Vereador = 7
    }

    //public class CargoEletivo2
    //{
    //    #region Private Fields

    //    private string _nome;
    //    private string _descricao;

    //    #endregion

    //    #region Properties

    //    public short CargoEletivoId { get; set; }

    //    public string Nome
    //    {
    //        get { return _nome; }
    //        set
    //        {
    //            AssertionConcern.AssertArgumentNotNull(value,
    //                ValidationErrorMessage.CargoEletivoNomeNotNull);

    //            AssertionConcern.AssertArgumentLength(value, 1, 30,
    //                ValidationErrorMessage.CargoEletivoNomeInvalidLength);

    //            _nome = value;
    //        }
    //    }

    //    public string Descricao
    //    {
    //        get { return _descricao; }
    //        set
    //        {
    //            AssertionConcern.AssertArgumentLength(value, 0, 255,
    //                ValidationErrorMessage.CargoEletivoDescricaoInvalidLength);

    //            _descricao = value;
    //        }
    //    }

    //    public bool Enabled { get; set; }

    //    #endregion
    //}
}