using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;
using TurnParts;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using static System.Windows.Forms.AxHost;

namespace MagnusSpace
{
    class Item
    {
        
        StringComparison comp = StringComparison.OrdinalIgnoreCase;
        private string itemInfoPath = "";
        private string log_path = "";
        private string set_log_path = "";
        private string adressesListPath = "";
        private string entradaNF = "ENTRADA.NF";
        public List<string> itemInfoList = new List<string>();
        bool itemInfoupdated = false;
        public List<string> logsList;
        public List<string> SetlogsList;
        public List<string> AdressesList;
        public List<string> groupList;
        public List<string> modelList;
        public List<string> CNList;
        public List<string> LastLogs;
        bool logsListUpdated = false;
        bool setlogsListUpdated = false;
        bool adressesListUpdated = false;
        public string ItemCN = "";
        public string ID_OP = "";
        public string Item_description = "";
        public string fixtureLoc = "";
        public string ItemPN = "";
        public string ItemName = "";
        public string ItemModelo = "";
        public string qtdInicial = "";
        public int qtdBin = 0;
        public string npiDate = "";
        public string Status = "";
        public string location = "";
        public string location2 = "";
        public string location3 = "";
        public string location4 = "";
        public string turnsNaLinha = "";
        public string skipScrapLogThatContains = "";



        public string respValidação = "";
        public string dataVistoria = "";
        public string proxVistoria = "";



        public string picPath = "";
        public string rastrear = "";
        public string grupo = "";
        public string picture = "";
        public string validação = "";
        public int Placas_Produzidas_Periodo = 0;///
        public int forcast = 0;
        public float cycleLife = 0;
        public int Eminimo = 0;
        public int QTD_Finalizar_P = 0;
        public bool itemExists = false;
        public bool itemLocked = false;
        public bool fixtureValue = false; //é fixture?
        public int IN_group = 0;
        public int total_group = 0;
        public bool itemPresentinGroup = false;
        private int totalStock = 0;
        bool failItem = false;
        char VarDash = ((char)887);
        char VarDashPlus = ((char)888);
        string[] statas = { "Trivial", "Moderado", "Alerta", "Crítico", "Extremamente crítico", "Zerado", "Sem Controle" };
        public string openCN = "";

        public string PPdateStart = "";
        public string PPdateEnd = "";
        public string PPinRange = "";
        public string duraçãoEstoque = "";





        public char vd()
        {
            return VarDash;
        }
        public string linhaMestra()
        {
            string linhaExcel = "";

            linhaExcel += ItemCN + vd();
            linhaExcel += ItemModelo + vd();
            linhaExcel += ItemName + vd();
            linhaExcel += ItemPN + vd();
            linhaExcel += Item_description + vd();
            linhaExcel += QTD("get").ToString() + vd();
            linhaExcel += Eminimo.ToString() + vd();
            linhaExcel += Status + vd();
            linhaExcel += qtdInicial.ToString() + vd(); ;
            linhaExcel += getVar("QTD_Finalizar").ToString() + vd();
            linhaExcel += cycleLife.ToString("#.##") + vd();
            if (itemPresentinGroup)
                linhaExcel += "IN" + vd();
            else linhaExcel += "OUT" + vd();
            linhaExcel += location3 + vd();
            linhaExcel += location2 + vd();
            linhaExcel += location;
            return linhaExcel;
        }
        private void c(string c)
        {

        }
        public void Open(string CN, string ID = "")
        {

            openCN = CN;
            ItemCN = CN;
            ID_OP = ID;

            //first of all check if item exists


            Folders folder = new Folders();
            itemInfoPath = folder.itemInfoPath(CN);
            itemExistsMethod();
            if (itemExists == false)
            {
                return;
            }
            
            log_path = folder.itemLogPath(CN);
            set_log_path = folder.itemSetLogPath(CN);
            adressesListPath = folder.itemAdressesPath(CN);
            CheckTXT_exists(log_path);
            CheckTXT_exists(set_log_path);
            CheckTXT_exists(adressesListPath);
            try { readList("mainList"); } //update list}
            catch {  }
            try  { readList(log_path); } // both lists
            catch {  }
            try { readList(set_log_path); } // both lists
            catch { }
            try { readList(adressesListPath); } // both lists
            catch { }
            
            try
            {
                //ItemCN = itemInfoList[0].Split(VarDash)[1];
                //ItemName = itemInfoList[1].Split(VarDash)[1];
                //ItemPN = itemInfoList[2].Split(VarDash)[1];
                //Item_description = itemInfoList[5].Split(VarDash)[1];
               // Eminimo = Convert.ToInt32(itemInfoList[4].Split(VarDash)[1]);

            }
            catch
            {
                ListClass lc = new ListClass();
                lc.Open("failLog", "listFolder");
                lc.mainList.Add("CN <" + CN +"> fail to read " + DateTime.Now.ToString());
                lc.Close();
                Close();
                return;
            }

            refresh();
            

        }
        public void itemExistsMethod()
        {
            ListClass lc = new ListClass();
            lc.Open("Mestra", "ListaGeral");
            foreach (string l in lc.mainList.ToList())
            {
                List<string> subList = l.Split(VarDashPlus).ToList();
                foreach (string l2 in subList)
                {
                    if (l2.Split(VarDash)[0] == "CN" && l2.Split(VarDash)[1] == openCN)
                    {
                        itemExists = true;
                    }
                }
            }
        }
        public int getScraps(DateTime ini, DateTime fim) 
        {
            //ferro
            int scraps = 0;
            List<string > list = new List<string>();
            if(ini == fim)
            {
                list = FilterLogList(ini, fim,"ALL");
            }
            else
            {
                list = FilterLogList(ini, fim);
            }
            

            foreach (string s in list)
            {
                if (s.Contains(entradaNF))
                {
                    continue;
                }

                try
                {
                    scraps += Convert.ToInt32(s.Split(' ')[3].Split(':')[1]);
                }
                catch { }
            }

            return scraps*(-1);
        }
        public int getTurns()
        {
            return 0;
        }
        public void loadhowlongwegotModel()
        {
            
            
            
        }
        bool thisistheone = false;
        public int[] howlongwegot()
        {
            int[] days= new int[2];
            float qtd = (float) QTD("get");
            float turns = (float) getTurns();
            float cycle = cycleLife;
            float howmanywecanproduce = (qtd + turns) * cycle;
            float howmanywecanproduceStockOnly = (qtd) * cycle;
            int daysWeGot = howlongUtill((int)howmanywecanproduce);
            int daysWeGotStockOnly = howlongUtill((int)howmanywecanproduceStockOnly);
            days[0] = daysWeGot;
            days[1] = daysWeGotStockOnly;
            thisistheone = true;
            if(daysWeGot == -1)
            {
                
                stream("daysWeGot", "OK");
                stream("daysWeGotStockOnly", "OK");
            }
            else
            {
                stream("daysWeGot", daysWeGot.ToString());
                stream("daysWeGotStockOnly", daysWeGotStockOnly.ToString());
            }
            setStatus(daysWeGot);
            
 
            return days;

        }
        public int howlongUtill(int board)//we only can produce this many
        {
            int forcast = 0;
            ListClass lc = new ListClass();
            Folders folder = new Folders();
            lc.Open(ItemModelo, folder.Forecast);
            List<string> list2 = new List<string>();
            list2 = lc.mainList;
            list2.Sort();
            DateTime dt1 = DateTime.Now;

            foreach (string l in list2)
            {
                bool bk = false;
                if (l.StartsWith("CN"))
                {
                    string currentCN = l.Split(VarDashPlus)[0].Split(VarDash)[1];
                    int CCN = 0;
                    try
                    {
                        CCN = Convert.ToInt32(currentCN);
                    }
                    catch { bk = true; }
                    if (bk == false)
                    {

                        DateTime dt = DateTime.Now;
                        int yr = dt.Year;
                        if (CCN >= yr)
                        {
                            List<string> list = new List<string>();
                            list = l.Split(VarDashPlus).ToList();
                            list.RemoveAt(0);
                            if (CCN == yr)
                            {
                                int mon = dt.Month;
                                int cm = 0;
                                foreach (string m in list)
                                {
                                    cm = Convert.ToInt32(m.Split(VarDash)[0]);
                                    if (cm >= mon)
                                    {
                                        forcast += Convert.ToInt32(m.Split(VarDash)[1]);
                                        display(forcast,board,cm,CCN);
                                        if(forcast > board)
                                        {
                                            DateTime dt2 = DateTime.Now;
                                            return (int)dt2.Subtract(dt1).TotalDays*(-1);
                                        }
                                        else
                                        {
                                            dt1 = new DateTime(CCN, cm, 1);
                                            
                                        }
                                        
                                    }
                                }
                            }
                            if (CCN > yr)
                            {
                                foreach (string m in list)
                                {
                                    
                                    int cm = Convert.ToInt32(m.Split(VarDash)[0]);
                                    forcast += Convert.ToInt32(m.Split(VarDash)[1]);
                                    display(forcast,board,cm,CCN);
                                    if (forcast > board)
                                    { 
                                        DateTime dt2 = DateTime.Now;
                                        return (int)dt2.Subtract(dt1).TotalDays*(-1);
                                    }
                                    else
                                    {
                                        dt1 = new DateTime(CCN, cm, 1);

                                    }
                                    /*
                                    if ((forcast + board) > (forcast + Convert.ToInt32(m.Split(VarDash)[1])))
                                    {
                                        forcast += Convert.ToInt32(m.Split(VarDash)[1]);
                                    }
                                        
                                    else
                                    {
                                        DateTime dt1 = new DateTime(CCN, cm, 0);
                                        DateTime dt2 = DateTime.Now;
                                        return (int)dt2.Subtract(dt1).TotalDays;
                                    };
                                    */
                                }
                            }
                        }
                    }
                }

            }

            return -1;

        }

        public void display(int forvc , int boar , int mont , int year)
        {

        }
        public int getForecast()
        {
            int forcast = 0;
            ListClass lc = new ListClass();
            Folders folder = new Folders();
            lc.Open(ItemModelo, folder.Forecast);

            foreach (string l in lc.mainList)
            {
                bool bk = false;
                if (l.StartsWith("CN"))
                {
                    string currentCN = l.Split(VarDashPlus)[0].Split(VarDash)[1];
                    int CCN = 0;
                    try
                    {
                        CCN = Convert.ToInt32(currentCN);
                    }
                    catch { bk = true; }
                    if (bk == false)
                    {

                        DateTime dt = DateTime.Now;
                        int yr = dt.Year;
                        if (CCN >= yr)
                        {
                            List<string> list = new List<string>();
                            list = l.Split(VarDashPlus).ToList();
                            list.RemoveAt(0);
                            if (CCN == yr)
                            {
                                int mon = dt.Month;
                                int cm = 0;
                                foreach (string m in list)
                                {
                                    cm = Convert.ToInt32(m.Split(VarDash)[0]);
                                    if (cm >= mon)
                                    {
                                        forcast += Convert.ToInt32(m.Split(VarDash)[1]);
                                    }
                                }
                            }
                            if (CCN > yr)
                            {
                                foreach (string m in list)
                                {
                                    forcast += Convert.ToInt32(m.Split(VarDash)[1]);
                                }
                            }
                        }
                    }
                }

            }
        
            return forcast;

        }


        public List<string> groupToList()
        {
            List<string> gl = new List<string>();
            ListClass lc = new ListClass();
            lc.Open("Mestra", "ListaGeral");
            foreach (string l in lc.mainList.ToList())
            {
                List<string> subList = l.Split(VarDashPlus).ToList();
                foreach (string l2 in subList)
                {
                    if (l2.Split(VarDash)[0] == "grupo" && l2.Split(VarDash)[1] == grupo)
                    {
                        string dash = VarDash.ToString();
                        string check = "";
                        string linha_do_tem = ""; /// linha que deve ser salva no padrão da lista de grupo
                        List<string> itemInfoList_doItem = subList;
                        ListClass lc2 = new ListClass();
             
             
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "CN") + dash;
         
                        check = lc2.streamSEARCH(itemInfoList_doItem, "groupPosition");
    
                        if (check == "")
                        {
                            linha_do_tem += "IN" + dash;
                        }
                        else
                        {
                            linha_do_tem += check + dash;
                        }
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "P/N") + dash;
              
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "Descrição") + dash;
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "Responsavel") + dash;
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "DATA_MANUT") + dash;
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "versao") + dash;
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "location3") + dash;
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "location2") + dash;
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "location") + dash;

            
                        //esquilo



                        ///// este item pertence ao grupo
                        ///
                        
                        gl.Add(linha_do_tem);
                    }
                }
            }
            return gl;
        }

        public List<string> modelToList()
        {
            List<string> gl = new List<string>();
            ListClass lc = new ListClass();
            lc.Open("Mestra", "ListaGeral");
            foreach (string l in lc.mainList.ToList())
            {
                List<string> subList = l.Split(VarDashPlus).ToList();
                foreach (string l2 in subList)
                {
                    if (l2.Split(VarDash)[0] == "Modelo" && l2.Split(VarDash)[1] == ItemModelo)
                    {
                        string dash = VarDash.ToString();
                        string check = "";
                        string linha_do_tem = ""; /// linha que deve ser salva no padrão da lista de grupo
                        List<string> itemInfoList_doItem = subList;
                        ListClass lc2 = new ListClass();


                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "CN") + dash;

                        check = lc2.streamSEARCH(itemInfoList_doItem, "groupPosition");

                        if (check == "")
                        {
                            linha_do_tem += "IN" + dash;
                        }
                        else
                        {
                            linha_do_tem += check + dash;
                        }
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "P/N") + dash;

                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "Descrição") + dash;
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "Responsavel") + dash;
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "DATA_MANUT") + dash;
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "versao") + dash;
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "location3") + dash;
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "location2") + dash;
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "location") + dash;


                        //esquilo



                        ///// este item pertence ao grupo
                        ///

                        gl.Add(linha_do_tem);
                    }
                }
            }
            return gl;
        }
        private void failLog(string message)
        {
            ListClass lc = new ListClass();
            lc.Open("failLog", "listFolder");
            lc.mainList.Add("CN <" + openCN + "> "+ message + " " + DateTime.Now.ToString());
            lc.Close();
            Close();
            return;
        }
        public void refresh()
        {
            int a = 0;
            foreach (string line in itemInfoList.ToList())
            {
                if (line.Split(VarDash)[0] == "CN")
                {
                    ItemCN = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "Name")
                {
                    ItemName = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "P/N")
                {
                    ItemPN = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "Descrição")
                {
                    Item_description = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "EstoqueM")
                {
                    try
                    {
                        Eminimo = Convert.ToInt32(itemInfoList[a].Split(VarDash)[1]);
                    }
                    catch { Eminimo = 0; }
                }
                if (line.Split(VarDash)[0] == "qtdBin")
                {
                    try
                    {
                        qtdBin = Convert.ToInt32(itemInfoList[a].Split(VarDash)[1]);
                    }
                    catch { qtdBin = 0; }
                }
                //qtdBin
                if (line.Split(VarDash)[0] == "Estoque Inicial")
                {
                    qtdInicial = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "Modelo")
                {
                    ItemModelo = itemInfoList[a].Split(VarDash)[1];
                    //modelList = modelToList();
                }


                if (line.Split(VarDash)[0] == "picPath")
                {
                    picPath = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "rastrear")
                {
                    rastrear = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "location")
                {
                    location = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "location2")
                {
                    location2 = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "location3")
                {
                    location3 = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "location4")
                {
                    location4 = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "respValidação")
                {
                    respValidação = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "dataVistoria")
                {
                    dataVistoria = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "proxVistoria")
                {
                    proxVistoria = itemInfoList[a].Split(VarDash)[1];
                }






                if (line.Split(VarDash)[0] == "grupo")
                {
                    grupo = itemInfoList[a].Split(VarDash)[1];
                    groupList = groupToList();
                    int total = 0;
                    int b = 0;
                    IN_group = 0;
                    total_group = 0;
                    foreach (string l in groupList.ToList())
                    {
                        if (l.Split(VarDash)[1] == "IN")
                        {
                            IN_group++;
                            if (l.Split(VarDash)[0] == ItemCN)
                            {
                                itemPresentinGroup = true;
                                stream("groupPosition","IN");
                            }
                        }

                        b++;
                        if (l == "")
                            continue;
                        total++;
                    }
                    total_group = total;
                    if(itemPresentinGroup == false)
                    {
                        stream("groupPosition", "OUT");
                    }
                }

                if (line.Split(VarDash)[0] == "picture")
                {
                    picture = itemInfoList[a].Split(VarDash)[1];
                }

                if (line.Split(VarDash)[0] == "Data NPI")
                {
                    npiDate = itemInfoList[a].Split(VarDash)[1];
                }
                if (line.Split(VarDash)[0] == "PlacasProd")
                {
                    try { Placas_Produzidas_Periodo = Convert.ToInt32(line.Split(VarDash)[1]); }
                    catch { }
                }
                if (line.Split(VarDash)[0] == "Forcast")
                {
                    try { forcast = Convert.ToInt32(line.Split(VarDash)[1]); }
                    catch { }
                }
                if (line.Split(VarDash)[0] == "CycleLife")
                {
                    try { cycleLife = (float)Convert.ToDouble(line.Split(VarDash)[1]); }
                    catch { }
                }
                if (line.Split(VarDash)[0] == "QTDPFP")
                {
                    try { QTD_Finalizar_P = (int)Convert.ToDouble(line.Split(VarDash)[1]); }
                    catch { }
                }
                //ItensINLine
                if (line.Split(VarDash)[0] == "Status")
                {
                    try { Status = itemInfoList[a].Split(VarDash)[1]; }
                    catch { }
                }
                if (line.Split(VarDash)[0] == "ItensINLine")
                {
                    try { turnsNaLinha = itemInfoList[a].Split(VarDash)[1]; }
                    catch { }
                }
                if (line.Split(VarDash)[0] == "itemLocked")
                {

                    try
                    {
                        string l4 = line.Split(VarDash)[1];
                        if (l4 == "true")
                        {
                            itemLocked = true;
                        }
                        else
                        {
                            itemLocked = false;
                        }
                    }
                    catch { }
                }
                a++;
            }
        }
        public int barValue()
        {
            int qtdIni = 0;
            try { qtdIni = Convert.ToInt32(qtdInicial); }
            catch { }
            int qtd = (int)QTD("get");
            if(qtdIni == 0) //SEM CONTROLE
            {
                return -1;
            }
            else
            {
                if (qtd > qtdIni)
                {
                    return -2;
                }
                else
                {
                    return (qtd * 255) / qtdIni;
                }
                
            }
        }
        public string stream(string varName, string value = "null")
        {
            int a = 0;
            if(value == "null")
            {
                foreach (string l in itemInfoList)
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
                foreach(string l in itemInfoList)
                {
                    if (l.Split(VarDash)[0] == varName)
                    {
                        itemInfoList[a] = varName + VarDash + value;
                        return value;
                    }
                    a++;
                }
                itemInfoList.Add(varName + VarDash + value);
                return value;
            }
            
        } // set any var in list
        public void setStatus(int daysLeft1)
        {
            int daysLeft = 0;
            if(daysLeft1 == -1)
            {
                daysLeft = int.MaxValue - 1;
            }
            else
            {
                daysLeft = daysLeft1;
            }
            Form1 form = new Form1();
            form = System.Windows.Forms.Application.OpenForms["Form1"] as Form1;
            string statusRange = form.criticalRange;
            List<string> list = new List<string>();
            List<int> list1 = new List<int>();
            list = statusRange.Split(',').ToList();
            int a = 0;
            foreach(string l in list.ToList())
            {
                list1.Add(Convert.ToInt32(l))  ;
                a++;
            }
            list1.Sort();
            list1.Insert(0,0);
            list1.Add(int.MaxValue);
            int counter = 0;
            foreach(int b in list1)
            {
                if (counter == 0)
                {
                    counter++; continue;
                }
                if (counter == list.Count())
                    break;
                if (list1[counter -1] < daysLeft && list1[counter] > daysLeft)
                {
                    break ;
                }
                else
                {
                    counter++;
                }
                
            }
            switch (counter)
            {
                    case 1:
                    stream("Status", "Extremamente Crítico"); break;
                    case 2:
                    stream("Status", "Crítico"); break;
                    case 3:
                    stream("Status", "Alerta"); break;
                    case 4:
                    stream("Status", "Moderado"); break;
                    case 5:
                    stream("Status", "Trivial"); break;
            }

        }
        public void setName(string name)
        {
            stream("Name", name);
        }
        public void edit(string item, string text)
        {
            int number = 0;
            int a = 0;
            bool foundTheVar = false;
            switch (item)
            {
                case "nome":
                    foreach(string l in itemInfoList.ToList())
                    {
                        if (l.Split(VarDash)[0] == "Name")
                        {
                            itemInfoList[a] = l.Split(VarDash)[0] + VarDash.ToString() + text;
                            foundTheVar = true;
                        }
                            
                            a++;
                    }
                    if(!foundTheVar)
                        itemInfoList.Add("Name" + ":" + text);
                    return;
                case "modelo":
                    foreach (string l in itemInfoList.ToList())
                    {
                        if (l.Split(VarDash)[0] == "Modelo")
                        {
                            itemInfoList[a] = l.Split(VarDash)[0] + VarDash.ToString() + text;
                            foundTheVar = true;
                        }
                            
                        a++;
                    }
                    if (!foundTheVar) 
                        itemInfoList.Add("Modelo" + VarDash.ToString() + text);
                    return;
                case "cn":
                    foreach (string l in itemInfoList.ToList())
                    {
                        if (l.Split(VarDash)[0] == "CN")
                        {
                            itemInfoList[a] = l.Split(VarDash)[0] + VarDash.ToString() + text;
                            foundTheVar = true;
                        }
                            
                        a++;
                    }
                    if (!foundTheVar) 
                        itemInfoList.Add("CN" + VarDash.ToString() + text);
                    return;
                case "pn":
                    foreach (string l in itemInfoList.ToList())
                    {
                        if (l.Split(VarDash)[0] == "P/N")
                        {
                            itemInfoList[a] = l.Split(VarDash)[0] + VarDash.ToString() + text;
                            foundTheVar = true;
                        }
                            
                        a++;
                    }
                    if (!foundTheVar) 
                        itemInfoList.Add("P/N" + VarDash.ToString() + text);
                    return;
                case "em":
                    foreach (string l in itemInfoList.ToList())
                    {
                        try
                        {
                            number = Convert.ToInt32(text);
                        }
                        catch { return; }
                        if (l.Split(VarDash)[0] == "EstoqueM")
                        {
                            itemInfoList[a] = l.Split(VarDash)[0] + VarDash.ToString() + number.ToString();
                            foundTheVar = true;
                        }
                            
                        a++;
                    }
                    if (!foundTheVar) 
                        itemInfoList.Add("EstoqueM" + VarDash.ToString() + number.ToString());
                    return;
                case "qi":
                    foreach (string l in itemInfoList.ToList())
                    {
                        try
                        {
                            number = Convert.ToInt32(text);
                        }
                        catch { return; }
                        if (l.Split(VarDash)[0] == "Estoque Inicial")
                        {
                            itemInfoList[a] = l.Split(VarDash)[0] + VarDash.ToString() + number.ToString();
                            foundTheVar = true;
                            qtdInicial = number.ToString();
                        }
                            
                        a++;
                    }
                    if (!foundTheVar) 
                        itemInfoList.Add("Estoque Inicial" + VarDash.ToString() + number.ToString());
                    return;
                case "alerta":
                    foreach (string l in itemInfoList.ToList())
                    {
                        try
                        {
                            number = Convert.ToInt32(text);
                        }
                        catch { return; }
                        if (l.Split(VarDash)[0] == "Estoque Inicial")
                        {
                            itemInfoList[a] = l.Split(VarDash)[0] + VarDash.ToString() + number.ToString();
                            foundTheVar = true;
                        }
                            
                        a++;
                    }
                    if (!foundTheVar) 
                        itemInfoList.Add("Estoque Inicial" + VarDash.ToString() + number.ToString());
                    return;
                case "npiDate":
                    foreach (string l in itemInfoList.ToList())
                    {
                        if (l.Split(VarDash)[0] == "Data NPI")
                        {
                            itemInfoList[a] = l.Split(VarDash)[0] + VarDash.ToString() + text;
                            foundTheVar = true;
                        }
                            
                        a++;
                    }
                    if (!foundTheVar) 
                        itemInfoList.Add("Data NPI" + VarDash.ToString() + text);
                    return;
            }
        }
        public void setDesc(string desc)
        {
            stream("Descrição", desc);
        }
        public int setQTDINI(int qtdI)
        {
            int a = 0;
            foreach(string l in itemInfoList)
            {
                if (itemInfoList[a].Split(VarDash)[0]== "Estoque Inicial")
                {
                    itemInfoList[a] = "Estoque Inicial" + VarDash.ToString() + qtdI.ToString();
                    return qtdI;
                }
                a++;
            }
            itemInfoList.Add("Estoque Inicial:" + qtdI.ToString());
            return qtdI;

        }
        public int setQTD_Finalizar(int qtdI)
        {
            int a = 0;
            foreach (string l in itemInfoList)
            {
                if (itemInfoList[a].Split(VarDash)[0] == "QTDPFP")
                {
                    itemInfoList[a] = "QTDPFP" + VarDash.ToString() + qtdI.ToString();
                    return qtdI;
                }
                a++;
            }
            itemInfoList.Add("QTDPFP" + VarDash.ToString() + qtdI.ToString());
            return qtdI;

        }
        public bool createItemFromList(List<string> list = null)
        {
            
  
            Folders folder = new Folders();
            List<string> l = new List<string>();
            ExcelClass ex = new ExcelClass();
            if(list != null)
            {
                l = list;
            }
            else
            {
                l = ex.sheetToList(folder.createItensPlanilhaPath());

                l.RemoveAt(0);
            }
            
            int total = l.Count();
            
            int b = 0;
            string sCN, sNOME, sPN, sQTD, sEM,sDesc,model2,QTDINI,npiDATE,grupoItem,pictu,valid;


           

            foreach (string a in l)
            {
      
                if (a != "")
                {
                  
                    try { sCN = l[b].Split(vd())[0]; } catch {  return false; }
                    Item i = new Item();
                    i.Open(sCN);
                    try 
                    {
                        if (l[b].Split(vd())[2] != "")
                        {
                            sNOME = l[b].Split(vd())[2];
                        }
                        else
                        {
                            sNOME = i.ItemName;
                        }
                          
                    } catch { sNOME = i.ItemName; }
                    try 
                    {
                        if (l[b].Split(vd())[3]!="")
                        {
                            sPN = l[b].Split(vd())[3];
                        }
                        else
                        {
                            sPN = i.ItemPN;
                        }
                        
                    } catch { sPN = i.ItemPN; }
                    try 
                    {
                        if (l[b].Split(vd())[5]!="")
                        {
                            sQTD = l[b].Split(vd())[5];
                        }
                        else
                        {
                            sQTD = i.QTD("get").ToString();
                        }
                        
                    } catch { sQTD = i.QTD("get").ToString(); }
                    try 
                    {
                        if (l[b].Split(vd())[6]!="")
                        {
                            sEM = l[b].Split(vd())[6];
                        }
                        else
                        {
                            sEM = i.Eminimo.ToString();
                        }
                        
                    } catch { sEM = i.Eminimo.ToString(); }
                    try 
                    {
                        if (l[b].Split(vd())[4]!="")
                        {
                            sDesc = l[b].Split(vd())[4];
                        }
                        else
                        {
                            sDesc = i.Item_description;
                        }
                        
                    } catch { sDesc = i.Item_description; }
                    try 
                    {
                        if (l[b].Split(vd())[1]!="")
                        {
                            model2 = l[b].Split(vd())[1];
                        }
                        else
                        {
                            model2 = i.ItemModelo;
                        }
                        
                    } catch { model2 = i.ItemModelo; }




                    try
                    {
                        if (l[b].Split(vd())[7] != "")
                        {
                            QTDINI = l[b].Split(vd())[7];
                        }
                        else
                        {
                            QTDINI = i.qtdInicial;
                        }

                    }
                    catch { QTDINI = i.qtdInicial; }
                    try
                    {
                        if (l[b].Split(vd())[8] != "")
                        {
                            npiDATE = l[b].Split(vd())[8];
                        }
                        else
                        {
                            npiDATE = i.npiDate;
                        }

                    }
                    catch { npiDATE = i.npiDate; }
                    try
                    {
                        if (l[b].Split(vd())[9] != "")
                        {
                            if(l[b].Split(vd())[9] == "0")
                            {
                                grupoItem = "";
                            }
                            else
                            {
                                grupoItem = l[b].Split(vd())[9];
                            }
                            
                        }
                        else
                        {
                            grupoItem = i.grupo;
                        }

                    }
                    catch { grupoItem = i.grupo; }


                    try
                    {
                        

                        if (l[b].Split(vd())[10] != "")
                        {
                            pictu = l[b].Split(vd())[10];
                        }
                        else
                        {
                            pictu = i.picture;
                        }

                    }
                    catch { pictu = i.picture; }
                    try
                    {
                        if (l[b].Split(vd())[11] != "")
                        {
                            valid = l[b].Split(vd())[11];
                        }
                        else
                        {
                            valid = i.validação;
                        }

                    }
                    catch { valid = i.validação; }


                    i.Close();
                    




                    if (sCN!= "")
                    {
                        
                        try
                        {
                           
                            try { Convert.ToInt32(sQTD); }
                            catch { sQTD = "0"; }
                            try { Convert.ToInt32(sEM); }
                            catch { sEM = "0"; }
                            createItem(sNOME, sPN, Convert.ToInt32(sQTD), Convert.ToInt32(sEM), sDesc, sCN,null,model2, QTDINI, npiDATE, grupoItem,pictu,valid);
                        }
                        catch { }
                    }
                       

                }
                b++;
            }
            return true;
        }
        public int getVar(string VAR)
        {
            int scraps;
            int IIL = 0;
            if (qtdInicial == "")
                scraps = 0;
            else
            {
                
                try { IIL =  Convert.ToInt32(stream("ItensINLine")); }
                catch { IIL = 0; }
                    scraps = Convert.ToInt32(qtdInicial) - Convert.ToInt32(QTD("get")) - IIL; //qtd scraps
            }
                

            switch (VAR)
            {
                case "scraps":
                    if (qtdInicial == "")
                        return 0;
             
                    return scraps;
                    break;
                case "QTD_Finalizar":
                    if (Placas_Produzidas_Periodo == 0)
                        return 0;
                    float Quant_para_Finalizar = ((float)(forcast) * (scraps)) / ((float)(Placas_Produzidas_Periodo));
                    return (int)Math.Ceiling(Quant_para_Finalizar);
                    break;
                /*case "CycleLife":
                    float cycle = (float)Placas_Produzidas_Periodo / (float)scraps;
                    return (int)Math.Ceiling(cycle);
                    break;
                */
            }
            return 0;
        }
        public string loadCycleLife()
        {
            return "";
            refresh();
            float cycle = 0;
            try { cycle = (float)Placas_Produzidas_Periodo / (float)(getVar("scraps")); }
            catch { }
            setVar("CycleLife", cycle);

            foreach (string line in itemInfoList)
            {
                if (line.Split(VarDash)[0] == "CycleLife")
                {
                    try { cycleLife = (float)Convert.ToDouble(line.Split(VarDash)[1]); }
                    catch { }
                    break;
                }

            }
  
            return cycleLife.ToString();




        }

        public void setVar(string VAR, float value)
        {
            int a = 0;
            switch (VAR)
            {
                case "PPNP":
                    a = 0;
                    foreach(string l in itemInfoList)
                    {
                        if (l.Split(VarDash)[0] == "PPNP")
                        {
                            itemInfoList[a] = "PPNP" + VarDash.ToString() + (Convert.ToInt32(value)).ToString();
                            Placas_Produzidas_Periodo = Convert.ToInt32(value);
                            return;
                        }
                        a++;
                    }
                    itemInfoList.Add("PPNP" + VarDash.ToString() + (Convert.ToInt32(value)).ToString());
                    Placas_Produzidas_Periodo = Convert.ToInt32(value);
                    break;
                case "Forcast":
                    a = 0;
                    foreach (string l in itemInfoList)
                    {
                        if (l.Split(VarDash)[0] == "Forcast")
                        {
                            itemInfoList[a] = "Forcast" + VarDash.ToString() + (Convert.ToInt32(value)).ToString();
                            forcast = Convert.ToInt32(value);
                            return;
                        }
                        a++;
                    }
                    itemInfoList.Add("Forcast" + VarDash.ToString() + (Convert.ToInt32(value)).ToString());
                    forcast = Convert.ToInt32(value);
                    break;
                case "CycleLife":
                    a = 0;

                    stream("CycleLife", value.ToString("#.##"));
                   
                    break;
            }
        }
        public void set_EstoqueMinimo(int value)
        {
            
            string a = itemInfoList[4];
            string b = a.Split(VarDash)[0];
            itemInfoList[4] = b + ":" + value.ToString();
            writeList(itemInfoPath, itemInfoList);
        }
        public string addID_inLog(string text,string action = "ID",int logNum = 1)
        {
            int qtdInLog = 0;
            bool oldID = false;
            bool newID = false;
            string oldIDText = "";
            string newIDText = "";
            bool isLogBecamingNF = false;
            string ID = "";
            string logRet = "";
            if (!itemExists)
                return "";
            int totalC = logsList.Count();
            string cLog = logsList[totalC - logNum];
            string Clog = "";
            string log = "";
            int n = 0;
            string l = "";
            foreach(string l1 in cLog.Split(' ').ToList())
            {
                l = l1;
                try
                {
                    if (l1.Split(':')[0] == "Q")
                    {
                        qtdInLog = Convert.ToInt32(l1.Split(':')[1]);
                    }
                }
                catch
                {
                   
                }
                try
                {
                    if (l1.Split(':')[0] == action)
                    {
                        if(action == "ID")
                        {
                            newID = text.Contains(entradaNF);
                            newIDText = text;
                            ID = text;
                        }
                        else
                        {
                            newIDText = l1.Split(':')[1];
                            newID = l1.Split(':')[1].Contains(entradaNF);
                            ID = l1.Split(':')[1];
                        }
                        oldIDText = l1.Split(':')[1];
                        oldID = l1.Split(':')[1].Contains(entradaNF);
                        l = l1.Split(':')[0] + ":" + text;
                    }
                }
                catch
                {
                    l = l1;
                }
                
                if(n == 0)
                {
                    Clog += l;
                }
                else
                {
                    Clog += " " + l;
                }
                n++;
            }
            int nl = logNum;
            bool isthereequallogs = false;
            string oldLog = logsList[totalC - logNum];
            logsList[totalC - logNum] = Clog;
            if(logNum != 1 && totalC != logNum)
            {
                if (oldLog == logsList[totalC - logNum - 1] || oldLog == logsList[totalC - logNum + 1])
                {
                    isthereequallogs = true;
                }
            }
            else //edge log
            {
                if (totalC > 1) //there is at least 2 logs
                {
                    if (logNum == 1) // primeiro log
                    {
                        if (oldLog == logsList[1])
                            isthereequallogs = true;
                    }
                    else // lastLog
                    {
                        if (oldLog == logsList[totalC - 2])
                            isthereequallogs = true;
                    }
                }
               
            }
            ListClass lc2 = new ListClass();
            Folders folders2 = new Folders();
            DateTime dt2 = logtoDatetime(Clog);
            string logLibraryName = dt2.Year.ToString() + "-" + dt2.Month.ToString();
            lc2.Open(logLibraryName, folders2.logLibrary);
            if (isthereequallogs) //manually search each
            {
                int equalLogsCounter = 0;
                for(int p = 0; p < logNum; p++)
                {
                    if (logsList[p] == oldLog)
                    {
                        equalLogsCounter++;
                    }
                }
                for(int p1 = 0; p1 < lc2.mainList.Count(); p1++)
                {
                    if (lc2.mainList[p1] == oldLog)
                    {
                        equalLogsCounter--;
                    }
                    if(equalLogsCounter == 0)
                    {
                        lc2.mainList[p1] = Clog;
                        break;
                    }
                }
            }
            else
            {
                for (int r = 0; r < lc2.mainList.Count(); r++)
                {
                    if (lc2.mainList[r] == oldLog)
                    {
                        lc2.mainList[r] = Clog;
                        break;
                    }
                }
            }
            lc2.Close();
            writeList(log_path, logsList);
            if(oldIDText != newIDText) //somente salvar sem ID forem diferentes
            {
                if (oldIDText != "" && newIDText != "")//Verificar os NF dos dois
                {
                    if (!oldID && !newID)//não existe NF nos ID
                    {
                        AddToScrapList(oldIDText,qtdInLog*(-1));
                        AddToScrapList(newIDText,qtdInLog);
                    }
                    else//verificar os nf
                    {
                        if(oldID && newID)
                        {
                            //fazer nada
                        }
                        else
                        {
                            if (oldID)//old era NF e novo não é
                            {
                                AddToScrapList(newIDText, qtdInLog);
                            }
                            else
                            {
                                AddToScrapList(oldIDText, qtdInLog * (-1));
                            }
                        }
                    }
                }
                if (oldIDText != "" && newIDText == "")//Verificar se o antigo tem NF
                {
                    if (!oldID)
                    {
                        AddToScrapList(oldIDText, qtdInLog * (-1)); 
                    }
                }
                if (oldIDText == "" && newIDText != "")//Verificar NF
                {
                    if (!newID)
                    {
                        AddToScrapList(newIDText,qtdInLog);
                    }
                }

            }





          
            //Add remove log
            ListClass lc = new ListClass();
            lc.Open("Logs", "ListaGeral");
            int foundIt = 0;
            for (int i = lc.mainList.Count - 1; i >= 0; i--)
            {
                if(Clog.Split(' ')[0] == lc.mainList[i].Split(' ')[0])
                {
                    foundIt++;
                    if(foundIt == nl)
                    {
                        lc.mainList[i] = Clog;
                        LastLogs = lc.mainList;
                        lc.Close();
                        break;
                    }
                }
            }

                return Clog;
        }
        public void createItem(string itemName, string PN = "", int qtd = 0, int EsM = 0, string description = "", string CN_ = "", List<string> SKUs = null,string Model1="",string qtdINI = "0", string NPIDate = "", string groupItem = "", string picture1 = "",string validacao1 = "")
        {///// FATA CADASTRAR OS ITENS DIREITO MUDAR O CADASTRO
     
            Folders folder = new Folders();
            folder.buildStructure();
            List<string> TP = new List<string>();
            if(CN_ != "")
                TP.Add("CN" + VarDash.ToString() + CN_);
            else
                TP.Add("CN" + VarDash.ToString() + ItemCN);

            TP.Add("Name" + VarDash.ToString() + itemName);
            TP.Add("P/N" + VarDash.ToString() + PN);
            TP.Add("QTD" + VarDash.ToString() + qtd.ToString());
            TP.Add("EstoqueM" + VarDash.ToString() + EsM.ToString());
            TP.Add("Descrição" + VarDash.ToString() + description);
            string SKUs_ = "";
            int SKUsQTD;
            if (SKUs!= null)
            {
                SKUsQTD = SKUs.Count();
                int a = 0;
                foreach (string l in SKUs)
                {
                    SKUs_ += l;
                    a++;
                    if (a != SKUsQTD)
                    {
                        SKUs_ += ",";
                    }
                }
            }
            else
            {
                SKUsQTD = 0;
            }
            
            TP.Add("SKUs" + VarDash.ToString() + SKUs_);
            TP.Add("Modelo" + VarDash.ToString() + Model1);
            TP.Add("Status" + VarDash.ToString());
            TP.Add("Estoque Inicial" + VarDash.ToString() + qtdINI);
            TP.Add("Data NPI" + VarDash.ToString() + NPIDate);
            TP.Add("grupo" + VarDash.ToString() + groupItem);
            TP.Add("picture" + VarDash.ToString() + picture1);
            TP.Add("validacao" + VarDash.ToString() + validacao1);

            //extrair

            if (CN_ == "") //editar currentCN
            {
                itemInfoList = TP;
                itemInfoupdated = true;
                //writeList(itemInfoPath, TP);
                //c("no cn");
              
            }
            else //novo CN
            {
            

                int CNline = find_line_in_mainCNList(CN_);
                if(CNline == -1)//CN não existente, precisa criar um novo
                {
             
                    ListClass lc = new ListClass();
                    lc.Open("Mestra", "ListaGeral");
                    string line = "";//criar linha na planilha
                    line = list_tostring(TP);
                    lc.mainList.Add(line);
                    lc.Close();
                }
                else //CN ja existe
                {
              
            
                    string cnLine = list_tostring(TP);
                    string varName = "";
                    string varValue = "";

                    ListClass lc = new ListClass();
                    lc.Open("Mestra", "ListaGeral");
                    foreach(string l in TP)
                    {
                        varName = l.Split(VarDash)[0];
                        varValue = l.Split(VarDash)[1];
                        if (varValue != "")
                        {
                            lc.streamPlus(CN_, varName,varValue);
                        }
                        
                    }
            
                    lc.Close();
            

                }
            }
            Folders f = new Folders();
            try
            {
                TurnParts.Form1.CNList.Add(f.CNline(TP));
            }
            catch 
            {

            }
            


        }
       
        int find_line_in_mainCNList(string cn_) //ENCONTRA A LINHA ONDE O CN ESTA REGISTRADO
        {
            bool foundCN = false;
            int CNline = 0;
            int a = 0;
            ListClass lc = new ListClass();
            lc.Open("Mestra", "ListaGeral");
            foreach (string l in lc.mainList.ToList())
            {
                foreach (string l2 in l.Split(VarDashPlus).ToList())
                {
                    if (l2.Split(VarDash)[0] == "CN" && l2.Split(VarDash)[1] == cn_)
                    {
                        foundCN = true;
                        CNline = a;
                        return CNline; // CN Found
                        break;
                    }
                }
                if (foundCN)
                    break;
                a++;
            }
            return -1; //didnt found CN in main list
        }
       
        public int getScrapSemanal()
        {
            List<string> logList = new List<string>();
            logList = FilterLogList(DateTime.Now, DateTime.Now, "semanal");
            int scrap = 0; 
            foreach(string l in logList.ToList())
            {
                if (l.Contains(entradaNF))
                {
                    continue;
                }
                try
                {
                    scrap += Convert.ToInt32(l.Split(' ')[3].Split(':')[1]);
                }
                catch { }
                
            }
            return scrap;
        }
        public int getScrap()
        {
            List<string> logList = new List<string>();
            logList = logsList;
            int scrap = 0;
            foreach (string l in logList.ToList())
            {
                if (l.Contains(entradaNF))
                {
                    continue;
                }
                try
                {
                    scrap += Convert.ToInt32(l.Split(' ')[3].Split(':')[1]);
                }
                catch { }

            }
            return scrap*(-1);
        }
        public List<string> scrapsPerTurno(string mode = "")
        {
            List<string> fullList = new List<string>();
            List<string> logList = new List<string>();
            //List<string> TimeDivisionList = new List<string>();
            DateTime a = DateTime.Today;
            DateTime b = DateTime.Today;
            DateTime c = DateTime.Today;
            DateTime currentHour = DateTime.Today;
            a = new DateTime(a.Year, a.Month, a.Day, 6, 0, 0);
            b = new DateTime(b.Year, b.Month, b.Day, 15, 48, 0);
            c = new DateTime(b.Year, b.Month, b.Day, 1, 0, 0);


            switch(mode)
            {
                case "semanal":
                    logList = FilterLogList(DateTime.Now, DateTime.Now, "semanal");
                    break;
                case "mensal":
                    logList = FilterLogList(DateTime.Now, DateTime.Now, "mensal");
                    break;
                case "3 meses":
                    logList = FilterLogList(DateTime.Now, DateTime.Now, "3 meses");
                    break;
                default:
                    logList = FilterLogList(DateTime.Now, DateTime.Now, "diario");
                    break;

            }


            int hour = 0;
            int min = 0;
            int sec = 0;
            int trocas1 = 0;
            int trocas2 = 0;
            int trocas3 = 0;
            foreach (string l in logList.ToList())
            {
                if (l.Contains(entradaNF))
                {
                    continue;
                }
                hour = Convert.ToInt32(l.Split(' ')[2].Split(':')[0]);
                min = Convert.ToInt32(l.Split(' ')[2].Split(':')[1]);
                sec = Convert.ToInt32(l.Split(' ')[2].Split(':')[2]);
                currentHour = new DateTime(currentHour.Year, currentHour.Month, currentHour.Day, hour, min, sec);
                if(currentHour >= c && currentHour< a)
                {
                    trocas3 += Convert.ToInt32(l.Split(' ')[3].Split(':')[1]);
                }
                else
                {
                    if (currentHour >= a && currentHour < b)
                    {
                        trocas1 += Convert.ToInt32(l.Split(' ')[3].Split(':')[1]);
                    }
                    else
                    {
                        trocas2 += Convert.ToInt32(l.Split(' ')[3].Split(':')[1]);
                    }
                    
                }
            }
         
            string hour1 = a.Hour.ToString() + "h" + a.Minute.ToString();
            fullList.Add(hour1 + "@" +trocas1.ToString());
            hour1 = b.Hour.ToString() + "h" + b.Minute.ToString();
            fullList.Add(hour1 + "@" + trocas2.ToString());
            hour1 = c.Hour.ToString() + "h" + c.Minute.ToString();
            fullList.Add(hour1 + "@" + trocas3.ToString());
            ListClass lc = new ListClass();

            itemInfoList = lc.stream_SET(itemInfoList, "trocas1", trocas1.ToString());
            itemInfoList = lc.stream_SET(itemInfoList, "trocas2", trocas2.ToString());
            itemInfoList = lc.stream_SET(itemInfoList, "trocas3", trocas3.ToString());

            return fullList;
        }




        public List<string> FilterLogList(DateTime start1, DateTime end1, string frequencia = "")
        {
            List<string> scrapList = new List<string>();
            List<string> fullList = new List<string>();
            List<string> idList = new List<string>();
        
            DateTime start = start1;
            DateTime end = end1;

            TimeSpan ts = new TimeSpan(0,0,0);
            start = start.Date + ts;
            int a = 0;
            int b = 0;
            int c = 0;
            string ID = "";
            int scrap = 0;
            DateTime current = new DateTime();
            switch (frequencia)
            {
                case "diario":
                    end = DateTime.Now;
                    start = end.AddDays(-1);
                    break;
                case "LastDay":
                    end = DateTime.Today;
                    start = end.AddDays(-1);
                    break;
                case "semanal":
                    end = DateTime.Now;
                    start = end.AddDays(-7);
                    break;
                case "mensal":
                    end = DateTime.Now;
                    start = end.AddDays(-30);
                    break;
                case "3 meses":
                    end = DateTime.Now;
                    start = end.AddDays(-90);
                    break;
            }
            if (logsList == null)
                return scrapList;
    
            if (logsList.Count() == 0)
            {
                return scrapList;
            }
            if (frequencia == "ALL")
                return logsList.ToList();

            foreach (string line in logsList.ToList())
            {
                current = logtoDatetime(line);
                if (current > end || current < start)
                    continue;
                fullList.Add(line);
            }
       
            return fullList;
        }
            public List<string> scrapList(DateTime start1, DateTime end1, string frequencia = "diario")
        {
            List<string> scrapList = new List<string>();
            List<string> fullList = new List<string>();
            List<string> idList = new List<string>();
            DateTime start = start1;
            DateTime end = end1;

            int a = 0;
            int b = 0;
            int c = 0;
            string ID = "";
            int scrap = 0;
            DateTime current = new DateTime();
            switch (frequencia)
            {
                case "diario":
                    end = DateTime.Now;
                    start = end.AddDays(-1);
                    break;
                case "semanal":
                    end = DateTime.Now;
                    start = end.AddDays(-7);
                    break;
                case "mensal":
                    end = DateTime.Now;
                    start = end.AddDays(-30);
                    break;
            }

            try
            {
                if (logsList.Count() == 0)
                {
                    return scrapList;
                }
            }
            catch { failLog("scrapList(), logsList.Count fail"); return scrapList; }
            
            foreach(string line in logsList.ToList())
            {
                if (line.Contains(entradaNF))
                {
                    continue;
                }
     
                current = logtoDatetime(line);
                if (current > end || current < start)
                    continue;

                ID = line.Split(' ')[4].Split(':')[1];
                try { scrap = Convert.ToInt32(line.Split(' ')[3].Split(':')[1]); }
                catch { }
                fullList.Add(ID+"@"+scrap.ToString());
                idList.Add(ID);
                scrap = 0;
            }
            int scraplistIndex = 0;
            int fulllistIndex = 0;
            idList = idList.Distinct().ToList();
            foreach(string l in idList)
            {
                scrapList.Add(l + "@0" );
            }
            if (fullList.Count() >= 1)
            {
                foreach(string ids in fullList.ToList())
                {

                    string fullListID = ids.Split('@')[0];
                    int fullListValue = 0;
                    try { fullListValue = Convert.ToInt32(ids.Split('@')[1]); }
                    catch { }
                    foreach(string slist in scrapList.ToList())
                    {

                        if(fullListID == slist.Split('@')[0])
                        {
                            int ain = 0;
                            try { ain = Convert.ToInt32(slist.Split('@')[1]); }
                            catch { }
                            scrapList[scraplistIndex] = fullListID + "@" + (fullListValue + ain).ToString();
                        }

                        scraplistIndex++;

                    }



                    scraplistIndex = 0;
                    fulllistIndex++;
                }
            }

           

            return scrapList;
        }
        public void editItem( string itemName = "", string PN = "", double qtd = -1, List<string> SKUs = null)
        {
            List<string> TP = new List<string>();
            if(ItemCN != "") { TP[0] = ItemCN; }
            if (itemName != "") { TP[1] = itemName; }
            if (PN != "") { TP[2] = PN; }
            if (qtd != -1) { TP[3] = qtd.ToString(); }
            if (SKUs != null) { TP[4] = ItemCN; } //< editar


            
            string SKUs_ = "";
            int SKUsQTD;
            if (SKUs != null)
            {
                SKUsQTD = SKUs.Count();
            }
            else
            {
                SKUsQTD = 0;
            }
            int a = 0;
            foreach (string l in SKUs)
            {
                SKUs_ += l;
                a++;
                if (a != SKUsQTD)
                {
                    SKUs_ += ",";
                }
            }
            TP.Add("SKUs:" + SKUs_);
        }
        public void deleteItem()
        {
            Folders folder = new Folders();
            folder.deleteTPFolder(ItemCN);
        }
        public List<string> chartList(string mode = "byDay", int num_of_logs = 12)
        {
            if(mode == "ID")
            {
                return scrapList(DateTime.Today, DateTime.Today);
            }
            List<string> charList = new List<string>();
            string logDateReferencia = ""; //data do primeiro log de determinado dia
            string newLogDate = "";
            int a = 0;
            try
            {
                a = logsList.Count() - 1; //incremento de logs na lista
            }
            catch(Exception ex)
            {
                failLog(ex.ToString());
            }
            
            if (a < 0 || logsList == null)
                return charList;
            int dia = 1; //dia na lista
            int qtdDia = 0; //quantidade de itens em determinado dia
            int b = 0;
            string cLog = ""; //log em analise
            cLog = logsList[a];
            logDateReferencia = cLog.Split(' ')[1];
            qtdDia = Convert.ToInt32(cLog.Split(' ')[3].Split(':')[1]);
            //if (qtdDia > 0)
                //qtdDia = 0;

            while (dia <= num_of_logs)
            {

                a--;
                //c("a = " + a.ToString());
                try
                {
                    if (a < 0)
                    {
                        charList.Add(logDateReferencia + "@" + qtdDia.ToString());
                        
                        
                        return charList;
                    }
                    cLog = logsList[a];
                }
                catch 
                {
                    //c("cant read; a = " + a.ToString());
                    return charList; }
                
                newLogDate = cLog.Split(' ')[1];
                if(logDateReferencia == newLogDate)
                {
                    //c("item do mesmo dia");
                    b = Convert.ToInt32(cLog.Split(' ')[3].Split(':')[1]);
                    //if (b < 0)
                        qtdDia += b;

                }
                else
                {
                    //c("novo dia = "+ (dia + 1).ToString() );
                    //c(newLogDate);
                    charList.Add(logDateReferencia+ "@" + qtdDia.ToString());
                    logDateReferencia = newLogDate;
                    //c("new date = " + logDateReferencia);
                    qtdDia = 0;
                    dia++;
                   // c("numero do dia " + dia.ToString());
                    b = 0;
                    b = Convert.ToInt32(cLog.Split(' ')[3].Split(':')[1]);
                    //if (b < 0)
                        qtdDia = b;

                }




            }

            
            return charList;

        }
        public bool MissingDesatualizado = false;
        public void moveAllonGroup(string command = "IN")
        {
            ListClass lc = new ListClass();
            int a = 0;
            switch (command)
            {
                case "IN":
                    
                    lc.Open(grupo, "Grupo");
                    foreach(string l in lc.mainList.ToList())
                    {
                        lc.mainList[a] = lc.mainList[a].Split(VarDash)[0] + VarDash.ToString() + "IN"; 
                        a++;
                    }
                    lc.Close();
                    IN_group = total_group;
                    break;

                case "OUT":
                    lc.Open(grupo, "Grupo");
                    foreach (string l in lc.mainList.ToList())
                    {
                        lc.mainList[a] = lc.mainList[a].Split(VarDash)[0] + VarDash.ToString() + "OUT";
                        a++;
                    }

                    lc.Close();
                    IN_group = 0;
                    break;
            }
        }
        public void clearAdress()
        {
            location = "";
            location2 = "";
            location3 = "";
            location4 = "";
            stream("location", "");
            stream("location2", "");
            stream("location3", "");
            stream("location4", "");

        }
        double addLog_QTD = 0;
        public List<string> lastLogs()
        {
            if(LastLogs == null)
            {
                ListClass lc = new ListClass();
                lc.Open("Logs", "ListaGeral");
                return lc.mainList;
            }
            else
            {
                return LastLogs;
            }
        }
        public double QTD(string action, double value = 0)
        {
            string qtd_ = "0";


            ListClass lc2 = new ListClass();
            qtd_ =  lc2.streamSEARCH(itemInfoList, "QTD");
            if (qtd_ == "")
                qtd_ = "0";

            double qtd = Convert.ToDouble(qtd_);
            int QuantidadeAntiga = Convert.ToInt32(qtd);
            int QuantidadeNova;
            string position = "";
            List<string> infoList = new List<string>();
            switch (action)
            {
                case "add":
                    if(grupo == "")
                    {

                        double bin1 = QTD("bin");
                        bin1 += value;
                        if (bin1 < 0)
                        {
                            // itemInfoList = lc2.stream_SET(itemInfoList,"QTD","0");

                            return -9876543;
                        }

                        qtd += value;
                        addLog_QTD = qtd;
                        QuantidadeNova = Convert.ToInt32(qtd);
                        if (QuantidadeNova < Eminimo && QuantidadeAntiga >= Eminimo)
                        {
                            MissingDesatualizado = true;
                        }
                        
                        
                        
                        
                        Console.WriteLine($"QTD set to {qtd}");
                        AddLog(value);
                    }
                    else
                    {
                        ListClass lc = new ListClass();
                        lc.Open(grupo, "Grupo");

                        // salvar a lista do
                        List<string> glist = new List<string>();
                        glist = groupToList();
                        int a = 0;
                        
                        qtd = 1;
                        ListClass lc5 = new ListClass();
           
                        
                        if (value > 0)
                        {
                            if(stream("groupPosition")!= "IN")
                            {
                                AddLog(1);
                                IN_group++;
                            }
                            stream("groupPosition", "IN");
                            
                            position = "IN";
                            
                            itemPresentinGroup = true;
                            
                        }
                        else
                        {

                            
                            
                            position = "OUT";
                            if (stream("groupPosition") == "IN")
                            {
                                AddLog(1);
                                IN_group--;
                            }
                            stream("groupPosition", "OUT");
                            itemPresentinGroup = false;
                            clearAdress();
                        }

                        if (ItemName.Contains("fixture", comp))
                        {
                            string logpathAdress = "";
                            List<string> settigns = new List<string>();
                            ListClass lc3 = new ListClass();
                            //Form f = new Form as Form1;
                            Form1 form; //= new Form9();
                            form = System.Windows.Forms.Application.OpenForms["Form1"] as Form1;

                            logpathAdress = form.config("FixtureOUTlogPath");
                            if (logpathAdress == "")
                            {
                                Folders folder = new Folders();
                                logpathAdress = form.config("FixtureOUTlogPath", folder.fixtureOutLOGS.ToString());
                               // lc3.Close();
                            }
                            try
                            {
                      
                                ListClass lc4 = new ListClass();
             
                                lc4.Open(ItemCN, Path.GetFullPath(logpathAdress));
                                lc4.mainList.Clear();
                                lc4.mainList.Add(position);
                                lc4.mainList.Add(DateTime.Now.ToString());
                                //lc4.Open(ItemCN + " " + DateTime.Now.ToString(), Path.GetFullPath(logpathAdress) );

                                //newDate = String.

                                lc4.Close();

                            }
                            catch (Exception ex) {  }
                        }
                        lc.Close();
                        return 1;
                    }
                    
                    break;
                case "bin":
                    if(grupo != "")
                    {
                        return 0;
                    }
                    int total = sumCX();
                    
                    double bin = Convert.ToInt32(QTD("get")) - total;
                    if (bin < 0)
                    {
                        //QTD("set", bin * (-1));
                        QTD("set", total);
                        Console.WriteLine($"bin set to {0}");
                        return 0;
                    }
                    else
                    {
                        Console.WriteLine($"Bin set to {bin}");
                        return bin;
                    }
                    
                     
                    break;
                case "set":
                    string log = "";
                    log += "SET" + VarDash.ToString() + qtd.ToString() + "->";
                    log +=  value.ToString() + VarDash.ToString() + DateTime.Now.ToString();
                    SetlogsList.Add(log);
                    itemInfoList = lc2.stream_SET(itemInfoList, "QTD", value.ToString());
                    //itemInfoList[3] = "QTD:" + value.ToString();
                    QuantidadeNova = Convert.ToInt32(value);
                    if (QuantidadeNova < Eminimo && QuantidadeAntiga >= Eminimo)
                    {
                        MissingDesatualizado = true;
                    }
             
                    return value;
                    break;
                case "get":
                    if (grupo == "")
                        return qtd;
                    else
                    {
                        return 1;
                    }
                    
            }
            //vaca
            itemInfoList = lc2.stream_SET(itemInfoList, "QTD", qtd.ToString());
            return qtd;
        }
        public int sumCX()
        {
            int total = 0;

            foreach (string l in AdressesList)
            {
                total += cv(l.Split(VarDashPlus)[1]);
            }
            return total;
        }
        public void AddToScrapList(string ID, int qtd) // somente se não tiver NF
        {

            ListClass lc = new ListClass();
            Folders folder = new Folders();
            DateTime dt = DateTime.Now;
            string listName = dt.Month + "-" + dt.Year;
            lc.Open(listName, folder.Scraps);
            string qtd_ = lc.streamPlus(ID,"QTD");
            int newValue = 0;
            try { newValue = Convert.ToInt32(qtd_); }
            catch {  }
            newValue += qtd;
            lc.streamPlus(ID, "QTD",newValue.ToString(),true);
            lc.Close();
        }
        public string AddLog(double quantidade = -1, string action = "none")
        {
            ListClass lc2 = new ListClass();
            ListClass lc = new ListClass();
            Folders folder = new Folders();
            DateTime dt = DateTime.Now;
            lc2.Open(dt.Year.ToString()+"-"+dt.Month.ToString(),folder.logLibrary);
            lc.Open("Logs", "ListaGeral");
            string logFormat = "";
            string modo = "OUT";
            string charEspaciador = " ";
            if(quantidade>0)
                modo = "IN";
            switch (action)
            {
                case "new":
                    if (quantidade > 0)
                        modo = "Entrada_de_Material";
                    else
                        modo = "Saida_de_Material";
                    break;
                default:
                    break;
            }
            string __time = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string time = __time.Substring(3, 2) + "/" + __time.Substring(0, 2) + __time.Substring(5, __time.Length - 5);
            logFormat = logFormat + time;
            logFormat = logFormat + charEspaciador + "Q:" + quantidade.ToString();
            logFormat = "CN:" + ItemCN + charEspaciador + logFormat;
            logFormat = logFormat + charEspaciador + "ID:" + ID_OP;
            logFormat = logFormat + charEspaciador + "Fixture:" + fixtureLoc;
            logFormat = logFormat + charEspaciador + modo;
            logFormat = logFormat + charEspaciador + "QTD:"+ addLog_QTD.ToString();
            if(action == "get")
            {
                return logFormat;
            }
            else
            {
                lc.mainList.Add(logFormat);
                lc2.mainList.Add(logFormat);
                int total = lc.mainList.Count;
                while(total != 0)
                {
                    if (total == 0)
                        break;
                    if(DateTime.Today.AddMonths(-3) > logtoDatetime(lc.mainList[0]))
                    {
                        lc.mainList.RemoveAt(0);
                        total--;
                    }
                    else
                    {
                        break;
                    }
                    
                }
                lc.Close();
                lc2.Close();
                logsList.Add(logFormat);
                return "";
            }
            
        }
        public int numberScraps(string time = "day") // revisar!!! esta deixando lento
        {
      
            int value = 0;
            List<string> logs = new List<string>();
            logs = getLogList(0,"","",time);
            foreach (string l in logs)
            {

                if (l.Contains(entradaNF))
                {
                    continue;
                }
                try
                {
                    value += Convert.ToInt32(l.Split(' ')[3].Split(':')[1]);
                }
                catch
                {

                }
            }
            return value;
        }
        public DateTime stringtoDatetime(string rightDate)
        {
            try
            {
                int year = Convert.ToInt32(rightDate.Split('/')[2]);
                int month = Convert.ToInt32(rightDate.Split('/')[1]);
                int day = Convert.ToInt32(rightDate.Split('/')[0]);

                return new DateTime(year, month, day);
            }
            catch
            {
                failLog("Fail to convert String to DateTime");
                return DateTime.Now;
            }

        }
        public DateTime logtoDatetime(string log)
        {
            try
            {
                string rightDate = log.Split(' ')[1];
                int year = Convert.ToInt32(rightDate.Split('/')[2]);
                int month = Convert.ToInt32(rightDate.Split('/')[1]);
                int day = Convert.ToInt32(rightDate.Split('/')[0]);
                rightDate = log.Split(' ')[2];
                int hour = Convert.ToInt32(rightDate.Split(':')[0]);
                int min = Convert.ToInt32(rightDate.Split(':')[1]);
                int seg = Convert.ToInt32(rightDate.Split(':')[2]);
                return new DateTime(year, month, day, hour, min, seg);
            }
            catch
            {
                failLog("Fail to convert Log to DateTime");
                return DateTime.Now;
            }
            
        }
        public List<string> returnAllLogs(bool replaceBar)
        {
            if (!itemExists)
                return null;
            string log = "";
            string newData = "";
            int n = 0;
            int a = 0;
            List<string> newlogList = new List<string>();
            if (replaceBar)
            {
                foreach(string l in logsList)
                {
                    n = 0;
                    foreach(string cell in l.Split(' ').ToList())
                    {

                        switch (n)
                        {
                            case 0:
                                log = cell;
                                break;
                            case 1:
                                newData = cell.Replace('/', '.');
                                log += " " + newData;
                                break;
                            default:
                                log += " " + cell;
                                break;
                        }
                        

                        n++;

                    }

                    newlogList.Add(log);
                    log = "";
                    a++;

                }
                return newlogList;
            }
            
            return logsList;

        }   

            public List<string> getLogList(int num_of_logs = 10,string dateStart = "",string dateEnd = "",string dayWeekMonth = "", bool colapsar = false)
        {
            
            if (!itemExists)
                return null;
            if(num_of_logs == -1) //<<<<<<<<<<<<<<<<<
            {
       
            }
            List<string> logs = new List<string>();
            int a = 0;
            int b = logsList.Count();
            string cLog = "";
   
            if (dayWeekMonth != "" || num_of_logs == -1)
            {

                DateTime dStart = DateTime.Now;
                DateTime dEnd = DateTime.Now; ;
                switch (dayWeekMonth)
                {
                    case "day":
                        dStart = DateTime.Now.AddDays(-1);
                        break;
                    case "week":
                        dStart = DateTime.Now.AddDays(-7);
                        break;
                    case "month":
                        dStart = DateTime.Now.AddDays(-30);
                        break;
                }
                DateTime currentDate;
              
                foreach (string log in logsList)
                {
                    try
                    {
                        string rightDate = log.Split(' ')[1];
                        int year = Convert.ToInt32(rightDate.Split('/')[2]);
                        int month = Convert.ToInt32(rightDate.Split('/')[1]);
                        int day = Convert.ToInt32(rightDate.Split('/')[0]);
                        rightDate = log.Split(' ')[2];
                        int hour = Convert.ToInt32(rightDate.Split(':')[0]);
                        int min = Convert.ToInt32(rightDate.Split(':')[1]);
                        int seg = Convert.ToInt32(rightDate.Split(':')[2]);
                        currentDate = new DateTime(year,month,day,hour,min,seg);
                         

                    }
                    catch
                    {
 
                        continue;
                    }
            
                    if(num_of_logs == -1)
                    {
                        logs.Add(log);
                    }
                    else 
                    {
                        if((currentDate >= dStart && currentDate <= dEnd))
                            logs.Add(log);
                    }
             
                }
                if (num_of_logs == -1) //<<<<<<<<<<<<<<<<<
                {
              
                }
                return logs;
            }

            if (dateStart != "" && dateEnd != "") //retornar filtro de datas
            {

                DateTime dStart = Convert.ToDateTime(dateStart);
                DateTime dEnd = Convert.ToDateTime(dateEnd);
                if(num_of_logs == -2)
                {
                    if(dateStart == dateEnd && dateStart == DateTime.Now.ToString().Split(' ')[0])
                    dEnd = DateTime.Now;
                }
                DateTime currentDate;
                foreach (string log in logsList)
                {
                    try
                    {
                        currentDate = Convert.ToDateTime(log.Split(' ')[1]);
                    }
                    catch
                    {
                        continue;
                    }
                    if (currentDate>=dStart && currentDate <= dEnd)
                    {
                        logs.Add(log);
                    }
                }
                if (num_of_logs == -1) //<<<<<<<<<<<<<<<<<
                {
            
                }
                return logs;
            }
            else
            {
                while (a < num_of_logs)
                {

                    try
                    {
                        cLog = logsList[b - a];
                        cLog = cLog.Split(' ')[1] + " " + cLog.Split(' ')[2] + " " + cLog.Split(' ')[3] + " " + cLog.Split(' ')[4] + "  FX:" + cLog.Split(' ')[5].Split(':')[1]; // + "@"+ cLog.Split(' ')[4].Split(':')[1];

                        logs.Add(cLog);
                    }
                    catch
                    {
                        logs.Add("");
                    }


                    a++;
                }
                if (num_of_logs == -1) //<<<<<<<<<<<<<<<<<
                {
  
                }

                return logs;
            }
            
        }
        public List<string> _idScrapsList()
        {
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            List<string> listLogs = new List<string>();
            DateTime dt = DateTime.Now;
            listLogs = FilterLogList(dt.AddDays(-30), dt);
            list.AddRange(listLogs);
            list2.AddRange(listLogs);
            int counter = 0;
            foreach(string l in list.ToList())
            {
                list[counter] = l.Split(' ')[4].Split(':')[1];
                counter++;
            }
            list = list.Distinct().ToList();
            int counter2 = 0;
            int skipV = 0;
            bool skip = false;
            foreach(string l in list.ToList())
            {
                if (l.Split(VarDash)[0] == "ENTRADA.NF")
                {
                    skipV = counter2;
                    skip = true;
                    counter2++;
                    continue;
                }
                list[counter2] = list[counter2] + VarDash.ToString() + "0";
                counter2++;
            }
            if(skip)
                list.RemoveAt(skipV);
            foreach(string l in list2) //all logs
            {
                int qtd = 0;
                string cn = l.Split(' ')[0].Split(':')[1];
                string id = l.Split(' ')[4].Split(':')[1];
                try { qtd = Convert.ToInt32(l.Split(' ')[3].Split(':')[1]); }
                catch { qtd = 0; }
                int counter3 = 0;
                foreach(string l2 in list.ToList()) //id list
                {
                    if(id == l2.Split(VarDash)[0])
                    {
                        int qtdS = Convert.ToInt32(list[counter3].Split(VarDash)[1]);
                        int novaQTD = qtd + qtdS;
                        list[counter3] = id + VarDash.ToString() + novaQTD.ToString();
                        break;
                    }
                    counter3++;
                }

            }
            return list;
        }
        private int cv(string number)
        {
            string to_convert = number;
            if (number.Contains(VarDash))
            {
                to_convert = number.Split(VarDash)[1];
            }
            try
            {
                return Convert.ToInt32(to_convert);
            }
            catch { return 0; }
        }
        public List<string> CXlist()
        {
            return AdressesList;
        }
        public string editCX(string CX, int qtd,string operation = "") //retorna a mensagem de erro
        {
            ListClass lc = new ListClass();
            
            lc.mainList = AdressesList;
            int qtdAtual = 0;
            string _qtd = "";
            switch (operation)
            {
                case "+":
                    if (qtd > QTD("bin"))
                    {
                        return "Valor insulficiente no bin";
                    }
                    qtdAtual = cv(lc.streamPlus(CX, "QTD"));
                    qtdAtual = qtd + qtdAtual; 
                    lc.streamPlus(CX, "QTD", qtdAtual.ToString(), true);
                    if (qtdAtual <= 0)
                    {
                        int c = 0;
                        foreach(string l in lc.mainList.ToList())
                        {
                            if (l.StartsWith("CN" + VarDash + CX + VarDashPlus))
                            {
                                lc.mainList.RemoveAt(c);
                                break;
                            }
                            c++;
                        }
                    }
                    break;

                case "-":
                    qtdAtual = cv(lc.streamPlus(CX, "QTD"));
                    if (qtdAtual < qtd)
                    {
                        return "Quantidade indisponível na Caixa";
                    } 
                    qtdAtual = qtdAtual - qtd;
                    lc.streamPlus(CX, "QTD", qtdAtual.ToString(), true);
                    if (qtdAtual <= 0)
                    {
                        int c = 0;
                        foreach (string l in lc.mainList.ToList())
                        {
                            if (l.StartsWith("CN" + VarDash + CX + VarDashPlus))
                            {
                                lc.mainList.RemoveAt(c);
                                break;
                            }
                            c++;
                        }
                    }
                    break;
                case "":
                    int qtdNaCaixa = 0;
                    int c2 = 0;
                    foreach (string l in lc.mainList.ToList())
                    {
                        if (l.StartsWith("CN" + VarDash + CX + VarDashPlus))
                        {
                            qtdNaCaixa = cv(l.Split(VarDashPlus)[1]);
                            break;
                        }
                        c2++;
                    }
                    if (qtd > QTD("bin") + qtdNaCaixa)
                    {
                        return "Valor insulficiente no bin";
                    }
                    if (qtd <= 0)
                    {
                        int c = 0;
                        foreach (string l in lc.mainList.ToList())
                        {
                            if (l.StartsWith("CN" + VarDash + CX + VarDashPlus))
                            {
                                lc.mainList.RemoveAt(c);
                                break;
                            }
                            c++;
                        }
                        //lc.streamPlus(CX, "QTD", 0.ToString(), true);
                    }
                    else
                    {
                        lc.streamPlus(CX, "QTD", qtd.ToString(), true);
                    }
                    
                    break;
            }
            AdressesList = lc.mainList;
            if(QTD("get") < sumCX())
            {
                QTD("set", sumCX());
            }
            return "";
            //ler e editar qtd abc
        }
 
        public List<string> filterLogsFromLibrary(DateTime start_, DateTime end_, bool wholeDay = false)
        {
            DateTime start = start_;
            DateTime end = end_;
            if (wholeDay)
            {
                TimeSpan ts = new TimeSpan(0, 0, 0);
                TimeSpan ts2 = new TimeSpan(24, 0, 0);
                start = start_.Date + ts;
                end = end_.Date + ts2;
            }


            List<string> logs = new List<string>();
            List<string> listNames = new List<string>();
            string NameStart = start.Year.ToString() + "-" + start.Month.ToString();
            string NameEnd = end.Year.ToString() + '-' + end.Month.ToString();
            if(NameStart != NameEnd)
            {
                string currentName = NameStart;
                listNames.Add(currentName);
                while(currentName!= NameEnd)
                {
                    string _year = currentName.Split('-')[0];
                    string _month = currentName.Split('-')[1];
                    int year = Convert.ToInt32(_year);
                    int month = Convert.ToInt32(_month);
                    if(month == 12)
                    {
                        year++;
                        month = 1;
                    }
                    else
                    {
                        month++;
                    }
                    currentName = year.ToString() + "-" + month.ToString();
                    listNames.Add(currentName);
                }

            }
            else//only 1 month list
            {
                listNames.Add(NameEnd);
            }
            int counter = 0;
            int total = listNames.Count;
            foreach(string ln in listNames)
            {
                ListClass lc2 = new ListClass();
                Folders folders2 = new Folders();
                lc2.Open(ln, folders2.logLibrary);
                if(listNames.Count > 2 && counter != 0 && counter != (total-1))
                {
                    logs.AddRange(lc2.mainList);
                }
                else
                {
                    foreach (string l in lc2.mainList.ToList())
                    {
                        DateTime currentDate = logtoDatetime(l);
                        if (currentDate >= start && currentDate <= end)
                        {
                            logs.Add(l);
                        }
                    }
                }
               counter++;
            }


           


            return logs;

        }

            private void CheckTXT_exists(string path)
        {
            MagnusSpace.Folders folder = new MagnusSpace.Folders();
            folder.createTPFolder(ItemCN);
            
            if (!File.Exists(path))
            {
                try
                {
                    StreamWriter myfile = File.CreateText(path);
                    myfile.Close();
                }
                catch (System.IO.DirectoryNotFoundException)
                {
                    
                }

            }
        }
        //ebook
        private List<string> readList(string path)
        {
            if(itemExists == false)
            {
                return null;
            }
            //CheckTXT_exists(path);
            if (path == "mainList")//itemInfoPath)
            {
                if (!itemInfoupdated)
                {

                    //itemInfoList = File.ReadAllLines(itemInfoPath).ToList();
                    ListClass lc = new ListClass();
                    lc.Open("Mestra","ListaGeral");
                    foreach(string l in lc.mainList.ToList())
                    {
                        List<string> subList = l.Split(VarDashPlus).ToList();
                        foreach(string l2 in subList)
                        {
                            if (l2.Split(VarDash)[0] == "CN" && l2.Split(VarDash)[1] == openCN)
                            {
                                itemInfoList = subList;
                            }
                        }
                    }
                    //itemInfoList = File.ReadAllLines(itemInfoPath).ToList();
                    itemInfoupdated = true;
                }
                return itemInfoList;
            }
            if (path == log_path)
            {
                if (!logsListUpdated)
                {
                    logsList = File.ReadAllLines(log_path).ToList();
                    logsListUpdated = true;
                }
                return logsList;
            }
            if (path == set_log_path)
            {
                if (!setlogsListUpdated)
                {
                    SetlogsList = File.ReadAllLines(set_log_path).ToList();
                    setlogsListUpdated = true;
                }
                return SetlogsList;
            }
            if (path == adressesListPath)
            {
                if (!adressesListUpdated)
                {
                    AdressesList = File.ReadAllLines(adressesListPath).ToList();
                    adressesListUpdated = true;

                }
                updateBinQTD();
                return AdressesList;
            }

            return null;


        }
        public void updateBinQTD()
        {
            
           // qtdBin;


        }
        private void writeList(string path, List<string> _list)
        {
            CheckTXT_exists(path);
            if (path == itemInfoPath)
            {
                itemInfoList = _list;
                itemInfoupdated = true;
            }
            if (path == log_path)
            {
                logsList = _list;
                logsListUpdated = true;
            }

        }
        public List<string> cnList()
        {
            List<string> cns = new List<string>();
            Folders folder = new Folders();
            string[] CNarray = Directory.GetDirectories(folder.TPFolder);
            foreach(string a in CNarray)
            {
 
                if (File.Exists(folder.itemInfoPath(a.Split('\\').Last())))
                {
                    cns.Add(a.Split('\\').Last());
                }
            }

            return cns;
            
        }
      public string list_tostring(List<string> TP = null)
        {
            string combinedString = ""; 
            if(TP == null)
            {
                combinedString = string.Join(VarDashPlus.ToString(), itemInfoList.ToArray());
            }
            else
            {
                combinedString = string.Join(VarDashPlus.ToString(), TP.ToArray());
            }
            
            return combinedString;
        }
       
        public void Close()
        {

            
            //vaca
            itemInfoupdated = true; //burlar o sistema
            if (itemInfoupdated)
            {


                string combinedString = list_tostring();
                ListClass lc = new ListClass();
                lc.Open("Mestra", "ListaGeral");

                int a = 0;
                foreach (string l in lc.mainList.ToList())
                {
                    List<string> subList = l.Split(VarDashPlus).ToList();
                    foreach (string l2 in subList)
                    {
                        if (l2.Split(VarDash)[0] == "CN" && l2.Split(VarDash)[1] == ItemCN)
                        {
                            lc.mainList[a] = combinedString;

                            lc.Close();
                            break;
                        }
                    }
                    a++;
                }



                //MessageBox.Show(itemInfoList[0]);
                itemInfoupdated = false;
            }
            if (logsListUpdated)
            {
                try
                {
                    File.WriteAllLines(log_path, logsList);
                }
                catch
                {
                    Directory.CreateDirectory(log_path);
                    File.WriteAllLines(log_path, logsList);
                }


                logsListUpdated = false;
            }
            if (setlogsListUpdated)
            {

                try
                {
                    File.WriteAllLines(set_log_path, SetlogsList);
                }
                catch
                {
                    Directory.CreateDirectory(set_log_path);
                    File.WriteAllLines(set_log_path, SetlogsList);
                }

                setlogsListUpdated = false;





            }

            if (adressesListUpdated)
            {

                try
                {
                    File.WriteAllLines(adressesListPath, AdressesList);
                }
                catch
                {
                    Directory.CreateDirectory(adressesListPath);
                    File.WriteAllLines(adressesListPath, AdressesList);
                }

                adressesListUpdated = false;





            }

        }

    }


}
