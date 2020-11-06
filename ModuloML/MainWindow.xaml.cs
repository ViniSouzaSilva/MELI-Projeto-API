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
using System.Windows.Media.Media3D;
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
        MELIdataset.TB_MELIDataTable TB_MELIdatatable;
        ChromeDriver chrome = new ChromeDriver();
        public MainWindow()
        {

            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1252 = Encoding.GetEncoding(1252);
            VerificaValidadeToken();
            PopulaComboBox();
            RefreshTodosToken();


            // RefreshTodosToken();
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
                MessageBox.Show("Preencha os campos", "Atenção");
            }

        }

        private void GerarCodTG_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void geraToken_btn_Click(object sender, RoutedEventArgs e)
        {
            //GeraToken();
        }
        public string GeraRefreshToken(string clientId, string clientSecret, string redirectUri, string refreshToken)
        {

            File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt", String.Empty);
            MeliLibTools.Client.Configuration config = new MeliLibTools.Client.Configuration();
            config.BasePath = "https://api.mercadolibre.com";
            var apiInstance = new OAuth20Api(config);
            var grantType = "refresh_token";

            try
            {
                using (var seleciona = new MELIdatasetTableAdapters.TB_MELITableAdapter())
                {
                    // Request Access Token
                    Token result = apiInstance.GetToken(grantType, clientId, clientSecret, redirectUri, null, refreshToken);
                    audit("json", result.ToString());
                    token = result.AccessToken;
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


        public string GeraToken()
        {
            using (var seleciona = new MELIdatasetTableAdapters.TB_MELITableAdapter())
            {
                //var info = seleciona.RetornaInfo(ID_APP);
                //  if (info is null)
                //  {
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

                    //var info = seleciona.RetornaInfo(result.AccessToken.Substring(7,15));
                    var info = seleciona.RetornaInfo(clientId);
                    if (info is null || info.Equals(""))
                    {
                        seleciona.InsereInfo(clientId, clientSecret, redirectUri, code);
                        seleciona.SalvaToken(result.AccessToken, DateTime.Now, result.RefreshToken, clientId);

                    }
                    else
                    {
                        seleciona.SalvaToken(result.AccessToken, DateTime.Now, result.RefreshToken, result.AccessToken.Substring(8, 16));
                    }
                    return result.AccessToken;
                    // To see output in console
                    // var console = apiInstance.GetTokenWithHttpInfo(grantType, clientId, clientSecret, redirectUri, code);
                    // Console.Write("Resultado get:" + console.Data);
                }
                catch (ApiException e)
                {

                    MessageBox.Show("Atenção", e.Message + e.ErrorCode + e.StackTrace);

                    audit("Atenção", e.Message + e.ErrorCode + e.StackTrace);

                    return e.Message;
                }

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshToken_btn_Click(object sender, RoutedEventArgs e)
        {
            //GeraRefreshToken();
        }

        private void PuxarVendas_btn_Click(object sender, RoutedEventArgs e)
        {
            // RetornaVenda();
            batatagrid.ItemsSource = results;
        }
        public List<Result> RetornaVenda()
        {
            var a = chrome.Url;
            VerificaValidadeToken();
            if (lojas_cxb.SelectedItem.Equals(""))
            {
                MessageBox.Show("Atenção", "Selecione o a conta ML corretamente");
                return results ;
            }
            else
            {
                using (var consulta = new MELIdatasetTableAdapters.TB_MELITableAdapter())
                {
                    var loja = consulta.RetornaInfo(lojas_cxb.Text);
                    var client = new RestClient("https://api.mercadolibre.com/orders/search?seller=" + loja[0].ID_APP + "&access_token=" + loja[0].TOKEN);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                    IRestResponse response = client.Execute(request);
                    audit("vendas", response.Content);
                    RetornaVendaML myDeserializedClass = JsonConvert.DeserializeObject<RetornaVendaML>(response.Content.ToString());

                    return myDeserializedClass.results;
                }
            }
            // results = myDeserializedClass.results;

            // batatagrid.Items.Add(results);
        }

        public List<Result> RetornaVendaPorStatusDesc()
        {
            var a = chrome.Url;
            VerificaValidadeToken();
            var client = new RestClient(" https://api.mercadolibre.com/orders/search?seller=" + id_cliente + "&order.status=" + Status_cxb.Text + "&sort=date_desc&access_token=" + token);
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            //GeraTGcode(ID_txb.Text, URL_txb.Text);
            using (var registra = new MELIdatasetTableAdapters.TB_MELITableAdapter())
            {
                if (!CodTG_txb.Text.Equals(""))
                {
                    try
                    {
                        registra.InsereInfo(ID_txb.Text.ToString(), AppSecret_txb.Text.ToString(), URL_txb.Text.ToString(), CodTG_txb.Text.ToString());

                        GeraToken();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Atenção", ex.Message);
                    }

                }
                else
                {
                    MessageBox.Show("Atenção", "Digite o TGCODE");

                }
            }
        }
        public void GeraTGcode(string clientId, string redirectUri)
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
        public void RefreshTodosToken()
        {
            try
            {
                using (var consulta = new MELIdatasetTableAdapters.TB_MELITableAdapter())
                {
                    foreach (MELIdataset.TB_MELIRow row in consulta.PegaTodosOsValores().Rows)
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
        public void VerificaValidadeToken()
        {
            bool ExisteTokenVencido = false;
            using (var consulta = new MELIdatasetTableAdapters.TB_MELITableAdapter())
            {
                foreach (MELIdataset.TB_MELIRow row in consulta.PegaTodosOsValores().Rows)
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
        public void PopulaComboBox()
        {
            using (var consulta = new MELIdatasetTableAdapters.TB_MELITableAdapter())
            {
                foreach (MELIdataset.TB_MELIRow row in consulta.PegaTodosOsValores().Rows)
                {

                    lojas_cxb.Items.Add(row.ID_APP);

                }
            }
        }
    }
}
