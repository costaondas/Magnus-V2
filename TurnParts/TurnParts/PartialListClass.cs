using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagnusSpace
{
    partial class ListClass
    {
        private string vd()
        {
            return VarDash.ToString();
        }
        public List<string> RowTitle(int index)
        {
            List<string> list = new List<string>();
            switch (index)
            {
                case 0:

                    list.Add("CN" + vd() + "CN");
                    list.Add("category" + vd() + "Categoria");
                    list.Add("Modelo" + vd() + "Modelo");
                    list.Add("Name" + vd() + "Nome");
                    list.Add("P/N" + vd() + "P/N");
                    list.Add("Descrição" + vd() + "Descrição");
                    list.Add("QTD" + vd() + "Quantidade");
                    list.Add("EstoqueM" + vd() + "Estoque Mínimo");
                    list.Add("Status" + vd() + "Status");
                    list.Add("Estoque Inicial" + vd() + "Quantidade Inicial");
                    list.Add("Forcast" + vd() + "Forecast");
                    list.Add("QTDcompra" + vd() + "QTD Compra");
                    list.Add("PlacasProd" + vd() + "Placas Produzidas");
                    list.Add("dataDoCalculo" + vd() + "Data Calcuclo");
                    list.Add("inicioIntervalo" + vd() + "Data Start");
                    list.Add("fimIntervalo" + vd() + "Data end");
                    list.Add("QTDFinalizar" + vd() + "QTD Finalizar");
                    list.Add("CycleLife" + vd() + "Cycle Life");
                    list.Add("grupo" + vd() + "Grupo");
                    list.Add("groupPosition" + vd() + "Posição");
                    list.Add("location3" + vd() + "Rua");
                    list.Add("location2" + vd() + "Coluna");
                    list.Add("location" + vd() + "Linha");
                        



                    break;
                case 1:

                    list.Add("CN" + vd() + "CN");
                    list.Add("Modelo" + vd() + "Modelo");
                    list.Add("Name" + vd() + "Nome");
                    list.Add("P/N" + vd() + "P/N");
                    list.Add("QTD" + vd() + "Quantidade");
                    list.Add("Forcast" + vd() + "Forecast");
                    list.Add("QTDcompra" + vd() + "QTD Compra");
                    list.Add("PlacasProd" + vd() + "Placas Produzidas");
                    list.Add("dataDoCalculo" + vd() + "Data Calcuclo");
                    list.Add("inicioIntervalo" + vd() + "Data Start");
                    list.Add("fimIntervalo" + vd() + "Data end");
                    list.Add("QTDFinalizar" + vd() + "QTD Finalizar");
                    list.Add("CycleLife" + vd() + "Cycle Life");
                        
                    break;
                case 2:

                    list.Add("CN" + vd() + "CN");
                    list.Add("Modelo" + vd() + "Modelo");
                    list.Add("Name" + vd() + "Nome");
                    list.Add("P/N" + vd() + "P/N");
                    list.Add("Descrição" + vd() + "Descrição");
                    list.Add("QTD" + vd() + "Quantidade");
                        
                    break;
                case 3:

                    list.Add("CN" + vd() + "CN");
                    list.Add("Modelo" + vd() + "Modelo");
                    list.Add("Name" + vd() + "Nome");
                    list.Add("P/N" + vd() + "P/N");
                    list.Add("grupo" + vd() + "Grupo");
                    list.Add("groupPosition" + vd() + "Posição");
                    list.Add("location3" + vd() + "Rua");
                    list.Add("location2" + vd() + "Coluna");
                    list.Add("location" + vd() + "Linha");


                    break;
                case 4:

                    list.Add("CN" + vd() + "CN");
                    list.Add("Modelo" + vd() + "Modelo");
                    list.Add("Name" + vd() + "Nome");
                    list.Add("P/N" + vd() + "P/N");

                    break;
                case 5:

                    list.Add("CN" + vd() + "CN");
                    list.Add("Modelo" + vd() + "Modelo");
                    list.Add("Name" + vd() + "Nome");
                    list.Add("grupo" + vd() + "Grupo");
                    list.Add("groupPosition" + vd() + "Posição");

                    break;
                case 6:

                    list.Add("CN" + vd() + "CN");
                    list.Add("Modelo" + vd() + "Modelo");
                    list.Add("Name" + vd() + "Nome");
                    list.Add("P/N" + vd() + "P/N");
                    list.Add("Descrição" + vd() + "Descrição");
                    list.Add("QTD" + vd() + "Quantidade");
                   
                    list.Add("grupo" + vd() + "Grupo");
                    list.Add("groupPosition" + vd() + "Posição");
                    list.Add("location3" + vd() + "Rua");
                    list.Add("location2" + vd() + "Coluna");
                    list.Add("location" + vd() + "Linha");


                    break;
                case 7:

                    list.Add("CN" + vd() + "CN");
                    list.Add("Modelo" + vd() + "Modelo");
                    list.Add("Name" + vd() + "Nome");
                    list.Add("P/N" + vd() + "P/N");
                    list.Add("Descrição" + vd() + "Descrição");
                    list.Add("Responsavel" + vd() + "Responsável");
                    list.Add("DATA_MANUT" + vd() + "Data de Modificação");
                    list.Add("versao" + vd() + "Versão");
                    // list.Add("Responsavel" + vd() + "Responsável");
                    
                    list.Add("grupo" + vd() + "Grupo");
                    list.Add("groupPosition" + vd() + "Posição");
                    list.Add("location3" + vd() + "Rua");
                    list.Add("location2" + vd() + "Coluna");
                    list.Add("location" + vd() + "Linha");
                   
                    break;
                case 8:

                    list.Add("CN" + vd() + "CN");
                    list.Add("Modelo" + vd() + "Modelo");
                    list.Add("Name" + vd() + "Nome");
                    list.Add("P/N" + vd() + "P/N");
                    list.Add("QTD" + vd() + "Quantidade");
                    list.Add("CycleLife" + vd() + "Cycle Life");
                    list.Add("daysWeGot" + vd() + "Dias");
                    list.Add("Status" + vd() + "Status");

                    break;
                case 9:

                    list.Add("CN" + vd() + "CN");
                    list.Add("Modelo" + vd() + "Modelo");
                    list.Add("Name" + vd() + "Nome");
                    

                    break;
                case 10:

                    list.Add("CN" + vd() + "CAIXA");
                    list.Add("QTD" + vd() + "Quantidade");

                    break;
                case 11:

                    list.Add("CN" + vd() + "CN");
                    list.Add("Modelo" + vd() + "Modelo");
                    list.Add("Name" + vd() + "Nome");
                    list.Add("P/N" + vd() + "P/N");
                    list.Add("qtdBin" + vd() + "Bin");
                    list.Add("trocas3Meses" + vd() + "Scrap 3 Meses");
                    list.Add("QTD" + vd() + "Total");
                    list.Add("location3" + vd() + "Rua");
                    list.Add("location2" + vd() + "Coluna");
                    list.Add("location" + vd() + "Linha");
                    break;
                case 12:

                    list.Add("CN" + vd() + "CN");
                    list.Add("Modelo" + vd() + "Modelo");
                    list.Add("Name" + vd() + "Nome");
                    list.Add("P/N" + vd() + "P/N");
                    list.Add("grupo" + vd() + "grupo");
                    list.Add("qtdBin" + vd() + "Bin");
                    list.Add("QTD" + vd() + "Total");
                    break;
                case 13:
                    list.Add("Status" + vd() + "Status");
                    list.Add("CN" + vd() + "Item");
                    list.Add("Data_de_Verificacao" + vd() + "Data da verificação");
                    list.Add("Responsavel" + vd() + "Técnico");
                    list.Add("DaysGone" + vd() + "Dias Corridos");
                    break;
                case 14:
                    list.Add("CN" + vd() + "Item");
                    list.Add("maintDate" + vd() + "DATA");
                    break;
                case 15:
                    list.Add("CN" + vd() + "Item");
                    list.Add("Status" + vd() + "STATUS");
                    break;
                //qtdBin
                case 16:

                    list.Add("CN" + vd() + "CN");
                    list.Add("Name" + vd() + "Nome");
                    list.Add("P/N" + vd() + "P/N");
                    list.Add("QTD" + vd() + "Quantidade");
                    list.Add("location3" + vd() + "Rua");
                    list.Add("location2" + vd() + "Coluna");
                    list.Add("location" + vd() + "Linha");
                    break;
                case 17:

                    list.Add("CN" + vd() + "CN");
                    list.Add("Modelo" + vd() + "Modelo");
                    list.Add("Name" + vd() + "Nome");
                    list.Add("P/N" + vd() + "P/N");
                    list.Add("qtdBin" + vd() + "Bin");
                    list.Add("QTD" + vd() + "Total");
                    list.Add("location3" + vd() + "Rua");
                    list.Add("location2" + vd() + "Coluna");
                    list.Add("location" + vd() + "Linha");
                    break;
            }



            return list;
        }


        public List<string> getItensdeManutenção(bool CNsOnly = false)
        {
            List<string> list = new List<string>();
            //list = item.maintanenceList(); // lista global
            ListClass lc = new ListClass();
            // ListClass lc = new ListClass();
            lc.Open("Itens de Manutenção");
            foreach (string l in lc.mainList)
            {
                if (l.StartsWith("CN"))
                {
                    if (CNsOnly)
                    {
                        list.Add(l.Split(VarDashPlus)[0].Split(VarDash)[1]);
                    }
                    else
                    {
                        list.Add(l);
                    }

                }
            }
            return list;
        }
    }
}
