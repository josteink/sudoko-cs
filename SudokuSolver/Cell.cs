using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public class Cell : IEquatable<Cell>
	{
		public Cell(int value)
		{
			Value = value;
		}

		public const int Unassigned = -1;

		public int Value { get; set; }

		public bool IsAssigned()
		{
			return Value != Unassigned;
		}

		public override string ToString()
		{
			if (!IsAssigned())
			{
				return " ";
			}
			else
			{
				return Value.ToString();
			}
		}

		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}

			return Equals ((Cell)other);
		}

		public bool Equals(Cell other)
		{
			if (other == null)
			{
				return false;
			}

			return this.Value == other.Value;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode ();
		}
	}
}
