using OfficeOpenXml;
using PPVR.Common.Extensions;
using PPVR.Common.Helpers.OCR;
using PPVR.Common.Helpers.OCR.OCRSpace;
using PPVR.OCRBenchmark.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
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
                    Console.WriteLine("{0,-15} {1,15} {2,35} {3,25} {4,20}", "TIME", "Nº ELEITORAL", "NOME", "OCR",
                        "MATCH TYPE");

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

                            Console.WriteLine("{0,-15} {1,15} {2,35} {3,25} {4,20}", DateTime.Now.ToLongTimeString(),
                                santinhoPolitico.NumeroEleitoral, santinhoPolitico.NomeCandidato, "Tesseract",
                                santinhoPolitico.MatchTesseract);

                            SantinhosPoliticos.Add(santinhoPolitico);
                        }
                    }
                }
                var t = MainAsync();
                t.Wait();
                ExportResultToExcel();
            }
            else
            {
                Console.WriteLine("O diretório não foi informado.");
            }
            Console.WriteLine("Precione qualquer tecla para encerrar.");
            Console.ReadKey();
        }

        private static async Task MainAsync()
        {
            // OCR.Space
            foreach (var item in SantinhosPoliticos)
            {
                item.TextoOCRSpace = await SpaceHelper.UploadAndRecognizeImage(item.ImageFilePath);

                item.MatchOCRSpace = PesquisarCandidatoTexto(item.NomeCandidato, item.NumeroEleitoral,
                    item.TextoOCRSpace);

                Console.WriteLine("{0,-15} {1,15} {2,35} {3,25} {4,20}", DateTime.Now.ToLongTimeString(),
                    item.NumeroEleitoral, item.NomeCandidato, "OCR.Space", item.MatchOCRSpace);

                Thread.Sleep(5000);
            }

            // Microsoft Cognitive Service
            foreach (var item in SantinhosPoliticos)
            {
                item.TextoMicrosoftCognitiveServices =
                    await MicrosoftCognitiveServicesHelper.UploadAndRecognizeImage(item.ImageFilePath);

                item.MatchMicrosoftCognitiveServices = PesquisarCandidatoTexto(item.NomeCandidato, item.NumeroEleitoral,
                    item.TextoMicrosoftCognitiveServices);

                Console.WriteLine("{0,-15} {1,15} {2,35} {3,25} {4,20}", DateTime.Now.ToLongTimeString(),
                    item.NumeroEleitoral, item.NomeCandidato, "MS Cognitive Services",
                    item.MatchMicrosoftCognitiveServices);

                Thread.Sleep(5000);
            }
        }

        private static void ExportResultToExcel()
        {
            var directoryInfo = Directory.CreateDirectory("output");
            var fileInfo = new FileInfo(directoryInfo.FullName + @"\analise_ocr.xlsx");

            if (fileInfo.Exists)
                File.Delete(fileInfo.FullName);

            var excelPackage = new ExcelPackage(fileInfo);
            var worksheet = excelPackage.Workbook.Worksheets.Add("Analise OCRs");

            // Headers
            worksheet.Cells[1, 1].Value = "Nome Candidato";
            worksheet.Cells[1, 2].Value = "Número Eleitoral";
            worksheet.Cells[1, 3].Value = "Arquivo";
            worksheet.Cells[1, 4].Value = "OCR.Space Match";
            worksheet.Cells[1, 5].Value = "MS Cognitive Services Match";
            worksheet.Cells[1, 6].Value = "Tesseract Match";
            worksheet.Cells[1, 7].Value = "OCR.Space Texto";
            worksheet.Cells[1, 8].Value = "MS Cognitive Services Texto";
            worksheet.Cells[1, 9].Value = "Tesseract Texto";

            var rowNumber = 2;

            foreach (var santinhoPolitico in SantinhosPoliticos)
            {
                worksheet.Cells[rowNumber, 1].Value = santinhoPolitico.NomeCandidato;
                worksheet.Cells[rowNumber, 2].Value = santinhoPolitico.NumeroEleitoral;
                worksheet.Cells[rowNumber, 3].Value = Path.GetFileName(santinhoPolitico.ImageFilePath);

                // Matches
                worksheet.Cells[rowNumber, 4].Value = santinhoPolitico.MatchOCRSpace;
                worksheet.Cells[rowNumber, 5].Value = santinhoPolitico.MatchMicrosoftCognitiveServices;
                worksheet.Cells[rowNumber, 6].Value = santinhoPolitico.MatchTesseract;

                // Textos
                worksheet.Cells[rowNumber, 7].Value = santinhoPolitico.TextoOCRSpace;
                worksheet.Cells[rowNumber, 8].Value = santinhoPolitico.TextoMicrosoftCognitiveServices;
                worksheet.Cells[rowNumber, 9].Value = santinhoPolitico.TextoTesseract;

                rowNumber++;
            }
            excelPackage.Save();
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