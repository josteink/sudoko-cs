using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public static class IndexUtils
    {
        public static bool IsNewGridRow(this int index)
        {
            return index % 27 == 0;
        }

        public static bool IsNewRow(this int index)
        {
            return index % 9 == 0;
        }

        public static bool IsNewColumn(this int index)
        {
            return index % 3 == 0;
        }
    }
}
