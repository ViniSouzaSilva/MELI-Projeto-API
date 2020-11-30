using MeliLibTools.Client;
using MeliLibTools.MeliLibApi;
using MeliLibTools.Model;
using ModuloML.Objetos;
using ModuloML.Servicos;
using ModuloML.Telas;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using RestSharp;
using RestSharp.Extensions;
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
using static ModuloML.Servicos.MeliAPI;

namespace ModuloML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {

            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1252 = Encoding.GetEncoding(1252);
            VerificaValidadeToken();
            //RetornaDadosEmitente();
            PopulaComboBox();
            //RefreshTodosToken();


            // RefreshTodosToken();
        }
        #region Atributos
        public List<Result> results { get { return RetornaVenda(); } }
        public List<Result> resultsFiltrados { get { return RetornaVendaPorStatusDesc(); } }
        MELIDataSet.TB_MELIDataTable TB_MELIdatatable;
        //ChromeDriver chrome = new ChromeDriver();
        DadosDoEmitente.Root DadosEmitente = new DadosDoEmitente.Root();
        RetornaVendaML retorno = new RetornaVendaML();
        string token = "";
        string id_cliente = "664147756";
        #endregion
        #region Métodos
        public void AcessaUrlAutenticadora()
        {
            if (!ID_txb.Text.Equals("") || !ID_txb.Text.Equals(null) || !URL_txb.Text.Equals("") || !URL_txb.Text.Equals(null))
            {
                try
                {
                     var chrome = new ChromeDriver();

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
        public string GeraToken()
        {
            using (var seleciona = new MELIDataSetTableAdapters.TB_MELITableAdapter())
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
        public void PopulaComboBox()
        {
            using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
            {
                foreach (MELIDataSet.TB_MELIRow row in consulta.PegaTodosOsValores().Rows)
                {

                    lojas_cxb.Items.Add(row.ID_APP);

                }
            }
        }
        public List<Result> RetornaVenda()
        {
            RetornaDadosEmitente();
            VerificaValidadeToken();
            if (lojas_cxb.SelectedItem.Equals(""))
            {
                MessageBox.Show("Atenção", "Selecione o a conta ML corretamente");
                return results;
            }
            else
            {
                using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
                {
                    var loja = consulta.RetornaInfo(lojas_cxb.Text);
                    var client = new RestClient("https://api.mercadolibre.com/orders/search?seller=" + loja[0].TOKEN.Substring(loja[0].TOKEN.Length - 9, 9) + "&access_token=" + loja[0].TOKEN); ;
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                    IRestResponse response = client.Execute(request);
                    audit("vendas", response.Content);
                    RetornaVendaML myDeserializedClass = JsonConvert.DeserializeObject<RetornaVendaML>(response.Content.ToString());
                    retorno = myDeserializedClass;
                    //batatagrid.Items.Add(results);
                    //results = myDeserializedClass.results;
                    return myDeserializedClass.results;
                }
            }
        }
            public List<Result> RetornaVendaPorStatusDesc()
            {
                //var a = chrome.Url;
                RetornaDadosEmitente();
                VerificaValidadeToken();
                var client = new RestClient(" https://api.mercadolibre.com/orders/search?seller=" + id_cliente + "&order.status=paid&sort=date_desc&access_token=" + token);
                //var client = new RestClient("https://api.mercadolibre.com/orders/search?seller=" + id_cliente + "&access_token=" + token);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                IRestResponse response = client.Execute(request);
                audit("vendas", response.Content);
                RetornaVendaML myDeserializedClass = JsonConvert.DeserializeObject<RetornaVendaML>(response.Content.ToString());
            //batatagrid.Items.Add(results);
                ProcessaVenda(myDeserializedClass);
                return myDeserializedClass.results;

                //results = myDeserializedClass.results;

                // batatagrid.Items.Add(results);
            }
            public void RetornaInfoUser()
            {

                VerificaValidadeToken();
                if (lojas_cxb.SelectedItem.Equals(""))
                {
                    MessageBox.Show("Atenção", "Selecione o a conta ML corretamente");
                    //return results;
                }
                else
                {
                    using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
                    {
                        var loja = consulta.RetornaInfo(lojas_cxb.Text);
                        var client = new RestClient("https://api.mercadolibre.com/users/$USER_ID/addresses?access_token=$ACCESS_TOKEN");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.GET);
                        request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                        IRestResponse response = client.Execute(request);
                        //Console.WriteLine(response.Content);
                        RetornaVendaML myDeserializedClass = JsonConvert.DeserializeObject<RetornaVendaML>(response.Content.ToString());

                        //return myDeserializedClass.results;
                    }
                }
                // results = myDeserializedClass.results;

                // batatagrid.Items.Add(results);
            }
            public void RetornaDadosEmitente()
            {

                VerificaValidadeToken();
                if (lojas_cxb.SelectedItem.Equals(""))
                {
                    MessageBox.Show("Atenção", "Selecione o a conta ML corretamente");
                    // return results;
                }
                else
                {
                    using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
                    {
                        var loja = consulta.RetornaInfo(lojas_cxb.Text);
                        var client = new RestClient("https://api.mercadolibre.com/users/me?access_token=" + loja[0].TOKEN);
                        client.Timeout = -1;
                        var request = new RestRequest(Method.GET);
                        request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                        IRestResponse response = client.Execute(request);

                        DadosDoEmitente.Root myDeserializedClass = JsonConvert.DeserializeObject<DadosDoEmitente.Root>(response.Content.ToString());

                        DadosEmitente = myDeserializedClass;


                        //return myDeserializedClass.results;
                    }
                }
                // results = myDeserializedClass.results;

                // batatagrid.Items.Add(results);
            }
            public void ProcessaVenda(RetornaVendaML vendas) 
            {
            try
            {
                using (var consulta = new MELIDataSetTableAdapters.TB_VENDASTableAdapter())
                {
                    foreach (var a in vendas.results)
                    {
                        var ResultConsulta = consulta.ExisteVenda(vendas.results[0].id.ToString());
                        if (ResultConsulta == 1)
                        {

                        }
                        else if (ResultConsulta == 0)
                        {
                            consulta.InsereVenda(vendas.results[0].id.ToString(),vendas.results[0].buyer.first_name+" "+vendas.results[0].buyer.last_name, vendas.results[0].buyer.billing_info.doc_number,DateTime.Now,"","0");
                        }
                        else 
                        {
                            audit("Existe mais de uma venda com o mesmo ID, favor verificar com suporte", vendas.results[0].id.ToString());
                            MessageBox.Show("Existe mais de uma venda com o mesmo ID, favor verificar com suporte","Atenção",MessageBoxButton.OK,MessageBoxImage.Error);
                        }

                    }
                }
            }
            catch (Exception ex )
            {
                audit("Ocorreu um Erro no Metodo ProcessaVenda()",ex.Message);

               
            }  
            }
        public DadosAdicionaisProd.Root RetornaDadosVenda( string ID_anuncio) 
        {
            var client = new RestClient("https://api.mercadolibre.com/items/"+ ID_anuncio +"?include_attributes=all");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
            IRestResponse response = client.Execute(request);
            DadosAdicionaisProd.Root myDeserializedClass = JsonConvert.DeserializeObject<DadosAdicionaisProd.Root>(response.Content.ToString());

            return myDeserializedClass;

        }
            #endregion
            #region Métodos Click
            private void Button_Click(object sender, RoutedEventArgs e)
            {
                AcessaUrlAutenticadora();


                //WebBrowser web = new WebBrowser();
                // web.Navigate("https://www.youtube.com/");
                // web.Source
                // string a =  Navegador.Source.ToString();
                // int a;
            }

            private void GerarCodTG_btn_Click(object sender, RoutedEventArgs e)
            {

            }

            private void geraToken_btn_Click(object sender, RoutedEventArgs e)
            {
                //GeraToken();
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

            private void PuxarVendasfiltradas_btn_Click(object sender, RoutedEventArgs e)
            {
            //ProcessaVenda(resultsFiltrados);
                batatagrid.ItemsSource = resultsFiltrados;
                /// RetornaVendaPorStatusDesc();
            }

            private void Button_Click_2(object sender, RoutedEventArgs e)
            {

            }

            private void Add_btn_Click(object sender, RoutedEventArgs e)
            {
                //GeraTGcode(ID_txb.Text, URL_txb.Text);
                using (var registra = new MELIDataSetTableAdapters.TB_MELITableAdapter())
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

            private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
            {

                using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
                {
                    var loja = consulta.RetornaInfo(lojas_cxb.Text);
                    RetornoVendaML.Result id = (RetornoVendaML.Result)batatagrid.SelectedItem;

                    var IdSeller = id.seller.id;
                    var IdOrder = id.payments[0].id;
                DadosVerificacao dados = new DadosVerificacao();
                dados.DescricaoProd = id.order_items[0].item.title;
                dados.Preco = id.order_items[0].full_unit_price;
                dados.Quantidade = id.order_items[0].quantity;
                dados.NumeroAnuncio = id.order_items[0].item.id;
                DadosAdicionaisProd.Root a = RetornaDadosVenda(dados.NumeroAnuncio);
                dados.CodBarras = ((a.attributes.Select(x => x).Where(x => x.id.Equals("GTIN")).FirstOrDefault()) ?? (new DadosAdicionaisProd.Attribute())).value_name;
                dados.NomeComprador = id.buyer.first_name + id.buyer.last_name;
                dados.CPFcomprador = id.buyer.billing_info.doc_number;
                dados.IdCompra = id.payments[0].order_id.ToString();

                    CadastroNF tela = new CadastroNF(id);
                     tela.ShowDialog();
                    //ConversaoML.Conversao(retorno,DadosEmitente);
                    // RetornaInfoVenda(IdSeller.ToString(), IdOrder.ToString(),loja[0].TOKEN);
                    // RetornaXmlVenda(IdSeller.ToString(), IdOrder.ToString(), loja[0].TOKEN);
            }
                // RetornoVendaML.Result a = (RetornoVendaML.RetornaVendaML)id;

            }

            
            #endregion
        }
    } 
