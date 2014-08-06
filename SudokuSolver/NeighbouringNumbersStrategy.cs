using System;
using System.Linq;
using System.Collections.Generic;

namespace SudokuSolver
{
	/*
	 * IntersectionStrategy takes it this far:

		  9 8                
		  3 2 7  1 9      8  
		    5 4    8 2       

		  7 4    6        9  
		  2 9                
		    1 3      7       

		 >4<       5    2    
		  5 6      1 4  8 7 3
		                5   4
		                
 	 * Take it further! (resolve that >4<)
 	 */
	public static class NeighbouringNumbersStrategy
	{
		public static Board Iterate(Board board, out int moveIndex)
		{
			// iterate over all numbers
			foreach (var cellValue in Cell.All)
			{
				// for each sub-grid
				for (var x = 0; x < 3; x++)
				{
					for (var y = 0; y < 3; y++)
					{
						// and search
						var values = board.GetGridValues (y*9*3 + x*3);
						if (values.Contains(cellValue))
						{
							continue;
						}

						// attempt solve this
						var availablePositions = GetAvailableForGrid (board, x, y);

						// check columns
						var adjacentColumnPositions = GetColumnPositions (board, cellValue, x);
						var adjacentRowPositions =    GetRowPositions (board, cellValue, y);

						var candidates = 
							availablePositions
								.IntersectExcept(adjacentColumnPositions, i => i.X)
								.IntersectExcept(adjacentRowPositions,    i => i.Y)
								.ToArray();

						if (candidates.Length == 1)
						{
							var cord = candidates [0];

							moveIndex = cord.ToIndex ();
							var newBoard = board.Clone ();
							newBoard [moveIndex] = cellValue;

							Statistics.NeighbouringNumbersStrategyMoves++;

							return newBoard;
						}
					}
				}
			}

			moveIndex = IndexUtils.Unsolved;
			return board;

}

		private static Cord[] GetAvailableForGrid (Board board, int gx, int gy)
		{
			var result = new List<Cord> ();

			for (var x = 0; x < 3; x++)
			for (var y = 0; y < 3; y++)
			{
				var actualY = gy * 3 + y;
				var actualX = gx * 3 + x;
				var index = actualY*9 + actualX;

				var value = board [index];
				if (value.IsAssigned())
				{
					continue;
				}

				var cord = new Cord {
					X = actualX,
					Y = actualY
				};
				result.Add (cord);
			}

			return result.ToArray ();
		}

		private static Cord[] GetColumnPositions(Board board, Cell value, int gx)
		{
			var buffer = new List<Cord> ();

			for (int index = 0; index < board.Count; index++)
			{
				if (!index.IsInSubCellColumn (gx))
				{
					continue;
				}

				var cellValue = board[index];
				if (cellValue.Equals(value))
				{
					buffer.Add (Cord.FromIndex(index));
				}
			}

			return buffer.ToArray ();
		}

		private static Cord[] GetRowPositions(Board board, Cell value, int gy)
		{
			var buffer = new List<Cord> ();

			for (int index = 0; index < board.Count; index++)
			{
				if (!index.IsInSubCellRow(gy))
				{
					continue;
				}

				var cellValue = board[index];
				if (cellValue.Equals(value))
				{
					buffer.Add (Cord.FromIndex(index));
				}
			}

			return buffer.ToArray ();
		}
}
}