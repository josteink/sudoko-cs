﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public static class BoardUtils
    {
        public static int[] GetColumnValues(this int[] board, int index)
        {
            var columnNumber = index % 9;
            var buffer = new List<int>();

            for (int i=0; i < 9; i++)
            {
                var value = board[columnNumber + (i*9)];
                buffer.Add(value);
            }

            return buffer.ToArray();
        }

        public static int[] GetRowValues(this int[] board, int index)
        {
            var rowNumber = (index - (index%9))/9;

            var buffer = new List<int>();

            for (int i=0; i < 9; i++)
            {
                var value = board[(rowNumber*9) + i];
                buffer.Add(value);
            }

            return buffer.ToArray();
        }

        public static int[] GetGridValues(this int[] board, int index)
        {
            int horiz = index%9;           // 16 -> 7
            int vert = (index - horiz)/9;  // 16 -> (16-7)/9 -> 1

            int gridStartH = horiz - (horiz%3); // 7 - 1 -> 6
            int gridStartV = vert - (vert%3);   // 1 - 1 -> 0

            var buffer = new List<int>();

            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    var value = board[9*(y + gridStartV) + x + gridStartH];
                    buffer.Add(value);
                }
            }

            return buffer.ToArray();
        }

        public static int[] GetValueCandidates(this int[] board, int index)
        {
            var all = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9};

            var columnValues = board.GetColumnValues(index).Where(ValueUtils.IsAssigned).ToArray();
            var rowValues = board.GetRowValues(index).Where(ValueUtils.IsAssigned).ToArray();
            var gridValues = board.GetGridValues(index).Where(ValueUtils.IsAssigned).ToArray();

            var missingColumnValues = all.Except(columnValues).ToArray();
            var missingRowValues = all.Except(rowValues).ToArray();
            var missingGridValues = all.Except(gridValues).ToArray();

            var candidates = missingColumnValues
                .Intersect(missingRowValues)
                .Intersect(missingGridValues);

            return candidates.ToArray();
        }

        public static bool Validate(this int[] board)
        {
            for (int index = 0; index < board.Length; index++)
            {
                var columnValues = board.GetColumnValues(index).Where(ValueUtils.IsAssigned).ToArray();
                var rowValues = board.GetRowValues(index).Where(ValueUtils.IsAssigned).ToArray();
                var gridValues = board.GetGridValues(index).Where(ValueUtils.IsAssigned).ToArray();

                bool valid = true;
                valid &= ValidateAllAsignedUnique(columnValues);
                valid &= ValidateAllAsignedUnique(rowValues);
                valid &= ValidateAllAsignedUnique(gridValues);

                if (!valid)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateAllAsignedUnique(int[] values)
        {
            var numItems = values.Length;
            var numUniqueItems = values.Distinct().Count();

            return numItems == numUniqueItems;
        }

        public static string GetDump(this int[] board)
        {
            var buffer = new StringBuilder();

            for (int i = 0; i < board.Length; i++)
            {
                if (i % 27 == 0)
                {
                    //new grid row
                    buffer.AppendLine();
                    buffer.AppendLine();
                }
                else if (i % 9 == 0)
                {
                    // new row
                    buffer.AppendLine();
                }
                else if (i % 3 == 0)
                {
                    // new grid column
                    buffer.Append("  ");
                }

                var value = board[i];
                var textValue = value.GetText();

                buffer.Append(textValue + " ");
            }

            buffer.AppendLine();
            buffer.AppendLine();

            return buffer.ToString();
        }
    }
}