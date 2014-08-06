using System;

namespace SudokuSolver
{
	public static class OverallStrategy
	{
		public static Board Iterate(Board board, out int moveIndex)
		{
			// apply defaults
			Board result;

			result = IntersectionStrategy.Iterate(board, out moveIndex);
			if (moveIndex.IsSolved())
			{
				return result;
			}

			result = NeighbouringNumbersStrategy.Iterate(board, out moveIndex);
			if (moveIndex.IsSolved())
			{
				return result;
			}

			result = TentativeStrategy.Iterate(board, out moveIndex);
			if (moveIndex.IsSolved())
			{
				return result;
			}

			// give up
			moveIndex = IndexUtils.Unsolved;
			return board;
		}
	}
}

