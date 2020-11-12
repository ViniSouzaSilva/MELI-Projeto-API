using ModuloML.Objetos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static ModuloML.Enum.Enums;

namespace ModuloML.Servicos
{
   public static class ConversaoML
    {

        public static void Conversao(RetornoVendaML.RetornaVendaML Resultado,DadosDoEmitente.Root Emitente)       
        {
            File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt", String.Empty);
            string valorSalvo = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}text.txt");
            if (valorSalvo.Length < 1)
            {

                
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
                







            }


            
        }

        public static StringBuilder AppendLineIfNotNull(this StringBuilder sbb, string chave, string valor) 
        {
            if (valor is null) return sbb;
            else return sbb.AppendLine($"{chave}={valor}");
        }


    }
}
