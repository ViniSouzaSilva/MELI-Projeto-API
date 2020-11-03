using MeliLibTools.Client;
using MeliLibTools.MeliLibApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ModuloML.ConsultasAPI
{
    class AuthProtocolo
    {
        public static void GeraCodAutenticacaoToken( string clientId, string redirectUri)
        {
            Configuration config = new Configuration();
            config.BasePath = "https://auth.mercadolibre.com.ar";
            var apiInstance = new OAuth20Api(config);
            var responseType = "code";
             //clientId = "3486164010731858";  // string 
             //redirectUri = "urlRedirect";  // string 
            try
            {
                // Authentication Endpoint
                apiInstance.Auth(responseType, clientId, redirectUri);
                // To see output in console
                // var getCode = apiInstance.AuthWithHttpInfo(responseType, clientId, redirectUri);
                // Console.Write("Resultado get:" + getCode);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling ItemsApi.ItemsIdGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
