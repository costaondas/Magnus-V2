using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Drawing2D;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MagnusSpace
{
    partial class ListClass
    {
        #region Vars
        ////////////////////////
        string ListPath = "";
        public List<string> mainList = new List<string>();
        private bool ListUpdated = false;
        public char VarDash = ((char)887);
        public char VarDashPlus = ((char)888);
        TurnParts.Form1 form;// new TurnParts.Form1();
        public List<string> barName;
        public int barIndex = -1;

        ////////////////////////
        #endregion

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
            if(listName == "Mestra")
            {
                folderName = folder.ListaGeralFolderPath;
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
        
        public bool newCN = false;
        public List<string[]> listtoArray(List<string> inList, List<string> title)
        {
            List<string[]> list = new List<string[]>();
            string[] titleArray;
            int b = 0;
            titleArray = title.ToArray();
            foreach (string l3 in title.ToList())
            {
                titleArray[b] = l3.Split(VarDash)[1];
                b++;
            }
            //list.Add(titleArray);]
            List<string> listtoMod = new List<string>();
            listtoMod = filterList(inList, title);
            string[] array;
            foreach (string l in listtoMod.ToList())
            {
                array = l.Split(VarDashPlus).ToArray();
                int a = 0;
                foreach (string l2 in array.ToList())
                {
                    array[a] = l2.Split(VarDash)[1];
                    a++;
                }
                list.Add(array);
            }
            return list;
        } 
        public void hasBar(List<string> bar)
        {
            barName = bar;
            barIndex= 0;
            form = System.Windows.Forms.Application.OpenForms["Form1"] as TurnParts.Form1;
        }
        private void setBar(int max)
        {
            int total = barName.Count();
            if (total != 0)
            {
                form.setProgressiveBar(max);
                barIndex++;
            }
            MessageBox.Show("");
        }
        private void checkBar()
        {
            int total = barName.Count();
            if (total != 0)
            {

                if(barName.Count() > barIndex)
                {
                    form.AddProgressiveBar(barName[barIndex]);
                }
                
            }
            System.Windows.Forms.Application.DoEvents();
        }
        
        public List<string> toVardashFormat(List<string> list, List<string> title,bool includeTitle = false)
        {
            List<string> retList = new List<string>();
            List<string[]> list2 = listtoArray(list, title);
            setBar(list2.Count);
            foreach(string[] l in list2)
            {
                string line = string.Join(VarDash.ToString(),l);
                retList.Add(line);
                Console.WriteLine(line);
                line = "";
                checkBar();
                
            }
            if (includeTitle)
            {
                string[] titleArray;
                int b = 0;
                titleArray = title.ToArray();
                foreach (string l3 in title.ToList())
                {
                    titleArray[b] = l3.Split(VarDash)[1];
                    b++;
                }
                string line = string.Join(VarDash.ToString(), titleArray);
                retList.Insert(0, line);
            }
            return retList;
        }
       
        public string build(List<string> list)
        {
            return string.Join(VarDashPlus.ToString(), list.ToArray());
        }

        #region stabel
        public List<string> search(List<string> searxhList, string searchPhrase)
        {

            //label11.Text = "Search: " + text;
            int a = 0;
            bool containsAllStrings = true;

            //textBox1.Text = "";

            List<string> resolts = new List<string>();
            foreach (string l in searxhList)
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                try
                {

                    if (searchPhrase.Contains(' '))
                    {
                        containsAllStrings = true; ;
                        foreach (string l2 in searchPhrase.Split(' ').ToList())
                        {
                            if (!l.Contains(l2, comp))
                            {
                                containsAllStrings = false;
                            }
                        }
                        if (containsAllStrings)
                        {
                            resolts.Add(l);
                        }
                    }
                    else
                    {
                        if (l.Contains(searchPhrase, comp))
                        {
                            resolts.Add(l);

                        }
                    }
                }
                catch
                {

                }


            }
            return resolts;
        }
        public DataTable toDataTable(List<string> inList, List<string> title)//title valeus are gonna be the name
        {

            List<string[]> list = new List<string[]>();
            list = listtoArray(inList, title);
            DataTable dt = new DataTable();
            string[] titleArray;
            int b = 0;
            titleArray = title.ToArray();
            foreach (string l3 in title.ToList())
            {
                titleArray[b] = l3.Split(VarDash)[1];
                b++;
            }
            dt = ConvertListToDataTable(list, titleArray);


            return dt;
        }
        public void Show(List<string> head, List<string> list = null)
        {
            Form12 form = new Form12();
            if (list == null)
            {
                form.displayList = mainList;
            }
            else
            {
                form.displayList = list;
            }
            form.headList = head;
            form.Show();

        }

        public List<string> filterListVar(string VarName, string VarValue, List<string> inList = null)
        {
            List<string> list = new List<string>();
            List<string> retlist = new List<string>();
            if (inList != null)
            {
                list = inList;
            }
            else
            {
                list = mainList;
            }
            setBar(list.Count());
            foreach (string l in list)
            {
                new List<string>();
                List<string> subList = l.Split(VarDashPlus).ToList();
                foreach (string l2 in subList.ToList())
                {
                    if (l2 == VarName + VarDash.ToString() + VarValue)
                    {
                        retlist.Add(l);
                        break;
                    }
                }
                checkBar();
            }
            return retlist;
        }
        public List<string> filterList(List<string> inList, List<string> title)
        {
            List<string> list = new List<string>();
            List<string> lineList = new List<string>();

            string dashSlot = "";
            foreach (string l in inList.ToList())
            {
                foreach (string l2 in title.ToList())
                {
                    string varName = l2.Split(VarDash)[0];
                    List<string> itemList = l.Split(VarDashPlus).ToList();
                    dashSlot = varName + VarDash.ToString() + streamSEARCH(itemList, varName);
                    lineList.Add(dashSlot);
                    dashSlot = "";

                }
                string newItemline = build(lineList);
                lineList.Clear();
                list.Add(newItemline);
                //lineList is a list with all vars of a item

            }


            return list;
        }

        #endregion

        #region internal


        static DataTable ConvertListToDataTable(List<string[]> list, string[] names)
        {

            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }

            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
                table.Columns[i].ColumnName = names[i];
            }

            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }

            return table;
        }
        public void s(string a)
        {

        }

        #endregion internal

        #region writeFiles

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
                    foundVAR = false;
                    foundCN = false;
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
        public static bool IsFileReady(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return inputStream.Length > 0;
            }
            catch (Exception)
            {
                return false;
            }
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
                //while (!IsFileReady(ListPath)) { }
                bool done = true;
                while (done)
                {
                    try
                    {
                        File.WriteAllLines(ListPath, mainList);
                        ListUpdated = false;
                        done = false;
                    }
                    catch
                    {

                    }
                }
                
            }


        }

        #endregion writeFiles
    }
}
