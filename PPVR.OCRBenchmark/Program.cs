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
        #region Properties

        private static readonly ICollection<SantinhoPolitico> SantinhosPoliticos = new List<SantinhoPolitico>();
        private static readonly ContadorMatches MatchesOCRSpace = new ContadorMatches();
        private static readonly ContadorMatches MatchesMicrosoftCognitiveServices = new ContadorMatches();
        private static readonly ContadorMatches MatchesTesseract = new ContadorMatches();

        #endregion

        public static void Main(string[] args)
        {
            if (args.Any())
            {
                var files = Directory.GetFiles(args[0]);

                if (files.Any())
                {
                    Console.WriteLine("{0,-15} {1,15} {2,35} {3,25} {4,20}", "TIME", "Nº ELEITORAL", "NOME", "OCR",
                        "MATCH TYPE");

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
                                NomeCandidato = candidato[1]
                            };

                            #region Tesseract

                            santinhoPolitico.TextoTesseract = TesseractHelper.UploadAndRecognizeImage(imageFilePath);

                            santinhoPolitico.MatchTesseract = PesquisarCandidatoTexto(santinhoPolitico.NomeCandidato,
                                santinhoPolitico.NumeroEleitoral, santinhoPolitico.TextoTesseract);

                            switch (santinhoPolitico.MatchTesseract)
                            {
                                case MatchType.Nome:
                                    MatchesTesseract.QtdeMatchesNome++;
                                    break;
                                case MatchType.NumeroEleitoral:
                                    MatchesTesseract.QtdeMatchesNumeroEleitoral++;
                                    break;
                                case MatchType.NomeENumeroEleitoral:
                                    MatchesTesseract.QtdeMatchesNomeENumeroEleitoral++;
                                    break;
                            }

                            Console.WriteLine("{0,-15} {1,15} {2,35} {3,25} {4,20}", DateTime.Now.ToLongTimeString(),
                                santinhoPolitico.NumeroEleitoral, santinhoPolitico.NomeCandidato, "Tesseract",
                                santinhoPolitico.MatchTesseract);

                            #endregion

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
            #region OCR.Space

            foreach (var item in SantinhosPoliticos)
            {
                item.TextoOCRSpace = await SpaceHelper.UploadAndRecognizeImage(item.ImageFilePath);

                item.MatchOCRSpace = PesquisarCandidatoTexto(item.NomeCandidato, item.NumeroEleitoral,
                    item.TextoOCRSpace);

                // Conta a quantidade de matches para cada tipo de match.
                switch (item.MatchOCRSpace)
                {
                    case MatchType.Nome:
                        MatchesOCRSpace.QtdeMatchesNome++;
                        break;
                    case MatchType.NumeroEleitoral:
                        MatchesOCRSpace.QtdeMatchesNumeroEleitoral++;
                        break;
                    case MatchType.NomeENumeroEleitoral:
                        MatchesOCRSpace.QtdeMatchesNomeENumeroEleitoral++;
                        break;
                }

                Console.WriteLine("{0,-15} {1,15} {2,35} {3,25} {4,20}", DateTime.Now.ToLongTimeString(),
                    item.NumeroEleitoral, item.NomeCandidato, "OCR.Space", item.MatchOCRSpace);

                Thread.Sleep(5000);
            }

            #endregion

            #region Microsoft Cognitive Service

            foreach (var item in SantinhosPoliticos)
            {
                item.TextoMicrosoftCognitiveServices =
                    await MicrosoftCognitiveServicesHelper.UploadAndRecognizeImage(item.ImageFilePath);

                item.MatchMicrosoftCognitiveServices = PesquisarCandidatoTexto(item.NomeCandidato, item.NumeroEleitoral,
                    item.TextoMicrosoftCognitiveServices);

                // Conta a quantidade de matches para cada tipo de match.
                switch (item.MatchMicrosoftCognitiveServices)
                {
                    case MatchType.Nome:
                        MatchesMicrosoftCognitiveServices.QtdeMatchesNome++;
                        break;
                    case MatchType.NumeroEleitoral:
                        MatchesMicrosoftCognitiveServices.QtdeMatchesNumeroEleitoral++;
                        break;
                    case MatchType.NomeENumeroEleitoral:
                        MatchesMicrosoftCognitiveServices.QtdeMatchesNomeENumeroEleitoral++;
                        break;
                }

                Console.WriteLine("{0,-15} {1,15} {2,35} {3,25} {4,20}", DateTime.Now.ToLongTimeString(),
                    item.NumeroEleitoral, item.NomeCandidato, "MS Cognitive Services",
                    item.MatchMicrosoftCognitiveServices);

                Thread.Sleep(5000);
            }

            #endregion
        }

        private static void ExportResultToExcel()
        {
            var directoryInfo = Directory.CreateDirectory("output");
            var fileInfo = new FileInfo(directoryInfo.FullName + @"\analise_ocr.xlsx");

            if (fileInfo.Exists)
                File.Delete(fileInfo.FullName);

            #region Dados Extraídos Arquivos

            var excelPackage = new ExcelPackage(fileInfo);
            var worksheet = excelPackage.Workbook.Worksheets.Add("Dados Extraídos");

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
                // Informações do Candidato
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

            #endregion

            #region Analise Resultados

            var worksheetAnaliseResultados = excelPackage.Workbook.Worksheets.Add("Analise Resultados OCRs");

            // Horizontal Headers
            worksheetAnaliseResultados.Cells[1, 2].Value = "OCR.Space";
            worksheetAnaliseResultados.Cells[1, 3].Value = "Microsoft Cognitive Services";
            worksheetAnaliseResultados.Cells[1, 4].Value = "Tesseract";

            // Vertical Headers
            worksheetAnaliseResultados.Cells[2, 1].Value = "Matches Nome";
            worksheetAnaliseResultados.Cells[3, 1].Value = "Matches Número Eleitoral";
            worksheetAnaliseResultados.Cells[4, 1].Value = "Matches Nome/Número Eleitoral";
            worksheetAnaliseResultados.Cells[5, 1].Value = "Total";

            var totalCandidatos = SantinhosPoliticos.Count;

            #region Analise Resultados OCR.Space

            worksheetAnaliseResultados.Cells[2, 2].Value =
                $"{MatchesOCRSpace.QtdeMatchesNome}/{totalCandidatos} => {MatchesOCRSpace.QtdeMatchesNome * 100 / totalCandidatos}%";

            worksheetAnaliseResultados.Cells[3, 2].Value =
                $"{MatchesOCRSpace.QtdeMatchesNumeroEleitoral}/{totalCandidatos} => {MatchesOCRSpace.QtdeMatchesNumeroEleitoral * 100 / totalCandidatos}%";

            worksheetAnaliseResultados.Cells[4, 2].Value =
                $"{MatchesOCRSpace.QtdeMatchesNomeENumeroEleitoral}/{totalCandidatos} => {MatchesOCRSpace.QtdeMatchesNomeENumeroEleitoral * 100 / totalCandidatos}%";

            var totalMatchesOCRSpace = MatchesOCRSpace.QtdeMatchesNome + MatchesOCRSpace.QtdeMatchesNumeroEleitoral +
                                       MatchesOCRSpace.QtdeMatchesNomeENumeroEleitoral;

            worksheetAnaliseResultados.Cells[5, 2].Value =
                $"{totalMatchesOCRSpace}/{totalCandidatos} => {totalMatchesOCRSpace * 100 / totalCandidatos}%";

            #endregion

            #region Analise Resultados Microsoft Cognitive Services

            worksheetAnaliseResultados.Cells[2, 3].Value =
                $"{MatchesMicrosoftCognitiveServices.QtdeMatchesNome}/{totalCandidatos} => {MatchesMicrosoftCognitiveServices.QtdeMatchesNome * 100 / totalCandidatos}%";

            worksheetAnaliseResultados.Cells[3, 3].Value =
                $"{MatchesMicrosoftCognitiveServices.QtdeMatchesNumeroEleitoral}/{totalCandidatos} => {MatchesMicrosoftCognitiveServices.QtdeMatchesNumeroEleitoral * 100 / totalCandidatos}%";

            worksheetAnaliseResultados.Cells[4, 3].Value =
                $"{MatchesMicrosoftCognitiveServices.QtdeMatchesNomeENumeroEleitoral}/{totalCandidatos} => {MatchesMicrosoftCognitiveServices.QtdeMatchesNomeENumeroEleitoral * 100 / totalCandidatos}%";

            var totalMatchesMicrosoftCognitiveServices = MatchesMicrosoftCognitiveServices.QtdeMatchesNome +
                                                         MatchesMicrosoftCognitiveServices.QtdeMatchesNumeroEleitoral +
                                                         MatchesMicrosoftCognitiveServices.QtdeMatchesNomeENumeroEleitoral;

            worksheetAnaliseResultados.Cells[5, 3].Value =
                $"{totalMatchesMicrosoftCognitiveServices}/{totalCandidatos} => {totalMatchesMicrosoftCognitiveServices * 100 / totalCandidatos}%";

            #endregion

            #region Analise Resultados Tesseract

            worksheetAnaliseResultados.Cells[2, 4].Value =
                $"{MatchesTesseract.QtdeMatchesNome}/{totalCandidatos} => {MatchesTesseract.QtdeMatchesNome * 100 / totalCandidatos}%";

            worksheetAnaliseResultados.Cells[3, 4].Value =
                $"{MatchesTesseract.QtdeMatchesNumeroEleitoral}/{totalCandidatos} => {MatchesTesseract.QtdeMatchesNumeroEleitoral * 100 / totalCandidatos}%";

            worksheetAnaliseResultados.Cells[4, 4].Value =
                $"{MatchesTesseract.QtdeMatchesNomeENumeroEleitoral}/{totalCandidatos} => {MatchesTesseract.QtdeMatchesNomeENumeroEleitoral * 100 / totalCandidatos}%";

            var totalMatchesTesseract = MatchesTesseract.QtdeMatchesNome + MatchesTesseract.QtdeMatchesNumeroEleitoral +
                                        MatchesTesseract.QtdeMatchesNomeENumeroEleitoral;

            worksheetAnaliseResultados.Cells[5, 4].Value =
                $"{totalMatchesTesseract}/{totalCandidatos} => {totalMatchesTesseract * 100 / totalCandidatos}%";

            #endregion

            #endregion

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

            if (containsNumeroEleitoral)
            {
                if (texto.Contains(nomeCandidato, StringComparison.OrdinalIgnoreCase))
                    matchType = MatchType.NomeENumeroEleitoral;
            }
            else
            {
                if (texto.Contains(nomeCandidato, StringComparison.OrdinalIgnoreCase))
                    matchType = MatchType.Nome;
            }

            return matchType;
        }
    }

    internal class ContadorMatches
    {
        public int QtdeMatchesNome { get; set; }
        public int QtdeMatchesNumeroEleitoral { get; set; }
        public int QtdeMatchesNomeENumeroEleitoral { get; set; }
    }
}