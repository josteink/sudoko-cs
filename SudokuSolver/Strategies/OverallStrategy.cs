using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace SudokuSolver
{
	public class OverallStrategy : IStrategy
	{
	    public Cell Solve(Board board)
	    {
	        var strategies = new IStrategy[]
	        {
	            new IntersectionStrategy(),
	            new NeighbouringNumbersStrategy(),
	            new TentativeStrategy()
	        };

	        foreach (var strategy in strategies)
	        {
	            var result = strategy.Solve(board);
	            if (result != null)
	            {
	                return result;
	            }
	        }

	        return null;
	    }
	}
}

