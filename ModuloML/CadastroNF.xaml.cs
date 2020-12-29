using ModuloML.Objetos;
using ModuloML.Properties;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;
using static ModuloML.Enum.Enums;
using static ModuloML.Servicos.Static;

namespace ModuloML.Telas
{
    /// <summary>
    /// Lógica interna para CadastroNF.xaml
    /// </summary>
    public partial class CadastroNF : Window
    {
        string id_shipment;
        string ID_venda;
        public CadastroNF(string id_venda, string ID_SHIPMENT)
        {
            InitializeComponent();
            id_shipment = ID_SHIPMENT;
            ID_venda = id_venda;
            VisualizaDados(id_venda);

        }

        public Endereco.Root ConsultaEndereco(string id_shipment) 
        {
            try
            { using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())

                {
                    var appid = consulta.PegaTodosOsValores();
                    var client = new RestClient("https://api.mercadolibre.com/shipments/" + id_shipment + "?access_token=" + appid[0].TOKEN);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                    IRestResponse response = client.Execute(request);
                    Endereco.Root myDeserializedClass = JsonConvert.DeserializeObject<Endereco.Root>(response.Content);
                    return myDeserializedClass;
                    
                }
            }
            catch (Exception ex)
            {

                throw;
            }
           

        }
        public DadosAdicionaisProd.Root PegaGTIN (string ID_ANUNCIO) 
        {
            using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
            {
                try
                {
                    var client = new RestClient("https://api.mercadolibre.com/items/" + ID_ANUNCIO + "?include_attributes=all");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    var appid = consulta.PegaTodosOsValores();
                    request.AddHeader("Authorization", "Bearer" + appid[0].TOKEN);
                    request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                    IRestResponse response = client.Execute(request);
                    DadosAdicionaisProd.Root myDeserializedClass = JsonConvert.DeserializeObject<DadosAdicionaisProd.Root>(response.Content);
                    return myDeserializedClass;
                }
                catch (Exception ex) 
                {
                    throw; 

                }
            }

        }
        public DadosEspecificosVenda.Root PesquisaDadosVenda(string ID_VENDA) 
        {
            try
            {
                using (var consulta = new MELIDataSetTableAdapters.TB_MELITableAdapter())
                {
                    var appid = consulta.PegaTodosOsValores();
                    var client = new RestClient("https://api.mercadolibre.com/orders/" + ID_VENDA + "?access_token=" + appid[0].TOKEN);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("x-format-new", "true");
                    request.AddHeader("Cookie", "_d2id=c289e63e-d0d7-467c-8630-a8cfdba640d3-n");
                    IRestResponse response = client.Execute(request);
                    DadosEspecificosVenda.Root myDeserializedClass = JsonConvert.DeserializeObject<DadosEspecificosVenda.Root>(response.Content);
                    return myDeserializedClass;
                }
            }
            catch (Exception ex) 
            {
                throw;
            }
        }






        public void VisualizaDados(string ID_VENDA)//RetornoVendaML.Result Resultado
        {
            using (var produ = new MELIDataSetTableAdapters.TB_PRODUTOSTableAdapter())
            using (var consulta = new MELIDataSetTableAdapters.TB_CLIENTESTableAdapter())
            {
                try
                {
                    // int existe = (int)consulta.ExisteCliente(Resultado.buyer.billing_info.doc_number);
                    //  if (existe == 0)
                    var Resultado = PesquisaDadosVenda(ID_VENDA);
                    NomeComprador_txb.Text = Resultado.buyer.first_name + " " + Resultado.buyer.last_name;
                    DTEmissao_txb.Text = Convert.ToDateTime(Resultado.date_created).ToString("dd/MM/yyyy HH:mm:ss");
                    DTSaida_txb.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    RegimeTrb_cxb.SelectedIndex = 0;
                    RegimeTrb_cxb.IsReadOnly = true;
                    Finalidade_cxb.SelectedIndex = 0;
                    Finalidade_cxb.IsReadOnly = true;
                    IndPress_cxb.SelectedIndex = 0;
                    IndPress_cxb.IsReadOnly = true;

                    if (Resultado.buyer.billing_info.doc_number.Length == 11)
                        {
                            TipoPessoa_cxb.SelectedIndex = 0 ;
                            Contribuinte_cxb.SelectedIndex = 2;
                            TipoPessoa_cxb.IsReadOnly = true;
                            Contribuinte_cxb.IsReadOnly = true;
                        }
                        else
                        if (Resultado.buyer.billing_info.doc_number.Length == 14)
                        {
                            TipoPessoa_cxb.SelectedIndex = 1 ;
                            TipoPessoa_cxb.IsReadOnly = true;

                        }
                        else
                        {
                            TipoPessoa_cxb.SelectedIndex = 2 ;
                            Contribuinte_cxb.SelectedIndex = 2;
                            TipoPessoa_cxb.IsReadOnly = true;
                            Contribuinte_cxb.IsReadOnly = true;
                        }
                    
                    CPF_txb.Text = Resultado.buyer.billing_info.doc_number;
                    CPF_txb.IsReadOnly = true;
                    List<DadosProduNota> orderItems = new List<DadosProduNota>();
                    int id = 0;
                    orderItems.Clear();
                    foreach (var a in Resultado.order_items)
                    {
                        orderItems.Add(new DadosProduNota() {  ID = id, DESCRICAO = Resultado.order_items[0].item.title, NCM = "000", CFOP = "5555", CST="0000", QUANTIDADE = Resultado.order_items[0].quantity.ToString(), UNIDADE = "UN" });
                        id++;
                    }

                    Produ_datagrid.ItemsSource = orderItems;
                    Produ_datagrid.Items.Refresh();
                    // MessageBox.Show("Usuário não cadastrado, preencha os dados", "Atenção",MessageBoxButton.OK,MessageBoxImage.Information);


                    //else if (existe == 1)
                    {
                       // var result = consulta.RetornaInfoCliente(Resultado.buyer.billing_info.doc_number);

                        NaturezaOP_txb.Text = "Venda de Mercadoria";
                       // NomeComprador_txb.Text = result[0].NOME_RAZAO;
                       

                       

                        
                        var endereco = ConsultaEndereco(id_shipment);
                        if (endereco.receiver_address.zip_code != null)
                        { //Endereço do cliente 
                            CEP_txb.Text = endereco.receiver_address.zip_code;
                            CEP_txb.IsReadOnly = true;

                            Endereco_txb.Text = endereco.receiver_address.street_name;

                            Numero_txb.Text = endereco.receiver_address.street_number;
                            Complemento_txb.Text = endereco.receiver_address.comment;
                            Bairro_txb.Text = endereco.receiver_address.neighborhood.name;
                            Municipio_txb.Text = endereco.receiver_address.city.name;
                            UF_txb.Text = endereco.receiver_address.state.id.Substring(3, 2);
                            Telefone_txb.Text = endereco.receiver_address.receiver_phone;
                        }
                        else 
                        {// Endereço de retirada
                            CEP_txb.Text = endereco.receiver_address.zip_code;
                            CEP_txb.IsReadOnly = true;

                            Endereco_txb.Text = endereco.sender_address.street_name;

                            Numero_txb.Text = endereco.sender_address.street_number;
                            Complemento_txb.Text = endereco.sender_address.comment.ToString();
                            Bairro_txb.Text = endereco.sender_address.neighborhood.name;
                            Municipio_txb.Text = endereco.sender_address.city.name;
                            UF_txb.Text = endereco.sender_address.state.id.Substring(3, 2);
                            Telefone_txb.Text = endereco.receiver_address.receiver_phone;
                        }

                        Conversao(ID_VENDA);
                        // inicio do processamento dos produtos

                        /*  List<Produtos> ordItems = new List<Produtos>();

                          foreach (var a in Resultado.order_items)
                          {
                              ordItems.Add(new Produtos() { CodProdu = Resultado.order_items[0].item.id, Descricao = Resultado.order_items[0].item.title, VlrUni = Resultado.order_items[0].unit_price.ToString(), VlrTotal = Resultado.order_items[0].full_unit_price.ToString(), Quantidade = Resultado.order_items[0].quantity.ToString() });

                          }

                          Produ_datagrid.ItemsSource = orderItems;*/



                        /*
                        NaturezaOP_txb.Text = "Venda de Mercadoria";
                        NomeComprador_txb.Text = Resultado.results[0].buyer.nickname;
                        if (Resultado.results[0].buyer.billing_info.doc_type.Equals("CPF"))
                        {
                            TipoPessoa_cxb.SelectedItem = "Física";
                            TipoPessoa_cxb.IsReadOnly = true;

                        }
                        else
                        if (Resultado.results[0].buyer.billing_info.doc_type.Equals("CNPJ"))
                        {
                            TipoPessoa_cxb.SelectedItem = "Jurídica";
                            TipoPessoa_cxb.IsReadOnly = true;

                        }
                        else
                        {
                            TipoPessoa_cxb.SelectedItem = "Estrangeiro";
                            TipoPessoa_cxb.IsReadOnly = true;
                        }

                        CPF_txb.Text = Resultado.results[0].buyer.billing_info.doc_number;
                        CPF_txb.IsReadOnly = true;*/

                        // }
                        /* else 
                        {
                              MessageBox.Show("Mais de um usuário cadastrado com mesmo CPF", "Atenção");

                          }*/

                    }
                }
                catch (Exception ex)
                {

                }
             }
        }


        public void Conversao(string ID_VENDA)
        {
            try
            {
                File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt", String.Empty);
                string valorSalvo = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt");
                var Resultado = PesquisaDadosVenda(ID_venda);
                var Endereco = ConsultaEndereco(id_shipment);
                if (valorSalvo.Length < 1)
                {

                    string idDest = "";
                    StringBuilder Tx2 = new StringBuilder();
                    Tx2.AppendLine("INCLUIR");
                    Tx2.AppendLine("versao_A02=4.00");
                    Tx2.AppendLine("Id_A03=0");
                     switch (Endereco.sender_address.state.id)
                     {


                         case "BR-AC":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Acre}");
                             break;
                         case "BR-AL":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Alagoas}");
                             break;
                         case "BR-AP":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Amapa}");
                             break;
                         case "BR-AM":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Amazonas}");
                             break;
                         case "BR-BA":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Bahia}");
                             break;
                         case "BR-CE":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Ceara}");
                             break;
                         case "BR-DF":
                             Tx2.AppendLine($"cUF_B02={CodEstado.DistritoFederal}");
                             break;
                         case "BR-ES":
                             Tx2.AppendLine($"cUF_B02={CodEstado.EspiritoSanto}");
                             break;
                         case "BR-GO":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Goias}");
                             break;
                         case "BR-MA":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Maranhao}");
                             break;
                         case "BR-MT":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Amazonas}");
                             break;
                         case "BR-MS":
                             Tx2.AppendLine($"cUF_B02={CodEstado.MatoGrossoDoSul}");
                             break;
                         case "BR-MG":
                             Tx2.AppendLine($"cUF_B02={CodEstado.MinasGerais}");
                             break;
                         case "BR-PA":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Para}");
                             break;
                         case "BR-PB":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Paraiba}");
                             break;
                         case "BR-PR":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Parana}");
                             break;
                         case "BR-PE":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Pernambuco}");
                             break;
                         case "BR-PI":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Piaui}");
                             break;
                         case "BR-RR":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Roraima}");
                             break;
                         case "BR-RO":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Rondonia}");
                             break;
                         case "BR-RJ":
                             Tx2.AppendLine($"cUF_B02={CodEstado.RioDeJaneiro}");
                             break;
                         case "BR-RN":
                             Tx2.AppendLine($"cUF_B02={CodEstado.RioGrandeDoNorte}");
                             break;
                         case "BR-RS":
                             Tx2.AppendLine($"cUF_B02={CodEstado.RioGrandeDoSul}");
                             break;
                         case "BR-SC":
                             Tx2.AppendLine($"cUF_B02={CodEstado.RioGrandeDoSul}");
                             break;
                         case "BR-SP":
                             Tx2.AppendLine($"cUF_B02={CodEstado.SaoPaulo}");
                             idDest = "1";
                             break;
                         case "BR-SE":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Sergipe}");
                             break;
                         case "BR-TO":
                             Tx2.AppendLine($"cUF_B02={CodEstado.Tocantins}");
                             break;

                         default:
                             break;

                     }

                    Tx2.AppendLine("natOp_B04=VENDA DE MERCADORIA");
                    Tx2.AppendLine("mod_B06=55");
                    Tx2.AppendLine("serie_B07 =1");
                    using (var consulta = new MELIDataSetTableAdapters.TB_INFONFTableAdapter())
                    {
                        var valor = consulta.RetornaNumDocFiscal();
                        Tx2.AppendLine($"nNF_B08={valor[0].NUM_DOC_FISCAL}");

                    }
                    Tx2.AppendLine($"DHEMI_B09={DateTime.UtcNow}");
                    Tx2.AppendLine($"DHSAIENT_B10={DateTime.UtcNow}");
                    Tx2.AppendLine("tpNF_B11=1");
                    if (idDest.Equals("1"))
                    {
                        Tx2.AppendLine("IDDEST_B11A=1");
                    }
                    else
                    {
                        Tx2.AppendLine("IDDEST_B11A=2");
                    }
                    CodIBGEMunicipio.municipios cod;
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CodIBGEMunicipio.municipios));
                    StreamReader reader = new StreamReader($@"{AppDomain.CurrentDomain.BaseDirectory}munseed_temp.xml");
                    cod = (CodIBGEMunicipio.municipios)serializer.Deserialize(reader);
                    //File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt", String.Empty);


                    var codMunEmit = ((cod.municipio.Select(x => x).Where(x => x.Mun_Desc == Endereco.sender_address.city.name).FirstOrDefault()) ?? (new CodIBGEMunicipio.municipiosMunicipio())).ID_Municipio;

                    Tx2.AppendLine($"cMunFG_B12={codMunEmit}");
                    Tx2.AppendLine("tpImp_B21=1");
                    Tx2.AppendLine("tpEmis_B22=1");
                    Tx2.AppendLine("cDV_B23=0");
                    Tx2.AppendLine("tpAmb_B24=2");//Ambiente 1 = produção 2 = homolog
                    Tx2.AppendLine("finNFe_B25=1");
                    Tx2.AppendLine("indFinal_B25a=1");
                    Tx2.AppendLine("indPres_B25b=2");
                    Tx2.AppendLine("procEmi_B26=0");
                    Tx2.AppendLine("AmbiStore_ModuloML 1.0");
                    Tx2.AppendLine("CRT_C21=1");
                    Tx2.AppendLine($"CNPJ_C02={Settings.Default.CNPJ}");
                    Tx2.AppendLine($"xNome_C03={Settings.Default.RAZAO_SOCIAL}");
                    Tx2.AppendLine($"xFant_C04={Settings.Default.NOME_FANTASIA}");
                    Tx2.AppendLine($"xLgr_C06="+ Endereco.sender_address.street_name);
                    Tx2.AppendLine($"nro_C07="+ Endereco.sender_address.street_number);
                    Tx2.AppendLine($"xBairro_C09="+ Endereco.sender_address.street_name);
                    Tx2.AppendLine($"cMun_C10={Settings.Default.CODMUNICIPIO}");
                    Tx2.AppendLine($"xMun_C11="+ Endereco.sender_address.city.name);
                    Tx2.AppendLine($"UF_C12="+ Endereco.sender_address.state.id.Substring(3, 2));
                    Tx2.AppendLine($"CEP_C13="+ Endereco.sender_address.zip_code);
                    Tx2.AppendLine("cPais_C14=1058");
                    Tx2.AppendLine("xPais_C15=BRASIL");
                    Tx2.AppendLine("fone_C16=");
                    Tx2.AppendLine($"IE_C17={Settings.Default.IE}");


                    if (Resultado.buyer.billing_info.doc_type.Equals("CPF"))
                    {

                        Tx2.AppendLine($"CPF_E02={Resultado.buyer.billing_info.doc_number}");

                    }
                    else if (Resultado.buyer.billing_info.doc_type.Equals("CPF"))
                    {
                        Tx2.AppendLine($"CNPJ_E03={Resultado.buyer.billing_info.doc_number}");

                    }
                    else
                    {
                        Tx2.AppendLine($"idEstrangeiro_E03a={Resultado.buyer.billing_info.doc_number}");

                    }
                    Tx2.AppendLine($"xNome_E04={Resultado.buyer.nickname}");
                    //Tx2.AppendLine($"xLgr_E06={Resultado.}");
                    Tx2.AppendLine($"fone_E16="+ Endereco.receiver_address.receiver_phone);
                    Tx2.AppendLine("indEDest_E16a=9");//Verificar essa questão 

                    Tx2.AppendLine($"email_E19={Resultado.seller.email}");
                    //Tx2.AppendLine($"IE_F15=[{}");,
                    int indiceProd = 1;
                    using (var GTIN = new MELIDataSetTableAdapters.TB_EST_PRODUTOTableAdapter())
                    using (var ADD = new MELIDataSetTableAdapters.TB_ESTOQUETableAdapter())
                    using (var produ = new MELIDataSetTableAdapters.TB_PRODUTOSTableAdapter())
                    {

                       // for (int item = 0; item < Resultado.results[0].order_items[0].quantity; item++)
                       // {

                            Tx2.AppendLine("INCLUIRITEM");
                            Tx2.AppendLine($"nItem_H02={indiceProd}");
                        //var Produtos = produ.RetornaProduto(Resultado.results[item].order_items[item].item.id);
                        //var DadosAdicionaisVenda = PegaGTIN();
                        DadosAdicionaisProd.Root DadosAdicionaisVenda = PegaGTIN(Resultado.order_items[0].item.id);
                        string codbarras = ((DadosAdicionaisVenda.attributes.Select(x => x).Where(x => x.id.Equals("GTIN")).FirstOrDefault()) ?? (new DadosAdicionaisProd.Attribute())).value_name;
                        if (codbarras is null)
                        {
                            codbarras = ((DadosAdicionaisVenda.variations[0].attributes.Select(x => x).Where(x => x.id.Equals("GTIN")).FirstOrDefault()) ?? (new DadosAdicionaisProd.Attribute())).value_name;
                        }
                        var DadosFiscaisVenda = GTIN.PegaDadosViaCodBarras(codbarras);
                        var DadosFiscaisVendaAdicionais = ADD.PegaDadosViaCodBarrasAdicionais(DadosFiscaisVenda[0].ID_IDENTIFICADOR);


                        //if (DadosFiscaisVenda[0].ID_IDENTIFICADOR == null) { DadosFiscaisVenda[0].ID_IDENTIFICADOR = 0; }
                        Tx2.AppendLine($"cProd_I02="+SeNulo(DadosFiscaisVenda[0].ID_IDENTIFICADOR.ToString()));
                        //var dadosProd = ((dadosAdicionaisProd.attributes.Select(x => x).Where(x => x.id.Equals("GTIN")).FirstOrDefault()) ?? (new DadosAdicionaisProd.Attribute())).value_name;
                            Tx2.AppendLine($"cEAN_I03=" + SeNulo(DadosFiscaisVenda[0].COD_BARRA));
                            Tx2.AppendLine($"xProd_I04={Resultado.order_items[0].item.title}");
                            Tx2.AppendLine($"NCM_I05="+SeNulo(DadosFiscaisVenda[0].COD_NCM));
                            Tx2.AppendLine($"CEST_I05c="+SeNulo(DadosFiscaisVenda[0].COD_CEST));
                            Tx2.AppendLine($"indEscala_I05d=S");
                            Tx2.AppendLine($"CFOP_I08="+ SeNulo(DadosFiscaisVendaAdicionais[0].CFOP));
                            Tx2.AppendLine($"uCom_I09="+ SeNulo(DadosFiscaisVendaAdicionais[0].UNI_MEDIDA));
                            Tx2.AppendLine($"qCom_I10="+Resultado.order_items[0].quantity);
                            Tx2.AppendLine($"vUnCom_I10a=");
                            Tx2.AppendLine($"vProd_I11="+(double.Parse(Resultado.order_items[0].full_unit_price) * double.Parse(Resultado.order_items[0].quantity)));
                            Tx2.AppendLine($"cEANTrib_I12="+SeNulo(DadosFiscaisVenda[0].COD_BARRA));
                            Tx2.AppendLine($"uTrib_I13="+SeNulo(DadosFiscaisVendaAdicionais[0].UNI_MEDIDA));
                            Tx2.AppendLine($"qTrib_I14="+Resultado.order_items[0].quantity);
                            Tx2.AppendLine($"vUnTrib_I14a="+Resultado.order_items[0].unit_price);
                            Tx2.AppendLine("indTot_I17b=1");
                            Tx2.AppendLine("orig_N11=0");
                            Tx2.AppendLine("CST_N12=00");
                            Tx2.AppendLine("modBC_N13=0");
                            decimal desconto = decimal.Parse(Endereco.cost_components.special_discount) + decimal.Parse(Endereco.cost_components.loyal_discount) + decimal.Parse(Endereco.cost_components.compensation) + decimal.Parse(Endereco.cost_components.gap_discount);
                            decimal Vbc = (decimal.Parse(Resultado.order_items[0].full_unit_price)+ decimal.Parse(Resultado.payments[0].shipping_cost)
                            + 0 ) - desconto ;
                            Tx2.AppendLine($"vBC_N15={Vbc}");
                            Tx2.AppendLine($"pICMS_N16=7.00");//TODO Criar uma forma de alterar esse valor de acordo com o estado.
                            var vICMS = Vbc*(7/100); //https://blog.tecnospeed.com.br/como-calcular-o-icms-na-nf-e-e-nfc-e/
                            Tx2.AppendLine($"vICMS_N17{vICMS}");
                            Tx2.AppendLine("VICMSDESON_N28A=0.00");
                            Tx2.AppendLine($"CST_Q06={SeNulo(DadosFiscaisVendaAdicionais[0].CST_PIS)}");
                        if (DadosFiscaisVendaAdicionais[0].CST_PIS.Equals("01"))
                        {
                            Tx2.AppendLine("vBC_Q07=0.02");
                            Tx2.AppendLine("pPIS_Q08=1.65");
                            Tx2.AppendLine("vPIS_Q09=0.00");

                        }
                            Tx2.AppendLine("CST_S06=01");
                            Tx2.AppendLine("vBC_S07=0.02");
                            Tx2.AppendLine("pCOFINS_S08=7.60");
                            Tx2.AppendLine("vCOFINS_S11=0.01");
                          
                            Tx2.AppendLine($"nLote_I81={SeNulo(DadosFiscaisVenda[0].ID_IDENTIFICADOR.ToString())}");
                            Tx2.AppendLine($"qLote_I82={SeNulo(DadosFiscaisVenda[0].QTD_ATUAL.ToString())}");
                            Tx2.AppendLine($"dFab_I83={SeNulo(DadosFiscaisVendaAdicionais[0].DT_CADAST.ToString())}");
                            var validade = DateTime.Now.AddYears(100);                     
                            Tx2.AppendLine($"dVal_I84={validade.ToString("yyyy/MM/dd")}");
                            Tx2.AppendLine("SALVARITEM");
                            Tx2.AppendLine($"vBC_W03={Vbc}");
                            Tx2.AppendLine($"vICMS_W04={vICMS}");
                            Tx2.AppendLine("vICMSDeson_W04a=0.00");
                            Tx2.AppendLine("vFCP_W04h=0.00");
                            Tx2.AppendLine("vBCST_W05=");//Base do ICMS ST = (Valor do produto + Valor do IPI + Frete + Seguro + Outras Despesas Acessórias – Descontos) x (1+MVA)
                            Tx2.AppendLine("vST_W06="); //Valor do ICMS ST = (Base do ICMS ST *(Alíquota do ICMS  / 100)) -Valor do ICMS
                            Tx2.AppendLine("vFCPST_W06a=0.00");
                            Tx2.AppendLine("vFCPSTRet_W06b=0.00");
                            Tx2.AppendLine($"vProd_W07{Resultado.payments[0].total_paid_amount}");
                            Tx2.AppendLine($"vFrete_W08={Resultado.payments[0].shipping_cost}");
                            Tx2.AppendLine("vSeg_W09a=0.00");
                            Tx2.AppendLine($"vDesc_W10={desconto}");
                            Tx2.AppendLine("vII_W11=0.00"); // imposto de importação
                            Tx2.AppendLine($"vIPI_W12={SeNulo(DadosFiscaisVenda[0].VLR_IPI.ToString())}");
                            Tx2.AppendLine($"vPIS_W13={SeNulo(DadosFiscaisVendaAdicionais[0].PIS.ToString())}");
                            Tx2.AppendLine($"vCOFINS_W14={SeNulo(DadosFiscaisVendaAdicionais[0].COFINS.ToString())}");
                            Tx2.AppendLine($"vOutro_W15=0.00");
                            Tx2.AppendLine($"vNF_W16={Resultado.payments[0].total_paid_amount}");
                            Tx2.AppendLine($"modFrete_X02=1");

                        // Preencher negócio de fatura 
                        int parcelas = 001;
                        DateTime DataParcela = DateTime.Now;
                       // for (var a in Resultado.payments[0].installments)

                        do
                        {

                                Tx2.AppendLine("INCLUIRCOBRANCA");
                                Tx2.AppendLine($"nDup_Y08={parcelas}");
                                Tx2.AppendLine("dVenc_Y09="+ DataParcela.ToString("yyyy/MM/dd"));
                                Tx2.AppendLine($"vDup_Y10={Resultado.payments[0].installment_amount}");
                                Tx2.AppendLine("SALVARCOBRANCA");
                                 parcelas++;
                        }
                            while (int.Parse(Resultado.payments[0].installments) < parcelas);
                        Tx2.AppendLine("INCLUIRPARTE=YA");
                       // if (int.Parse(Resultado.payments[0].installments) == 1) //TODO verifique as formas de pagamento depois 
                      //  {
                            Tx2.AppendLine("indPag_YA01b=0");
                            Tx2.AppendLine("tPag_YA02=01");
                       // }
                      //  else
                      //  {
                          //  Tx2.AppendLine("indPag_YA01b=1");
                         //   Tx2.AppendLine("tPag_YA02=03");
                      //  }
                        Tx2.AppendLine("vPag_YA03="+Resultado.payments[0].installment_amount);
                        Tx2.AppendLine("tpIntegra_YA04a=1");

                        Tx2.AppendLine("SALVARPARTE=YA");

                        Tx2.AppendLine("SALVAR");
                    }


                    File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt", Tx2.ToString()) ;



                }


            }
            catch (Exception ex) 
            {
            
            
            }
        }

        public void SalvaInfoCliente()
        {
            using (var Insere = new MELIDataSetTableAdapters.TB_CLIENTESTableAdapter()) 
            {
                try
                {
                    var codmuni = "";
                    int existe = (int)Insere.ExisteCliente(CPF_txb.Text);
                    if (existe == 1)
                    {
                        //MessageBox.Show("Usuário já cadastrado", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                        Insere.UpdateCliente(NomeComprador_txb.Text, Endereco_txb.Text, Numero_txb.Text, Complemento_txb.Text, Bairro_txb.Text, codmuni, Municipio_txb.Text, UF_txb.Text, CEP_txb.Text, Telefone_txb.Text, Contribuinte_cxb.SelectedIndex.ToString(), IE_txb.Text, CPF_txb.Text);
                    }
                    else if (existe == 0)
                    {
                        
                        Insere.InsereInfoCliente(CPF_txb.Text, NomeComprador_txb.Text, Endereco_txb.Text, Numero_txb.Text, Complemento_txb.Text, Bairro_txb.Text, codmuni, Municipio_txb.Text, UF_txb.Text, CEP_txb.Text, Telefone_txb.Text, Contribuinte_cxb.SelectedIndex.ToString(), IE_txb.Text);
                    }
                    else
                    {
                        MessageBox.Show("Mais de um usuário cadastrado com mesmo CPF, consulte sua área de suporte.", "Atenção");

                    }
                }
                catch (Exception ex)
                {

                    //throw;
                }
                
                

            
            }
        
        
        }
        private void Salvar_btn_Click(object sender, RoutedEventArgs e)
        {
            SalvaInfoCliente();

        }


        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Produtos item = (Produtos) Produ_datagrid.SelectedItem; 
            CadastroInfoProd tela = new CadastroInfoProd(item);
            tela.ShowDialog();
        }
    }
}
