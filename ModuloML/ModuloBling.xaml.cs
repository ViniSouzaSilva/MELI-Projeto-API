using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace ModuloML
{
    /// <summary>
    /// Lógica interna para ModuloBling.xaml
    /// </summary>
    public partial class ModuloBling : Window
    {
        public ModuloBling()
        {
            InitializeComponent();
            ExecuteUpdateOrder();
        }

        public static void ExecuteUpdateOrder()
        {
            try
            {
                XmlReader xmlReader = XmlReader.Create("C:\\Users\\Usuario\\Pictures\\new 21.xml");
                var XML = xmlReader.Read();
                var client = new RestClient("https://bling.com.br/Api/v2/pedido/34798/json/?apikey=6ad56d1a4c84c091d358193982cf7a47ad1fa6afad11975a5c76fcecf84affcc6cc13884&xml="+XML);
                client.Timeout = -1;
                var request = new RestRequest(Method.PUT);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch (Exception ex) 
            { }
        }

    }
}
