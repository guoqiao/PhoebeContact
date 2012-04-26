using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PhoebeContact
{
    public class DateComparer : BaseComparer
    {
        public DateComparer(int column)
            : base(column)
        {
        }

        public DateComparer(int column, SortOrder order)
            : base(column, order)
        {
        }

        //public override int Compare(object x, object y)
        //{
        //    DateTime fx;
        //    DateTime fy;

        //    try
        //    {
        //        fx = DateTime.Parse(((ListViewItem)x).SubItems[col].Text);
        //        fy = DateTime.Parse(((ListViewItem)y).SubItems[col].Text);
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //    int ret = 0;
        //    if (this.sortOrder == SortOrder.Ascending)
        //    {
        //        if (fx == fy)
        //        {
        //            ret = 0;
        //        }
        //        else if (fx < fy)
        //        {
        //            ret = -1;
        //        }
        //        else
        //        {
        //            ret = 1;
        //        }
        //    }
        //    else
        //    {
        //        if (fx == fy)
        //        {
        //            ret = 0;
        //        }
        //        else if (fx < fy)
        //        {
        //            ret = 1;
        //        }
        //        else
        //        {
        //            ret = -1;
        //        } 
        //    }
        //    return ret;


        //}

        public override int Compare(object x, object y)
        {
            DateTime fx = DateTime.MinValue;
            DateTime fy = DateTime.MinValue;

            if (((ListViewItem)x).SubItems[col].Text != string.Empty)
                fx = DateTime.Parse(((ListViewItem)x).SubItems[col].Text);
            if (((ListViewItem)y).SubItems[col].Text != string.Empty)
                fy = DateTime.Parse(((ListViewItem)y).SubItems[col].Text);

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
