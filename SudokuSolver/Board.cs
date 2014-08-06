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
		public Board()
		{
		}

		public Board(IEnumerable<Cell> board)
			:base(board)
		{
		}

		public Cell[] GetColumnValues(int index)
		{
			var columnNumber = index % 9;
			var buffer = new List<Cell>();

			for (int i=0; i < 9; i++)
			{
				var value = this[columnNumber + (i*9)];
				buffer.Add(value);
			}

			return buffer.ToArray();
		}

		public Cell[] GetRowValues(int index)
		{
			var rowNumber = (index - (index%9))/9;

			var buffer = new List<Cell>();

			for (int i=0; i < 9; i++)
			{
				var value = this[(rowNumber*9) + i];
				buffer.Add(value);
			}

			return buffer.ToArray();
		}

		public Cell[] GetGridValues(int index)
		{
			int horiz = index%9;           // 16 -> 7
			int vert = (index - horiz)/9;  // 16 -> (16-7)/9 -> 1

			int gridStartH = horiz - (horiz%3); // 7 - 1 -> 6
			int gridStartV = vert - (vert%3);   // 1 - 1 -> 0

			var buffer = new List<Cell>();

			for (var x = 0; x < 3; x++)
			{
				for (var y = 0; y < 3; y++)
				{
					var value = this[9*(y + gridStartV) + x + gridStartH];
					buffer.Add(value);
				}
			}

			return buffer.ToArray();
		}

		public bool Validate()
		{
			for (int index = 0; index < this.Count; index++)
			{
				var columnValues = GetColumnValues(index).Where(i => i.IsAssigned()).ToArray();
				var rowValues = GetRowValues(index).Where(i => i.IsAssigned()).ToArray();
				var gridValues = GetGridValues(index).Where(i => i.IsAssigned()).ToArray();

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

		public string GetDump(int highlightIndex)
		{
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

		public Board Clone ()
		{
			Board clone = new Board (this);
			return clone;
		}
	}
}
