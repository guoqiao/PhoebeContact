using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace PhoebeContact
{
    public class Popup
    {
        private static readonly String m_caption = "PhoebeContact";

        public static void Info(String message)
        {
            MessageBox.Show(message, m_caption + " ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Warn(String message)
        {
            MessageBox.Show(message, m_caption + " ����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(String message)
        {
            MessageBox.Show(message, m_caption + " ����", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult Question(String message)
        {
            return MessageBox.Show(message, m_caption + " ȷ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
