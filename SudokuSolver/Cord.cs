using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
	public class Cord
	{
		public int X;
		public int Y;

		public static Cord FromIndex(int index)
		{
			var x = index % 9;
			var y = (index - x) / 9;
			return new Cord
			{
				X = x,
				Y = y
			};
		}

		public int ToIndex()
		{
			return Y * 9 + X;
		}
	}

	public static class CordUtils
	{
		public static IEnumerable<TItem> IntersectOn<TItem, TIntersect>(this IEnumerable<TItem> list1, IEnumerable<TItem> list2, Func<TItem,TIntersect> valueGetter)
			where TIntersect : IEquatable<TIntersect>
		{
			var result = new List<TItem> ();



			foreach (var l1item in list1)
			{
				var valueL1Item = valueGetter (l1item);
				foreach (var l2item in list2)
				{
					var valueL2Item = valueGetter (l1item);

					if (valueL1Item.Equals(valueL2Item))
					{
						result.Add (l1item);
					}
				}
			}

			return result;
		}

		public static IEnumerable<TItem> IntersectOnOrig<TItem, TIntersect>(this IEnumerable<TItem> list1, IEnumerable<TItem> list2, Func<TItem,TIntersect> valueGetter)
			where TIntersect : IEquatable<TIntersect>
		{
			var result = new List<TItem> ();

			foreach (var l1item in list1)
			{
				var valueL1Item = valueGetter (l1item);
				foreach (var l2item in list2)
				{
					var valueL2Item = valueGetter (l1item);

					if (valueL1Item.Equals(valueL2Item))
					{
						result.Add (l1item);
					}
				}
			}

			return result;
		}

		//public static IEnumerable<Cord> IntersectExcept(this IEnumerable<Cord> list1, IEnumerable<Cord> list2, Func<Cord,int> valueGetter)
		public static IEnumerable<TItem> IntersectExcept<TItem, TIntersect>(this IEnumerable<TItem> list1, IEnumerable<TItem> list2, Func<TItem,TIntersect> valueGetter)
			where TIntersect : IEquatable<TIntersect>
		{
			// keep only items from list1 when value from valueGuetter does NOT collide with list2

			var l2values = list2.Select (valueGetter);

			var buffer = new List<TItem> ();
			foreach (var item in list1)
			{
				var value = valueGetter (item);
				if (!l2values.Contains(value))
			    {
					buffer.Add (item);
				}
			}

			return buffer;
		}
	}
}

