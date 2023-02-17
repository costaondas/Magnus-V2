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
                    list.Add("DATA_MANUT" + vd() + "Data");
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
            }



            return list;
        }
    }
}
