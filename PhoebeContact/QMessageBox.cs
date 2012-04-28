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
            MessageBox.Show(message, m_caption + " 信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Warn(String message)
        {
            MessageBox.Show(message, m_caption + " 警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(String message)
        {
            MessageBox.Show(message, m_caption + " 错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult Question(String message)
        {
            return MessageBox.Show(message, m_caption + " 确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
