using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
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
	public static class TentativeStrategy
	{
		public static int NestingLevel = 0;
		public const int MaxNesting = 5;

		public static Board Iterate(Board board, out int moveIndex)
		{
			if (NestingLevel >= MaxNesting) {
				moveIndex = IndexUtils.Unsolved;
				return board;
			}

			using (new NestingContainer())
			{
				return IterateNested (board, out moveIndex);
			}
		}

		private static Board IterateNested(Board board, out int moveIndex)
		{
			// build map
			var candidateMap = new Dictionary<int,Cell[]> ();
			for (int index = 0; index < board.Count; index++)
			{
				var value = board[index];
				if (value.IsAssigned())
				{
					continue;
				}

				var candidates = IntersectionStrategy.GetValueCandidates(board, index);
				candidateMap [index] = candidates;
			}

			// find a candidate for a "what if" operation
			if (candidateMap.Keys.Count == 0)
			{
				moveIndex = IndexUtils.Unsolved;
				return board;
			}
			var lowestCount = candidateMap.Values.Min (i => i.Length);
			var firstLow = candidateMap.First (i => i.Value.Length == lowestCount);

			// apply
			var whatIfIndex = firstLow.Key;
			var whatIfValues = firstLow.Value;

			// verify with recursion.
			foreach (var whatIfValue in whatIfValues)
			{
				Statistics.TentativeStrategyMoves++;

				var newBoard = board.Clone ();
				newBoard [whatIfIndex] = whatIfValue;

				var result = GameEngine.AttemptSolve (newBoard);
				if (GameEngine.IsSolved(result))
				{
					moveIndex = whatIfIndex;
					return result;
				}

				Statistics.TentativeStrategyUndos++;
			}

			moveIndex = IndexUtils.Unsolved;
			return board;
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

