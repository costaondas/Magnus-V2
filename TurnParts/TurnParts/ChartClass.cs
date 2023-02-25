using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagnusSpace
{
    internal class ChartClass
    {
        char VarDash = ((char)887);
        char VarDashPlus = ((char)888);

        public List<Button> chartBut(List<string> list)
        {
            float maxButSize = (float)Convert.ToInt32(list[0].Split(VarDash)[1]);
            float maxsize = 200;
           List<Button> buttons= new List<Button>();
            int counter = 0;
            int space = 12;
            int Ymargem = 10;
            int Xmargem = 10;
            foreach (string l in list)
            {
                string name = l.Split(VarDash)[0];
                float qtd = (float) Convert.ToInt32(l.Split(VarDash)[1]);
                Button but = new Button();
                float butWidth = (qtd / maxButSize)*maxsize;
                but.Width = (int)butWidth;
                but.Height = 100;
                but.Name = "but"+ (counter * 10).ToString() ;
                but.Text = counter.ToString();
                but.Enabled= true;
                but.Location = new Point(Ymargem, Xmargem + counter*(but.Height + space));
                but.MouseEnter += (s,arg) =>
                {
                   // but.Location = new Point(but.Location.X + but.Width, but.Location.Y);
                };
                Console.WriteLine("Location button = " + but.Location.ToString());
                buttons.Add(but);
                counter++;
            }
            return buttons;
        }
        public Chart createChart(DataTable dt)
        {
            DataSet dataSet = new DataSet();
           // DataTable dt = new DataTable();
           
            dataSet.Tables.Add(dt);

            Chart chart = new Chart();
            chart.DataSource = dataSet.Tables[0];
            chart.Width = 1920;
            chart.Height = 1080;

            Series serie1 = new Series();
            serie1.Name = "Serie1";
            serie1.BorderColor = Color.FromArgb(164, 164, 164);
            serie1.ChartType = SeriesChartType.Bar;
            serie1.BorderDashStyle = ChartDashStyle.Solid;
            serie1.BorderWidth = 1;
            serie1.ShadowOffset= 0;
            serie1.ShadowColor = Color.FromArgb(128, 128, 128);
            serie1.ShadowOffset = 1;
            serie1.IsValueShownAsLabel = false;
            serie1.XValueMember = "Text";
            serie1.YValueMembers = "Value";
            serie1.Font = new Font("Tahoma", 20.0f);
            serie1.BackSecondaryColor = Color.FromArgb(0, 102, 153);
            serie1.LabelForeColor = Color.FromArgb(100, 100, 100);
            chart.Series.Add(serie1);

            ChartArea ca = new ChartArea();
            ca.Name = "ChartArea1";
            ca.BackColor = Color.FromArgb(45, 70, 80);//
            ca.BorderColor = Color.FromArgb(26, 59, 105);
            ca.BorderWidth = 0;
            ca.BorderDashStyle = ChartDashStyle.Solid;
            ca.AxisX = new Axis();
            ca.AxisY = new Axis();
            chart.ChartAreas.Add(ca);
            chart.DataBind();

            chart.ChartAreas["ChartArea1"].AxisX.Interval = 1;

            foreach (DataPoint point in chart.Series[0].Points)
            {
                
                if (point.YValues[0] < 15)
                {
                    point.Color = Color.Yellow;
                }
                if (point.YValues[0] > 15 && point.YValues[0] < 30)
                {
                    point.Color = Color.Blue;
                }
                if (point.YValues[0] > 30)
                {
                    point.Color = Color.Red;
                }
            }
            Folders folders= new Folders();

            chart.SaveImage(folders.Chart, ChartImageFormat.Png);
            return chart;
           

        }
    }
}
