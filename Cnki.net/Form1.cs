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
namespace Cnkinet
{
    public partial class Form1 : Form
    {
        DataTable standardTable = new DataTable("standardTable");
        public Form1()
        {
            InitializeComponent();

            webBrowser1.Navigate("http://kns.cnki.net/kns/brief/default_result.aspx");

            InitData();
            standardTable.Columns.Add("篇名");
            standardTable.Columns.Add("作者");
            standardTable.Columns.Add("刊名");
            standardTable.Columns.Add("年期");
            standardTable.Columns.Add("下载数");
            buttonX2.Enabled = false;
        }


        private void InitData()
        {
            //var db1 = new Model1();

            //var list = db1.Cnkis.Where(o => o.Category != null).Select(o => o.Category).Distinct().ToList();
            //comboBoxEx1.DataSource = list;

            comboBoxEx1.DataSource = new List<string> { "文献", "期刊", "博硕士", "会议", "报纸", "专利" };
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
                    dt.Columns.Add(string.IsNullOrEmpty(column.InnerText) ? "序号" : column.InnerText);
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
                if (checkBoxX1.Checked)
                {
                    saveFileDialog1.Filter = "(*.xls)|*.xls";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        NPOIHelper.DataTableToExcel(dt, "中国知网", saveFileDialog1.FileName);
                    }
                }



                switch (currentTag)
                {
                    case "文献":
                        foreach (DataRow row in dt.Rows)
                        {
                            var newrow = standardTable.NewRow();
                            newrow["篇名"] = row["题名"];
                            newrow["作者"] = row["作者"];
                            newrow["刊名"] = row["来源"];
                            newrow["年期"] = row["发表时间"];
                            newrow["下载数"] = row["下载"];
                            standardTable.Rows.Add(newrow);
                        }
                        break;
                    case "期刊":
                        foreach (DataRow row in dt.Rows)
                        {
                            var newrow = standardTable.NewRow();
                            newrow["篇名"] = row["篇名"];
                            newrow["作者"] = row["作者"];
                            newrow["刊名"] = row["刊名"];
                            newrow["年期"] = row["年/期"];
                            newrow["下载数"] = row["下载"];
                            standardTable.Rows.Add(newrow);
                        }
                        break;
                    case "博硕士":
                        foreach (DataRow row in dt.Rows)
                        {
                            var newrow = standardTable.NewRow();
                            newrow["篇名"] = row["中文题名"];
                            newrow["作者"] = row["作者"].ToString() + row["数据库"].ToString();
                            newrow["刊名"] = row["学位授予单位"];
                            newrow["年期"] = row["学位年度"];
                            newrow["下载数"] = row["下载"];
                            standardTable.Rows.Add(newrow);
                        }
                        break;
                    case "会议":
                        foreach (DataRow row in dt.Rows)
                        {
                            var newrow = standardTable.NewRow();
                            newrow["篇名"] = row["篇名"];
                            newrow["作者"] = row["作者"];
                            newrow["刊名"] = row["会议名称"];
                            newrow["年期"] = row["时间"];
                            newrow["下载数"] = row["下载"];
                            standardTable.Rows.Add(newrow);
                        }
                        break;
                    case "报纸":
                        foreach (DataRow row in dt.Rows)
                        {
                            var newrow = standardTable.NewRow();
                            newrow["篇名"] = row["题名"];
                            newrow["作者"] = row["作者"];
                            newrow["刊名"] = row["报纸名称"];
                            newrow["年期"] = row["日期"];
                            newrow["下载数"] = row["下载"];
                            standardTable.Rows.Add(newrow);
                        }
                        break;
                    case "专利":
                        foreach (DataRow row in dt.Rows)
                        {
                            var newrow = standardTable.NewRow();
                            newrow["篇名"] = row["专利名称"];
                            newrow["作者"] = row["申请人"];
                            newrow["刊名"] = row["数据库"];
                            newrow["年期"] = row["申请日"];
                            newrow["下载数"] = row["下载"];
                            standardTable.Rows.Add(newrow);
                        }
                        break;
                    default:
                        break;
                }
                var body = DataTableSerializer.SerializeDataTableXml(standardTable);

                var key = string.Join(",", columnList);
                var db1 = new Model1();
                foreach (DataRow item in standardTable.Rows)
                {

                    var title = item["篇名"].ToString();
                    var author = item["作者"].ToString();
                    var journalName = item["刊名"].ToString();
                    var publishDate = item["年期"].ToString();
                    var downloadCount = item["下载数"].ToString().Trim();

                    if (db1.Cnkis.FirstOrDefault(o => o.Title == title && o.Author == author) == null)
                    {
                        db1.Cnkis.Add(new Common.Cnki
                        {
                            KeyString = key,
                            KeyWord = keyword,
                            Category = currentTag,
                            Title = title,
                            Author = author,
                            JournalName = journalName,
                            PublishDate = publishDate,
                            DownloadedCount = string.IsNullOrEmpty(downloadCount) ? 0 : int.Parse(downloadCount)
                        });
                    }
                }
                db1.SaveChanges();
                MessageBox.Show("下载完成");
            }
            else
            {
                MessageBox.Show("无数据可下载");
            }
        }


        List<Cnki> cnkis = null;
        DataTable dataTable = null;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            buttonX2.Enabled = false;
            if (string.IsNullOrEmpty(textBoxX1.Text))
            {
                MessageBox.Show("关键词不能为空");
                return;
            }
            this.dataGridViewX1.Columns.Clear();
            var db1 = new Model1();

            cnkis = db1.Cnkis.Where(o => o.Category == comboBoxEx1.Text && o.KeyWord.Contains(textBoxX1.Text)).ToList();
            if (cnkis == null || cnkis.Count == 0)
            {
                MessageBox.Show("没找到记录");
                dataGridViewX1.DataSource = new DataTable();
                return;
            }

            System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
            Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            Column1.HeaderText = "选择";
            Column1.Name = "Select";

            this.dataGridViewX1.Columns.Add(Column1);
            var searchedDataTable = standardTable.Clone();
            foreach (var dataitem in cnkis.Select(o => new
            {
                o.Title,
                o.Author,
                o.JournalName,
                o.PublishDate,
                o.DownloadedCount
            }).ToList())
            {
                var newrow = searchedDataTable.NewRow();
                newrow["篇名"] = dataitem.Title;
                newrow["作者"] = dataitem.Author;
                newrow["刊名"] = dataitem.JournalName;
                newrow["年期"] = dataitem.PublishDate;
                newrow["下载数"] = dataitem.DownloadedCount;
                searchedDataTable.Rows.Add(newrow);
            }

            dataGridViewX1.DataSource = searchedDataTable;
            buttonX2.Enabled = searchedDataTable.Rows.Count > 0;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            var selectedDataTable = standardTable.Clone();
            foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
            {
                if (row.DataBoundItem == null)
                    continue;

                var newrow = ((System.Data.DataRowView)(row.DataBoundItem)).Row.ItemArray;

                selectedDataTable.Rows.Add(newrow);
            }

            var dataString = DataTableSerializer.SerializeDataTableXml(selectedDataTable);

            SendToCMSForm f = new SendToCMSForm(dataString);

            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            InitData();
        }

        private void dataGridViewX1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dataGridViewX1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)this.dataGridViewX1.Rows[e.RowIndex].Cells[0];
                Boolean flag = Convert.ToBoolean(checkCell.Value);

                this.dataGridViewX1.Rows[e.RowIndex].Selected = flag;
            }
        }

        private void dataGridViewX1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)e.Row.Cells[0];
            checkCell.Value = e.Row.Selected;

        }
    }
}
