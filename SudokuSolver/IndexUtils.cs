using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public static class IndexUtils
    {
		public const int Unsolved = -2;

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

		public static bool IsSolved(this int index)
		{
			return index != Unsolved;
		}
    }
}
