using NPOIHelperTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerateData
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            labelX6.Text = "(" + rangeSlider1.Value.Min + "-" + rangeSlider1.Value.Max + ")";
            dateTimeInput1.Value = DateTime.Now.AddHours(-2);
            dateTimeInput2.Value = DateTime.Now;

        }
        Dictionary<Guid, List<string>> dic = new Dictionary<Guid, List<string>>();
        List<string> dateList = new List<string>();
        bool generated = false;
        decimal previousTemperature = 0;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();

            dic.Add(Guid.NewGuid(), list);
            var random = new Random();
            var temperature = decimal.Parse(txtstarttemperature.Text);
            var min = rangeSlider1.Value.Min;
            var max = rangeSlider1.Value.Max;
            previousTemperature = temperature;
            bool isUp = temperature < rangeSlider1.Value.Min;

            list.Add(temperature.ToString("0.0"));
            for (DateTime i = dateTimeInput1.Value; i < dateTimeInput2.Value; i = i.AddMinutes(1))
            {
                if (!generated)
                {
                    var date = i.ToString("yyyy年MM月dd日HH时mm分");
                    dateList.Add(date);
                }
                if (isUp)
                {
                    var diff = int.Parse(textBoxX2.Text);
                    var doublerandom = random.NextDouble();
                    if (diff < 1)
                    {
                        diff = 0;
                    }
                    ///升高
                    var diffvalue = random.Next(0, diff) + doublerandom;

                    temperature += Convert.ToDecimal(diffvalue);
                }
                else
                {
                    var diff = int.Parse(textBoxX1.Text);
                    var doublerandom = random.NextDouble();
                    if (diff < 1)
                    {
                        diff = 0;
                    }
                    //下降
                    var diffvalue = random.Next(0, int.Parse(textBoxX1.Text)) + doublerandom;
                    temperature -= Convert.ToDecimal(diffvalue);
                }
                isUp = IsUp(temperature);
                previousTemperature = temperature;
                list.Add(temperature.ToString("0.0"));
            }

            label1.Text = "已有列数：" + dic.Count.ToString();
            generated = true;
        }

        private bool IsUp(decimal temperature)
        {
            return temperature < rangeSlider1.Value.Min || (previousTemperature < temperature && temperature < rangeSlider1.Value.Max);
        }

        private void rangeSlider1_ValueChanged_1(object sender, EventArgs e)
        {
            labelX6.Invoke(new MethodInvoker(delegate { labelX6.Text = "(" + rangeSlider1.Value.Min + "-" + rangeSlider1.Value.Max + ")"; }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable datatable = new DataTable();
            datatable.Columns.Add(new DataColumn("日期"));

            foreach (var item in dic.Keys)
            {
                datatable.Columns.Add(new DataColumn(item.ToString()));
            }
            for (int i = 0; i < dateList.Count; i++)
            {
                var newrow = datatable.NewRow();
                newrow[0] = dateList[i];
                foreach (var key in dic.Keys)
                {
                    newrow[key.ToString()] = dic[key][i];
                }
                datatable.Rows.Add(newrow);
            }
            saveFileDialog1.Filter = "(*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                NPOIHelper.DataTableToExcel(datatable, "温度数据", saveFileDialog1.FileName);
            }
        }
    }
}
