using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace PhoebeContact
{
    public class BaseComparer : IComparer
	{
        protected int col;
        protected SortOrder sortOrder;


        public BaseComparer()
        {
            col = 0;
        }
        public BaseComparer(int column)
        {
            col = column;

        }

        public BaseComparer(int column, SortOrder order)
        {
            col = column;
            sortOrder = order;
        }

        public virtual int Compare(object x, object y)
        {
            return 0;
        }




    }
}
