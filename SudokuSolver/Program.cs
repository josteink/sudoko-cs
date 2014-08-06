using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            fileName = fileName ?? "Board.txt";

            string boardContents = File.ReadAllText(fileName);
            int[] board = ParseBoard(boardContents);

            bool keepTrying = true;
            int moves = 0;
            int moveIndex = 0;

            while (keepTrying)
            {
                if (moves == 0)
                {
                    DumpBoard(board, "Intial board:");
                }
                else
                {
                    DumpBoard(board, "Current board (" + moves + " moves):", moveIndex);
                }

                board = Iterate(board, out moveIndex);

                keepTrying = moveIndex.IsSolved();

                moves++;

                var success = board.Validate();
                if (!success)
                {
                    Console.WriteLine("ERROR! Board has been corrupted.");
                    keepTrying = false;
                }
            }

            bool unsolved = board.Any(i => !i.IsAssigned());
            if (unsolved)
            {
                Console.WriteLine("UNSOLVED. Gave up after {0} moves.", moves);
            }
            else
            {
                Console.WriteLine("SOLVED! Finished after {0} moves.", moves);
            }

            Console.ReadLine();
        }

        private static int[] ParseBoard(string contents)
        {
            var buffer = new List<int>();

            int _ = ValueUtils.Unassigned;

            var items = contents.Split(new char[] {'\n', ' ', '\t', '\r'}, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length != 9*9)
            {
                throw new Exception("Provided board does not contain a 9*9 grid.");
            }

            foreach (var item in items)
            {
                int value;
                bool isInt = int.TryParse(item, out value);
                if (isInt)
                {
                    buffer.Add(value);
                }
                else
                {
                    buffer.Add(ValueUtils.Unassigned);
                }
            }

            return buffer.ToArray();
        }

        private static void DumpBoard(int[] board, string header, int highlightIndex = -1)
        {
            Console.WriteLine(header);

            var dump = board.GetDump(highlightIndex);
            Console.WriteLine(dump);
        }

        private static int[] Iterate(int[] board, out int moveIndex)
        {
            // apply defaults
            int[] result;

            result = IntersectionStrategy.Iterate(board, out moveIndex);
            if (moveIndex.IsSolved())
            {
                return result;
            }

            //// TODO
            //result = NeighbouringNumbersStrategy.Iterate(board, out keepTrying);
            //if (!keepTrying)
            //{
            //    return result;
            //}

            // give up
            moveIndex = ValueUtils.Unsolved;
            return board;


        }
    }
}
