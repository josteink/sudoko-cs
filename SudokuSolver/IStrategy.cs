using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public interface IStrategy
    {
        // TODO later
        //IEnumerable<Cell> GetSolutions(Board board);
        Cell Solve(Board board);
    }
}
