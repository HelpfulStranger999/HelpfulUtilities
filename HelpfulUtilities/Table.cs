using System;
using System.Collections.Generic;
using System.Text;

namespace HelpfulUtilities
{
    public class Table<R, C, V> : ICollection<DualKeyPair<R, C>
    {
    }

    public struct DualKeyPair<R, C>
    {
        public R Row { get; }
        public C Column { get; }

        public DualKeyPair(R row, C column)
        {
            Row = row;
            Column = column;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Row.GetHashCode() ^ Column.GetHashCode();
        }
    }
}
