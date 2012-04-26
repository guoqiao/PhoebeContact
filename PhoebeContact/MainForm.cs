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
            dateTimePickerUpdateOn.Value = DateTime.Today.AddDays(-30);

            Database db = DbAccess.GetInstance();
            var objs = db.Query<State>("SELECT * FROM State");

            comboBoxState.Items.Add("全部");
            foreach (var obj in objs)
            {
                comboBoxState.Items.Add(obj);
            }
            comboBoxState.SelectedIndex = 0;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            CustomerForm form = new CustomerForm();
            form.ShowDialog();
        }
    }
}
