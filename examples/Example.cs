using System;
using System.Collections.Generic;
using System.Diagnostics;
using MeliLibTools.MeliLibApi;
using MeliLibTools.Client;
using MeliLibTools.Model;
using static Example.GetTokenExample;
using System.Security.Cryptography.X509Certificates;
using RestSharp;

namespace Example
{
    class Example
    {
        public static void Main(){
            // Insert code example here
            //refreshToken();();
            // test();
            //abc();
            Mainext();
        }
        public static void refreshToken()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.mercadolibre.com";
            var apiInstance = new OAuth20Api(config);
            var grantType = "refresh_token";
            var clientId = "3816195610189148";  // string 
            var clientSecret = "SM9XYvPsmxhvhHXp44AxDbDtbmmLXGj2";  // string 
            var redirectUri = "https://www.google.com";  // string 
            // var code = "code_example"; // in this case code is null
            var refreshToken = "refresh_token_example";
            try
            {
                // Request Access Token
                Token result = apiInstance.GetToken(grantType, clientId, clientSecret, redirectUri, null, refreshToken);
                Debug.WriteLine(result);
                // To see output in console
                // var refresh = apiInstance.GetTokenWithHttpInfo(grantType, clientId, clientSecret, redirectUri, null, refreshToken);
                // Console.Write("Resultado get:" + refresh.Data);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling ItemsApi.ItemsIdGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
            
        }
        public static void test() 
        {
            try
            {
                var client = new RestClient("https://api.mercadolibre.com/users/test_user?access_token=APP_USR-3486164010731858-102717-61b2bfb67e3bd15662688dcb38d6e755-509562030");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\r\n   \t\"site_id\":\"MLB\"\r\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public static void abc() 
        {

            try
            {
                var client = new RestClient("https://api.mercadolibre.com/orders/search?seller=664147756&access_token=APP_USR-3486164010731858-102813-b714d9bec3f1b6dc94304df68c033657-664147756");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
