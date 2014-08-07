using System;

namespace SudokuSolver
{
	public static class OverallStrategy
	{
		public static Board Iterate(Board board, out int moveIndex)
		{
			// TODO: Create IStrategy
			// - GetSolutionSpace List<Cell>(board);

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

			// TODO: implement better strategy which understands the board better?

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

