using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloML.Objetos
{
    class RetornoErro
    {
        public class Erro
        {
            public string message { get; set; }
            public string error { get; set; }
            public int status { get; set; }
            public List<object> cause { get; set; }
        }
    }
}
