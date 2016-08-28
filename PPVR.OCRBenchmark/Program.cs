using PPVR.Common.Extensions;
using PPVR.Common.Helpers.OCR;
using PPVR.OCRBenchmark.Entities;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace PPVR.OCRBenchmark
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args[0] != null)
            {
                foreach (var imageFilePath in Directory.GetFiles(args[0]))
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFilePath);

                    if (fileNameWithoutExtension != null)
                    {
                        var candidato = fileNameWithoutExtension.Split('_');

                        var santinhoPolitico = new SantinhoPoliticoTest
                        {
                            NumeroEleitoral = int.Parse(candidato[0]),
                            NomeCandidato = candidato[1],
                            TextTesseract = TesseractHelper.UploadAndRecognizeImage(imageFilePath)
                        };

                        var msg1 = PesquisarCandidatoTexto(santinhoPolitico.NomeCandidato,
                            santinhoPolitico.NumeroEleitoral, santinhoPolitico.TextTesseract);

                        Console.WriteLine("{0,-20} {1,50} {2,20}", santinhoPolitico.NumeroEleitoral,
                            santinhoPolitico.NomeCandidato, msg1);
                    }
                }
                //var t = MainAsync(args);
                //t.Wait();
            }
            Console.ReadKey();
        }

        //private static async Task MainAsync(string[] args)
        //{
        //    var imageFilePaths = Directory.GetFiles(@"");
        //    Console.WriteLine("{0,-20} {1,50} {2,20}", "Número Eleitoral", "Nome", "Encontrado?");

        //    foreach (var imageFilePath in imageFilePaths)
        //    {
        //        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFilePath);

        //        if (fileNameWithoutExtension != null)
        //        {
        //            var candidato = fileNameWithoutExtension.Split('_');

        //            var santinhoPolitico = new SantinhoPoliticoTest
        //            {
        //                NumeroEleitoral = int.Parse(candidato[0]),
        //                NomeCandidato = candidato[1],
        //                TextMicrosoftCognitiveServices =
        //                    await MicrosoftCognitiveServicesHelper.UploadAndRecognizeImage(imageFilePath)
        //            };

        //            var msg1 = PesquisarCandidatoTexto(santinhoPolitico.NomeCandidato,
        //                santinhoPolitico.NumeroEleitoral, santinhoPolitico.TextMicrosoftCognitiveServices);

        //            var msg2 = PesquisarCandidatoTexto(santinhoPolitico.NomeCandidato, santinhoPolitico.NumeroEleitoral,
        //                santinhoPolitico.TextMicrosoftCognitiveServices);

        //            Console.WriteLine("{0,-20} {1,50} {2,20}", santinhoPolitico.NumeroEleitoral,
        //                santinhoPolitico.NomeCandidato, msg1);

        //            Console.WriteLine("{0,-20} {1,50} {2,20}", santinhoPolitico.NumeroEleitoral,
        //                santinhoPolitico.NomeCandidato, msg2);
        //        }
        //    }

        //    //foreach (var santinhoPolitico in SantinhosPoliticos)
        //    //{
        //    //    PesquisarCandidatoTexto(santinhoPolitico);
        //    //}
        //}

        private static string PesquisarCandidatoTexto(string nomeCandidato, int numeroEleitoral, string textoSantinho)
        {
            var msg = "";
            var containsNumeroEleitoral = false;
            var regex = new Regex(@"\d+");
            var palavras = textoSantinho.Replace(".", "").Replace(",", "").Split(' ');

            foreach (var palavra in palavras)
            {
                var match = regex.Match(palavra);
                if (match.Success)
                {
                    if (match.Value == numeroEleitoral.ToString())
                    {
                        msg = string.Format("O NomeCandidato {0} - {1} foi encontrado pelo Número Eleitoral.",
                            nomeCandidato, numeroEleitoral);

                        containsNumeroEleitoral = true;
                    }
                }
            }
            if (!containsNumeroEleitoral)
            {
                if (textoSantinho.Contains(nomeCandidato, StringComparison.OrdinalIgnoreCase))
                {
                    msg = string.Format("O Candidato {0} - {1} foi encontrado pelo Nome.", nomeCandidato,
                        numeroEleitoral);
                }
            }
            return msg;
        }
    }
}