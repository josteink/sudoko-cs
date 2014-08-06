using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public static class IntersectionStrategy
    {
        public static Board Iterate(Board board, out int moveIndex)
        {
            for (int index = 0; index < board.Count; index++)
            {
                var value = board[index];
                if (value.IsAssigned())
                {
                    continue;
                }

                var candidates = GetValueCandidates(board, index);

                // only one possible option.
                if (candidates.Length == 1)
                {
                    var newBoard = (Board)board.Clone();
                    newBoard[index] = candidates[0];

                    Statistics.IntersectonStrategyMoves++;
                    moveIndex = index;
                    return newBoard;
                }
            }

            moveIndex = IndexUtils.Unsolved;
            return board;
        }

		public static Cell[] GetValueCandidates(Board board, int index)
		{
			var allValues = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
			var all = allValues.Select(i => new Cell(i));

			var columnValues = board.GetColumnValues(index).Where(i => i.IsAssigned()).ToArray();
			var rowValues = board.GetRowValues(index).Where(i => i.IsAssigned()).ToArray();
			var gridValues = board.GetGridValues(index).Where(i => i.IsAssigned()).ToArray();

			var missingColumnValues = all.Except(columnValues).ToArray();
			var missingRowValues = all.Except(rowValues).ToArray();
			var missingGridValues = all.Except(gridValues).ToArray();

			var candidates = missingColumnValues
				.Intersect(missingRowValues)
					.Intersect(missingGridValues);

			return candidates.ToArray();
		}

    }
}
