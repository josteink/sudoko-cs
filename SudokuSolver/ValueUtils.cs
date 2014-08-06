using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public static class ValueUtils
    {
        public const int Unassigned = -1;
        public const int Unsolved = -2;

        public static bool IsAssigned(this int value)
        {
            return value != Unassigned;
        }

        public static bool IsSolved(this int value)
        {
            return value != Unsolved;
        }

        public static string GetText(this int value)
        {
            if (!value.IsAssigned())
            {
                return " ";
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
