using System;
using System.Diagnostics;
using MeliLibTools.MeliLibApi;
using MeliLibTools.Client;
using MeliLibTools.Model;

namespace Example
{
    class GetTokenExample
    {
        public static void Mainext(){
            Configuration config = new Configuration();
            config.BasePath = "https://api.mercadolibre.com";
            var apiInstance = new OAuth20Api(config);
            var grantType = "authorization_code"; 
            var clientId = "8858081937589207";  // string 
            var clientSecret = "TET0K7X2lh0jo23B3yULhpkuWWM834Um";  // string 
            var redirectUri = "https://www.google.com";  // string 
            var code = "TG-5f9ac0b9dd5613000622d92d-664147756";
            try
            {
              // Request Access Token
               Token result = apiInstance.GetToken(grantType, clientId, clientSecret, redirectUri, code);
               Debug.WriteLine(result);
                // To see output in console
                // var console = apiInstance.GetTokenWithHttpInfo(grantType, clientId, clientSecret, redirectUri, code);
                // Console.Write("Resultado get:" + console.Data);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling ItemsApi.ItemsIdGet: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
