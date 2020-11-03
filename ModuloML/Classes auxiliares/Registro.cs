using ModuloML.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ModuloML.Classes_auxiliares
{
   public  static class Registro
    {
        static StreamWriter auditwriter = new StreamWriter($@"{AppDomain.CurrentDomain.BaseDirectory}\Logs\audit-{DateTime.Today.ToString("dd-MM-yy")}.txt", true) { AutoFlush = true };
        readonly static object auditObj = new object();


        public static void audit(string tag, string texto)
        {
            System.IO.Directory.CreateDirectory($@"{AppDomain.CurrentDomain.BaseDirectory}\Logs");
            lock (auditObj)
            {
               // if (nivelDeAuditoria <= Settings.Default.Auditoria)
              //  {
                    auditwriter.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}\t{tag} >>\t{texto}");
               // }
            }
        }//Escreve no arquivo da auditoria.
    }
}
