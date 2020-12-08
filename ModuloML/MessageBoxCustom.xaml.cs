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
    /// Lógica interna para MessageBoxCustom.xaml
    /// </summary>
    public partial class MessageBoxCustom : Window
    {
        string Nome = String.Empty;
        public MessageBoxCustom(string Message, MessageType Type, MessageButtons Buttons)
        {
            InitializeComponent();
            Nome = Message;
            
            
            switch (Type)
            {

                case MessageType.Info:
                    txtMessage.Content = Message;
                    txtTitle.Text = "Info";
                    break;
                case MessageType.Confirmation:
                    txtMessage.Content = Message;
                    txtTitle.Text = "Atenção";
                    break;
                case MessageType.Success:
                    {
                        txtMessage.Content = Message;
                        txtTitle.Text = "Success";
                    }
                    break;
                case MessageType.Warning:
                    txtMessage.Content = Message;
                    txtTitle.Text = "Warning";
                    break;
                case MessageType.Error:
                    {
                        txtMessage.Content = Message;
                        txtTitle.Text = "Error";
                    }
                    break;
                case MessageType.Dado:
                    {

                        txtTitle.Text = "Insira seu Nome!";
                        txtMessage.Visibility = Visibility.Collapsed;
                        Nome_txb.Visibility = Visibility.Visible;

                        
                    }
                    break;
            }
            switch (Buttons)
            {
                case MessageButtons.OkCancel:
                    Salvar_btn.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Collapsed; btnNo.Visibility = Visibility.Collapsed;
                    break;
                case MessageButtons.YesNo:
                    Salvar_btn.Visibility = Visibility.Collapsed;
                    btnOk.Visibility = Visibility.Collapsed; btnCancel.Visibility = Visibility.Collapsed;
                    break;
                case MessageButtons.Ok:
                    Salvar_btn.Visibility = Visibility.Collapsed;
                    btnOk.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Collapsed; btnNo.Visibility = Visibility.Collapsed;
                    break;
                case MessageButtons.Salvar:
                    btnOk.Visibility = Visibility.Collapsed;
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Collapsed; btnNo.Visibility = Visibility.Collapsed;
                    Salvar_btn.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {          
            this.DialogResult = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Salvar_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Nome.Equals(""))
                {

                 
                }
                else
                {
                    using (var consulta = new MELIDataSetTableAdapters.TB_USERSTableAdapter())
                    {
                        consulta.InsereNome(Nome_txb.Text, Nome);

                    }
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex) 
            {

                audit("Atenção",ex.Message);
                this.DialogResult = true;
                this.Close();

            }
        }
    }
    public enum MessageType
    {
        Info,
        Confirmation,
        Success,
        Warning,
        Error,
        Dado,
    }
    public enum MessageButtons
    {
        OkCancel,
        YesNo,
        Ok,
        Salvar,
    }
}
