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
            form.ShowDialog();
            LoadData();
        }

        private void LoadData()
        {
            Database db = DbAccess.GetInstance();
            var objs = db.Query<Customer>("SELECT * FROM Customer");

            listViewCustomer.Items.Clear();
            foreach (var obj in objs)
            {
                ListViewItem item = new ListViewItem(obj.contact);
                item.SubItems.Add(obj.country);
                item.SubItems.Add(obj.state_id.ToString());
                item.SubItems.Add(obj.email);
                item.SubItems.Add(obj.name);
                item.SubItems.Add(obj.update_on.ToShortDateString());
                item.Checked = true;
                listViewCustomer.Items.Add(item);
            }
        }
    }
}
