using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PhoebeContact
{
    public class FloatComparer : BaseComparer
    {

        public FloatComparer(int column)
            : base(column)
        {
        }
        public FloatComparer(int column, SortOrder order)
            : base(column, order)
        {
        }

        public override int Compare(object x, object y)
        {

            float fx;
            float fy;


            try
            {
                fx = Single.Parse(((ListViewItem)x).SubItems[col].Text);
                fy = Single.Parse(((ListViewItem)y).SubItems[col].Text);
            }
            catch (Exception)
            {
                return 0;
            }

            int ret = 0;
            if (this.sortOrder == SortOrder.Ascending)
            {
                if (fx == fy)
                {
                    ret = 0;
                }
                else if (fx < fy)
                {
                    ret = -1;
                }
                else
                {
                    ret = 1;
                }
            }
            else
            {
                if (fx == fy)
                {
                    ret = 0;
                }
                else if (fx < fy)
                {
                    ret = 1;
                }
                else
                {
                    ret = -1;
                }
            }
            return ret;


        }

    }
}
