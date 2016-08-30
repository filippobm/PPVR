using PPVR.Common.Extensions;
using PPVR.Common.Helpers.OCR;
using PPVR.Common.Helpers.OCR.OCRSpace;
using PPVR.OCRBenchmark.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                    Console.WriteLine("{0,-15} {1,30} {2,20} {3,20}", "Nº ELEITORAL", "NOME", "OCR", "MATCH TYPE");

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

                            Console.WriteLine("{0,-15} {1,30} {2,20} {3,20}", santinhoPolitico.NumeroEleitoral,
                                santinhoPolitico.NomeCandidato, "Tesseract", santinhoPolitico.MatchTesseract);

                            SantinhosPoliticos.Add(santinhoPolitico);
                        }
                    }
                }
                //var t = MainAsync();
                //t.Wait();
            }
            ExportResultCsv();
        }

        private static async Task MainAsync()
        {
            foreach (var item in SantinhosPoliticos)
            {
                // OCR.Space
                item.TextoOCRSpace = await SpaceHelper.UploadAndRecognizeImage(item.ImageFilePath);
                item.MatchOCRSpace = PesquisarCandidatoTexto(item.NomeCandidato, item.NumeroEleitoral,
                    item.TextoOCRSpace);

                // Microsoft Cognitive Service
                item.TextoMicrosoftCognitiveServices =
                    await MicrosoftCognitiveServicesHelper.UploadAndRecognizeImage(item.ImageFilePath);

                item.MatchMicrosoftCognitiveServices = PesquisarCandidatoTexto(item.NomeCandidato, item.NumeroEleitoral,
                    item.TextoMicrosoftCognitiveServices);
            }
        }

        private static void ExportResultCsv()
        {
            var directoryInfo = Directory.CreateDirectory("output");
            using (var writer = new StreamWriter(directoryInfo.FullName + @"\analise_ocr.csv", true, Encoding.UTF8))
            {
                writer.WriteLine(
                    "Nome Candidato,Número Eleitoral,Arquivo,OCR.Space Match,M$ Cognitive Services Match,Tesseract Match");

                foreach (var santinhoPolitico in SantinhosPoliticos)
                {
                    writer.Write(santinhoPolitico.NomeCandidato + ",");
                    writer.Write(santinhoPolitico.NumeroEleitoral + ",");
                    writer.Write(Path.GetFileName(santinhoPolitico.ImageFilePath) + ",");
                    writer.Write(santinhoPolitico.MatchOCRSpace + ",");
                    writer.Write(santinhoPolitico.MatchMicrosoftCognitiveServices + ",");
                    writer.WriteLine(santinhoPolitico.MatchTesseract);
                }
            }
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