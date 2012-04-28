﻿using System;
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

        private void LoadData()
        {
            StringBuilder sb = new StringBuilder("SELECT * FROM Customer WHERE id>0");

            string keyword = textBoxKeyword.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                string[] fields = { "name", "country", "site", "addr", "phone", "mobile", "contact", "email", "skype", "note" };
                string sep = string.Format(" LIKE '{0}' OR ", keyword);
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

            item.Text = obj.name;
            item.SubItems.Add(obj.country);
            item.SubItems.Add(m_states[obj.state_id].ToString());
            item.SubItems.Add(obj.email);
            item.SubItems.Add(obj.contact);
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
            builder.AppendLine(c.name);
            builder.AppendLine(c.country);
            builder.AppendLine(c.site);
            builder.AppendLine(c.addr);
            builder.AppendLine(c.contact);
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
                richTextBoxInfo.Text = BuildCustomerInfo(lv.SelectedItems[0].Tag as Customer);
            }
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewCustomer.Items)
            {
                item.Checked = checkBoxAll.Checked;
            }
        }
    }
}
