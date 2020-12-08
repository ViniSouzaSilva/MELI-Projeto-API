using ModuloML.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ModuloML.Servicos
{
    public static class Static
    {
         static StreamWriter auditwriter = new StreamWriter($@"{AppDomain.CurrentDomain.BaseDirectory}\Logs\audit-{DateTime.Today.ToString("dd-MM-yy")}.txt", true) { AutoFlush = true };
        readonly static object auditObj = new object();


        public static void audit(string tag, string texto, int nivelDeAuditoria = 1)
        {
            System.IO.Directory.CreateDirectory($@"{AppDomain.CurrentDomain.BaseDirectory}\Logs");
            lock (auditObj)
            {
                if (nivelDeAuditoria <= Settings.Default.Auditoria)
                {
                    auditwriter.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}\t{tag} >>\t{texto}");
                }
            }
        }//Escreve no arquivo da auditoria.
    }
    public static class USER 
    {
        public static int ID { get; set; }
        public static string NOME{ get; set; }
        public static string LOGIN { get; set; }
        public static int ACESSO { get; set; }


    }
    
}
