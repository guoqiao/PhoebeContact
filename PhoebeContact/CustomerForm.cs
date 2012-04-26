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
    public partial class CustomerForm : Form
    {
        private Customer m_customer = null;

        public CustomerForm()
        {
            InitializeComponent();
        }

        public void SetCustomer(Customer customer)
        {
            m_customer = customer;
        }

        public Customer GetCustomer()
        {
            return m_customer;
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            Database db = DbAccess.GetInstance();
            var objs = db.Query<State>("SELECT * FROM State");
            foreach (var obj in objs)
            {
                comboBoxState.Items.Add(obj);
            }
            comboBoxState.SelectedIndex = 0;

            if (m_customer == null)
            {
                return;//new
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (m_customer == null)
            {
                m_customer = new Customer();
            }

            m_customer.name = textBoxName.Text.Trim();
            m_customer.site = textBoxSite.Text.Trim();
            m_customer.addr = textBoxAddr.Text.Trim();
            m_customer.country = textBoxCountry.Text.Trim();
            m_customer.phone = textBoxPhone.Text.Trim();
            m_customer.contact = textBoxContact.Text.Trim();
            m_customer.mobile = textBoxMobile.Text.Trim();
            m_customer.email = textBoxEmail.Text.Trim();

            m_customer.create_on = dateTimePickerCreateOn.Value;
            m_customer.update_on = dateTimePickerUpdateOn.Value;

            m_customer.note = richTextBoxNote.Text.Trim();
            m_customer.state_id = (comboBoxState.SelectedItem as State).id;

            m_customer.count = 0;
        }
    }
}
