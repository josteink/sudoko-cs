using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            int _ = ValueUtils.Unassigned;
            int[] board =
            {
                _, 8, _,   4, _, 9,   6, 5, 3,
                6, 4, 2,   8, _, _,   _, 7, _,
                _, _, _,   _, _, _,   8, _, _,

                _, _, 7,   _, _, 5,   _, 4, 2,
                _, _, _,   7, _, 1,   _, _, _,
                8, 5, _,   6, _, _,   1, _, _,

                _, _, 6,   _, _, _,   _, _, _,
                _, 1, _,   _, _, 4,   7, 3, 6,
                2, 7, 3,   5, _, 8,   _, 1, _,
            };


            bool keepTrying = true;
            int moves = 0;
            while (keepTrying)
            {
                if (moves == 0)
                {
                    DumpBoard(board, "Intial board:");
                }
                else
                {
                    DumpBoard(board, "Current board (" + moves + " moves):");
                }
                board = Iterate(board, out keepTrying);
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

        private static void DumpBoard(int[] board, string header)
        {
            Console.WriteLine(header);

            var dump = board.GetDump();
            Console.WriteLine(dump);
        }

        private static int[] Iterate(int[] board, out bool keepTrying)
        {
            // apply defaults
            int[] result;

            result = IntersectionStrategy.Iterate(board, out keepTrying);
            if (keepTrying)
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
            keepTrying = false;
            return board;


        }
    }
}
