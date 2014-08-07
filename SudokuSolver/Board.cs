using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	/// <summary>
	/// Type-alias for Board to provide type-protection.
	/// </summary>
	public class Board : List<Cell>
	{
        public const int GridSize = 3;
        public const int RowSize = GridSize * GridSize;
        public const int CellSize = GridSize * RowSize;
        public const int BoardSize = GridSize * CellSize;

		public Board()
		{
		}

		public Board(IEnumerable<Cell> board)
			:base(board)
		{
		}

		private Cell[] GetColumn(Cell cell)
		{
            var columnValues =
                from c in this
                where c.X == cell.X
                select c;

		    return columnValues.ToArray();
		}

        public Cell[] GetAssignedColumnCells(Cell cell)
			{
            return GetColumn(cell)
                .Where(i => i.IsAssigned())
                .ToArray();
			}

        public Cell[] GetUnassignedColumnCells(Cell cell)
	    {
            return GetColumn(cell)
                .Where(i => !i.IsAssigned())
                .ToArray();
		}

		private Cell[] GetRow(Cell cell)
		{
            var rowValues =
                from c in this
                where c.IsAssigned() && c.Y == cell.Y
                select c;

		    return rowValues.ToArray();
		}

        public Cell[] GetAssignedRowCells(Cell cell)
			{
            return GetRow(cell)
                .Where(i => i.IsAssigned())
                .ToArray();
			}

        public Cell[] GetUnassignedRowCells(Cell cell)
        {
            return GetRow(cell)
                .Where(i => !i.IsAssigned())
                .ToArray();
		}

	    public Cell[] GetGridCells(int gx, int gy)
		{
	        var xStart = gx*Board.GridSize;
	        var yStart = gy*Board.CellSize;

	        var targetIndex = xStart + yStart;
	        var targetCell = new Cell(Cell.Unassigned, targetIndex);

            return GetGridCells(targetCell);
	    }

        public Cell[] GetGridCells(Cell cell)
		{
            var gridValues =
                from c in this
                where c.XSection == cell.XSection
                      && c.YSection == cell.YSection
                select c;

		    return gridValues.ToArray();
		}

	    public Board Apply(Cell move)
			{
	        if (move.Index < 0)
				{
	            throw new InvalidOperationException();
			}

	        var result = Clone();
	        result[move.Index] = move;

            // TODO: post-validate board-state
            // (avoid incompatible intersections like
            // 12345678_ and (missing 9)
            // 23456789_     (missing 1)

	        return result;
		}

		public bool Validate()
		{
		    foreach (var cell in this)
			{
		        var columnValues = GetAssignedColumnCells(cell);
		        var rowValues = GetAssignedRowCells(cell);
		        var gridValues = GetGridCells(cell)
                    .Where(i => i.IsAssigned())
                    .ToArray();

				bool valid = true;
				valid &= ValidateAllAsignedUnique(columnValues);
				valid &= ValidateAllAsignedUnique(rowValues);
				valid &= ValidateAllAsignedUnique(gridValues);

				if (!valid)
				{
					return false;
				}
			}

			return true;
		}

		private static bool ValidateAllAsignedUnique(Cell[] values)
		{
			var numItems = values.Length;
			var numUniqueItems = values.Distinct().Count();

			return numItems == numUniqueItems;
		}

		private const string Highlighter = "=";

		public string GetDump(Cell highlightCell)
		{
		    var highlightIndex = highlightCell == null ? -1 : highlightCell.Index;

			var buffer = new StringBuilder();

			for (int index = 0; index < this.Count; index++)
			{

				if (index.IsNewGridRow())
				{
					buffer.AppendLine();
				}
				// NOT else if and not AND. we want double-spacing for new grid rows.
				if (index.IsNewRow())
				{
					buffer.AppendLine();
				}

				bool isHighlightIndex = (index == highlightIndex);
				bool previousWasHighlight = (index - 1) == highlightIndex && !index.IsNewRow();
				var interSpacer = isHighlightIndex || previousWasHighlight ? Highlighter : " ";

				// normal new column
				if (index.IsNewColumn() && !previousWasHighlight)
				{
					buffer.Append(" ");
					buffer.Append(interSpacer);
				}
				else if (index.IsNewColumn() && previousWasHighlight)
				{
					buffer.Append(interSpacer);
					buffer.Append(" ");
				}
				else
				{
					buffer.Append(interSpacer);
				}

				var value = this[index];
				var textValue = value.ToString();
				buffer.Append(textValue);

				if ((index == highlightIndex) && (index + 1).IsNewRow())
				{
					buffer.Append(Highlighter);
				}
			}

			buffer.AppendLine();
			buffer.AppendLine();

			return buffer.ToString();
		}

		public bool IsTemplate()
		{
			return this.All (i => !i.IsAssigned());
		}

        public bool IsSolved()
        {
            bool solved = this.All(i => i.IsAssigned());
            return solved;
        }

		public Board Clone ()
		{
			Board clone = new Board (this);
			return clone;
		}
	}
}
