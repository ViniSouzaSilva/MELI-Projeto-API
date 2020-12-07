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
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
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
        //public List<Result> resultsFiltrados { get { return RetornaVendaPorStatusDesc(); } }
        public Collection<DadosVendas> DadosV = new Collection<DadosVendas>();
        public MELIDataSet.TB_VENDASDataTable tB_VENDASRows;
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
           // RetornaDadosEmitente();
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
                    //audit("vendas", response.Content);
                    RetornaVendaML myDeserializedClass = JsonConvert.DeserializeObject<RetornaVendaML>(response.Content.ToString());
                    retorno = myDeserializedClass;
                    //batatagrid.Items.Add(results);
                    //results = myDeserializedClass.results;
                    return myDeserializedClass.results;
                }
            }
        }
        public List<Result> RetornaVendaPorStatusDesc(string Id_App)
        {
            try
            {
                RetornaDadosEmitente(Id_App);
                VerificaValidadeToken();
               /* if (lojas_cxb.SelectedItem.Equals(""))
                {
                    MessageBox.Show("Atenção", "Selecione o a conta ML corretamente");
                    return results;
                }
                else
                {*/
                    using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
                    {
                        //var a = chrome.Url;
                        var loja = consulta.RetornaInfo(Id_App);
                        var client = new RestClient(" https://api.mercadolibre.com/orders/search?seller=" + id_cliente + "&order.status=paid&sort=date_desc&access_token=" + loja[0].TOKEN);
                        //var client = new RestClient("https://api.mercadolibre.com/orders/search?seller=" + id_cliente + "&access_token=" + token);
                        client.Timeout = -1;
                        var request = new RestRequest(Method.GET);
                        request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                        IRestResponse response = client.Execute(request);
                        //audit("vendas", response.Content);
                        RetornaVendaML myDeserializedClass = JsonConvert.DeserializeObject<RetornaVendaML>(response.Content.ToString());
                        //batatagrid.Items.Add(results);
                        if (myDeserializedClass.results != null)
                        {
                            ProcessaAsincronamente(myDeserializedClass);
                            AtualizaDataGridAsync();
                        }
                        // ProcessaVenda(myDeserializedClass);
                        return myDeserializedClass.results;
                    }
                    //results = myDeserializedClass.results;

                    // batatagrid.Items.Add(results);
                
            }
            catch (Exception ex) 
            {
                audit("Problemas com o Método de RetornaVendaPorStatusDesc()", ex.Message);
                MessageBox.Show("Não foi possível fazer a consulta, verifique o Log de erros","Atenção",MessageBoxButton.OK,MessageBoxImage.Error);
                return null;
            }

        }
        public List<Result> RetornaVendaPorStatusDescClone(string Id_App)
        {
            try
            {
                RetornaDadosEmitente(Id_App);
                VerificaValidadeToken();
               
                    using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
                    {
                        DateTime Intervalo = DateTime.Now.Date.AddDays(-60);

                        //var a = chrome.Url;
                        var loja = consulta.RetornaInfo(Id_App);
                        //  var client = new RestClient(" https://api.mercadolibre.com/orders/search?seller=" + id_cliente + "&order.status=paid&sort=date_desc&access_token=" + loja[0].TOKEN);
                        //var client = new RestClient("https://api.mercadolibre.com/orders/search?seller=" + id_cliente + "&access_token=" + token);
                        var client = new RestClient(" https://api.mercadolibre.com/orders/search?seller=" + id_cliente + "&order.date_created.from=" + Intervalo.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK") + "&order.date_created.to="+DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK") + "&order.status=paid&sort=date_desc&access_token=" + loja[0].TOKEN);
                        client.Timeout = -1;
                        
                                            
                        var request = new RestRequest(Method.GET);
                        request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                        IRestResponse response = client.Execute(request);
                        //audit("vendas", response.Content);
                        RetornaVendaML myDeserializedClass = JsonConvert.DeserializeObject<RetornaVendaML>(response.Content.ToString());
                        //batatagrid.Items.Add(results);
                        /* if (myDeserializedClass.results != null)
                         {
                             ProcessaAsincronamente(myDeserializedClass);
                         }*/
                        // ProcessaVenda(myDeserializedClass);
                        ProcessaVenda(myDeserializedClass);
                        return myDeserializedClass.results;
                    }
                    //results = myDeserializedClass.results;

                    // batatagrid.Items.Add(results);
                
            }
            catch (Exception ex)
            {
                audit("Problemas com o Método de RetornaVendaPorStatusDesc()", ex.Message);
                MessageBox.Show("Não foi possível fazer a consulta, verifique o Log de erros", "Atenção", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

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
            public void RetornaDadosEmitente(string Id_App)
            {
            try
            {
                VerificaValidadeToken();
                
                    using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
                    {
                        var loja = consulta.RetornaInfo(Id_App);
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
            catch (Exception ex) 
            {
                audit("Erro no método RetornaDadosEmitente()",ex.Message );
            
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
                    int count = 0;
                    string codbarras = String.Empty;
                    double PrecoCheio = 0;
                    foreach (var a in vendas.results)
                    {
                        var ResultConsulta = consulta.ExisteVenda(vendas.results[count].id.ToString());
                        if (ResultConsulta == 1)
                        {

                        }
                        else if (ResultConsulta == 0)
                        {
                            DadosAdicionaisProd.Root GTIN = RetornaDadosVenda(vendas.results[count].order_items[0].item.id);
                            codbarras = ((GTIN.attributes.Select(x => x).Where(x => x.id.Equals("GTIN")).FirstOrDefault()) ?? (new DadosAdicionaisProd.Attribute())).value_name;
                            PrecoCheio = vendas.results[count].order_items[0].quantity * vendas.results[count].order_items[0].full_unit_price;
                            if (String.IsNullOrEmpty(vendas.results[count].buyer.first_name)) { vendas.results[count].buyer.first_name = "Nenhum nome encontrado"; }
                            if (String.IsNullOrEmpty(vendas.results[count].buyer.billing_info.doc_number)) { vendas.results[count].buyer.billing_info.doc_number = "";}
                            if (String.IsNullOrEmpty(vendas.results[count].order_items[0].item.title)) { vendas.results[count].order_items[0].item.title = "Descrição não encontrada"; }
                            if (String.IsNullOrEmpty(codbarras)) { codbarras = " "; }
                            if (String.IsNullOrEmpty(vendas.results[count].order_items[0].item.id)) { vendas.results[count].order_items[0].item.id = " "; };
                            if (PrecoCheio == null) { PrecoCheio = 0; }
                            if (vendas.results[count].order_items[0].quantity == null) { vendas.results[count].order_items[0].quantity = 0; }

                            consulta.InsereVenda(vendas.results[count].id.ToString(),vendas.results[count].buyer.first_name+" "+vendas.results[count].buyer.last_name, vendas.results[count].buyer.billing_info.doc_number,DateTime.Now,"","0",vendas.results[count].order_items[0].item.title,codbarras,vendas.results[count].order_items[0].quantity,Convert.ToDecimal(PrecoCheio), vendas.results[count].order_items[0].item.id);
                        }
                        else 
                        {
                            audit("Existe mais de uma venda com o mesmo ID, favor verificar com suporte", vendas.results[count].id.ToString());
                           // MessageBox.Show("Existe mais de uma venda com o mesmo ID, favor verificar com suporte","Atenção",MessageBoxButton.OK,MessageBoxImage.Error);
                        }
                        count++;
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
        public async Task<string> ProcessaAsincronamente(RetornaVendaML vendas)

        {
            int count = 0;
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
                    {
                        var AppId = consulta.PegaTodosOsValores();
                        foreach (var a in AppId.Rows)
                        {
                            RetornaVendaPorStatusDescClone(AppId[count].ID_APP);
                            count++;
                        }
                        

                        // ProcessaVenda(vendas);
                        // RetornaVendaPorStatusDescClone();
                        // batatagrid.ItemsSource = RetornaVendaPorStatusDescClone();
                        // MessageBox.Show("OK, funcionou ","Atenção");
                        await Task.Delay(TimeSpan.FromMilliseconds(20000));

                        //900000
                        //Thread.Sleep(5000);
                        i--;
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                audit("Problema no método ProcessaAssincronamente",ex.Message);
                return "deu ruim as " + DateTime.Now;
               
                //throw;


            }


        }
        public async Task<string> AtualizaDataGridAsync()

        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    DadosV.Clear();
                    PuxaVendasNãoAtribuidas();
                  
                    batatagrid.ItemsSource = DadosV;
                    batatagrid.Items.Refresh();
                    // ProcessaVenda(vendas);
                    // RetornaVendaPorStatusDescClone();
                    // batatagrid.ItemsSource = RetornaVendaPorStatusDescClone();
                    // MessageBox.Show("OK, funcionou ","Atenção");
                    await Task.Delay(TimeSpan.FromMilliseconds(10000));

                    //900000
                    //Thread.Sleep(5000);
                    i--;
                }
                return "";
            }
            catch (Exception ex)
            {
                audit("Problema no método ProcessaAssincronamente", ex.Message);
                return "deu ruim as " + DateTime.Now;

                //throw;


            }


        }
        public Collection<DadosVendas> PuxaVendasNãoAtribuidas() 
        {
            try
            {
                using (var consulta = new MELIDataSetTableAdapters.TB_VENDASTableAdapter())
                {
                    tB_VENDASRows  = consulta.RetornaVendasNaoAtribuidas();
                    foreach (MELIDataSet.TB_VENDASRow row in tB_VENDASRows.Rows) 
                    {
                        /*
                        if (row.ID_VENDA.Equals(DBNull.Value)) { row.ID_VENDA = "";}
                        if (row.NOMECOMPRADOR.Equals(DBNull.Value)) { row.NOMECOMPRADOR = ""; }
                        if (row.CPF_COMPRADOR.Equals(DBNull.Value)) { row.CPF_COMPRADOR = ""; }
                        if (row.DATAVENDA.Equals(DBNull.Value)) { row.DATAVENDA = DateTime.UtcNow;}
                        if (row.ATRIBUICAO.Equals(DBNull.Value)) { row.ATRIBUICAO = "";}
                        if (row.STATUS_ATRIBUICAO.Equals(DBNull.Value)) { row.STATUS_ATRIBUICAO = "0"; }
                        if (row.DESCRICAOPROD.Equals(DBNull.Value)) { row.DESCRICAOPROD = ""; }
                        if (row.CODBARRAS.Equals(DBNull.Value)) { row.CODBARRAS = ""; }
                        if (row.QUANTIDADE.Equals(DBNull.Value)) { row.QUANTIDADE = 0; }
                        if (row.PRECO.Equals(DBNull.Value)) { row.PRECO = 0; }
                        if (row.ID_ANUNCIO.Equals(DBNull.Value)) { row.ID_ANUNCIO = ""; }*/
                        DadosV.Add(new DadosVendas() 
                        {   ID_VENDA = row.ID_VENDA,
                            NOMECOMPRADOR = row.NOMECOMPRADOR,
                            CPF_COMPRADOR = row.CPF_COMPRADOR,
                            DATAVENDA = row.DATAVENDA,
                            ATRIBUICAO = row.ATRIBUICAO,
                            STATUS_ATRIBUICAO = row.STATUS_ATRIBUICAO,
                            DESCRICAOPROD = row.DESCRICAOPROD,
                            CODBARRAS = row.CODBARRAS,
                            QUANTIDADE = row.QUANTIDADE,
                            PRECO = row.PRECO,
                            ID_ANUNCIO = row.ID_ANUNCIO });


                    }
                    return DadosV;

                }


            }
            catch (Exception ex)
            {
                return DadosV;
                
            }
        
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
            using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
            {
                int count = 0;
                var AppId = consulta.PegaTodosOsValores();
                foreach (var a in AppId.Rows)
                {
                    RetornaVendaPorStatusDesc(AppId[count].ID_APP);
                    count++;
                }
                //batatagrid.ItemsSource = DadosV;
            }/// RetornaVendaPorStatusDesc();
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
            try
            {
                    using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
                    {
                        // var loja = consulta.RetornaInfo(lojas_cxb.Text);
                        DadosVendas id = (DadosVendas)batatagrid.SelectedItem;

                        DadosVerificacao dadosVerificacao = new DadosVerificacao();
                        dadosVerificacao.DescricaoProd = id.DESCRICAOPROD;
                        dadosVerificacao.Preco = id.PRECO;
                        dadosVerificacao.Quantidade = id.QUANTIDADE;
                        dadosVerificacao.NumeroAnuncio = id.ID_ANUNCIO;
                        dadosVerificacao.CodBarras = id.CODBARRAS;
                        dadosVerificacao.NomeComprador = id.NOMECOMPRADOR;
                        dadosVerificacao.CPFcomprador = id.CPF_COMPRADOR;
                        dadosVerificacao.IdCompra = id.ID_VENDA;
                    }  
            }
            catch (Exception ex) 
            {
                audit("Erro ao selecionar a venda",ex.Message);
            }
               /*     var IdSeller = id.seller.id;
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
                     tela.ShowDialog();*/
                    //ConversaoML.Conversao(retorno,DadosEmitente);
                    // RetornaInfoVenda(IdSeller.ToString(), IdOrder.ToString(),loja[0].TOKEN);
                    // RetornaXmlVenda(IdSeller.ToString(), IdOrder.ToString(), loja[0].TOKEN);
            
                // RetornoVendaML.Result a = (RetornoVendaML.RetornaVendaML)id;

            }

            
            #endregion
        }
    } 
