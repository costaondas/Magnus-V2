using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace MagnusSpace
{
    class ListClass
    {
        ////////////////////////
        string ListPath = "";
        public List<string> mainList = new List<string>();
        private bool ListUpdated = false;
        char VarDash = ((char)887);
        char VarDashPlus = ((char)888);

        ////////////////////////

        public List<string> Open(string listName, string listFolder = "listFolder") //rastrear port
        {
            Folders folder = new Folders();
            string folderName = listFolder;
          
            switch (listFolder)
            {
                case "listFolder":
                    folderName = folder.genericPath;
                    break;
                case "settings":
                    folderName = folder.settingsPath;
                    break;
                case "Tasks":
                    folderName = folder.TasksFolderPath;
                    break;
                case "Grupo":
                    folderName = folder.GrupoFolderPath;
                    break;
                case "ListaGeral":
                    folderName = folder.ListaGeralFolderPath;
                    break;



            }
            
            ListPath = folderName + @"\" + listName + ".txt";
            if (!File.Exists(ListPath))
            {
                folder.buildStructure();
                try {
                    using (File.CreateText(ListPath))
                    {

                    }
                         }
                catch { }
            }
            mainList = readList();
            return mainList;
        }
        public void s(string a)
        {
  
        }
        public bool newCN = false;
        public string streamPlus(string CN,string varName, string value = "null")
        {
            string varValue = "";
            bool foundCN = false;
            bool foundVAR = false;
            string buildString = "";
            string combinedString = "";
            int a = 0;
            s("stream plus");
            if (value == "null")
            {
                s("null value");
                
                foreach (string l in mainList)
                {
                    List<string> subList = new List<string>();
                    subList = l.Split(VarDashPlus).ToList();
                    foreach(string l2 in subList)
                    {
                        try
                        {
                            if (l2.Split(VarDash)[0] == varName)
                            {
                                varValue =  l2.Split(VarDash)[1];
                            }
                            if (l2.Split(VarDash)[0] == "CN" && l2.Split(VarDash)[1] == CN)
                            {
                                foundCN = true;
                            }
                        }
                        catch { }
                    }
                    if (foundCN)
                        return varValue;



                }
                return "";
            }
            else
            {

                a = 0;
                foreach (string l in mainList.ToList())
                {
                    List<string> subList = new List<string>();
                    subList = l.Split(VarDashPlus).ToList();
                    int b = 0;
                    foreach (string l2 in subList.ToList())
                    {
                        s(l2);
                        try
                        {
                            if (l2.Split(VarDash)[0] == varName)
                            {
                                buildString = varName + VarDash.ToString() + value.ToString();
                                subList[b] = buildString;
                                foundVAR = true;


                            }
                            if (l2.Split(VarDash)[0] == "CN" && l2.Split(VarDash)[1] == CN)
                            {
                                foundCN = true;
                            }

                        }
                        catch { }
                        b++;
                    }
                    if (foundVAR && foundCN)
                    {
                        combinedString = string.Join(VarDashPlus.ToString(), subList.ToArray());
                        mainList[a] = combinedString;
                        break;
                    }
                    else
                    {
                        s("no var found");
                        if (foundCN)
                        {
                            subList.Add(varName + VarDash.ToString() + value.ToString());
                            combinedString = string.Join(VarDashPlus.ToString(), subList.ToArray());
                            mainList[a] = combinedString;
                            break;
                        }
                        
                    }


                    a++;
                }
                return "";
                // mainList.Add(varName + VarDash.ToString() + value);
                // return value;
            }
        }

        public string stream(string varName, string value = "null")
        {
            int a = 0;
            if (value == "null")
            {
                foreach (string l in mainList)
                {
                    try
                    {
                        if (l.Split(VarDash)[0] == varName)
                        {
                            return l.Split(VarDash)[1];
                        }
                    }
                    catch { }

                }
                return "";
            }
            else
            {
                foreach (string l in mainList)
                {
                    if (l.Split(VarDash)[0] == varName)
                    {
                        mainList[a] = varName + VarDash.ToString() + value;
                        return value;
                    }
                    a++;
                }
                mainList.Add(varName + VarDash.ToString() + value);
                return value;
            }

        }
        public string streamSEARCH(List<string> list, string varName) // PESQUISA EM UMA LISTA EXTERNA  
        {
            foreach (string l in list)
            {
                try
                {
                    if (l.Split(VarDash)[0] == varName)
                    {
                        return l.Split(VarDash)[1];
                    }
                }
                catch { }

            }
            return "";

        }
        /// <summary>
        /// RETORNA UMA LISTA ALTERADA
        /// </summary>
        /// <param name="list"></param>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        /// <returns></returns> Console.WriteLine
        public List<string> stream_SET(List<string> list,string varName, string value) 
        {
            
            List<string> listTo_return = new List<string>();
            listTo_return = list;
            int a = 0;
            foreach (string l in listTo_return.ToList())
            {
                if (l.Split(VarDash)[0] == varName)
                {
                    listTo_return[a] = varName + VarDash.ToString() + value;
                    return listTo_return;
                }
                a++;
            }
            listTo_return.Add(varName + VarDash.ToString() + value);
            return listTo_return;

        }
        bool canYOUREAD = true;
        private List<string> readList()
        {
            try
            {
                if (!ListUpdated)
                {
                    mainList = File.ReadAllLines(ListPath).ToList();
                    
                    ListUpdated = true;
                }
            }
            catch { canYOUREAD = false; }
            
            return mainList;
        }
        private void writeList()
        {
            ListUpdated = true;
        }
        public void Close()
        {
            if(!canYOUREAD)
            {
                return;
            }
                writeList();
            if (ListUpdated)
            {
                File.WriteAllLines(ListPath, mainList);
                ListUpdated = false;
            }


        }

    }
}
