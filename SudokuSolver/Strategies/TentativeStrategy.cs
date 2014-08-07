using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
	public class TentativeStrategy : IStrategy
	{
        /*
    Current board (14 moves):


      9 8                
      3 2 7  1 9      8  
        5 4    8 2  9    

      7 4    6        9  
      2 9                
        1 3  9   7       

      4        5    2   9
      5 6 9  2 1 4  8 7 3
          2      9  5   4

    NeighbouringNumbersStrategy fails here.

    We have several places where we can attempt one of "few" options, and see if that works out.

    Like from

      9 8                
      3 2 7 
        5 4 

    To

      9 8 1              
      3 2 7 
        5 4 



             */
        public static int NestingLevel = 0;
		public const int MaxNesting = 5;

        public Cell Solve(Board board)
        {
            if (NestingLevel >= MaxNesting)
            {
                return null;
            }

            using (new NestingContainer())
            {
                return IterateNested(board);
            }
        }

		private Cell IterateNested(Board board)
		{
			// build map
		    var solutions =
		        from c in board
		        where !c.IsAssigned()
		        let cands = IntersectionStrategy.GetValueCandidates(board, c)
                where cands.Length > 0
		        select new
		        {
		            Cell = c,
		            Candidates = cands
		        };
		    var candidateMap = solutions.ToDictionary(i => i.Cell.Index, i => i.Candidates);

			// find a candidate for a "what if" operation
			if (candidateMap.Keys.Count == 0)
			{
                // give up
			    return null;
			}

			var lowestCount = candidateMap.Values.Min (i => i.Length);
			var firstLow = candidateMap.First (i => i.Value.Length == lowestCount);

			// apply
			var whatIfCell = board[firstLow.Key];
			var whatIfCandidates = firstLow.Value;

			// verify with recursion.
			foreach (var whatIfCand in whatIfCandidates)
			{
				Statistics.TentativeStrategyMoves++;

			    var newValue = whatIfCell.Apply(whatIfCand);
                var newBoard = board.Apply(newValue);

			    var subEngine = new GameEngine();
                var result = subEngine.Solve(newBoard);
				if (result != null && result.IsSolved())
				{
                    return newValue;
				}

				Statistics.TentativeStrategyUndos++;
			}

		    return null;
		}

		private class NestingContainer : IDisposable
		{
			public NestingContainer()
			{
				NestingLevel++;
			}

			public void Dispose()
			{
				NestingLevel--;
			}
		}
	}
}

