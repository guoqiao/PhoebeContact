using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PetaPoco;

namespace PhoebeContact
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Database db = DbAccess.GetInstance();
            var objs = db.Query<State>("SELECT * FROM State");

            comboBoxState.Items.Add("全部");
            foreach (var obj in objs)
            {
                comboBoxState.Items.Add(obj);
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

        private void LoadData()
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT * FROM Customer");

            string keyword = textBoxKeyword.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                sql.Append("WHERE name like '%@0%'", keyword);
                sql.Append("OR site like '%@0%'", keyword);
                sql.Append("OR addr like '%@0%'", keyword);
                sql.Append("OR country like '%@0%'", keyword);
                sql.Append("OR phone like '%@0%'", keyword);
                sql.Append("OR contact like '%@0%'", keyword);
                sql.Append("OR mobile like '%@0%'", keyword);
                sql.Append("OR email like '%@0%'", keyword);
                sql.Append("OR skype like '%@0%'", keyword);
                sql.Append("OR note like '%@0%'", keyword);
            }

            int state = comboBoxState.SelectedIndex;
            if (state > 0)
            {
                sql.Append("AND state=@0", state);
            }

            Database db = DbAccess.GetInstance();
            var objs = db.Query<Customer>(sql);

            List<Customer> results = new List<Customer>();


            listViewCustomer.Items.Clear();
            foreach (var obj in objs)
            {
                if (radioButtonToday.Checked)
                {
                    State st = db.SingleOrDefault<State>("WHERE id=@0", obj.state_id);
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
            
            item.Text = obj.contact;
            item.SubItems.Add(obj.country);
            item.SubItems.Add(obj.state_id.ToString());
            item.SubItems.Add(obj.email);
            item.SubItems.Add(obj.name);
            item.SubItems.Add(obj.update_on.ToShortDateString());
            item.Checked = checkBoxAll.Checked;
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
    }
}
