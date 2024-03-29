﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloML.Objetos
{
    public class DadosVendas
    {
        public string ID_VENDA { get; set; }
        public string NOMECOMPRADOR { get; set; }
        public string CPF_COMPRADOR { get; set; }
        public DateTime DATAVENDA { get; set; }
        public string ATRIBUICAO { get; set; }
        public string STATUS_ATRIBUICAO { get; set; }
        public string DESCRICAOPROD { get; set; }
        public string CODBARRAS { get; set; }
        public int QUANTIDADE { get; set; }
        public double PRECO { get; set; }
        public string ID_ANUNCIO { get; set; }
        public string ID_SHIPMENT { get; set; }

    }
}
