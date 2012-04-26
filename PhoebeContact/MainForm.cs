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

            foreach (var obj in objs)
            {
                string name = obj.name;
            }
        }
    }
}
