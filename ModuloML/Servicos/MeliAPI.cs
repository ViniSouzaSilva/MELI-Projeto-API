using MeliLibTools.Client;
using MeliLibTools.MeliLibApi;
using MeliLibTools.Model;
using ModuloML.Objetos;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using static ModuloML.Objetos.RetornoVendaML;
using static ModuloML.Classes_auxiliares.Registro;
namespace ModuloML.Servicos
{
    public static class MeliAPI
    {
       // MainWindow main = new MainWindow();
        
        

        
        //DadosDoEmitente.Root DadosEmitente = new DadosDoEmitente.Root();
        //RetornaVendaML retorno = new RetornaVendaML();
       // string token = "";
      //  string id_cliente = "664147756";

        public static string GeraRefreshToken(string clientId, string clientSecret, string redirectUri, string refreshToken)
        {

            File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt", String.Empty);
            MeliLibTools.Client.Configuration config = new MeliLibTools.Client.Configuration();
            config.BasePath = "https://api.mercadolibre.com";
            var apiInstance = new OAuth20Api(config);
            var grantType = "refresh_token";

            try
            {
                using (var seleciona = new MELIDataSetTableAdapters.TB_MELITableAdapter())
                {
                    // Request Access Token
                    Token result = apiInstance.GetToken(grantType, clientId, clientSecret, redirectUri, null, refreshToken);
                    audit("json", result.ToString());
                    //token = result.AccessToken;
                    // id_cliente = result.UserId.ToString();
                    File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt", result.AccessToken);
                    var info = seleciona.RetornaInfo(clientId);

                    seleciona.SalvaToken(result.AccessToken, DateTime.Now, result.RefreshToken, result.AccessToken.Substring(8, 16));

                    return result.AccessToken;
                    // To see output in console
                    // var refresh = apiInstance.GetTokenWithHttpInfo(grantType, clientId, clientSecret, redirectUri, null, refreshToken);

                }// Console.Write("Resultado get:" + refresh.Data);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling ItemsApi.ItemsIdGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
                audit("", e.Message);
                return e.Message;
            }
        }
        
        
            // 


        
        public static void GeraTGcode(string clientId, string redirectUri)
        {
            MeliLibTools.Client.Configuration config = new MeliLibTools.Client.Configuration();
            config.BasePath = "https://auth.mercadolibre.com.ar";
            var apiInstance = new OAuth20Api(config);
            var responseType = "code";
            //var clientId = "3486164010731858";  // string 
            // var redirectUri = "urlRedirect";  // string 
            try
            {
                // Authentication Endpoint
                apiInstance.Auth(responseType, clientId, redirectUri);
                // To see output in console
                var getCode = apiInstance.AuthWithHttpInfo(responseType, clientId, redirectUri);
                //Console.Write("Resultado get:" + getCode);
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling ItemsApi.ItemsIdGet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
        public static void RefreshTodosToken()
        {
            try
            {
                using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
                {
                    foreach (MELIDataSet.TB_MELIRow row in consulta.PegaTodosOsValores().Rows)
                    {
                        if (row.IsREFRESH_TOKENNull())
                        {
                            audit("Refresh code não encontrado", "Refresh token não encontrado, favor gerar o token pela primeira vez");
                        }
                        else
                        {
                            GeraRefreshToken(row.ID_APP, row.APP_SECRET, row.URL_REDIRECT, row.REFRESH_TOKEN);
                        }
                    }

                }
            }
            catch (Exception ex)
            {


            }
        }
        public static void VerificaValidadeToken()
        {
            bool ExisteTokenVencido = false;
            using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
            {
                foreach (MELIDataSet.TB_MELIRow row in consulta.PegaTodosOsValores().Rows)
                {
                    DateTime DateToken = row.DT_TOKEN;
                    System.TimeSpan diferenca = DateTime.Now.Subtract(DateToken);
                    if (diferenca.TotalHours >= 5)
                    {
                        ExisteTokenVencido = true;
                    }

                }
                if (ExisteTokenVencido == true)
                {
                    RefreshTodosToken();
                }
            }
            ///return true;
        }
        
        public static void RetornaInfoVenda(string userId, string orderId, string Token)
        {
            try
            {
                //var client = new RestClient("http://api.mercadolibre.com/users/"+userId+"/invoices/orders/"+orderId+"?access_token="+Token);
                var client = new RestClient("http://api.mercadolibre.com/users/" + userId + "/invoices/" + orderId + "?access_token=" + Token);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch (Exception ex)
            {

            }
        }
        public static void RetornaXmlVenda(string userId, string orderId, string Token)
        {
            try
            {
                //var client = new RestClient("http://api.mercadolibre.com/users/"+userId+"/invoices/orders/"+orderId+"?access_token="+Token);
                var client = new RestClient("https://api.mercadolibre.com/users/" + userId + "/invoices/documents/xml/" + orderId + "/authorized");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch (Exception ex)
            {

            }
        }
       
        
        public static DadosAdicionaisProd.Root RetornaDadosProd(string IdMeli)
        {
            try
            {
                var client = new RestClient("https://api.mercadolibre.com/items/" + IdMeli + "?include_attributes=all");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                IRestResponse response = client.Execute(request);
                DadosAdicionaisProd.Root myDeserializedClass = JsonConvert.DeserializeObject<DadosAdicionaisProd.Root>(response.Content);
                return myDeserializedClass;
            }
            catch (Exception ex)
            {
                return null;
            }
            // Console.WriteLine(response.Content);

        }


    }
}

