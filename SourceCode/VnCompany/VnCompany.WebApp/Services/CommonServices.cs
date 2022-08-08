using VnCompany.WebApp.Settings;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace VnCompany.WebApp.Services
{
    public static class CommonServices
    {
        //public static HttpClient AuthorizationHttpClientCore(this HttpClient client)
        //{
        //    //Set Basic Auth
        //    var user = (!string.IsNullOrEmpty(AuthorizationCoreSettings.Username)) ? AuthorizationCoreSettings.Username : "username";
        //    var password = (!string.IsNullOrEmpty(AuthorizationCoreSettings.Password)) ? AuthorizationCoreSettings.Password : "password";
        //    var authorizeHeaderKey = (!string.IsNullOrEmpty(AuthorizationCoreSettings.HeaderKey)) ? AuthorizationCoreSettings.HeaderKey : "key";
        //    var authorizeHeaderValue = (!string.IsNullOrEmpty(AuthorizationCoreSettings.HeaderValue)) ? AuthorizationCoreSettings.HeaderValue : "value";

        //    var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{password}"));
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);

        //    client.DefaultRequestHeaders.Add(authorizeHeaderKey, authorizeHeaderValue);

        //    return client;
        //}

        //public static HttpClient AuthorizationHttpCustomerCore(this HttpClient client)
        //{
        //    //Set Basic Auth
        //    var user = (!string.IsNullOrEmpty(AuthorizationCustomerCoreSettings.Username)) ? AuthorizationCustomerCoreSettings.Username : "username";
        //    var password = (!string.IsNullOrEmpty(AuthorizationCustomerCoreSettings.Password)) ? AuthorizationCustomerCoreSettings.Password : "password";
        //    var authorizeHeaderKey = (!string.IsNullOrEmpty(AuthorizationCustomerCoreSettings.HeaderKey)) ? AuthorizationCustomerCoreSettings.HeaderKey : "key";
        //    var authorizeHeaderValue = (!string.IsNullOrEmpty(AuthorizationCustomerCoreSettings.HeaderValue)) ? AuthorizationCustomerCoreSettings.HeaderValue : "value";

        //    var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{password}"));
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);

        //    client.DefaultRequestHeaders.Add(authorizeHeaderKey, authorizeHeaderValue);

        //    return client;
        //}

        //public static HttpClient AuthorizationHttpClientSocial(this HttpClient client)
        //{
        //    //Set Basic Auth
        //    var user = (!string.IsNullOrEmpty(AuthorizationSocialSettings.Username)) ? AuthorizationSocialSettings.Username : "username";
        //    var password = (!string.IsNullOrEmpty(AuthorizationSocialSettings.Password)) ? AuthorizationSocialSettings.Password : "password";
        //    var authorizeHeaderKey = (!string.IsNullOrEmpty(AuthorizationSocialSettings.HeaderKey)) ? AuthorizationSocialSettings.HeaderKey : "key";
        //    var authorizeHeaderValue = (!string.IsNullOrEmpty(AuthorizationSocialSettings.HeaderValue)) ? AuthorizationSocialSettings.HeaderValue : "value";

        //    var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{password}"));
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);

        //    client.DefaultRequestHeaders.Add(authorizeHeaderKey, authorizeHeaderValue);

        //    return client;
        //}

        //public static RestRequest RakutenAuthorization(this RestRequest request)
        //{

        //    var secretKey = ConfigurationManager.AppSettings["Rakuten:ApiSecretKey"];
        //    var licenseKey = ConfigurationManager.AppSettings["Rakuten:ApiLicenseKey"];

        //    var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{secretKey}:{licenseKey}"));
        //    request.AddHeader("Authorization", string.Format("ESA {0}", base64String));

        //    return request;
        //}

        //public static HttpClient RakutenAuthorization(this HttpClient request)
        //{

        //    var secretKey = ConfigurationManager.AppSettings["Rakuten:ApiSecretKey"];
        //    var licenseKey = ConfigurationManager.AppSettings["Rakuten:ApiLicenseKey"];

        //    var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{secretKey}:{licenseKey}"));
            
        //    request.DefaultRequestHeaders.Add("Authorization", string.Format("ESA {0}", base64String));

        //    return request;
        //}

        //public static RestRequest FaximoAuthorization(this RestRequest request)
        //{

        //    var userId = ConfigurationManager.AppSettings["Faximo:ApiUserId"];
        //    var userPwd = ConfigurationManager.AppSettings["Faximo:ApiUserPwd"];

        //    var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userId}:{userPwd}"));
        //    request.AddHeader("X-Auth", string.Format("{0}", base64String));

        //    return request;
        //}
    }
}