using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
	public static class BoardParser
	{
		public static List<Board> GetFromFile(string fileName)
		{
			string boardContents = File.ReadAllText(fileName);
			var boards = Parse(boardContents);
			return boards;
		}

		public static List<Board> Parse(string contents)
		{
			contents = FilterComments(contents);

			var allBoards = new List<Board>();
			var boardBuffer = new List<Cell>();

		    int index = 0;
			var tokens = contents.Split(new char[] {'\n', ' ', '\t', '\r'}, StringSplitOptions.RemoveEmptyEntries);
			foreach (var token in tokens)
			{
				int value;
				bool isInt = int.TryParse(token, out value);
			    var cellValue = isInt ? value : Cell.Unassigned;

                boardBuffer.Add(new Cell(cellValue, index));


                if (boardBuffer.Count == Board.BoardSize)
				{
					var newBoard = new Board (boardBuffer);
					if (! newBoard.IsTemplate())
					{
						allBoards.Add(newBoard);
					}
					boardBuffer.Clear();
				    index = 0;
				}

			    index++;
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
	}
}

