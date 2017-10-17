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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            labelX7.Text = "(" + rangeSlider2.Value.Min + "-" + rangeSlider2.Value.Max + ")";
            labelX6.Text = "(" + rangeSlider1.Value.Min + "-" + rangeSlider1.Value.Max + ")";
            dateTimeInput1.Value = DateTime.Now.AddDays(-7);
            dateTimeInput2.Value = DateTime.Now;
        }

        private void rangeSlider2_ValueChanged(object sender, EventArgs e)
        {
            labelX7.Invoke(new MethodInvoker(() =>
            {
                labelX7.Text = "(" + rangeSlider2.Value.Min + "-" + rangeSlider2.Value.Max + ")";
            }));
        }

        private void rangeSlider1_ValueChanged(object sender, EventArgs e)
        {
            labelX6.Invoke(new MethodInvoker(delegate { labelX6.Text = "(" + rangeSlider1.Value.Min + "-" + rangeSlider1.Value.Max + ")"; }));
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            var random = new Random();

            DataTable datatable = new DataTable();
            datatable.Columns.Add(new DataColumn("his_time"));
            datatable.Columns.Add(new DataColumn("his_data"));
            datatable.Columns.Add(new DataColumn("his_chan"));
            datatable.Columns.Add(new DataColumn("his_reason"));
            datatable.Columns.Add(new DataColumn("his_recordinterval"));

            for (DateTime i = dateTimeInput1.Value; i < dateTimeInput2.Value; i = i.AddMinutes(int.Parse(textBoxX1.Text)))
            {
                var newrow = datatable.NewRow();
                var temperature = Convert.ToDecimal(random.Next(rangeSlider1.Value.Min, rangeSlider1.Value.Max) + random.NextDouble()).ToString("0.0");
                var humidity = Convert.ToDecimal(random.Next(rangeSlider2.Value.Min, rangeSlider2.Value.Max) + random.NextDouble()).ToString("0.0");
                var date = i.ToString("yyyy年MM月dd日HH时mm分");
                newrow[0] = date;
                newrow[1] = temperature + ";" + humidity;
                newrow[2] = "1;2";
                newrow[3] = "数据正常";
                newrow[4] = textBoxX1.Text;
                datatable.Rows.Add(newrow);
            }

            saveFileDialog1.Filter = "(*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                NPOIHelper.DataTableToExcel(datatable, "温湿度数据", saveFileDialog1.FileName);
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }
    }
}
