using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace MagnusSpace
{
    internal class Grupo
    {
        char VarDash = ((char)887);
        char VarDashPlus = ((char)888);
        string grupo;
        public List<string > allGrupos() {

           // List<string> all = new List<string>();
            List<string> list = new List<string>();
            ListClass lc = new ListClass();
            ListClass all = new ListClass();
            lc.Open("Mestra", "ListaGeral");
            foreach (string line in lc.mainList.ToList())
            {
                List<string> sublist = new List<string>();
                sublist = line.Split(VarDashPlus).ToList();
                foreach (string l in sublist)
                {
                    if (l.Split(VarDash)[0] == "grupo"&& l.Split(VarDash)[1] != "")
                    {
                        grupo = l.Split(VarDash)[1];
                        bool foundCN = false;
                        foreach(string cngp in all.mainList.ToList())
                        {
                            if(cngp.StartsWith("CN" + VarDash.ToString()+ grupo + VarDashPlus.ToString()))
                            {
                                foundCN = true;
                            }
                        }
                        if (!foundCN)
                        {
                            all.mainList.Add("CN"+VarDash.ToString()+grupo);
                        }
                        ListClass lc2 = new ListClass();


                        string check = "";
                        check = lc2.streamSEARCH(sublist, "groupPosition");
                        if(check == "")
                        {
                            check = "IN";
                        }
                        all.streamPlus(grupo, line.Split(VarDashPlus)[0].Split(VarDash)[1], check);
                        break;
                    }
                }
            }
            return all.mainList;

        }
    }
}
