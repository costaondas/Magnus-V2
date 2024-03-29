﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
//using System.Threading.Tasks;
using System.IO;
using System.Collections;
using TurnParts;

namespace MagnusSpace
{

    class Folders
    {
        string dash = ((char)886).ToString();
        char VarDash = ((char)887);
        char VarDashPlus = ((char)888);

        public static string dataFolder = @"C:\MagnusControl";
        public static string imagePath = @"C:\MagnusImages";
        public static string backUpFolder = @"C:\MagnusBackup";
        //T:\Turn_Parts\Magnus\Control
        //public static string dataFolder = @"T:\Turn_Parts\Magnus\Control";
        //public string imagePath = dataFolder + @"\" + "Images";
        public string printPath = dataFolder + @"\" + "Print";
        public string skuPath = dataFolder + @"\" + "SKU";
        public string settingsPath = dataFolder + @"\" + "Config";
        public string TasksFolderPath = dataFolder + @"\" + "Tasks";
        public string GrupoFolderPath = dataFolder + @"\" + "Grupos";
        public string versoesFX = dataFolder + @"\" + "VersoesFX";
        public string logFolder = dataFolder + @"\" + "logFolder";
        public string Labels = dataFolder + @"\" + "Labels";
        public string logLibrary = dataFolder + @"\" + "logLibrary";
        public string Forecast = dataFolder + @"\" + "Forecast";
        public string Chart = dataFolder + @"\" + "Chart.png";
        public string Scraps = dataFolder + @"\" + "Scraps";
        public string ListaGeralFolderPath = dataFolder + @"\" + "Other Lists";
        public static string Logs = dataFolder + @"\" + "Logs";
        //public static string LogsManutencao = dataFolder + @"\" + "Logs";
        //public static string backUpFolder = dataFolder + @"\" + "Backup";
        public string fixtureOutLOGS = Logs + @"\" + "FixtureOUT";
        public string ManutenfixtureOutLOGS = Logs + @"\" + "ManutençãoLogs";

        public string planilhaPath = dataFolder + @"\" + "Planilha";
        public string genericPath = dataFolder + @"\" + "Listas Genericas";
        public string mainListEXPath = dataFolder + @"\" + "Lista Geral";
        public string TPFolder = dataFolder + @"\" + "TurnPartsList";
        private string itemInfo = "tpInfo.txt";
        private string setLog = "setLog.txt";
        private string adresses = "adresses.txt";
        private string itemLog = "tpLog.txt";
        private string addItensPla = "AddItens.xls";
        private string fxpositions = "FXpositions.xls";
        private string forcastPla = "Forcast.xls";
        private string fixturesFileName = "Fixtures.txt";
        private string planilhaLogs = "logs.xls";
        public string listaGeralName = "Lista Geral";
        public string listaComprasName = "Lista Compras";
        public string fixtureFolder = dataFolder + @"\" + "Fixtures";
        public List<string> listaGeral1 = new List<string>();
        public string lastImagePath = "";
        public string fulllastImagePath = "";
        private void c(string c)
        {
    
        }
        public string ReturnBackUpFolder()
        {
            return backUpFolder;
        }
        public void buildStructure() 
        {
            build(dataFolder);
            build(planilhaPath);
            build(Logs);
            build(skuPath);
            build(ManutenfixtureOutLOGS);
            build(logFolder);
            build(Labels);
            build(logLibrary);
            build(versoesFX);
            build(Forecast);
            build(Scraps); 
            build(printPath);
            build(fixtureOutLOGS);
            build(ListaGeralFolderPath);
            build(GrupoFolderPath);
            build(settingsPath);
            build(TasksFolderPath);
            build(genericPath);
            build(imagePath);
            build(TPFolder);
            build(mainListEXPath);
            build(fixtureFolder);
            build(backUpFolder);
            build(getFixturePath());
            build(skuPath);

        }
        public void newItemList(String CN)
        {
            string tpInfo = itemInfoPath(CN);
            if (File.Exists(tpInfo))
            {
                File.Delete(tpInfo);
               
            }
            StreamWriter sw = File.CreateText(tpInfo);
            sw.Close();

        }

        public void createTPFolder(string CN)
        {
            string CNpath = itemFolder(CN);
            buildStructure(); //adress exists
            if (!Directory.Exists(CNpath))
            {
                try { Directory.CreateDirectory(CNpath); }
                catch { }
            }
            //string tpInfo = itemInfoPath(CN);
            string tpLog = itemLogPath(CN);
            /*if (!File.Exists(tpInfo))
            {
                StreamWriter sw = File.CreateText(tpInfo);
                sw.Close();
            }*/
            if (!File.Exists(tpLog))
            {
                StreamWriter sw = File.CreateText(tpLog);
                sw.Close();
            }

        }
        public void deleteTPFolder(string CN) 
        {
            string CNpath = itemFolder(CN);
            if(Directory.Exists(CNpath))
                DeleteDirectory(CNpath);
        }
        private void DeleteDirectory(string target_dir) 
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
        public bool build(string Direc)
        {
            if (!Directory.Exists(Direc))
            {
                try { Directory.CreateDirectory(Direc); }
                catch { }
            }
            return Directory.Exists(Direc);
        }
       
        public List<string> allItensInModelList(string model)
        {
            List<string> l = Directory.GetDirectories(TPFolder).ToList(); // nothing returning
            List<string> cnList = new List<string>();
            List<string> excelList = new List<string>();
            Item item = new Item();
            int num = 0;
            string currentCN = "";
            string linhaExcel = "";


            foreach (string a in l)
            {
                linhaExcel = "";
                num = a.Split('\\').Count();
                currentCN = a.Split('\\')[num - 1];
                cnList.Add(currentCN);
                item.Open(currentCN);
                if (!item.itemExists)
                    break;
                

                if (item.ItemModelo != model)
                {
                    item.Close();
                    continue;
                }
                linhaExcel += item.ItemCN + dash;
                linhaExcel += item.ItemName + dash;
                linhaExcel += item.QTD("get").ToString() + dash;
                linhaExcel += item.Status + dash;
                item.Close();
                excelList.Add(linhaExcel);
                linhaExcel = "";
            }
            return excelList;
        }
        public static DateTime time1 = DateTime.Now;
        public static DateTime time2 = DateTime.Now;
        public static void time(string text)
        {
            int a = 0;
            time2 = time1;
            time1 = DateTime.Now;
            TimeSpan ts = time1 - time2;
            a = Convert.ToInt32(ts.TotalMilliseconds);
            string howlong = a.ToString();

        }
        public string CNline(List<string> subList, string action = "")
        {


            ListClass lc = new ListClass();
            string linhaExcel = "";


            linhaExcel += lc.streamSEARCH(subList, "CN") + vd();//0
            linhaExcel += lc.streamSEARCH(subList, "Modelo") + vd();//1
            linhaExcel += lc.streamSEARCH(subList, "Name") + vd();//2
            linhaExcel += lc.streamSEARCH(subList, "P/N") + vd();//3
            if (action == "reduzido")
            {
                return linhaExcel;
            }
            linhaExcel += lc.streamSEARCH(subList, "Descrição") + vd();//4
            linhaExcel += lc.streamSEARCH(subList, "QTD") + vd();//5
            linhaExcel += lc.streamSEARCH(subList, "EstoqueM") + vd();//6
            linhaExcel += lc.streamSEARCH(subList, "Status") + vd();//7
            linhaExcel += lc.streamSEARCH(subList, "Estoque Inicial") + vd();//8
            linhaExcel += lc.streamSEARCH(subList, "QTDPFP") + vd();//9
            linhaExcel += lc.streamSEARCH(subList, "CycleLife") + vd();//10
            linhaExcel += lc.streamSEARCH(subList, "groupPosition") + vd();///11
            int scraps = 0;
            try
            {
                scraps += Convert.ToInt32(lc.streamSEARCH(subList, "trocas1"));
                scraps += Convert.ToInt32(lc.streamSEARCH(subList, "trocas2"));
                scraps += Convert.ToInt32(lc.streamSEARCH(subList, "trocas3"));
            }
            catch
            {
                scraps = 0;
            }
            linhaExcel += scraps.ToString() + vd();
            linhaExcel += lc.streamSEARCH(subList, "location3") + vd();//12
            linhaExcel += lc.streamSEARCH(subList, "location2") + vd();//13
            linhaExcel += lc.streamSEARCH(subList, "location") + vd();//14
            linhaExcel += lc.streamSEARCH(subList, "grupo") + vd();//15


            return linhaExcel;
        }
        public string PBline(List<string> subList, string action = "")
        {


            ListClass lc = new ListClass();
            string linhaExcel = "";


            linhaExcel += lc.streamSEARCH(subList, "CN") + vd();//0
            linhaExcel += lc.streamSEARCH(subList, "Modelo") + vd();//1
            linhaExcel += lc.streamSEARCH(subList, "Name") + vd();//2
            linhaExcel += lc.streamSEARCH(subList, "P/N") + vd();//3
            if (action == "reduzido")
            {
                return linhaExcel;
            }
            linhaExcel += lc.streamSEARCH(subList, "Descrição") + vd();//4
            linhaExcel += lc.streamSEARCH(subList, "QTD") + vd();//5
            linhaExcel += lc.streamSEARCH(subList, "CycleLife") + vd();//6
                                                                       // linhaExcel += lc.streamSEARCH(subList, "EstoqueM") + vd();//6

            int scraps = 0;
            try
            {
                scraps += Convert.ToInt32(lc.streamSEARCH(subList, "trocas1"));
                scraps += Convert.ToInt32(lc.streamSEARCH(subList, "trocas2"));
                scraps += Convert.ToInt32(lc.streamSEARCH(subList, "trocas3"));
            }
            catch
            {
                scraps = 0;
            }
            linhaExcel += scraps.ToString() + vd();//7 // scrap 3 meses
            linhaExcel += lc.streamSEARCH(subList, "BA") + vd();//8 // busisness award
            linhaExcel += lc.streamSEARCH(subList, "ForcastGeral") + vd();//9 
            linhaExcel += lc.streamSEARCH(subList, "PlacasProd") + vd();//10
            linhaExcel += lc.streamSEARCH(subList, "Forcast") + vd();//11
            linhaExcel += lc.streamSEARCH(subList, "Previsao") + vd();//12
            linhaExcel += lc.streamSEARCH(subList, "Status") + vd();//13
            //PlacasProd
            int a = 0;
            DateTime dt = DateTime.Now;
            for (a = 0; a < 12; a++)
            {
                linhaExcel += "" + vd();//13
                // linhaExcel += dt.ToString("MMMM") + vd();//13
                // dt.AddMonths(1);
            }
            linhaExcel += lc.streamSEARCH(subList, "QTDcompra") + vd();//26
            linhaExcel += lc.streamSEARCH(subList, "SC_evetuada") + vd();//10
            linhaExcel += lc.streamSEARCH(subList, "QTD_SC") + vd();///11
            linhaExcel += lc.streamSEARCH(subList, "COUNT_CRIT") + vd();//12
            return linhaExcel;
        }
        public bool listmode(string mode)
        {
            ListClass lc = new ListClass();
            
            List<string> title = new List<string>();
            switch (mode)
            {
                case "padrão":
                    title = lc.RowTitle(0);
                    break;
                case "compras":
                    title = lc.RowTitle(1);
                    break;
                case "QTD":
                    title = lc.RowTitle(2);
                    break;
                case "location":

                    title = lc.RowTitle(3);
                    break;
                case "simples":
                    title = lc.RowTitle(4);
                    break;
                case "grupos":

                    title = lc.RowTitle(5);
                    break;
                case "Bin":

                    title = lc.RowTitle(17);
                    
                    break;
                case "BinScrap":
                    title = lc.RowTitle(11);
                    break;
            }
            return buildPlanilhaGeral(title);
        }
        public bool buildPlanilhaGeral(List<string> title)
        {
            ExcelClass ec = new ExcelClass();
            List<string> list = new List<string>();
            


            ListClass lc = new ListClass();
            lc.Open("Mestra");
            List<string> list2 = new List<string>();
            list2.Add("Filtrando lista");
            lc.hasBar(list2);
            list = lc.toVardashFormat(lc.mainList,title,true);
            bool resolt = false;
            resolt = ec.gerarPlanilhaGeral(list);
            return resolt;
            
        }
        public bool listaGeral(string action = "none")
        {
            time("lista geral start");
            //List<string> l = Directory.GetDirectories(TPFolder).ToList(); // nothing returning
            List<string> cnList = new List<string>();
            List<string> cnFounds = new List<string>();
            List<string> excelList = new List<string>();

            string linhaExcel = "";
            ListClass lc = new ListClass();
            lc.Open("Mestra", "ListaGeral");
            time("Open mestra");
            foreach (string l in lc.mainList.ToList()) // demora 28774
            {
                List<string> subList = l.Split(VarDashPlus).ToList();
                excelList.Add(CNline(subList,action));
            }
            time("foreach");


            if (action == "reduzido")
            {
                listaGeral1 = excelList;
                return true;
            }


            ExcelClass ec = new ExcelClass();
            
            excelList = ec.sortGeralList(excelList); // demora 47350 esse
            time("sort");
            listaGeral1 = excelList;
            if (action == "get")
            {
                return true;
            }



            linhaExcel += "CN" + vd();
            linhaExcel += "Modelo" + vd();
            linhaExcel += "Nome" + vd();
            linhaExcel += "P/N" + vd(); ;
            linhaExcel += "Descrição" + vd();
            linhaExcel += "Quantidade" + vd();
            linhaExcel += "Estoque Mínimo" + vd();
            linhaExcel += "Status" + vd();
            linhaExcel += "Quantidade Inicial" + vd(); ;
            linhaExcel += "Quantidade para finalizar projeto" + vd();
            linhaExcel += "Cycle Life" + vd(); 
            linhaExcel += "Posição" + vd(); 
            linhaExcel += "Scraps (3 meses)" + vd(); 
            linhaExcel += "Rua" + vd(); 
            linhaExcel += "Coluna" + vd(); 
            linhaExcel += "Linha";




            excelList.Insert(0,linhaExcel);
            time("insert line");
            bool resolt = false;
            resolt = ec.gerarPlanilhaGeral(excelList);
            time("gerar planilha");
            return resolt;
        }
        public bool listaPowerBI(string action = "none")
        {
            time("lista geral start");
            //List<string> l = Directory.GetDirectories(TPFolder).ToList(); // nothing returning
            List<string> cnList = new List<string>();
            List<string> cnFounds = new List<string>();
            List<string> excelList = new List<string>();

            string linhaExcel = "";
            ListClass lc = new ListClass();
            lc.Open("Mestra", "ListaGeral");
            time("Open mestra");
            foreach (string l in lc.mainList.ToList()) // demora 28774
            {
                List<string> subList = l.Split(VarDashPlus).ToList();
                excelList.Add(PBline(subList, action));
            }
            time("foreach");


            if (action == "reduzido")
            {
                listaGeral1 = excelList;
                return true;
            }


            ExcelClass ec = new ExcelClass();

            excelList = ec.sortGeralList(excelList); // demora 47350
            time("sort");
            listaGeral1 = excelList;
            if (action == "get")
            {
                return true;
            }



            linhaExcel += "CN" + vd();
            linhaExcel += "Modelo" + vd();
            linhaExcel += "Nome" + vd();
            linhaExcel += "P/N" + vd(); ;
            linhaExcel += "Descrição" + vd();
            linhaExcel += "Quantidade" + vd();
            linhaExcel += "Cycle Life" + vd();
            linhaExcel += "Scraps (3 meses)" + vd();
            linhaExcel += "BA" + vd(); ;
            linhaExcel += "FO" + vd();
            linhaExcel += "Produzido" + vd();
            linhaExcel += "FO RESTANTE" + vd();
            linhaExcel += "Previsão" + vd();
            linhaExcel += "Criticidade" + vd();

            int a = 0;
            DateTime dt = DateTime.Now;
            for (a = 0; a < 12; a++)
            {
               // linhaExcel += "" + vd();//13
                linhaExcel += dt.ToString("MMMM") + vd();//13
                dt.AddMonths(1);
            }


            linhaExcel += "Quantidade Necessaria" + vd();
            linhaExcel += "SC Efetuada" + vd();
            linhaExcel += "QTD SC" + vd();
            linhaExcel += "Count of Criticidade" + vd();




            excelList.Insert(0, linhaExcel);
            time("insert line");
            bool resolt = false;
            resolt = ec.gerarPlanilhaGeral(excelList);
            time("gerar planilha");
            return resolt;
        }
        public char vd()
        {
            return VarDash;
        }
        public void backup(string path)
        {
            if (!Directory.Exists(path))
            {
                try { Directory.CreateDirectory(path); }
                catch { }
            }
            /*
            ListClass lc = new ListClass();
            ListClass lc2 = new ListClass();
            lc.Open("Mestra", "ListaGeral");
            lc2.Open("Mestra", path);
            lc2.mainList = lc.mainList;
            lc2.Close();
            */
            string backupLogsFolder = path + @"\" + "MagnusControl";
            CopyFilesRecursively(dataFolder, backupLogsFolder);
        }
        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            Form1 form = new Form1();
            form = System.Windows.Forms.Application.OpenForms["Form1"] as Form1;
            
            //Now Create all of the directories
            string[] directories = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
            int total = directories.Count();
            int a = 1;
            form.setProgressiveBar(total);
            foreach (string dirPath in directories)
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                form.AddProgressiveBar($"Criando Diretorios {a}/{total}");
                a++;
            }

            //Copy all the files & Replaces any files with the same name
            string[] allFiles = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
            total = allFiles.Count();
            form.setProgressiveBar(total);
            a = 1;
            foreach (string newPath in allFiles)
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                form.AddProgressiveBar($"Copiando arquivos {a}/{total}");
                a++;
            }
            form.AddProgressiveBar("clear");
        }
        public bool listaCompras(string action = "none")
        {
            List<string> l = Directory.GetDirectories(TPFolder).ToList(); // nothing returning
            List<string> cnList = new List<string>();
            List<string> excelList = new List<string>();

            int num = 0;
            string currentCN = "";
            string linhaExcel = "";


            foreach (string a in l)
            {
                Item item = new Item();
                linhaExcel = "";
                num = a.Split('\\').Count();
                currentCN = a.Split('\\')[num - 1];
                cnList.Add(currentCN);
                item.Open(currentCN);
                if (!item.itemExists)
                    break;
                linhaExcel += item.ItemCN + vd();
                linhaExcel += item.ItemModelo + vd();
                linhaExcel += item.ItemName + vd();
                linhaExcel += item.ItemPN + vd();
                linhaExcel += item.QTD("get").ToString() + vd();
                linhaExcel += item.stream("ItensINLine") + vd();
                linhaExcel += item.qtdInicial + vd();
                int scraps = 0;
                try
                {
                    int qtdINI = 0;
                    int INline = 0;
                    if (item.qtdInicial != "")
                    {
                        qtdINI = Convert.ToInt32(item.qtdInicial);

                    }
                    else
                        continue;
                    if (item.stream("ItensINLine") != "")
                    {
                        INline = Convert.ToInt32(item.stream("ItensINLine"));
                    }
                    scraps = qtdINI - Convert.ToInt32(item.QTD("get")) - INline;
                }
                catch
                {

                }
                linhaExcel += item.stream("PlacasProd") + vd();
                linhaExcel += scraps.ToString() + vd();
                string rightDate = item.stream("DateUsed");
                try { rightDate = rightDate.Split('/')[1] + "/" + rightDate.Split('/')[0] + "/" + rightDate.Split('/')[2]; }
                catch { rightDate = ""; }
                linhaExcel += rightDate + vd();
                linhaExcel += item.cycleLife.ToString("#.##") + vd();
                linhaExcel += item.forcast.ToString() + vd();
                linhaExcel += item.stream("QTDFinalizar") + vd();
                linhaExcel += item.stream("QTDcompra");

                item.Close();
                excelList.Add(linhaExcel);
         
                linhaExcel = "";
            }
            ExcelClass ec = new ExcelClass();
            excelList = ec.sortGeralList(excelList);
            listaGeral1 = excelList;
            if (action == "get")
            {
                return true;
            }

            linhaExcel = "CN" + vd();
            linhaExcel += "Modelo" + vd();
            linhaExcel += "Nome" + vd();
            linhaExcel += "P\\N" + vd();
            linhaExcel += "Quantidade" + vd();
            linhaExcel += "Itens na Linha" + vd();
            linhaExcel += "Quantidade Inicial" + vd();
            linhaExcel += "Placas Produzidas" + vd();
            linhaExcel += "Scraps" + vd();
            linhaExcel += "Data do calculo" + vd();
            linhaExcel += "Cycle life" + vd();
            linhaExcel += "Forcast" + vd();
            linhaExcel += "Quantidade para fim de projeto" + vd();
            linhaExcel += "Quantidade para compra";

 
            excelList.Insert(0, linhaExcel);


            return ec.gerarPlanilhaGeral(excelList,"compras");
        }
        public bool listaForcast(string action = "none")
        {
            return false;
            List<string> excelList = new List<string>();
            ExcelClass ec = new ExcelClass();
            string line = "Modelo";
            DateTime a = new DateTime();

            
           // excelList.Add();
  


            return ec.gerarPlanilhaGeral(excelList, "forcast");
        }
        public System.Drawing.Image image(string codigo)
        {
            string a = "\\";
            StringBuilder sb = new StringBuilder(a);
            sb.Remove(0, 0);
            a = sb.ToString();
            try
            {
                lastImagePath = imagePath + a + codigo;// + ".jpg";
                fulllastImagePath = imagePath + a + codigo + ".jpg";
                return System.Drawing.Image.FromFile(imagePath + a + codigo + ".jpg");
            }
            catch
            {
            }
            try
            {
                lastImagePath = imagePath + a + codigo;// + ".png";
                fulllastImagePath = imagePath + a + codigo + ".png";
                return System.Drawing.Image.FromFile(imagePath + a + codigo + ".png");
            }
            catch
            {

            }
            try
            {
                lastImagePath = imagePath + a + codigo;// + ".jpeg";
                fulllastImagePath = imagePath + a + codigo + ".jpeg";
                return System.Drawing.Image.FromFile(imagePath + a + codigo + ".jpeg");
            }
            catch
            {

            }
            return null;
        }
        public string itemFolder(string CN)//pasta do CN especifico
        {
            string CNpath = TPFolder + @"\" + CN;
            return CNpath;
        }
        public string picFolder()
        {
            if (!Directory.Exists(imagePath))
            {
                try { Directory.CreateDirectory(imagePath); }
                catch { }
            }
            return imagePath;
        }
        public string AddItensPath()
        {
            if (!Directory.Exists(planilhaPath))
            {
                try { Directory.CreateDirectory(planilhaPath); }
                catch { }
            }
            return planilhaPath;
        }
        public string itemInfoPath(string CN)
        {
            string CNpath = TPFolder + @"\" + CN + @"\" + itemInfo;
            return CNpath;
        }
        public string itemSetLogPath(string CN)
        {
            string CNpath = TPFolder + @"\" + CN + @"\" + setLog;
            return CNpath;
        }
        public string itemAdressesPath(string CN)
        {
            string CNpath = TPFolder + @"\" + CN + @"\" + adresses;
            return CNpath;
        }
        //itemAdressesPath
        public string itemLogPath(string CN)
        {
            string CNpath = TPFolder + @"\" + CN + @"\" + itemLog;
            return CNpath;
        }
        public string planilhaLogsPath()
        {
            string CNpath = planilhaPath + planilhaLogs;
            return CNpath;
        }
        public string listaGeralPath()
        {
            string CNpath = mainListEXPath + @"\" +  listaGeralName+".xls" ;
            return CNpath;
        }
        public string listaForcastPath()
        {
            string CNpath = mainListEXPath + @"\" + forcastPla + ".xls";
            return CNpath;
        }
        public string listaComprasPath()
        {
            string CNpath = mainListEXPath + @"\" + listaComprasName + ".xls";
            return CNpath;
        }
        public string picturePath(string CN)
        {
            string CNpath = imagePath + @"\" + CN + ".jpge";
            if (File.Exists(CNpath))
            {
                return CNpath;
            }
            CNpath = imagePath + @"\" + CN + ".jpg";
            if (File.Exists(CNpath))
            {
                return CNpath;
            }
            CNpath = imagePath + @"\" + CN + ".png";
            if (File.Exists(CNpath))
            {
                return CNpath;
            }
            return "";
        }
        public string createItensPlanilhaPath()
        {
            string CNpath = planilhaPath + @"\" + addItensPla;
                return CNpath;
        }
        public string createFXpositionsPlanilhaPath()
        {
            string CNpath = planilhaPath + @"\" + fxpositions;
            return CNpath;
        }
        public string fxPositionPath()
        {
            string CNpath = planilhaPath + @"\" + fxpositions;
            return CNpath;
        }
        public string getFixturePath()
        {
            string CNpath = fixtureFolder + @"\" + fixturesFileName;
            return CNpath;
        }
        private void KillSpecificExcelFileProcess(string excelFileName)
        {
            var processes = from p in Process.GetProcessesByName("EXCEL")
                            select p;

            foreach (var process in processes)
            {
                
                if (process.MainWindowTitle.Contains(excelFileName))
                {
 
                    process.Kill();
                }
                    
                else
                {
                
                }
            }
        }
        public bool createPlanilha()
        {
            try
            {
                File.Delete(createItensPlanilhaPath());
            }
            catch { }
            List<string> l = new List<string>();
            string line = "CN" + vd();
            line += "Categoria" + vd();
            line += "Modelo" + vd();
            line += "Nome" + vd();
            line += "P/N" + vd();
            line += "Descrição" + vd();
            line += "Quantidade" + vd();
            line += "Estoque Minimo" + vd();
            line += "Quantidade Inicial" + vd();
            line += "Data NPI" + vd();
            line += "Grupo" + vd();
            line += "Imagem" + vd();
            line += "Validação" + vd();

            l.Add(line);
            ExcelClass ex = new ExcelClass();
            KillSpecificExcelFileProcess("AddItens.xls");
            KillSpecificExcelFileProcess("");
            bool value = ex.createFile(l, createItensPlanilhaPath());
            KillSpecificExcelFileProcess("AddItens.xls");
            KillSpecificExcelFileProcess("");
            return value;
            //File.WriteAllLines(getPlanilhaPath(), l);
        }

    }
}
