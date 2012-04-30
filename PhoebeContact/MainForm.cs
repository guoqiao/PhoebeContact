using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PetaPoco;
using Antlr.StringTemplate;
using System.IO;

namespace PhoebeContact
{
    public partial class MainForm : Form
    {
        Database db = DbAccess.GetInstance();

        Dictionary<int, State> m_states = new Dictionary<int, State>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var objs = db.Query<State>("SELECT * FROM State");

            comboBoxState.Items.Add("全部");
            foreach (var obj in objs)
            {
                comboBoxState.Items.Add(obj);
                m_states.Add(obj.id, obj);
            }
            comboBoxState.SelectedIndex = 0;

            LoadData();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            CustomerForm form = new CustomerForm();
            DialogResult r = form.ShowDialog();
            if (r == DialogResult.OK)
            {
                Customer c = form.GetCustomer();
                Database db = DbAccess.GetInstance();
                db.Insert(c);
                listViewCustomer.Items.Add(CreateListViewItem(c));
            }
        }

        private void toolStripMenuItemEditCustomer_Click(object sender, EventArgs e)
        {
            if (listViewCustomer.SelectedItems.Count <= 0)
            {
                return;
            }

            ListViewItem item = listViewCustomer.SelectedItems[0];

            Customer c = item.Tag as Customer;

            CustomerForm form = new CustomerForm();
            form.SetCustomer(c);
            DialogResult r = form.ShowDialog();
            if (r == DialogResult.OK)
            {
                db.Update(c);
                UpdateListViewItem(c, item);
            }
        }

        private void toolStripMenuItemDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (listViewCustomer.SelectedItems.Count <= 0)
            {
                return;
            }

            ListViewItem item = listViewCustomer.SelectedItems[0];

            Customer c = item.Tag as Customer;

            DialogResult r = Popup.Question(string.Format("确认删除客户 {0} ?", c.name));

            if (r == DialogResult.No)
            {
                return;
            }

            int ret = db.Delete(c);
            if (ret > 0)
            {
                item.Remove();
            }
        }

        private void LoadData()
        {
            StringBuilder sb = new StringBuilder("SELECT * FROM Customer WHERE id>0");

            string keyword = textBoxKeyword.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                string[] fields = { "company", "country", "site", "addr", "phone", "mobile", "name", "email", "skype", "note" };
                string sep = string.Format(" LIKE '%{0}%' OR ", keyword);
                string like = string.Join(sep, fields);
                sb.AppendFormat(" AND ({0})", like);
            }

            int state = comboBoxState.SelectedIndex;
            if (state > 0)
            {
                sb.AppendFormat(" AND state_id={0}", state);
            }
        
            var objs = db.Query<Customer>(sb.ToString());

            listViewCustomer.Items.Clear();
            foreach (var obj in objs)
            {
                if (radioButtonToday.Checked)
                {
                    State st = m_states[obj.state_id];
                    if (st.period < 1 || obj.update_on.AddDays(st.period) != DateTime.Today)
                    {
                        continue;
                    }
                }
                listViewCustomer.Items.Add(CreateListViewItem(obj));
            }
        }

        private DateTime GetNext(Customer obj)
        {
            State state = m_states[obj.state_id];
            DateTime next = obj.update_on.AddDays(state.period);
            return next;
        }

        private Country GetCountry(string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                return null;
            }

            var c = db.SingleOrDefault<Country>("WHERE abbr=@0", country);
            return c;
        }

        private void UpdateListViewItem(Customer obj, ListViewItem item)
        {
            item.SubItems.Clear();
            item.Text = obj.company;
            item.SubItems.Add(obj.country);
            State state = m_states[obj.state_id];
            item.SubItems.Add(state.ToString());
            item.SubItems.Add(obj.name);
            item.SubItems.Add(obj.email);
            item.SubItems.Add(obj.update_on.ToShortDateString());
            item.SubItems.Add(GetNext(obj).ToShortDateString());
            item.Checked = checkBoxAll.Checked;
            item.Tag = obj;
        }

        private ListViewItem CreateListViewItem(Customer c)
        {
            ListViewItem item = new ListViewItem();
            UpdateListViewItem(c,item);
            return item;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private string BuildCountryInfo(Country c)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("英文:" + c.english);
            builder.AppendLine("缩写:" + c.abbr);
            builder.AppendLine("中文:" + c.chinese);
            builder.AppendLine("区域:" + c.area);
            builder.AppendLine("语言:" + c.language);       
            builder.AppendLine("区号:" + c.code);
            builder.AppendLine("Google:" + c.google);
            builder.AppendLine("时差:" + c.hourdiff.ToString("F1"));

            DateTime from = DateTime.Today.AddHours((int)(9.0 + c.hourdiff));
            DateTime to = DateTime.Today.AddHours((int)(17.0 + c.hourdiff));

            builder.AppendFormat("工作:{0}-{1}", from.ToString("HH:mm"), to.ToString("HH:mm"));

            return builder.ToString();
        }

        private string BuildCustomerInfo(Customer c)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("公司:" + c.company);
            builder.AppendLine("国家:" + c.country);
            builder.AppendLine("网站:" + c.site);
            builder.AppendLine("地址:" + c.addr);
            builder.AppendLine("姓名:" + c.name);
            builder.AppendLine("Skype:" + c.skype);
            builder.AppendLine("邮箱:" + c.email);
            builder.AppendLine("手机:" + c.mobile);
            builder.AppendLine("电话:" + c.phone);
            builder.AppendLine("浏览:" + c.browse.ToString());
            builder.AppendLine("询盘:" + c.inquiry.ToString());

            builder.AppendLine("首次:" + c.create_on.ToShortDateString());
            builder.AppendLine("最近:" + c.update_on.ToShortDateString());
            builder.AppendLine("下次:" + GetNext(c).ToShortDateString());
            builder.AppendLine("状态:" + m_states[c.state_id].ToString());
            builder.AppendLine("剩余:" + c.count.ToString());

            if (!string.IsNullOrEmpty(c.country))
            {
                Country country = GetCountry(c.country);
                if (country != null)
                {
                    builder.AppendLine("===========国家===========");
                    builder.AppendLine(BuildCountryInfo(country));
                }
            }

            builder.AppendLine("===========备注===========");
            builder.AppendLine(c.note);
            return builder.ToString();
        }

        private void listViewCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;
            if (lv.SelectedItems.Count > 0)
            {
                Customer c = lv.SelectedItems[0].Tag as Customer;
                State s = m_states[c.state_id];
                richTextBoxInfo.Text = BuildCustomerInfo(c);
                richTextBoxEmail.Text = RenderEmail(c, s);
            }
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewCustomer.Items)
            {
                item.Checked = checkBoxAll.Checked;
            }
        }

        private string RenderEmail(Customer customer, State state)
        {
            string path = string.Format("template/{0}.txt", state.name);
            string content = File.ReadAllText(path);
            StringTemplate tmpl = new StringTemplate(content);
            tmpl.SetAttribute("NAME", customer.name);
            tmpl.SetAttribute("DATE", customer.update_on.ToShortDateString());
            return tmpl.ToString();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            int count = listViewCustomer.CheckedItems.Count;
            if (count <= 0)
            {
                return;
            }

            string text = buttonSend.Text;
            buttonSend.Enabled = false;
            Email postman = new Email();
            try
            {
                foreach (ListViewItem item in listViewCustomer.CheckedItems)
                {
                    buttonSend.Text = count.ToString() + "...";
                    count -= 1;
                    buttonSend.Update();
                    Customer c = item.Tag as Customer;
                    State s = m_states[c.state_id];
                    string email = RenderEmail(c, s);
                    postman.SendMail(c.email, "Re: Induction Light from ZKLighting", email);
                    c.update_on = DateTime.Today;
                    c.count -= 1;

                    if (c.count <= 0)
                    {
                        c.count = 0;
                    }

                    if (s.period > 0 && c.count == 0)
                    {
                        int new_state_id = c.state_id + 1;
                        if (m_states.ContainsKey(new_state_id))
                        {
                            c.state_id = new_state_id;
                            c.count = m_states[new_state_id].total;
                        }
                    }

                    db.Save(c);

                    UpdateListViewItem(c, item);
                }
            }
            catch (System.Exception ex)
            {
                Popup.Warn("发送邮件失败:" + ex.InnerException.Message);
            }
            finally
            {
                buttonSend.Enabled = true;
                buttonSend.Text = text;
            }
        }

        private void contextMenuStripCustomer_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = listViewCustomer.SelectedItems.Count == 0;
        }
    }
}
