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
        List<string> firstlist = new List<string>();

        private void buttonX1_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();

            dic.Add(Guid.NewGuid(), list);
            var random = new Random();
            var temperature = decimal.Parse(txtstarttemperature.Text);
            var min = rangeSlider1.Value.Min;
            var max = rangeSlider1.Value.Max;
            previousTemperature = temperature;
            bool isUp = temperature <= rangeSlider1.Value.Min;

            var upDiffs = textBoxX2.Text.Split('|');

            var downDiffs = textBoxX1.Text.Split('|');



            list.Add(temperature.ToString("0.0"));
            int index = 0;
            for (DateTime i = dateTimeInput1.Value; i < dateTimeInput2.Value; i = i.AddMinutes(int.Parse(textBox1.Text)))
            {
                if (!generated)
                {
                    var date = i.ToString("yyyy年MM月dd日HH时mm分");
                    dateList.Add(date);
                }
                if (isUp)
                {
                    ///升高
                    var diffvalue = upDiffs[random.Next(0, upDiffs.Length)];

                    temperature += Convert.ToDecimal(diffvalue);
                    temperature += decimal.Parse("0.001");
                    if (generated)
                    {
                        if (temperature >= rangeSlider1.Value.Max)
                        {
                            temperature = rangeSlider1.Value.Max;
                        }
                    }
                }
                else
                {
                    //下降
                    var diffvalue = downDiffs[random.Next(0, downDiffs.Length)];
                    temperature -= Convert.ToDecimal(diffvalue);
                    temperature -= decimal.Parse("0.001");
                    if (generated)
                    {
                        if (temperature <= rangeSlider1.Value.Min)
                        {
                            temperature = rangeSlider1.Value.Min;
                        }
                    }
                }

                if (generated)
                {
                    try
                    {
                        isUp = decimal.Parse(firstlist[index]) < decimal.Parse(firstlist[index + 1]);
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    isUp = IsUp(temperature);
                }
                previousTemperature = temperature;
                list.Add(temperature.ToString("0.0"));
                index += 1;
            }

            label1.Text = "已有列数：" + dic.Count.ToString();
            if (!generated)
            {
                firstlist = list;
            }
            generated = true;

        }

        private bool IsUp(decimal temperature)
        {
            return temperature <= rangeSlider1.Value.Min || (previousTemperature <= temperature && temperature < rangeSlider1.Value.Max && temperature > rangeSlider1.Value.Min);
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
