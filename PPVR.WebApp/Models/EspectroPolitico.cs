namespace PPVR.WebApp.Models
{
    public enum EspectroPolitico : byte
    {
        Centro = 1,
        CentroDireita = 2,
        CentroDireitaADireita = 3,
        Direita = 4,
        DireitaAExtremaDireita = 5,
        ExtremaDireita = 6,
        CentroEsquerda = 7,
        CentroEsquerdaAEsquerda = 8,
        Esquerda = 9,
        EsquerdaAExtremaEsquerda = 10,
        ExtremaEsquerda = 11
    }

    //public class EspectroPolitico2
    //{
    //    #region Private Fields

    //    private string _nome;
    //    private string _descricao;

    //    #endregion

    //    #region Properties

    //    public short EspectroPoliticoId { get; set; }

    //    public string Nome
    //    {
    //        get { return _nome; }
    //        set
    //        {
    //            AssertionConcern.AssertArgumentNotNull(value,
    //                ValidationErrorMessage.EspectroPoliticoNomeNotNull);

    //            AssertionConcern.AssertArgumentLength(value, 1, 30,
    //                ValidationErrorMessage.EspectroPoliticoNomeInvalidLength);

    //            _nome = value;
    //        }
    //    }

    //    public string Descricao
    //    {
    //        get { return _descricao; }
    //        set
    //        {
    //            AssertionConcern.AssertArgumentLength(value, 0, 255,
    //                ValidationErrorMessage.EspectroPoliticoDescricaoInvalidLength);

    //            _descricao = value;
    //        }
    //    }

    //    public bool Enabled { get; set; }

    //    #endregion
    //}
}