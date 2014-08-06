using System;
using System.Linq;

namespace SudokuSolver
{
	public static class GameEngine
	{
		public static void Play(Board board)
		{
			bool keepTrying = true;
			int moves = 0;
			int moveIndex = 0;
			Board currentBoard = board;

			while (keepTrying)
			{
				if (moves == 0)
				{
					DumpBoard(currentBoard, "Intial board:");
				}
				else
				{
					DumpBoard(currentBoard, "Current board (" + moves + " moves):", moveIndex);
				}

				currentBoard = OverallStrategy.Iterate(currentBoard, out moveIndex);

				// sanity-check
				var valid = currentBoard.Validate();
				if (valid)
				{
					keepTrying = moveIndex.IsSolved();
					moves++;
				}
				else
				{
					Console.WriteLine("ERROR! Board has been corrupted.");
					keepTrying = false;
				}
			}

			bool unsolved = currentBoard.Any(i => !i.IsAssigned());
			if (unsolved)
			{
				Console.WriteLine("UNSOLVED. Gave up after {0} moves.", moves);
			}
			else
			{
				Console.WriteLine("SOLVED! Finished after {0} moves.", moves);
			}
		}

		private static void DumpBoard(Board board, string header, int highlightIndex = -1)
		{
			Console.WriteLine(header);

			var dump = board.GetDump(highlightIndex);
			Console.WriteLine(dump);
		}

	}
}