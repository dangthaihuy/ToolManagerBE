using VnCompany.SharedLibs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VnCompany.WebApp.Helpers
{
    public static class CommonExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";

            return false;
        }

        public static string StringNormally(this string myString)
        {
            var output = string.Empty;
            if (!string.IsNullOrEmpty(myString))
            {
                output = myString.Normalize(NormalizationForm.FormKC);
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);

                output = regex.Replace(output, " ");
            }

            return output;
        }

        //public List<string> GetAllSentencesInform(ComparisonModel model)
        //{
        //    List<string> sentences = new List<string>();
        //    if (model.ProductInfo != null)
        //    {
        //        if (!string.IsNullOrEmpty(model.ProductInfo.Name))
        //            sentences.Add(StringNormally(model.ProductInfo.Name));

        //        if (!string.IsNullOrEmpty(model.ProductInfo.Included))
        //            sentences.Add(StringNormally(model.ProductInfo.Included));

        //        if (!string.IsNullOrEmpty(model.ProductInfo.NutritionLabel))
        //            sentences.Add(StringNormally(model.ProductInfo.NutritionLabel));

        //        if (!string.IsNullOrEmpty(model.ProductInfo.NutritionInfo))
        //        {
        //            var lines = model.ProductInfo.NutritionInfo.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //            if (lines != null && lines.Count > 0)
        //            {
        //                foreach (var item in lines)
        //                {
        //                    var l = StringNormally(item);
        //                    if (!string.IsNullOrEmpty(l) && l != " ")
        //                        sentences.Add(l);
        //                }
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(model.ProductInfo.Material))
        //        {
        //            var lines = model.ProductInfo.Material.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //            if (lines != null && lines.Count > 0)
        //            {
        //                foreach (var item in lines)
        //                {
        //                    var l = StringNormally(item);
        //                    if (!string.IsNullOrEmpty(l) && l != " ")
        //                        sentences.Add(l);
        //                }
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(model.ProductInfo.OtherInfo))
        //        {
        //            var lines = model.ProductInfo.OtherInfo.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //            if (lines != null && lines.Count > 0)
        //            {
        //                foreach (var item in lines)
        //                {
        //                    var l = StringNormally(item);
        //                    if (!string.IsNullOrEmpty(l) && l != " ")
        //                        sentences.Add(l);
        //                }
        //            }
        //        }
        //    }

        //    return sentences;
        //}

        //public List<string> GetAllSentencesFromTemplate()
        //{
        //    List<string> sentences = new List<string>();

        //    var file = Server.MapPath("~/App_Data/protein.pdf");
        //    using (var pdf = new BitMiracle.Docotic.Pdf.PdfDocument(file))
        //    {
        //        //var output = System.IO.Path.ChangeExtension(file, ".txt");
        //        if (!System.IO.File.Exists(file)) return sentences;

        //        var documentText = pdf.GetText();
        //        var result = documentText.Normalize(NormalizationForm.FormKC);
        //        StringReader strReader = new StringReader(result);

        //        RegexOptions options = RegexOptions.None;
        //        Regex regex = new Regex("[ ]{2,}", options);
        //        while (true)
        //        {
        //            var aLine = strReader.ReadLine();
        //            if (aLine == null) break;
        //            if (!string.IsNullOrEmpty(aLine))
        //            {
        //                aLine = regex.Replace(aLine, " ");
        //                sentences.Add(aLine);
        //            }
        //        }
        //    }

        //    return sentences;
        //}
    }

    public static class ClaimsPrincipalExtensions
    {
        public static T GetLoggedInUserId<T>(this ClaimsPrincipal principal)
        {
            if (principal == null)
                return default(T);

            var loggedInUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(loggedInUserId, typeof(T));
            }
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
            {
                return loggedInUserId != null ? (T)Convert.ChangeType(loggedInUserId, typeof(T)) : (T)Convert.ChangeType(0, typeof(T));
            }
            else
            {
                return default(T);
            }
        }

        public static string GetLoggedInUserName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                return string.Empty;

            return principal.FindFirstValue(ClaimTypes.Name);
        }

        public static string GetLoggedInUserDisplayName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                return string.Empty;

            return principal.FindFirstValue(ClaimTypes.GivenName);
        }

        public static string GetLoggedInUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
                return string.Empty;

            return principal.FindFirstValue(ClaimTypes.Email);
        }
    }
}
