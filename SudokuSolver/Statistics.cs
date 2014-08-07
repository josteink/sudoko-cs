using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public static class Statistics
    {
        public static int IntersectonStrategyMoves { get; set; }
        public static int NeighbouringNumbersStrategyMoves { get; set; }
		public static int TentativeStrategyMoves { get; set; }
		public static int TentativeStrategyUndos { get; set; }

        public static void Reset()
        {
            IntersectonStrategyMoves = 0;
            NeighbouringNumbersStrategyMoves = 0;
			TentativeStrategyMoves = 0;
			TentativeStrategyUndos = 0;
        }

        public static void DumpStats()
        {
            Console.WriteLine("L1: Intersecton-strategy:          {0}", IntersectonStrategyMoves);
            Console.WriteLine("L2: Neighbouring numbers-strategy: {0}", NeighbouringNumbersStrategyMoves);
			Console.WriteLine("L3: Tentative-strategy:            {0}", TentativeStrategyMoves);
			Console.WriteLine("L4: Tentative-strategy undo:       {0}", TentativeStrategyUndos);
        }
    }
}
