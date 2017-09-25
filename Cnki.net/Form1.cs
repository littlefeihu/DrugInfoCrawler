using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using NPOIHelperTest;
using Common;
namespace Cnki.net
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            webBrowser1.Navigate("http://kns.cnki.net/kns/brief/default_result.aspx");

        }

        private void btndownload_Click(object sender, EventArgs e)
        {
            var htmlDoc = webBrowser1.Document.Window.Frames["iframeResult"].Document;


            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlDoc.Body.InnerHtml);
            var table = doc.DocumentNode.SelectSingleNode("//table[@class='GridTableContent']");

            if (table == null)
            {
                MessageBox.Show("列表不存在，请检查页面");
                return;
            }

            var keyword = doc.GetElementbyId("txtValue").GetAttributeValue("Value", "");

            HtmlAgilityPack.HtmlDocument docParent = new HtmlAgilityPack.HtmlDocument();
            docParent.LoadHtml(webBrowser1.Document.Body.InnerHtml);
            var TagElement = docParent.GetElementbyId("dbTag");
            var currentTag = TagElement.SelectSingleNode("./li[@class='recur']").InnerText;



            var rows = table.SelectNodes("./tbody/tr");
            if (rows != null && rows.Count > 1)
            {
                DataTable dt = new DataTable("中国知网");
                var columns = rows[0].SelectNodes("./td");
                List<string> columnList = new List<string>();

                foreach (HtmlNode column in columns)
                {
                    dt.Columns.Add(column.InnerText);
                    columnList.Add(column.InnerText);
                }
                int rowIndex = 0;
                foreach (HtmlNode row in rows)
                {
                    if (rowIndex > 0)
                    {
                        var newRow = dt.NewRow();

                        var dataColumns = row.SelectNodes("./td");

                        for (int i = 0; i < dataColumns.Count; i++)
                        {
                            newRow[i] = dataColumns[i].InnerText;
                        }
                        dt.Rows.Add(newRow);
                    }
                    rowIndex += 1;
                }
                saveFileDialog1.Filter = "(*.xls)|*.xls";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    NPOIHelper.DataTableToExcel(dt, "中国知网", saveFileDialog1.FileName);
                }
                var body = DataTableSerializer.SerializeDataTableXml(dt);

                var key = string.Join(",", columnList);

                var db1 = new Model1();
                db1.Cnkis.Add(new Common.Cnki
                {
                    KeyString = key,
                    DataString = body,
                    KeyWord = keyword,
                    Category = currentTag
                });
                db1.SaveChanges();

            }
            else
            {
                MessageBox.Show("无数据可下载");
            }
        }
    }
}
