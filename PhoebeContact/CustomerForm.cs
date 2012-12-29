using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PetaPoco;
using System.Text.RegularExpressions;

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

            var countries = db.Query<Country>("SELECT * FROM Country");
            comboBoxCountry.Items.Add("");
            foreach (var obj in countries)
            {
                comboBoxCountry.Items.Add(obj);
            }

            var objs = db.Query<State>("SELECT * FROM State");
            foreach (var obj in objs)
            {
                comboBoxState.Items.Add(obj);
            }

            if (m_customer == null)
            {
                comboBoxState.SelectedIndex = 0;
                dateTimePickerCreateOn.Value = DateTime.Today;
                dateTimePickerUpdateOn.Value = DateTime.Today;
                return;//new
            }

            textBoxCompany.Text = m_customer.company;
            textBoxSite.Text = m_customer.site;
            textBoxAddr.Text = m_customer.addr;
            comboBoxCountry.Text = m_customer.country;
            textBoxPhone.Text = m_customer.phone;
            textBoxName.Text = m_customer.name;
            textBoxMobile.Text = m_customer.mobile;
            textBoxEmail.Text = m_customer.email;

            dateTimePickerCreateOn.Value = m_customer.create_on;
            dateTimePickerUpdateOn.Value = m_customer.update_on;
            numericUpDownInquiry.Value = m_customer.inquiry;
            numericUpDownBrowse.Value = m_customer.browse;

            richTextBoxNote.Text = m_customer.note;
            comboBoxState.SelectedIndex = m_customer.state_id - 1;
            numericUpDownCount.Value = (comboBoxState.SelectedItem as State).total;
        }

        private bool CheckInput(string input, string name)
        {
            if (string.IsNullOrEmpty(input))
            {
                Popup.Warn(string.Format("{0}不能为空!", name));
                return false;
            }
            return true;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (m_customer == null)
            {
                m_customer = new Customer();
                m_customer.create_on = m_customer.update_on = DateTime.Today;
            }

            //TODO: check input
            m_customer.company = textBoxCompany.Text.Trim();

            if (!CheckInput(m_customer.company, "公司"))
            {
                return;
            }

            m_customer.country = comboBoxCountry.Text.Trim();

            m_customer.site = textBoxSite.Text.Trim();

            m_customer.addr = textBoxAddr.Text.Trim();

            m_customer.phone = textBoxPhone.Text.Trim();

            m_customer.name = textBoxName.Text.Trim();

            if (!CheckInput(m_customer.name, "姓名"))
            {
                return;
            }

            m_customer.skype = textBoxSkype.Text.Trim();

            m_customer.mobile = textBoxMobile.Text.Trim();
            m_customer.email = textBoxEmail.Text.Trim();

            string re = @"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$";
            bool match = Regex.IsMatch(m_customer.email,re);
            if (!match)
            {
                Popup.Warn(string.Format("邮箱地址无效!"));
                return;
            }

            m_customer.inquiry = (int)numericUpDownInquiry.Value;
            m_customer.browse  = (int)numericUpDownBrowse.Value;

            m_customer.create_on = dateTimePickerCreateOn.Value;
            m_customer.update_on = dateTimePickerUpdateOn.Value;

            m_customer.note = richTextBoxNote.Text.Trim();
            m_customer.state_id = comboBoxState.SelectedIndex + 1;
            m_customer.count = (int)numericUpDownCount.Value;

            m_customer.count = (int)numericUpDownCount.Value;

            DialogResult = DialogResult.OK;
        }

        private void comboBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            var state = (comboBoxState.SelectedItem as State);
            numericUpDownCount.Value = state.total;
            dateTimePickerUpdateOn.Value = DateTime.Today;
        }
    }
}
