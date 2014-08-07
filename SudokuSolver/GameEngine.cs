using System;
using System.Linq;

namespace SudokuSolver
{
	public static class GameEngine
	{
		public static int TotalMoves { get; set; }

		public static void Play(Board board)
		{
			var bestEffort = AttemptSolve (board);
			if (IsSolved (bestEffort))
			{
				Console.WriteLine("SOLVED!");
			}
			else
			{
				Console.WriteLine("UNSOLVED.");
			}
		}

		public static Board AttemptSolve(Board board)
		{
			bool keepTrying = true;
			int moveIndex = 0;
			Board currentBoard = board;

			while (keepTrying)
			{
				if (TotalMoves == 0)
				{
					DumpBoard(currentBoard, "Intial board:");
				}
				else
				{
					DumpBoard(currentBoard, "Current board (" + TotalMoves + " moves):", moveIndex);
				}

				currentBoard = OverallStrategy.Iterate(currentBoard, out moveIndex);

				// TODO:
				/*
				 * var moves = strategy.GetSolutionSpace(board);
				 * if (moves != null)
				 * {
				 *     newBoard = moves.Aggregate(board, i => i == null : null ? i.Apply);
				 * }
				 */

				// sanity-check
				var valid = currentBoard.Validate();
				if (valid)
				{
					keepTrying = moveIndex.IsSolved();
					TotalMoves++;
				}
				else
				{
					Console.WriteLine("ERROR! Board has been corrupted.");
					keepTrying = false;
				}
			}

			return currentBoard;
		}

		// TODO: Move to Board.cs
		public static bool IsSolved(Board board)
		{
			bool solved = board.All(i => i.IsAssigned());
			return solved;
		}

		private static void DumpBoard(Board board, string header, int highlightIndex = -1)
		{
			Console.WriteLine(header);

			var dump = board.GetDump(highlightIndex);
			Console.WriteLine(dump);
		}

	}
}