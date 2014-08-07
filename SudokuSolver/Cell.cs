using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SudokuSolver
{
    [System.Diagnostics.DebuggerDisplay("{Value} ({X},{Y})")]
    public class Cell : IEquatable<Cell>
	{
        public const int Unassigned = -1;

        public int Value { get; set; }
        public int Index { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

	    public int XSection
	    {
	        get { return (X - (X%Board.GridSize))/Board.GridSize; }
	    }
        public int YSection
        {
            get { return (Y - (Y % Board.GridSize)) / Board.GridSize; }
        }

	    public Cell(int value)
            : this(value, IndexUtils.Unsolved)
	    {
	    }

        public Cell(int value, int index)
		{
			Value = value;
            Index = index;
            X = IndexUtils.GetX(index);
            Y = IndexUtils.GetY(index);
		}

		public bool IsAssigned()
		{
			return Value != Unassigned;
		}

	    public Cell Apply(Cell applicant)
	    {
            // pre-validate move
            if (IsAssigned())
            {
                throw new InvalidOperationException();
            }

	        var result = new Cell(applicant.Value, Index);
	        return result;
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

        #region Equality

        // TODO: override == operator.

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

        #endregion Equality

        public static Cell[] All
		{
			get
			{
			    const int minValue = 1;
                const int maxValue = Board.GridSize * Board.GridSize;

			    var buffer = new List<int>();
			    for (var i = minValue; i <= maxValue; i++)
			    {
			        buffer.Add(i);
			    }

                var all = buffer.Select(i => new Cell(i));
				return all.ToArray ();
			}
		}
	}
}
