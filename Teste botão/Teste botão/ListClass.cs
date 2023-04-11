using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teste_botão
{
    internal class ListClass
    {
        string Folder_adress = "";
        string fileName = "";
        string file_Adress = "";
        public List<string> mainList = new List<string>();
        public void Open(string name, string adress = "")
        {
            if(adress != "")
            {
                Folder_adress = @"" + adress;// + "/" + name + ".txt";
                file_Adress = Folder_adress + "/" + name + ".txt";
                readList();
            }
            else
            {
                //Folder_adress = @"" + adress;// + "/" + name + ".txt";
                file_Adress = name;
                readList();
            }
            
        }
        private void readList()
        {
            List<string> list = new List<string>();
            
            
            if (Directory.Exists(Folder_adress) && Folder_adress != "")
            {
                Console.WriteLine("Directory exists");
                if (!File.Exists(file_Adress))
                {
                    Console.WriteLine("File doesnt exist");
                    list.Add("");
                    //string[] lines = { "old falcon", "deep forest", "golden ring" };
                    File.WriteAllLines(file_Adress, list);
                    mainList.Clear();
                }
                else
                {
                    Console.WriteLine("There is a file!");
                    list = File.ReadAllLines(file_Adress).ToList();
                    mainList = list;
                }
                
               
            }
            else
            {
                if(Folder_adress == "")
                {
                    if (File.Exists(file_Adress))
                    {
                        list = File.ReadAllLines(file_Adress).ToList();
                        mainList = list;
                        Console.WriteLine("Found adress " + file_Adress);
                    }
                }
                Console.WriteLine("no directory found");
            }
            
            
        }

        private void writeList()
        {
            try
            {
   
                File.WriteAllLines(file_Adress, mainList);
                Console.WriteLine("writelist");
            }   
            catch(Exception ex) { Console.WriteLine(ex.Message); }
        }
        public void Close()
        {
            writeList();
        }

    }
}
