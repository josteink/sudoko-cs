using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace SudokuSolver
{
    public class IntersectionStrategy : IStrategy
    {
        public Cell Solve(Board board)
        {
            // results
            var solutions =
                from c in board
                where !c.IsAssigned()
                let cands = GetValueCandidates(board, c)
                where cands.Length == 1
                let cand = cands.First()
                select c.Apply(cand);

            //var result = solutions.ToArray();
            var result = solutions.FirstOrDefault();
            if (result != null)
            {
                Statistics.IntersectonStrategyMoves ++;
            }
            return result;
        }

		public static Cell[] GetValueCandidates(Board board, Cell cell)
		{
		    var columnValues = board.GetAssignedColumnCells(cell);
		    var rowValues =    board.GetAssignedRowCells(cell);
		    var gridValues = board.GetGridCells(cell)
		        .Where(i => i.IsAssigned())
		        .ToArray();

			var all = Cell.All;
            // BUG: "all" keeps bad positional value, corrupting board.

			var missingColumnValues = all.Except(columnValues).ToArray();
			var missingRowValues =    all.Except(rowValues)   .ToArray();
			var missingGridValues =   all.Except(gridValues)  .ToArray();

		    var candidates = missingColumnValues
		        .Intersect(missingRowValues)
		        .Intersect(missingGridValues)
		        .ToArray();

			return candidates;
		}
    }
}
