using PPVR.Common.Extensions;
using PPVR.Common.Helpers.OCR;
using PPVR.Common.Helpers.OCR.OCRSpace;
using PPVR.OCRBenchmark.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PPVR.OCRBenchmark
{
    internal class Program
    {
        public static ICollection<SantinhoPolitico> SantinhosPoliticos { get; set; }

        public static void Main(string[] args)
        {
            if (args[0] != null)
            {
                var files = Directory.GetFiles(args[0]);

                if (files.Any())
                {
                    SantinhosPoliticos = new List<SantinhoPolitico>();
                    foreach (var imageFilePath in Directory.GetFiles(args[0]))
                    {
                        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFilePath);

                        if (fileNameWithoutExtension != null)
                        {
                            var candidato = fileNameWithoutExtension.Split('_');

                            var santinhoPolitico = new SantinhoPolitico
                            {
                                ImageFilePath = imageFilePath,
                                NumeroEleitoral = int.Parse(candidato[0]),
                                NomeCandidato = candidato[1],
                                TextoTesseract = TesseractHelper.UploadAndRecognizeImage(imageFilePath)
                            };

                            santinhoPolitico.MatchTesseract = PesquisarCandidatoTexto(santinhoPolitico.NomeCandidato,
                                santinhoPolitico.NumeroEleitoral, santinhoPolitico.TextoTesseract);

                            SantinhosPoliticos.Add(santinhoPolitico);
                        }
                    }
                }

                var t = MainAsync(args);
                t.Wait();

                //Console.WriteLine("{0,-20} {1,40} {2,30}", "Número Eleitoral", "Nome", "Match");

                //foreach (var imageFilePath in Directory.GetFiles(args[0]))
                //{
                //    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFilePath);

                //    if (fileNameWithoutExtension != null)
                //    {
                //        var candidato = fileNameWithoutExtension.Split('_');

                //        var santinhoPolitico = new SantinhoPolitico
                //        {
                //            NumeroEleitoral = int.Parse(candidato[0]),
                //            NomeCandidato = candidato[1],
                //            TextoTesseract = TesseractHelper.UploadAndRecognizeImage(imageFilePath)
                //        };

                //        var match = PesquisarCandidatoTexto(santinhoPolitico.NomeCandidato,
                //            santinhoPolitico.NumeroEleitoral, santinhoPolitico.TextoTesseract);

                //        Console.WriteLine("{0,-20} {1,40} {2,30}", santinhoPolitico.NumeroEleitoral,
                //            santinhoPolitico.NomeCandidato, match);

                //        //using (StreamWriter writer = new StreamWriter("important.txt"))
                //        //{
                //        //    writer.Write("Word ");
                //        //    writer.WriteLine("word 2");
                //        //    writer.WriteLine("Line");
                //        //}
                //    }
                //}  
            }
            Console.ReadKey();
        }

        private static async Task MainAsync(string[] args)
        {
            foreach (var item in SantinhosPoliticos)
            {
                item.TextoOCRSpace = await SpaceHelper.UploadAndRecognizeImage(item.ImageFilePath);

                //item.TextoMicrosoftCognitiveServices =
                //    await MicrosoftCognitiveServicesHelper.UploadAndRecognizeImage(item.ImageFilePath);

                //item.MatchMicrosoftCognitiveServices = PesquisarCandidatoTexto(item.NomeCandidato, item.NumeroEleitoral,
                //    item.TextoMicrosoftCognitiveServices);
            }

            //foreach (var imageFilePath in Directory.GetFiles(args[0]))
            //{
            //    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFilePath);

            //    if (fileNameWithoutExtension != null)
            //    {
            //        var candidato = fileNameWithoutExtension.Split('_');

            //        var santinhoPolitico = new SantinhoPolitico
            //        {
            //            NumeroEleitoral = int.Parse(candidato[0]),
            //            NomeCandidato = candidato[1],
            //            //TextoMicrosoftCognitiveServices = await MicrosoftCognitiveServicesHelper.UploadAndRecognizeImage(imageFilePath)
            //        };

            //        var matchTextMicrosoftCognitiveServices = PesquisarCandidatoTexto(santinhoPolitico.NomeCandidato,
            //            santinhoPolitico.NumeroEleitoral, santinhoPolitico.TextoMicrosoftCognitiveServices);
            //    }
            //}
        }

        private static MatchType? PesquisarCandidatoTexto(string nomeCandidato, int numeroEleitoral, string texto)
        {
            MatchType? matchType = null;
            var containsNumeroEleitoral = false;
            var regex = new Regex(@"\d+");
            var palavras = texto.Replace(".", "").Replace(",", "").Split(' ');

            foreach (var palavra in palavras)
            {
                var match = regex.Match(palavra);
                if (match.Success)
                {
                    if (match.Value == numeroEleitoral.ToString())
                    {
                        matchType = MatchType.NumeroEleitoral;
                        containsNumeroEleitoral = true;
                    }
                }
            }
            if (!containsNumeroEleitoral)
            {
                if (texto.Contains(nomeCandidato, StringComparison.OrdinalIgnoreCase))
                    matchType = MatchType.Nome;
            }
            return matchType;
        }
    }
}