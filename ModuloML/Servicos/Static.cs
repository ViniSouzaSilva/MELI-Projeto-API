using ModuloML.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
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

        public static string SeNulo(string texto)
        {
            if (String.IsNullOrEmpty(texto)|| DBNull.Value.Equals(texto))
            {
                texto = "";
                return texto;
            }
            else 
            {
                return texto;
            }

        }
        public static string BytesToString(byte[] bytes)
        {
            string resultado = "";
            foreach (byte b in bytes)
            {
                resultado += b.ToString("x2");
            }
            return resultado;

        }
        public static byte[] StringToBytes(string valor)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(valor);
        }

    }





        public static class USER
        {
            public static int ID { get; set; }
            public static string NOME { get; set; }
            public static string LOGIN { get; set; }
            public static int ACESSO { get; set; }


        }

    
}
