using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public static class IntersectionStrategy
    {
        public static int[] Iterate(int[] board, out bool keepTrying)
        {
            for (int index = 0; index < board.Length; index++)
            {
                var value = board[index];
                if (value.IsAssigned())
                {
                    continue;
                }

                var candidates = board.GetValueCandidates(index);

                // only one possible option.
                if (candidates.Length == 1)
                {
                    var newBoard = (int[])board.Clone();
                    newBoard[index] = candidates[0];

                    Statistics.IntersectonStrategyMoves++;
                    keepTrying = true;
                    return newBoard;
                }
            }

            keepTrying = false;
            return board;
        }
    }
}
