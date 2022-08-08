using VnCompany.WebApp.Helpers;
using VnCompany.WebApp.Settings;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace VnCompany.WebApp
{    
    public class LanguageMessageHandler : DelegatingHandler
    {
        //private readonly List<string> _supportedLanguages = LanguagesProvider._supportedLanguages;
        public static bool IsSupported(string langCode)
        {
            //if (LanguagesProvider._supportedLanguages.Contains(langCode))
            //    return true;
            if (LanguagesProvider.GetListLanguages().Any(x => x.LanguageCultureName.Contains(langCode)))
                return true;

            return false;
        }

        private bool SetHeaderIfAcceptLanguageMatchesSupportedLanguage(HttpRequestMessage request)
        {
            foreach (var lang in request.Headers.AcceptLanguage)
            {
                if (IsSupported(lang.Value))
                {
                    SetCulture(request, lang.Value);
                    return true;
                }
            }

            return false;
        }

        private bool SetHeaderIfGlobalAcceptLanguageMatchesSupportedLanguage(HttpRequestMessage request)
        {
            var listLangs = LanguagesProvider.GetListLanguages();
            foreach (var lang in request.Headers.AcceptLanguage)
            {
                var globalLang = lang.Value.Substring(0, 2);
                if (LanguagesProvider.GetListLanguages().Any(t => t.LanguageCultureName.StartsWith(globalLang)))
                {
                    //SetCulture(request, _supportedLanguages.FirstOrDefault(i => i.StartsWith(globalLang)));
                    SetCulture(request, listLangs.FirstOrDefault(i => i.LanguageCultureName.StartsWith(globalLang)).LanguageCultureName);
                    return true;
                }
            }

            return false;
        }

        private void SetCulture(HttpRequestMessage request, string lang)
        {
            request.Headers.AcceptLanguage.Clear();
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(lang));
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!SetHeaderIfAcceptLanguageMatchesSupportedLanguage(request))
            {
                // Whoops no localization found. Lets try Globalisation
                if (!SetHeaderIfGlobalAcceptLanguageMatchesSupportedLanguage(request))
                {
                    // no global or localization found
                    SetCulture(request, LanguagesProvider._defaultLang);
                }
            }

            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }

        public void SetLanguage(string lang)
        {
            try
            {
                if (!IsSupported(lang)) lang = GetDefaultLanguage();
                var cultureInfo = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddYears(1);

                System.Web.HttpContext.Current.Response.Cookies.Delete(SystemSettings.CultureKey);

                System.Web.HttpContext.Current.Response.Cookies.Append(SystemSettings.CultureKey, lang, option);
            }
            catch
            {

            }
        }

        public static string GetDefaultLanguage()
        {
            return LanguagesProvider._defaultLang;
        }
    }


    /// <summary>
    /// Utility class that allows serialisation of .NET resource files (.resx) 
    /// into different formats
    /// </summary>
    public static class ResourceSerialiser
    {
        #region JSON Serialisation
        /// <summary>
        /// Converts a resrouce type into an equivalent JSON object using the 
        /// current Culture
        /// </summary>
        /// <param name="resource">The resoruce type to serialise</param>
        /// <returns>A JSON string representation of the resource</returns>
        public static string ToJson(Type resource)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            return ToJson(resource, culture);
        }

        /// <summary>
        /// Converts a resrouce type into an equivalent JSON object using the 
        /// culture derived from the language code passed in
        /// </summary>
        /// <param name="resource">The resoruce type to serialise</param>
        /// <param name="languageCode">The language code to derive the culture</param>
        /// <returns>A JSON string representation of the resource</returns>
        public static string ToJson(Type resource, string languageCode)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo(languageCode);
            return ToJson(resource, culture);
        }

        /// <summary>
        /// Converts a resrouce type into an equivalent JSON object
        /// </summary>
        /// <param name="resource">The resoruce type to serialise</param>
        /// <param name="culture">The culture to retrieve</param>
        /// <returns>A JSON string representation of the resource</returns>
        public static string ToJson(Type resource, CultureInfo culture)
        {
            Dictionary<string, string> dictionary = ResourceToDictionary(resource, culture);
            return JsonConvert.SerializeObject(dictionary);
        }
        #endregion

        /// <summary>
        /// Converts a resrouce type into a dictionary type while localising 
        /// the strings using the passed in culture
        /// </summary>
        /// <param name="resource">The resoruce type to serialise</param>
        /// <param name="culture">The culture to retrieve</param>
        /// <returns>A dictionary representation of the resource</returns>
        private static Dictionary<string, string> ResourceToDictionary(Type resource, CultureInfo culture)
        {
            ResourceManager rm = new ResourceManager(resource);
            var resourceSet = rm.GetResourceSet(culture, true, true);

            var dictionary = resourceSet.Cast<DictionaryEntry>()
                                    .ToDictionary(r => r.Key.ToString(),
                                                  r => r.Value.ToString());
            return dictionary;
        }
    }
}