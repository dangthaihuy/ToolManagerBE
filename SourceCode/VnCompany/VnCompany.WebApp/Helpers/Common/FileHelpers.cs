//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Newtonsoft.Json;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace VnCompany.WebApp.Helpers
{
    public class FileHelpers<T>
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(FileHelpers));
        public static T ReadJsonFileToObject(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
                else
                {
                    FileStream str = File.Create(filePath);
                    str.Close();
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Could not ReadJsonFileToObject because: {0}", ex.ToString());
                ErrorLogger.log(strError);
            }

            return default(T);
        }

        public static void WriteObjectToJsonFile(T model, string filePath)
        {
            try
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(model));
            }
            catch (Exception ex)
            {
                var strError = string.Format("Could not WriteObjectToJsonFile because: {0}", ex.ToString());
                ErrorLogger.log(strError);
            }
        }
    }

    public class FileHelpers
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(FileHelpers));
        public static string MakeSureFolderExists(string folderPath)
        {
            bool exists = System.IO.Directory.Exists(folderPath);

            if (!exists)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("MakeSureFolderExists error because: {0}", ex.ToString()));
                }
            }

            return folderPath;
        }

        public static string GetFileContent(string filePath)
        {            
            var fileContent = string.Empty;
            try
            {
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read the stream to a string, and write the string to the console.
                    fileContent = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("GetFileContent error because: {0}", ex.ToString()));
            }

            return fileContent;
        }

        public static async void WriteTextAsync(string folderPath, string fileName, string text, string fileExt = "txt")
        {
            // Set a variable to the My Documents path.
            //string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the text asynchronously to a new file named "WriteTextAsync.txt".
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(folderPath, fileName + "." + fileExt)))
            {
                await outputFile.WriteAsync(text);
            }
        }

        public static string ParsePdfToTextUsingPDFBox(string input)
        {
            PDDocument doc = null;

            try
            {
                input = input.Replace(@"\", @"/");
                doc = PDDocument.load(input);
                PDFTextStripper stripper = new PDFTextStripper();

                var pdfFileInText = stripper.getText(doc);

                //pdfFileInText = ConvertToUtf8(pdfFileInText);

                return pdfFileInText;
            }
            catch(Exception ex)
            {
                _logger.Error(string.Format("ParsePdfToTextUsingPDFBox error because: {0}", ex.ToString()));
            }
            finally
            {
                if (doc != null)
                {
                    doc.close();
                }
            }

            return string.Empty;
        }

        public static string ExtractTextFromPdf(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    if (new FileInfo(path).Length == 0)
                    {
                        return string.Empty;
                    }
                }

                PdfReader reader = new PdfReader(path);
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }

                var result = text.ToString();
                result = result.StringNormally();

                if (!string.IsNullOrEmpty(result))
                {
                    result = Regex.Replace(result, @"\r\n?|\n", Environment.NewLine);
                    //result = Regex.Replace(result, @"(\+[0-9]{2}|\+[0-9]{2}\(0\)|\(\+[0-9]{2}\)\(0\)|00[0-9]{2}|0)([0-9]{9}|[0-9\-\s]{9,50})", string.Empty);
                }

                reader.Close();

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("ExtractTextFromPdf error because: {0}", ex.ToString()));
            }

            return string.Empty;
        }
    }
}