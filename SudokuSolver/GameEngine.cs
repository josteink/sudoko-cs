using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
	public class GameEngine
	{
		public static int TotalMoves { get; set; }

		public static void Play(Board board)
		{
		    var strategy = new GameEngine();

            var bestEffort = strategy.Solve(board);
			if (bestEffort.IsSolved())
			{
				Console.WriteLine("SOLVED!");
			}
			else
			{
				Console.WriteLine("UNSOLVED.");
			}
		}

		public Board Solve(Board board)
		{
		    IStrategy strategy = new OverallStrategy();
			bool success = true;
			Board currentBoard = board;

            DumpBoard(currentBoard, "Intial board:");

		    do
		    {
		        var move = strategy.Solve(currentBoard);

		        success = (move != null);
		        if (success)
		        {
		            var newBoard = currentBoard.Apply(move);


                    // sanity-check
                    var valid = newBoard.Validate();
                    if (!valid)
                    {
                        Console.WriteLine("ERROR! Board has been corrupted.");
                        success = false;
                    }
                    else
                    {
                        currentBoard = newBoard;
                        TotalMoves++;
                        string msg = string.Format("Current board (Total {0} moves):", TotalMoves);
                        DumpBoard(currentBoard, msg, move);
                    }
                }

		    } while (success);

			return currentBoard;
		}

		private static void DumpBoard(Board board, string header, Cell highlightCell = null)
		{
			Console.WriteLine(header);

            var dump = board.GetDump(highlightCell);
			Console.WriteLine(dump);
		}

	}
}