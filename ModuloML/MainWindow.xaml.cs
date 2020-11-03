using MeliLibTools.Client;
using MeliLibTools.MeliLibApi;
using MeliLibTools.Model;
using ModuloML.Objetos;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ModuloML.Classes_auxiliares.Registro;
using static ModuloML.Objetos.RetornoVendaML;

namespace ModuloML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Result> results { get { return RetornaVenda(); } }
        public List<Result> resultsFiltrados { get { return RetornaVendaPorStatusDesc(); } }
        ChromeDriver chrome = new ChromeDriver();
        public MainWindow()
        {
            InitializeComponent();
        }
        string token = "";
        string id_cliente = "664147756";
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AcessaUrlAutenticadora();

            
            //WebBrowser web = new WebBrowser();
            // web.Navigate("https://www.youtube.com/");
            // web.Source
            // string a =  Navegador.Source.ToString();
            // int a;
        }
        public void AcessaUrlAutenticadora() 
        {
            if (!ID_txb.Text.Equals("") || !ID_txb.Text.Equals(null) || !URL_txb.Text.Equals("") || !URL_txb.Text.Equals(null))
            {
                try
                {
                   // var chrome = new ChromeDriver();

                    chrome.Navigate().GoToUrl("https://auth.mercadolivre.com.br/authorization?response_type=code&client_id=" + ID_txb.Text + "&redirect_uri=" + URL_txb.Text);
                    //System.Diagnostics.Process.Start("chrome.exe", "https://pt.stackoverflow.com");
                    
                  }
                catch (Exception ex) 
                {
                
                }
            }
            else 
            {
                MessageBox.Show("Preencha os campos","Atenção");
            }

        }

        private void GerarCodTG_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void geraToken_btn_Click(object sender, RoutedEventArgs e)
        {
            GeraToken();
        }
        public  string GeraRefreshToken()
        {
            string valorSalvo = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt");
            if (valorSalvo.Length < 75)
            {
                File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt", String.Empty);
                MeliLibTools.Client.Configuration config = new MeliLibTools.Client.Configuration();
                config.BasePath = "https://api.mercadolibre.com";
                var apiInstance = new OAuth20Api(config);
                var grantType = "refresh_token";
                var clientId = ID_txb.Text;  // string 
                var clientSecret = AppSecret_txb.Text;   // string 
                var redirectUri = URL_txb.Text; ;  // string 
                                                   // var code = "code_example"; // in this case code is null
                var refreshToken = CodTG_txb.Text;
                try
                {
                    // Request Access Token
                    Token result = apiInstance.GetToken(grantType, clientId, clientSecret, redirectUri, null, refreshToken);
                    audit("json", result.ToString());
                    token = result.AccessToken;
                   // id_cliente = result.UserId.ToString();
                    File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt", result.AccessToken);
                    return result.AccessToken;
                    // To see output in console
                    // var refresh = apiInstance.GetTokenWithHttpInfo(grantType, clientId, clientSecret, redirectUri, null, refreshToken);
                    // Console.Write("Resultado get:" + refresh.Data);
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
            else { return valorSalvo; }
        }
        public string GeraToken()
        {
            string valorSalvo = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt");
            string valorTG = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}ArquivoTG.txt");
            if (valorTG.Length < 74) 
            {
            
            
            }
            
            if (valorSalvo.Length < 74)
            {
                File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt", String.Empty);
                MeliLibTools.Client.Configuration config = new MeliLibTools.Client.Configuration();
                config.BasePath = "https://api.mercadolibre.com";
                var apiInstance = new OAuth20Api(config);
                var grantType = "authorization_code";
                var clientId = ID_txb.Text;  // string 
                var clientSecret = AppSecret_txb.Text;  // string 
                var redirectUri = URL_txb.Text;  // string 
                var code = CodTG_txb.Text;
                try
                {
                    // Request Access Token
                    Token result = apiInstance.GetToken(grantType, clientId, clientSecret, redirectUri, code);
                    audit("json", result.ToString());
                    token = result.AccessToken;
                   // id_cliente = result.UserId.ToString();
                    File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt", result.AccessToken);
                    return result.AccessToken;

                    // To see output in console
                    // var console = apiInstance.GetTokenWithHttpInfo(grantType, clientId, clientSecret, redirectUri, code);
                    // Console.Write("Resultado get:" + console.Data);
                }
                catch (ApiException e)
                {

                    Debug.Print("Exception when calling ItemsApi.ItemsIdGet: " + e.Message);
                    Debug.Print("Status Code: " + e.ErrorCode);
                    Debug.Print(e.StackTrace);
                    return e.Message;
                }
            }
            else
            {
                token = valorSalvo;
                return valorSalvo; }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshToken_btn_Click(object sender, RoutedEventArgs e)
        {
            GeraRefreshToken();
        }

        private void PuxarVendas_btn_Click(object sender, RoutedEventArgs e)
        {
           // RetornaVenda();
            batatagrid.ItemsSource = results;
        }
        public List<Result> RetornaVenda() 
        {
            var a = chrome.Url;
            var client = new RestClient("https://api.mercadolibre.com/orders/search?seller=" + id_cliente + "&access_token=" + token);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
            IRestResponse response = client.Execute(request);
            audit("vendas", response.Content);
            RetornaVendaML myDeserializedClass = JsonConvert.DeserializeObject<RetornaVendaML>(response.Content.ToString());
           
            return myDeserializedClass.results;

           // results = myDeserializedClass.results;

           // batatagrid.Items.Add(results);
        }
        
        public List<Result> RetornaVendaPorStatusDesc()
        {
            var a = chrome.Url;
            var client = new RestClient(" https://api.mercadolibre.com/orders/search?seller="+ id_cliente +"&order.status="+ Status_cxb.Text + "&sort=date_desc&access_token="+token);
            //var client = new RestClient("https://api.mercadolibre.com/orders/search?seller=" + id_cliente + "&access_token=" + token);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
            IRestResponse response = client.Execute(request);
            audit("vendas", response.Content);
            RetornaVendaML myDeserializedClass = JsonConvert.DeserializeObject<RetornaVendaML>(response.Content.ToString());

            return myDeserializedClass.results;

            // results = myDeserializedClass.results;

            // batatagrid.Items.Add(results);
        }

        private void PuxarVendasfiltradas_btn_Click(object sender, RoutedEventArgs e)
        {
            batatagrid.ItemsSource = resultsFiltrados;
           /// RetornaVendaPorStatusDesc();
        }
    }
}
