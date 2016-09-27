namespace PPVR.WebApp.Models
{
    public enum EspectroPolitico : byte
    {
        NaoInformado = 0,
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
}