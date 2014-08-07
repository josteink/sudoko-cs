using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SudokuSolver
{
	public class NeighbouringNumbersStrategy : IStrategy
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

        public Cell Solve(Board board)
        {
			// iterate over all numbers
			foreach (var cellValue in Cell.All)
			{
				// for each sub-grid
				for (var x = 0; x < Board.GridSize; x++)
				{
                    for (var y = 0; y < Board.GridSize; y++)
					{
						// and search
						var values = board.GetGridCells (x,y);
						if (values.Contains(cellValue))
						{
							continue;
						}

						// attempt solve this
					    var availablePositions = board
					        .GetGridCells(x, y)
					        .Where(i => !i.IsAssigned())
					        .ToArray();

						// check columns
						var adjacentColumnPositions = GetColumnPositions (board, cellValue, x);
						var adjacentRowPositions =    GetRowPositions (board, cellValue, y);

					    var cand1 = availablePositions
					        .ExcludeOn(adjacentColumnPositions, i => i.X)
					        .ToArray();
					    var cand2 = cand1
                            .ExcludeOn(adjacentRowPositions, i => i.Y)
					        .ToArray();

                        //var candidates = 
                        //    availablePositions
                        //        .ExcludeOn(adjacentColumnPositions, i => i.X)
                        //        .ExcludeOn(adjacentRowPositions,    i => i.Y)
                        //        .ToArray();

                        if (cand2.Length == 1)
						{
                            var target = cand2[0];
						    var move = target.Apply(cellValue);
							Statistics.NeighbouringNumbersStrategyMoves++;
							return move;
						}
					}
				}
			}

            // give up
            return null;
        }

		private static IEnumerable<Cell> GetColumnPositions(Board board, Cell value, int gx)
		{
            var result =
                from c in board
                where c.Value == value.Value && c.XSection == gx
                select c;

            return result.ToArray();
		}

        private static IEnumerable<Cell> GetRowPositions(Board board, Cell value, int gy)
		{
            var result =
                from c in board
                where c.Value == value.Value && c.YSection == gy
                select c;

            return result.ToArray();
        }
	}
}