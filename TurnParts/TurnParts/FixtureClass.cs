using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace MagnusSpace
{
    class FixtureClass
    {
        string fixtureListPath = "";
        string fixtureID = "";
        string model = "";
        string place = "";
        bool fixtureExists = false;
        List<string> fixtureList = new List<string>();
        public void Open(string Fmodel) //rastrear port
        {
            Folders folder = new Folders();
            fixtureListPath = folder.getFixturePath();
            if (!File.Exists(fixtureListPath))
                folder.buildStructure();
            int a = 0;
            int b = 0;
           
            List<string> modelsInLine = new List<string>();
            List<string> modelList = new List<string>();
            List<string> AllModels = new List<string>();
            modelsInLine = readList(fixtureListPath);
            bool modelsAlreadyAdded = false;
            foreach (string line in fixtureList)
            {
                modelsAlreadyAdded = false;
                modelsInLine = line.Split(':')[1].Split(';').ToList();
                foreach(string modelsandValue in modelsInLine)
                {
                    foreach (string model2 in AllModels)
                    {
                        if(model2 == modelsandValue.Split(',')[0])
                        {
                            modelsAlreadyAdded = true;
                        }
                    }
                    if(modelsAlreadyAdded = false)
                    {
                        AllModels.Add(modelsandValue.Split(',')[0]);
                    }
                    if (modelsandValue.Split(',')[0]==Fmodel)
                    {
                        modelList.Add(line.Split(':')[0]+":"+ modelsandValue.Split(',')[1]);
                    }
                }
                
            }
            Console.WriteLine("\r\n=====================");
            foreach (string l in AllModels)
            {
                Console.WriteLine(l);
            }

        }
        
        private bool fixtureListUpdated = false;
        private List<string> readList(string path)
        {
    
            if (path == fixtureListPath)
            {
                if (!fixtureListUpdated)
                {
                    fixtureList = File.ReadAllLines(fixtureListPath).ToList();
                    fixtureListUpdated = true;
                }
                return fixtureList;
            }
           

            return null;


        }
        private void writeList(string path, List<string> _list)
        {
            if (path == fixtureListPath)
            {
                fixtureList = _list;
                fixtureListUpdated = true;
            }
        }
        public void Close()
        {
            if (fixtureListUpdated)
            {
                File.WriteAllLines(fixtureListPath, fixtureList);
                fixtureListUpdated = false;
            }
            

        }
    }
}
