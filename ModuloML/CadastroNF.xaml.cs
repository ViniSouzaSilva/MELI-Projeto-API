﻿using ModuloML.Objetos;
using ModuloML.Properties;
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
using static ModuloML.Enum.Enums;

namespace ModuloML.Telas
{
    /// <summary>
    /// Lógica interna para CadastroNF.xaml
    /// </summary>
    public partial class CadastroNF : Window
    {
        public CadastroNF(RetornoVendaML.Result retorno)
        {
            InitializeComponent();
            VisualizaDados(retorno);

        }



        public void VisualizaDados(RetornoVendaML.Result Resultado)
        {
            using (var consulta = new MELIDataSetTableAdapters.TB_CLIENTESTableAdapter())
            {
                try {
                    int existe = (int)consulta.ExisteCliente(Resultado.buyer.billing_info.doc_number);
                    if (existe == 0)
                    {
                        MessageBox.Show("Usuário não cadastrado, preencha os dados", "Atenção",MessageBoxButton.OK,MessageBoxImage.Information);

                    }
                    else if (existe == 1)
                    {
                        var result = consulta.RetornaInfoCliente(Resultado.buyer.billing_info.doc_number);

                        NaturezaOP_txb.Text = "Venda de Mercadoria";
                        NomeComprador_txb.Text = result[0].NOME_RAZAO;
                        if (result[0].CPF_CNPJ.ToString().Length == 11)
                        {
                            TipoPessoa_cxb.SelectedItem = "Física";
                            TipoPessoa_cxb.IsReadOnly = true;

                        }
                        else
                        if (result[0].CPF_CNPJ.ToString().Length == 14)
                        {
                            TipoPessoa_cxb.SelectedItem = "Jurídica";
                            TipoPessoa_cxb.IsReadOnly = true;

                        }
                        else
                        {
                            TipoPessoa_cxb.SelectedItem = "Estrangeiro";
                            TipoPessoa_cxb.IsReadOnly = true;
                        }

                        CPF_txb.Text = result[0].CPF_CNPJ.ToString();
                        CPF_txb.IsReadOnly = true;

                        switch (result[0].INDICADOR_IE)
                        {
                            case "1":
                                Contribuinte_cxb.SelectedIndex = 1;
                                Contribuinte_cxb.IsReadOnly = true;
                                break;
                            case "2":
                                Contribuinte_cxb.SelectedIndex = 2;
                                Contribuinte_cxb.IsReadOnly = true;
                                break;
                            case "9":
                                Contribuinte_cxb.SelectedIndex = 9;
                                Contribuinte_cxb.IsReadOnly = true;
                                break;
                            default:
                                Contribuinte_cxb.SelectedIndex = 2;
                                Contribuinte_cxb.IsReadOnly = true;
                                break;
                        }

                        CEP_txb.Text = result[0].CEP;
                        CEP_txb.IsReadOnly = true;

                        Endereco_txb.Text = result[0].LOGRADOURO;

                        Numero_txb.Text = result[0].NUMERO;
                        Complemento_txb.Text = result[0].COMPLEMENTO;
                        Bairro_txb.Text = result[0].BAIRRO;
                        Municipio_txb.Text = result[0].NOME_MUNICIPIO;
                        UF_txb.Text = result[0].UF;
                        Telefone_txb.Text = result[0].TELEFONE;
                        List<RetornoVendaML.OrderItem> orderItems = new List<RetornoVendaML.OrderItem>();
                        foreach (var a in Resultado.order_items)
                        {
                            orderItems.Add(new RetornoVendaML.OrderItem() { item = Resultado.order_items[0].item });

                        }

                        Produ_datagrid.ItemsSource = orderItems;



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

                    }
                    else 
                    {
                        MessageBox.Show("Mais de um usuário cadastrado com mesmo CPF", "Atenção");

                    }

                }
                catch (Exception ex) 
                {
                
                }
             }
        }


        public void Conversao(RetornoVendaML.RetornaVendaML Resultado, DadosDoEmitente.Root Emitente, DadosAdicionaisProd.Root dadosAdicionaisProd)
        {
            File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt", String.Empty);
            string valorSalvo = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt");
            if (valorSalvo.Length < 1)
            {

                string idDest = "";
                StringBuilder Tx2 = new StringBuilder();
                Tx2.AppendLine("INCLUIR");
                Tx2.AppendLine("versao_A02=4.00");
                Tx2.AppendLine("Id_A03=0");
                switch (Emitente.address.state)
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
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CodIBGEMunicipio));
                StreamReader reader = new StreamReader($@"{AppDomain.CurrentDomain.BaseDirectory}munseed_temp.xml");
                cod = (CodIBGEMunicipio.municipios)serializer.Deserialize(reader);
                //File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt", String.Empty);


                var codMunEmit = ((cod.municipio.Select(x => x).Where(x => x.Mun_Desc == Emitente.address.state).FirstOrDefault()) ?? (new CodIBGEMunicipio.municipiosMunicipio())).ID_Municipio;

                Tx2.AppendLine($"cMunFG_B12={codMunEmit}");
                Tx2.AppendLine("tpImp_B21=1");
                Tx2.AppendLine("tpEmis_B22=1");
                Tx2.AppendLine("cDV_B23=0");
                Tx2.AppendLine("tpAmb_B24=2");
                Tx2.AppendLine("finNFe_B25=1");
                Tx2.AppendLine("indFinal_B25a=1");
                Tx2.AppendLine("indPres_B25b=2");
                Tx2.AppendLine("procEmi_B26=0");
                Tx2.AppendLine("AmbiStore_ModuloML 1.0");
                Tx2.AppendLine("CRT_C21=3");
                Tx2.AppendLine($"CNPJ_C02={Settings.Default.CNPJ}");
                Tx2.AppendLine($"xNome_C03={Settings.Default.RAZAO_SOCIAL}");
                Tx2.AppendLine($"xFant_C04={Settings.Default.NOME_FANTASIA}");
                Tx2.AppendLine($"xLgr_C06={Settings.Default.LOGRADOURO}");
                Tx2.AppendLine($"nro_C07={Settings.Default.NUMERO}");
                Tx2.AppendLine($"xBairro_C09={Settings.Default.BAIRRO}");
                Tx2.AppendLine($"cMun_C10={Settings.Default.CODMUNICIPIO}");
                Tx2.AppendLine($"xMun_C11={Settings.Default.NOMEMUN}");
                Tx2.AppendLine($"UF_C12={Settings.Default.UF}");
                Tx2.AppendLine($"CEP_C13={Settings.Default.CEP}");
                Tx2.AppendLine("cPais_C14=1058");
                Tx2.AppendLine("xPais_C15=BRASIL");
                Tx2.AppendLine("fone_C16=");
                Tx2.AppendLine($"IE_C17={Settings.Default.IE}");


                if (Resultado.results[0].buyer.billing_info.doc_type.Equals("CPF"))
                {

                    Tx2.AppendLine($"CPF_E02={Resultado.results[0].buyer.billing_info.doc_number}");

                }
                else if (Resultado.results[0].buyer.billing_info.doc_type.Equals("CPF"))
                {
                    Tx2.AppendLine($"CNPJ_E03={Resultado.results[0].buyer.billing_info.doc_number}");

                }
                else
                {
                    Tx2.AppendLine($"idEstrangeiro_E03a={Resultado.results[0].buyer.billing_info.doc_number}");

                }
                Tx2.AppendLine($"xNome_E04={Resultado.results[0].buyer.nickname}");
                //Tx2.AppendLine($"xLgr_E06={Resultado.}");
                Tx2.AppendLine($"fone_E16={Resultado.results[0].buyer.phone.area_code}+{Resultado.results[0].buyer.phone.number}");
                Tx2.AppendLine("indEDest_E16a=9");//Verificar essa questão 

                Tx2.AppendLine($"email_E19={Resultado.results[0].buyer.email}");
                //Tx2.AppendLine($"IE_F15=[{}");,
                int indiceProd = 1;
                using (var produ = new MELIDataSetTableAdapters.TB_PRODUTOSTableAdapter())
                {
                    
                    for (int item = 0; item < Resultado.results[0].order_items[0].quantity; item++)
                    {

                        Tx2.AppendLine("INCLUIRITEM");
                        Tx2.AppendLine($"nItem_H02={indiceProd}");
                        var Produtos = produ.RetornaProduto(Resultado.results[item].order_items[item].item.id);
                        Tx2.AppendLine($"cProd_I02={Produtos[0].CPROD}");

                        var dadosProd = ((dadosAdicionaisProd.attributes.Select(x => x).Where(x => x.id.Equals("GTIN")).FirstOrDefault()) ?? (new DadosAdicionaisProd.Attribute())).value_name;
                        Tx2.AppendLine($"cEAN_I03={dadosProd}");
                        Tx2.AppendLine($"xProd_I04={Resultado.results[item].order_items[item].item.title}");
                        Tx2.AppendLine($"");
                    }

                }


                



            }



        }

        public void SalvaInfoCliente()
        {
            
        
        
        }
        private void Salvar_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
