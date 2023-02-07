using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace MagnusSpace
{
    class Item
    {
        StringComparison comp = StringComparison.OrdinalIgnoreCase;
        private string itemInfoPath = "";
        private string log_path = "";
        private string set_log_path = "";
        public List<string> itemInfoList = new List<string>();
        bool itemInfoupdated = false;
        public List<string> logsList;
        public List<string> SetlogsList;
        public List<string> groupList;
        public List<string> modelList;
        public List<string> CNList;
        bool logsListUpdated = false;
        bool setlogsListUpdated = false;
        public string ItemCN = "";
        public string ID_OP = "";
        public string Item_description = "";
        public string fixtureLoc = "";
        public string ItemPN = "";
        public string ItemName = "";
        public string ItemModelo = "";
        public string qtdInicial = "";
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
            //Console.WriteLine("OPEN " + CN);
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
            CheckTXT_exists(log_path);
            CheckTXT_exists(set_log_path);
            try { readList("mainList"); } //update list}
            catch {  }
            try  { readList(log_path); } // both lists
            catch {  }
            try { readList(set_log_path); } // both lists
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
                        linha_do_tem += lc2.streamSEARCH(itemInfoList_doItem, "DATA_PROX") + dash;
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
        public void setStatus(string status)
        {
            Status = stream(Status, status);



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
        public void addID_inLog(string text,string action = "ID",int logNum = 1)
        {
            Console.WriteLine("addID_inLog");
            if (!itemExists)
                return;
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
                    if (l1.Split(':')[0] == action)
                    {
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

            logsList[totalC - logNum] = Clog;
            writeList(log_path, logsList);


        }
        public void createItem(string itemName, string PN = "", int qtd = 0, int EsM = 0, string description = "", string CN_ = "", List<string> SKUs = null,string Model1="",string qtdINI = "0", string NPIDate = "", string groupItem = "", string picture1 = "",string validacao1 = "")
        {///// FATA CADASTRAR OS ITENS DIREITO MUDAR O CADASTRO
            Console.WriteLine("createItem");
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
                Console.WriteLine("No CN");
                itemInfoList = TP;
                itemInfoupdated = true;
                //writeList(itemInfoPath, TP);
                //c("no cn");
              
            }
            else //novo CN
            {
            
                Console.WriteLine("find_line_in_mainCNList Edit");
                int CNline = find_line_in_mainCNList(CN_);
                if(CNline == -1)//CN não existente, precisa criar um novo
                {
                    Console.WriteLine("CN = -1");
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
        public string cycleLifeM()
        {
            int scraps = 0;
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Today;
            List<string> logList = new List<string>();
            if(PPdateStart == ""||PPdateEnd=="" || PPinRange == "")
            {
                setStatus(statas[6]);
                return "";
            }
            
            try
            {
                start = stringtoDatetime(PPdateStart);
                end = stringtoDatetime(PPdateEnd);
                Console.WriteLine("===============");
                Console.WriteLine(start.ToString());
                Console.WriteLine(end.ToString());
                Console.WriteLine(PPinRange.ToString());

                logList = FilterLogList(start, end,"");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Cant read: " + ex.Message);
            }
            foreach(string l in logList)
            {
                try
                {
                    scraps += Convert.ToInt32(l.Split(' ')[3].Split(':')[1]);
                }
                catch { }

                
            }
            //calcular o cycle life
            int days = 0;
            days = Convert.ToInt32((end - start).TotalDays);
            float cycle = 0;
            try { cycle = (float)Convert.ToInt32(PPinRange) / (float)(scraps); }
            catch { }
            setVar("CycleLife", cycle);

            int qtd = Convert.ToInt32(QTD("get"));
            int daysToend = 0;
            daysToend = qtd * days;
            int statasIndex = 0;
            if (scraps == 0) 
            {
                setStatus(statas[statasIndex]);
                return cycle.ToString("#.##");
            }
            daysToend = daysToend / scraps;
            duraçãoEstoque = daysToend.ToString();
            stream("stockLastsFor", daysToend.ToString());
            Console.WriteLine("ANALYSE: " + qtd + " days " + days + "scraps " + scraps);

            //"Trivial", "Moderado", "Alerta", "Crítico", "Extremamente crítico", "Zerado", "Sem Controle" };
            statasIndex = 4;
            if (daysToend < 30)
            {
                statasIndex = 4;
            }
            else
            {
                if (daysToend < 80)
                {
                    statasIndex = 3;
                }
                else
                {
                    if (daysToend < 120)
                    {
                        statasIndex = 2;
                    }
                    else
                    {
                        if (daysToend < 150)
                        {
                            statasIndex = 1;
                        }
                        else
                        {
                            statasIndex = 0;
                        }
                    }
                }
            }
            
          

            setStatus(statas[statasIndex]);

            Console.WriteLine("Duração: "+duraçãoEstoque + " index " + statasIndex.ToString());
            return cycle.ToString("#.##");
        }
        public int getScrapSemanal()
        {
            List<string> logList = new List<string>();
            logList = FilterLogList(DateTime.Now, DateTime.Now, "semanal");
            int scrap = 0; 
            foreach(string l in logList.ToList())
            {
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
                    logList = FilterLogList(DateTime.Now, DateTime.Now);
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




        public List<string> FilterLogList(DateTime start1, DateTime end1, string frequencia = "diario")
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

            if (logsList.Count() == 0)
            {
                return scrapList;
            }
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
                Console.WriteLine("<<<" + line);
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
        double addLog_QTD = 0;
        public double QTD(string action, double value = 0)
        {
            //Console.WriteLine("action " +action.ToString() + "      "+ value.ToString());
            //viewListtoConsole("QTD", itemInfoList);
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
    
                        qtd += value;
                        addLog_QTD = qtd;
                        QuantidadeNova = Convert.ToInt32(qtd);
                        if (QuantidadeNova < Eminimo && QuantidadeAntiga >= Eminimo)
                        {
                            MissingDesatualizado = true;
                        }

                        AddLog(value);
                        if (qtd < 0)
                        {
                            //itemInfoList[3] = itemInfoList[3].Split(':')[0] + ":" + "0";
                            itemInfoList = lc2.stream_SET(itemInfoList,"QTD","0");
                            return 0;
                        }
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
                            foreach(string l in glist.ToList())
                            {
                                if(l.Split(VarDash)[0] ==  ItemCN && l.Split(VarDash)[1] != "IN")
                                {
                                    itemInfoList = lc5.stream_SET(itemInfoList, "groupPosition", "IN");
                                    glist[a] = ItemCN + VarDash.ToString() + "IN";
                                    IN_group++;
                                    position = "IN";
                                    AddLog(1);
                                    a = 0;
                                    itemPresentinGroup = true;
                                    break;
                                }
                                a++;
                            }
                            //abrir lista do grupo e  colocar ele como entrada


                        }
                        else
                        {
                            foreach (string l in glist.ToList())
                            {
                                if (l.Split(VarDash)[0] == ItemCN && l.Split(VarDash)[1] != "OUT")
                                {
                                    itemInfoList = lc5.stream_SET(itemInfoList, "groupPosition", "OUT");
                                    glist[a] = ItemCN + VarDash.ToString() + "OUT";
                                    
                                    IN_group--;
                                    position = "OUT";
                                    AddLog(-1);
                                    a = 0;
                                    itemPresentinGroup = false;
                                    if (ItemName.Contains("fixture", comp))
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
                                    break;
                                }
                                a++;
                            }
                        }
      

                        if (ItemName.Contains("fixture", comp))
                        {
                            string logpathAdress = "";
                            List<string> settigns = new List<string>();
                            ListClass lc3 = new ListClass();
                            lc3.Open("Settings", "settings");
                            logpathAdress = lc3.stream("FixtureOUTlogPath");
                            if (logpathAdress == "")
                            {
                                Folders folder = new Folders();

                                logpathAdress = lc3.stream("FixtureOUTlogPath", folder.fixtureOutLOGS.ToString());
                                lc3.Close();
                            }
                            try
                            {
                      
                                Console.WriteLine("path " + logpathAdress);
                                ListClass lc4 = new ListClass();
             
                                lc4.Open(ItemCN, Path.GetFullPath(logpathAdress));
                                lc4.mainList.Clear();
                                lc4.mainList.Add(position);
                                lc4.mainList.Add(DateTime.Now.ToString());
                                //lc4.Open(ItemCN + " " + DateTime.Now.ToString(), Path.GetFullPath(logpathAdress) );


                                //newDate = String.

                                lc4.Close();
                                Console.WriteLine("close sucessfuly");

                            }
                            catch (Exception ex) { Console.WriteLine("catch at OUT: " + ex.Message); }
                        }
                            lc.Close();
                        return 1;
                    }
                    
                    break;
                case "addNew":
                    qtd += value;
                    QuantidadeNova = Convert.ToInt32(qtd);
                    if (QuantidadeNova < Eminimo && QuantidadeAntiga >= Eminimo)
                    {
                        MissingDesatualizado = true;
                    }

                    AddLog(value,"new");
                    if (qtd < 0)
                    {
                        itemInfoList = lc2.stream_SET(itemInfoList, "QTD", "0");
                        //itemInfoList[3] = itemInfoList[3].Split(':')[0] + ":" + "0";
                        return 0;
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
            //viewListtoConsole(action.ToString(), itemInfoList);
            //itemInfoList[3] = itemInfoList[3].Split(':')[0] + ":" + qtd.ToString();
            return qtd;
        }

        public void AddLog(double quantidade = -1, string action = "none")
        {
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
            logsList.Add(logFormat);
        }
        public int numberScraps(string time = "day") // revisar!!! esta deixando lento
        {
      
            int value = 0;
            List<string> logs = new List<string>();
            logs = getLogList(0,"","",time);
            foreach (string l in logs)
            {
                
              
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

            public List<string> getLogList(int num_of_logs = 10,string dateStart = "",string dateEnd = "",string dayWeekMonth = "")
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

            return null;


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
        public void viewListtoConsole(string text, List<string> ls)
        {
            DateTime t = DateTime.Now;
            Console.WriteLine("==================="+ t.ToString() + "======================");
            foreach (string l4 in ls)
            {
                Console.WriteLine(text+ "      " + l4);
            }
            Console.WriteLine("============================================================");
        }
        public void Close()
        {

           // Console.WriteLine("close " + ItemCN);
            //vaca
            itemInfoupdated = true; //burlar o sistema
            if (itemInfoupdated)
            {

               //viewListtoConsole("LIST AT CLOSE", itemInfoList); //VISUALIZAR LISTA NO CONSOLE

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


        }

    }


}
