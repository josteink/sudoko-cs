using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = args.FirstOrDefault();
            fileName = fileName ?? "Boards.txt";

			var boards = BoardParser.GetFromFile (fileName);

            foreach (var currentBoard in boards)
            {
				GameEngine.Play(currentBoard);
				Statistics.DumpStats();
				Statistics.Reset();
				Console.ReadLine();
            }
        }
    }
}
