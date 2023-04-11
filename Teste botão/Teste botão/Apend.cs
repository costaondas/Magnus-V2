using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teste_botão
{
    
    internal class Apend
    {
        public string modelo = "";
        public string Name = "";
        public string PN = "";
        public string CN = "";
        public string ID = "";
        public string grupo = "";
        public string ADRESS = "";
        public string QTD = "";
        public string categoria = "";
        public string nLabels = "1";
        public string position = "";
        public string desc1 = "";
        public string desc2 = "";
        public string desc3 = "";

        public int x = 50;
        public int y = 50;
        string intro = "^XA";
        string final = "^XZ";
        public string font = "A0";
        public int fontSize = 100;
        string mainString = "";
        public void Open()
        {
            mainString = "";
            mainString += intro;
        }
        public void Add(string txt)
        {
            mainString += "^FO" + x.ToString() + "," + y.ToString();
            mainString += "^" + font.ToString() + "N50," + fontSize.ToString();
            mainString+= "^FD"+txt+"^FS";
        }
        public string Quantidade_em_pacote()
        {
            int ZeroX = 120;
            int ZeroY = 100;
            mainString = "";
            mainString += intro;

            x = ZeroX;
            y = ZeroY;
            fontSize = 100;
            Add(modelo);
            ////////////////////////////
            ///
            x = 1200;
           // y = 100;
            fontSize = 100;
            Add(DateTime.Now.ToString("dd/MM/yyyy"));
            x = ZeroX;
            ////////////////////////////
            x = ZeroX;
            y += 125;
            fontSize = 100;
            Add(Name);

            ////////////////////////////
            x = ZeroX;
            y += 150;
            fontSize = 100;
            Add("P/N: "+PN);

            ////////////////////////////
            x = ZeroX;
            y += 125;
            fontSize = 100;
            Add("CN: "+CN);

            ////////////////////////////
            x += 400;
            //y == 125;
            fontSize = 100;
            
           // Add("ID: "+ ID);

            ////////////////////////////
            x += 400;
            //y == 125;
            x -= 300;
            fontSize = 100;
            Add(ADRESS);
            x += 300;
            x += 200;
            x -= 200;
            ////////////////////////////
            x -= 250;
            y += 200;
            fontSize = 400;
            Add(QTD);



            x = ZeroX;
            fontSize = 150;
            Add("QTD:");



            ////////////////////////////
            //x += 350;
            //y += 250; tem que adicionar o Y
            // fontSize = 800;
            y += 125;
            font = "B0";
            Add(QTD+"%"+ CN +"%QTD%");
            font = "A0";

            //////////////////


            //////////////////
            ///
            //////////////////
            ///
            x = ZeroX;
            y = ZeroY;
            y += 250;
            fontSize = 300;
            Add("___________________________________");
            //////////////////
            ///
            x = ZeroX;
            y = ZeroY;
            y -= 50;
            fontSize = 300;
            Add("___________________________________");



            mainString += final;
            return mainString;
        }
        public string etiquetaBin()
        {
            int ZeroX = 120;
            int ZeroY = 100;
            mainString = "";
            mainString += intro;



            


            x = ZeroX;
            y = ZeroY;


            

            y -= 20;
            fontSize = 150;
            Add(modelo);
            y += 20;
            ////////////////////////////
            ///
            x = 1200;
            // y = 100;
            fontSize = 100;
            Add(DateTime.Now.ToString("dd/MM/yyyy"));
            x = ZeroX;
            ////////////////////////////
            x = ZeroX;
            y += 150;
            fontSize = 100;
            Add(Name);
            y -= 25;

            ////////////////////////////
            x = ZeroX;
            y += 180;
            fontSize = 100;
            Add("P/N: " + PN);
            y += 25;

            ////////////////////////////
            x = ZeroX;
            y += 125;
            fontSize = 100;
            Add("CN: " + CN);

            ////////////////////////////
            x += 400;
            //y == 125;
            fontSize = 100;
            x += 100;
         //   Add("ID: " + ID);
            //x -= 100;

            ////////////////////////////
           // x += 300;
            y -= 10;
            fontSize = 130;
            Add(ADRESS);
            y += 10;
            x += 300;
            ////////////////////////////
            x += 350;
            y += 250;
            fontSize = 800;
            font = "B0";
            Add("@"+CN);
            font = "A0";

            //////////////////
            ///
            x = ZeroX;
           // y += 50;
            fontSize = 150;
            Add("CN:");
            //////////////////
            ///
            x += 300 ;
            fontSize = 300;
            Add(CN);

            //////////////////
            ///
            x =ZeroX;
            y = ZeroY;
            y += 280;
            fontSize = 300;
            Add("___________________________________");
            //////////////////
            ///
            x = ZeroX;
            y = ZeroY;
            y -= 20;
            fontSize = 300;
            Add("___________________________________");

            //////////////////
            ///
            x = ZeroX;
            y = ZeroY;
            y +=110;
            fontSize = 300;
            Add("___________________________________");

            mainString += final;
            return mainString;
        }
        public string etiquetaFixture()
        {
            int ZeroX = 120;
            int ZeroY = 100;
            mainString = "";
            mainString += intro;

            x = ZeroX;
            y = ZeroY;
            if (position != "IN")
            {
                Console.WriteLine("Sem endereço:" + ADRESS);
                y += 20;
                fontSize = 600;
                Add("SEM");

                y += 500;
                fontSize = 350;
                Add("ENTRADA");
                y += 30;
                x += 1020;
                fontSize = 300;
             

                mainString += final;
                return mainString;
            }
            if (ADRESS == "" || ADRESS == " "|| ADRESS == "  "|| ADRESS == "    " || ADRESS == "     " || ADRESS == "      ")
            {
                Console.WriteLine("Sem endereço:" + ADRESS);
                y += 20;
                fontSize = 600;
                Add("SEM");

                y += 500;
                fontSize = 300;
                Add("ENDERECO");
                y += 30;
                x += 1020;
                fontSize = 300;
                Add(",");

                mainString += final;
                return mainString;
            }
            else
            {
                Console.WriteLine("Endereço ok<" + ADRESS+ ">");
            }
            
            y -= 20;
            fontSize = 150;
            Add(Name);
            y += 20;
            ////////////////////////////
            ///
            x = 1200;
            // y = 100;
            fontSize = 100;
            Add(DateTime.Now.ToString("dd/MM/yyyy"));
            x = ZeroX;
            ////////////////////////////
            x = ZeroX;
            y += 150;
            fontSize = 100;
            Add("CN: " + CN);
            y -= 25;

            ////////////////////////////
            x = ZeroX;
            y += 180;
            fontSize = 100;
            Add("ESD: " + PN);

            ////////////////////////////
            ////////////////////////////
            x = ZeroX;
            y = ZeroY;
            x += 450;
            y += 500;
            fontSize = 130;
            Add(ADRESS);
            y += 10;

            ////////////////////////////
            x = ZeroX;
            y = ZeroY;
            x += 100;
            y += 430;
            fontSize = 800;
            font = "B0";
            Add(CN.Replace(';','/'));
            font = "A0";

            //////////////////
            ///

            x = ZeroX;
            y = ZeroY + 700;
            fontSize = 200;
            Add(modelo);

            //////////////////
            ///
            x = ZeroX;
            y = ZeroY;
            y += 350;
            fontSize = 300;
            Add("___________________________________");
            //////////////////
            ///
            x = ZeroX;
            y = ZeroY;
            y -= 20;
            fontSize = 300;
            Add("___________________________________");

            //////////////////
            ///
            x = ZeroX;
            y = ZeroY;
            y += 110;
            fontSize = 300;
            Add("___________________________________");

            mainString += final;
            return mainString;
        }
        public string grupoEtiqueta()
        {
            int ZeroX = 120;
            int ZeroY = 100;
            mainString = "";
            mainString += intro;

            x = ZeroX;
            y = ZeroY;

            y -= 20;
            fontSize = 110;
            Add(modelo);
            y += 20;
            ////////////////////////////
            ///
            x = 1330;
            // y = 100;
            fontSize = 80;
            Add(DateTime.Now.ToString("dd/MM/yyyy"));
            x = ZeroX;
            ////////////////////////////
            x = ZeroX;
            y += 80;
            fontSize = 80;
            Add(Name);
            y -= 25;

            ////////////////////////////
            ///
            ////////////////////////////
            x = ZeroX;
            y = ZeroY + 180;
            fontSize = 80;
            Add("CN: " + CN);
            y -= 25;

            ////////////////////////////
            x = ZeroX;
            y += 150;
            fontSize = 100;
            Add("PN: " + PN);

            ////////////////////////////
            ////////////////////////////
            x = ZeroX;
            y = ZeroY;
            //x += 450;
            y += 450;
            fontSize = 150;
            Add(ADRESS);
            y += 10;

            ////////////////////////////
            //x = ZeroX;
            //y = ZeroY;
            //x += 100;
            y += 160;
            fontSize = 100;
            font = "A0";

            //x = ZeroX;
            y = ZeroY;
            y += 640;
            Add(desc1);
            y = ZeroY + 750;
            Add(desc2);
            y = ZeroY + 860;
            Add(desc3);
           // y += 50;

            //////////////////
            ///

            x = ZeroX;
            y = ZeroY + 700;
            fontSize = 100;
            //Add(modelo); //descrição

            //////////////////
            ///
            x = ZeroX;
            y = ZeroY;
            y += 300;
            fontSize = 300;
            Add("___________________________________");
            //////////////////
            ///
            x = ZeroX;
            y = ZeroY;
            y -= 20;
            fontSize = 300;
            Add("___________________________________");

            //////////////////
            ///
            x = ZeroX;
            y = ZeroY;
            y += 110;
            fontSize = 300;
            Add("___________________________________");

            mainString += final;
            return mainString;
        }
        public string Close()
        {
            mainString += final;
            return mainString;
        }
    }
}
