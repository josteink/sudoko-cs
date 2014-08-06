using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = args.FirstOrDefault();
            fileName = fileName ?? "Boards.txt";

            string boardContents = File.ReadAllText(fileName);
            List<int[]> boards = ParseBoards(boardContents);

            foreach (var currentBoard in boards)
            {
                bool keepTrying = true;
                int moves = 0;
                int moveIndex = 0;
                int[] board = currentBoard;

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
        }

        private static List<int[]> ParseBoards(string contents)
        {
            contents = FilterComments(contents);

            var allBoards = new List<int[]>();
            var boardBuffer = new List<int>();

            int _ = ValueUtils.Unassigned;
            var tokens = contents.Split(new char[] {'\n', ' ', '\t', '\r'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens)
            {
                int value;
                bool isInt = int.TryParse(token, out value);
                if (isInt)
                {
                    boardBuffer.Add(value);
                }
                else
                {
                    boardBuffer.Add(ValueUtils.Unassigned);
                }

                if (boardBuffer.Count == 9*9)
                {
                    allBoards.Add(boardBuffer.ToArray());
                    boardBuffer.Clear();
                }
            }

            if (boardBuffer.Count != 0)
            {
                Console.WriteLine("{0} tokens ignored in input-data. Not enough to make a dull board.",
                    boardBuffer.Count);
            }

            return allBoards;
        }

        private static string FilterComments(string contents)
        {
            string[] lines = contents.Split('\n');

            var remaining = from l in lines
                let cleaned = l.Trim()
                where !cleaned.StartsWith("#")
                select cleaned;

            var newContent = string.Join("\n", remaining.ToArray());
            return newContent;
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
