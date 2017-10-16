using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cnkinet
{
    public partial class SendToCMSForm : Form
    {
        string _content;
        // string url = "http://www.yzd2017.com/";
        string url = " http://localhost:56362/";

        public SendToCMSForm(string content)
        {
            InitializeComponent();

            this._content = content;

            Task.Run(async () =>
            {
                Dictionary<string, string> postdata = new Dictionary<string, string>();
                postdata.Add("name", "中药数据库");
                var basetypes = await HttpHelper.Post<List<DataItem>>(url + "PublishAPI/GetBaseTypes", postdata);
                Action action = () =>
                {
                    comboBoxEx1.DisplayMember = "Name";
                    comboBoxEx1.ValueMember = "ID";
                    comboBoxEx1.DataSource = basetypes;
                };
                comboBoxEx1.Invoke(action);
                postdata.Clear();

                postdata.Add("typeid", "E70B4F1A-2A99-49A4-BFA3-3D1FF159992A");
                var datastructures = await HttpHelper.Post<List<DataItem>>(url + "PublishAPI/GetDataStructures", postdata);

                Action action1 = () =>
                {
                    comboBoxEx2.DisplayMember = "Name";
                    comboBoxEx2.ValueMember = "ID";
                    comboBoxEx2.DataSource = datastructures;

                };
                comboBoxEx2.Invoke(action1);
                comboBoxEx2.SelectedIndex = 1;
            });
        }

        private async void buttonX1_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> postdata = new Dictionary<string, string>();
            postdata.Add("keyword", textBoxX1.Text);
            postdata.Add("datastructureid", comboBoxEx2.SelectedValue.ToString());

            var basetypes = await HttpHelper.Post<List<DataItem>>(url + "PublishAPI/GetData", postdata);
            Action action1 = () =>
            {
                dataGridViewX1.DataSource = basetypes;
            };
            dataGridViewX1.Invoke(action1);

        }
        /// <summary>
        /// 确认发布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonX2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认把知网内容发布到 CMS " + dataGridViewX1.CurrentRow.Cells["Name"].Value.ToString() + "吗？", "发布确认", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Dictionary<string, string> postdata = new Dictionary<string, string>();
                postdata.Add("content", this._content);
                postdata.Add("datastructureid", comboBoxEx2.SelectedValue.ToString());
                postdata.Add("customerDataId", dataGridViewX1.CurrentRow.Cells["customerDataId"].Value.ToString());
                var result = await HttpHelper.Post(url + "PublishAPI/Publish", postdata);
                if (result == "true")
                {
                    MessageBox.Show("发布成功");
                }
                else
                {
                    MessageBox.Show("发布失败");
                }
            }

        }
    }
}
