using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ExcelM = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

namespace MagnusSpace
{

    class ExcelClass
    {
        ExcelM.Application xlApp = new
        Microsoft.Office.Interop.Excel.Application();
        string extencion = ".xls";
        public string listAdress = "";
        char VarDash = ((char)887);
        public char vd()
        {
            return VarDash;
        }
        public bool closeExcel() //true proceed
        {
            bool kill = false;
            string message = "Esta ação fechará todas as planilhas abertas em Excel, deseja continuar?";
            string title = "Planilha aberta";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            if (Process.GetProcessesByName("excel").Count() > 0)
            {
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                switch (result)
                {
                    case DialogResult.Yes:
                        kill = true;
                        break;
                    case DialogResult.No:
                        return false;
                        break;
                }
            }
            if (kill)
            {
                foreach (var process in Process.GetProcessesByName("excel"))
                {
                    process.Kill();
                }
                return true;
            }
            return true;
        }
        public bool createFile(List<string> exList,string Path)
        {
            bool Value = false;
            string adress = Path;
            ExcelM.Application excelAplication = new Microsoft.Office.Interop.Excel.Application();
            excelAplication.DisplayAlerts = false;

            if (excelAplication == null)
            {
                return false;
            }
            ExcelM.Workbook book;
            ExcelM.Worksheet sheet;
            object misValue = System.Reflection.Missing.Value;

            book = excelAplication.Workbooks.Add(misValue);
            sheet = (ExcelM.Worksheet)book.Worksheets.get_Item(1);
            
            int lineNumber = 1;
            int cellNumber = 1;
            string value = "";
            List<string> lineCells = new List<string>();
            foreach(string line in exList)
            {
                lineCells = line.Split(vd()).ToList();
                foreach (string cell in lineCells)
                {
                    sheet.Cells[lineNumber, cellNumber] = cell;
                    cellNumber++;
                }
                cellNumber = 1;
                lineNumber++;
            }
            sheet.Columns.AutoFit();

            try
            {
                book.SaveAs(adress, ExcelM.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, ExcelM.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                //System.Runtime.InteropServices.COMException: 'Não é possível acessar 'AddItens.xls'.'
                book.Close(true, misValue, misValue);
                Value = true;
            }
            catch
            {
                MessageBox.Show("Arquivo aberto");
                book.Close(false, misValue, misValue);
                Value = false;
            }
           
            excelAplication.Quit();
            /*
            Marshal.ReleaseComObject(sheet);
            Marshal.ReleaseComObject(book);
            Marshal.ReleaseComObject(excelAplication);
            Marshal.ReleaseComObject(xlApp);
            */
            while (Marshal.ReleaseComObject(sheet) != 0) ;
            while (Marshal.ReleaseComObject(book) != 0) ;
            while (Marshal.ReleaseComObject(excelAplication) != 0) ; 
            while (Marshal.ReleaseComObject(xlApp) != 0) ;


            return Value;


        }
        public bool GerarPlanilhaLogs(List<string> exList,string Path)
        {
            bool Value = false;
            Folders folder = new Folders();
            string adress = Path;
            ExcelM.Application excelAplication = new Microsoft.Office.Interop.Excel.Application();
            excelAplication.DisplayAlerts = false;
            //Console.WriteLine(exList.Last()+">last 1");

            if (excelAplication == null)
            {
                return false;
            }
            ExcelM.Workbook book;
            ExcelM.Worksheet sheet;
            object misValue = System.Reflection.Missing.Value;

            book = excelAplication.Workbooks.Add(misValue);
            sheet = (ExcelM.Worksheet)book.Worksheets.get_Item(1);
            int lineNumber = 1;
            int cellNumber = 1;
            string value = "";
            List<string> lineCells = new List<string>();
            exList.Insert(0,"CN Data Hora Quantidade ID Fixture Modo");
            Console.WriteLine(exList.Last() + ">last 2");  
            foreach (string line in exList)
            {
                lineCells = line.Split(' ').ToList();
              
                foreach (string cell in lineCells)
                {
                    if (cellNumber==1||cellNumber==4||cellNumber==5||cellNumber==6)
                    {
                        try
                        {
                            sheet.Cells[lineNumber, cellNumber] = cell.Split(':')[1];
                        }
                        catch
                        {
                            sheet.Cells[lineNumber, cellNumber] = cell;
                        }
                        
                    }
                    else
                    {
                        sheet.Cells[lineNumber, cellNumber] = cell;
                    }
                    
                    cellNumber++;
                }
                cellNumber = 1;
                lineNumber++;
            }

            Console.WriteLine(exList.Last() + ">last " +"3");
            sheet.Columns.AutoFit();
            try
            {
                book.SaveAs(adress, ExcelM.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, ExcelM.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                book.Close(true, misValue, misValue);
                Value = true;
            }
            catch
            {
                MessageBox.Show("Arquivo aberto");
                book.Close(false, misValue, misValue);
                Value = false;
            }
            Console.WriteLine(exList.Last() + ">last 4");

            excelAplication.Quit();

            /*
            Marshal.ReleaseComObject(sheet);
            Marshal.ReleaseComObject(book);
            Marshal.ReleaseComObject(excelAplication);
            Marshal.ReleaseComObject(xlApp);
            */
            while (Marshal.ReleaseComObject(sheet) != 0) ;
            while (Marshal.ReleaseComObject(book) != 0) ;
            while (Marshal.ReleaseComObject(excelAplication) != 0) ;
            while (Marshal.ReleaseComObject(xlApp) != 0) ;


            return Value;
        }
        public List<string> sheetToList(string path)
        {
           
            int numCellsInLine = 12;
            List<string> list = new List<string>();
            ExcelM.Application excelAplication = new Microsoft.Office.Interop.Excel.Application();
            if (excelAplication == null)
            {
                
                return null;
            }
            ExcelM.Workbook book;
            ExcelM.Worksheet sheet;
            object misValue = System.Reflection.Missing.Value;

            book = excelAplication.Workbooks.Open(path);
            sheet = (ExcelM.Worksheet)book.Worksheets.get_Item(1);
            int colum = 1;
            bool emptyCell = false;
            int lineNumber = 1;
            string temp = "";
            while (!emptyCell)
            {
                for (colum = 1; colum <= numCellsInLine; colum++)
                {
                    //.WriteLine(lineNumber.ToString() + " " + colum.ToString());
                    if (sheet.Cells[lineNumber, colum].Value == null)
                    {
                        temp = "";
                    }
                    else
                    {
                        temp = sheet.Cells[lineNumber, colum].Value.ToString();
                    }
                    
                    if (colum == 1 && temp == "")
                    {
                        emptyCell = true;
                        break;
                    }
                    if(colum == 1)
                    {
                        list.Add("");
                    }
                    if (colum != numCellsInLine)
                    {
                        
                        list[lineNumber-1] += temp + vd().ToString();
                    }
                    else
                    {
                        list[lineNumber-1] += temp;
                    }
                    


                }
                lineNumber++;
                colum = 1;
            }

            return list;
        }
        public List<string> sortGeralList(List<string> list)
        {
           


            List<string> list2 = new List<string>();
            List<string> temp = new List<string>();
            List<string> final = new List<string>();
            List<string> stringList = new List<string>();
            temp = list;
            list2 = list;
            int a = 0;
            int num = 0;
            string line = "";
            bool isNumber = false;
            int biggestNumber = 0;
            int numOfChars = 0;
            foreach (string l in list)
            {
                line = list[a].Split(vd())[0];
                try
                {
                    num = Convert.ToInt32(line);
                    isNumber = true;
                }
                catch { isNumber = false; }
                if (isNumber&&num>=0)
                {
                   
                    if (num > biggestNumber)
                    {
                        biggestNumber = num;
                    }

                }
                numOfChars = biggestNumber.ToString().ToCharArray().Count();


                    a++;
            }
            a = 0;
            int numOfZeros = 0;
            int b = 0;
            string[] many = null;
            foreach (string l in list.ToList())
            {

                line = list[a].Split(vd())[0];
                //line = line.Split(';')[0];
                try
                {
                    num = Convert.ToInt32(line);
                    isNumber = true;
                }
                catch { isNumber = false; }
                if (isNumber && num >= 0)
                {
                    
                    numOfZeros = numOfChars - line.ToCharArray().Count();
                    b = 0;
                    
                    for(b = 0; b < numOfZeros; b++)
                    {
                        line = "0" + line;
                    }

                    stringList = temp[a].Split(vd()).ToList();
                    stringList[0] = line;
                    temp[a] = String.Join(vd().ToString(), stringList);
                   
                }

                a++;
            }
           
            temp.Sort();
            a = 0;
            foreach (string l in list.ToList())
            {

                line = temp[a].Split(vd())[0];
                //line = line.Split(';')[0];
                try
                {
                    num = Convert.ToInt32(line);
                    isNumber = true;
                }
                catch { isNumber = false; }
                if (isNumber && num >= 0)
                {

                   

                    stringList = temp[a].Split(vd()).ToList();
                    stringList[0] = num.ToString();
                    temp[a] = String.Join(vd().ToString(), stringList);

                }



                a++;
            }
     
            return temp;
        }
    
        public bool gerarPlanilhaGeral(List<string> exList, string listName = "geral")
        {
            bool Value = false;
            Folders folder = new Folders();
            
            switch (listName)
            {
                case "geral":
                    listAdress = folder.mainListEXPath + @"\" + folder.listaGeralName + extencion;
                    break;
                case "compras":
                    listAdress = folder.mainListEXPath + @"\" + folder.listaComprasName + extencion;
                    break;
                default:
                    listAdress = folder.mainListEXPath + @"\" + listName + extencion;
                    break;
            }
            
            ExcelM.Application excelAplication = new Microsoft.Office.Interop.Excel.Application();
            excelAplication.DisplayAlerts = false;


            if (excelAplication == null)
            {
                return false;
            }
            ExcelM.Workbook book;
            ExcelM.Worksheet sheet;
            object misValue = System.Reflection.Missing.Value;

            book = excelAplication.Workbooks.Add(misValue);
            sheet = (ExcelM.Worksheet)book.Worksheets.get_Item(1);
            int lineNumber = 1;
            int cellNumber = 1;
            string value = "";
            List<string> lineCells = new List<string>();
            foreach (string line in exList)
            {
                lineCells = line.Split(vd()).ToList();
                foreach (string cell in lineCells)
                {
                    sheet.Cells[lineNumber, cellNumber] = cell;
                    cellNumber++;
                }
                cellNumber = 1;
                lineNumber++;
            }


            sheet.Columns.AutoFit();
            try
            {
                book.SaveAs(listAdress, ExcelM.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, ExcelM.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                book.Close(true, misValue, misValue);
                Value = true;
            }
            catch
            {
                MessageBox.Show("Arquivo aberto");
                book.Close(false, misValue, misValue);
                Value = false;
            }
            
            
            excelAplication.Quit();

            /*
            Marshal.ReleaseComObject(sheet);
            Marshal.ReleaseComObject(book);
            Marshal.ReleaseComObject(excelAplication);
            Marshal.ReleaseComObject(xlApp);
            */
            while (Marshal.ReleaseComObject(sheet) != 0) ;
            while (Marshal.ReleaseComObject(book) != 0) ;
            while (Marshal.ReleaseComObject(excelAplication) != 0) ;
            while (Marshal.ReleaseComObject(xlApp) != 0) ;


            return Value;
        }





    }
    

    
    
}
