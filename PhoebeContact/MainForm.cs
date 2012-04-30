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

        private void UpdateListViewItem(Customer obj, ListViewItem item)
        {
            item.SubItems.Clear();
            item.Text = obj.company;
            item.SubItems.Add(obj.country);
            item.SubItems.Add(m_states[obj.state_id].ToString());
            item.SubItems.Add(obj.name);
            item.SubItems.Add(obj.email);
            item.SubItems.Add(obj.update_on.ToShortDateString());
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

        private string BuildCustomerInfo(Customer c)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(c.company);
            builder.AppendLine(c.country);
            builder.AppendLine(c.site);
            builder.AppendLine(c.addr);
            builder.AppendLine(c.name);
            builder.AppendLine(c.skype);
            builder.AppendLine(c.email);
            builder.AppendLine(c.mobile);
            builder.AppendLine(c.phone);
            builder.AppendLine(c.browse.ToString());
            builder.AppendLine(c.inquiry.ToString());

            builder.AppendLine(c.create_on.ToShortDateString());
            builder.AppendLine(c.update_on.ToShortDateString());



            builder.AppendLine(m_states[c.state_id].ToString());
            builder.AppendLine(c.count.ToString());
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
                    buttonSend.Update();
                    Customer c = item.Tag as Customer;
                    State s = m_states[c.state_id];
                    string email = RenderEmail(c, s);
                    postman.SendMail(c.email, "Re: Induction Light from ZKLighting", email);
                    count -= 1;
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
