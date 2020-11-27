using ModuloML.Objetos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ModuloML
{
    /// <summary>
    /// Lógica interna para CadastroInfoProd.xaml
    /// </summary>
    public partial class CadastroInfoProd : Window
    {
        public CadastroInfoProd(Produtos item)
        {
            InitializeComponent();
            CarregaInfoProd(item);
        }
        public void CarregaInfoProd(Produtos orderItem) 
        {
            try
            {
                Xprod_txb.Text = orderItem.Descricao;
                CEAN_txb.Text = orderItem.CodProdu;
                Cprod_txb.Text = orderItem.CodProdu;
                VUNCOM_txb.Text = orderItem.VlrUni;
                Qcom_txb.Text = orderItem.Quantidade;


            }
            catch (Exception ex)
            {

                throw;
            }
               
        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Salva_btn_Click(object sender, RoutedEventArgs e)
        {
            using (var consulta = new MELIDataSetTableAdapters.TB_PRODUTOSTableAdapter()) 
            {
                try
                {
                    consulta.InsereProdu(1,Cprod_txb.Text,CEAN_txb.Text,Xprod_txb.Text,NCM_txb.Text,"","",int.Parse(CFOP_txb.Text),Ucom_txb.Text,decimal.Parse(Qcom_txb.Text),decimal.Parse(VUNCOM_txb.Text),decimal.Parse(Vprod_txb.Text),"",0,0,"",Origem_cxb.SelectedIndex,CST_cxb.SelectedIndex.ToString(),0,0,0,0,0,0,0,0,0,0,DateTime.Now,DateTime.Now);

                }
                catch (Exception ex)
                {

                    throw;
                }
            
            }

        }
    }
}
