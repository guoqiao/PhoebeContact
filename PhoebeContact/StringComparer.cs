using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PhoebeContact
{
	public class StringComparer : BaseComparer
	{
       
        public StringComparer(int col)
            : base(col)
        {
          
        }
        public StringComparer(int column, SortOrder order)
            : base(column, order)
        {
        }

        public override int Compare(object x, object y)
        {
            if (this.sortOrder == SortOrder.Ascending)
            {
                return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            }
            else
            {
                 return String.Compare(((ListViewItem)y).SubItems[col].Text, ((ListViewItem)x).SubItems[col].Text);

            }
        }
	}
}
