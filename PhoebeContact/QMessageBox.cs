using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace PhoebeContact
{
    public class QMessageBox
    {
        private static readonly String m_caption = "PhoebeContact";

        public static void ShowInfomation(String message)
        {
            MessageBox.Show(message, m_caption + " 信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowWarning(String message)
        {
            MessageBox.Show(message, m_caption + " 警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowError(String message)
        {
            MessageBox.Show(message, m_caption + " 错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowQuestion(String message)
        {
            return MessageBox.Show(message, m_caption + " 确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
