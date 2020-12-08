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
using static ModuloML.Servicos.Static;

namespace ModuloML
{
    /// <summary>
    /// Lógica interna para TelaLogin.xaml
    /// </summary>
    public partial class TelaLogin : Window
    {
        public TelaLogin()
        {
            InitializeComponent();
        }
        

        private void SalvarRegistro_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Senha_txb.Text.Equals(ConfirmaSenha_txb.Text) || Senha_txb.Text.Equals("") || ConfirmaSenha_txb.Text.Equals(""))
                {
                    using (var Consulta = new MELIDataSetTableAdapters.TB_USERSTableAdapter())
                    {
                        string valor = Login_txb.Text;

                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        var enc1252 = Encoding.GetEncoding(1252);
                        var Existe = Consulta.ExisteUser(valor);
                        if (Existe >= 1)
                        {
                            bool? Result = new MessageBoxCustom("Login já existe!", MessageType.Confirmation, MessageButtons.Ok).ShowDialog();

                        }
                        else
                        {
                            Consulta.InsereUsuario(1, "", Login_txb.Text, Senha_txb.Text, 0);
                            bool? Result = new MessageBoxCustom("Login cadastrado com sucesso!", MessageType.Confirmation, MessageButtons.Ok).ShowDialog();

                        }

                    }
                }
                else
                {
                    bool? Result = new MessageBoxCustom("As senhas não coincidem!", MessageType.Confirmation, MessageButtons.Ok).ShowDialog();


                }
            }
            catch (Exception ex)
            {
                audit("Atençao" ,ex.Message);
            }
        }

       

        private void EsqueciSenha_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Fechar_btn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Entrar_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var consulta = new MELIDataSetTableAdapters.TB_USERSTableAdapter())
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    var enc1252 = Encoding.GetEncoding(1252);
                    var user = consulta.SelecionaUser(User_txb.Text);
                    if (user[0].NOME.Equals(""))
                    {
                        bool? Result = new MessageBoxCustom(user[0].LOGIN, MessageType.Dado, MessageButtons.Salvar).ShowDialog();


                    }
                    else
                    {
                        ModuloML.Servicos.USER.ACESSO = user[0].ACESSO;
                        ModuloML.Servicos.USER.LOGIN = user[0].LOGIN;
                        ModuloML.Servicos.USER.NOME = user[0].NOME;
                        ModuloML.Servicos.USER.ID = user[0].ID;
                        


                    }
                    if (user[0].SENHA.Equals(UserSenha_txb.Password))
                    {

                        MainWindow tela = new MainWindow();
                        tela.ShowDialog();
                        Hide();

                    }
                    else 
                    {
                        bool? Result = new MessageBoxCustom("A senha está incorreta!", MessageType.Error, MessageButtons.Ok).ShowDialog();


                    }

                }
            }
            catch (Exception ex) 
            { 
            
            }
        }
    }
}
