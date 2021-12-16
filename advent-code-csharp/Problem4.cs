using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent_code_csharp
{
    class BingoNumber
    {
        public int Number { get; set; }
        public bool IsMarked { get; set; }
        public BingoNumber(int n) => Number = n;

        public override string ToString() => Number.ToString();

        public static implicit operator BingoNumber(int n) => new BingoNumber(n);
        public static bool operator ==(BingoNumber n, int a) => n.Number == a;
        public static bool operator !=(BingoNumber n, int a) => n.Number != a;
    }


    class Board
    {
        public int Id { get; set; }
        public BingoNumber[][] Values { get; set; }
        public int[] RowMarks { get; set; }
        public int[] ColMarks { get; set; }
        public bool HasWon { get; set; }

        public void PrintRow(int row)
        {
            for (int i = 0; i < Values[row].Length; i++)
            {
                if (i != 0) Console.Write(" ");
                Console.Write(Values[row][i]);
            }
        }

        public void PrintColumn(int col)
        {
            for (int i = 0; i < Values.Length; i++)
            {
                if (i != 0) Console.Write(" ");
                Console.Write(Values[i][col]);
            }
        }
    }

    enum Part
    {
        ONE, TWO
    }

    public class Problem4
    {
        public static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch watch = new();
            watch.Start();

            string[] lines = FileUtils.ReadFile<string>(FileUtils.GetFileUrl(Type.INPUT, 4));
            int[] bingoNumbers = FileUtils.GetColumns<int>(lines[0], ',');
            List<Board> boards = new();
            InitializeBoard(boards, lines);

            (Board? winnerPart1, int victoryNumberPart1) = GetWinner(boards, bingoNumbers, Part.ONE);
            int sumUnmarkedPart1 = 0;
            foreach (var bingoArr in winnerPart1!.Values)
            {
                foreach (var bingoNumber in bingoArr)
                {
                    if (!bingoNumber.IsMarked) sumUnmarkedPart1 += bingoNumber.Number;
                }
            }

            InitializeBoard(boards, lines);
            (Board? winnerPart2, int victoryNumberPart2) = GetWinner(boards, bingoNumbers, Part.TWO);
            int sumUnmarkedPart2 = 0;
            foreach (var bingoArr in winnerPart2!.Values)
            {
                foreach (var bingoNumber in bingoArr)
                {
                    if (!bingoNumber.IsMarked) sumUnmarkedPart2 += bingoNumber.Number;
                }
            }

            watch.Stop();
            Console.WriteLine($"All unmarked numbers in board({sumUnmarkedPart1}) and number won multiplied({victoryNumberPart1}) = {sumUnmarkedPart1 * victoryNumberPart1}");
            Console.WriteLine($"All unmarked numbers in board({sumUnmarkedPart2}) and number won multiplied({victoryNumberPart2}) = {sumUnmarkedPart2 * victoryNumberPart2}");
            Console.WriteLine($"Execution time {watch.ElapsedMilliseconds}ms");
        }

        private static (Board?, int) GetWinner(in List<Board> boards, in int[] bingoNumbers, Part part)
        {
            foreach (var bingoNumber in bingoNumbers)
            {
                foreach (var board in boards)
                {
                    for (int i = 0; i < board.Values.Length; i++)
                    {
                        for (int j = 0; j < board.Values[i].Length; j++)
                        {
                            if (board.Values[i][j] == bingoNumber)
                            {
                                board.Values[i][j].IsMarked = true;
                                board.RowMarks[i] += 1;
                                board.ColMarks[j] += 1;
                                if (board.RowMarks[i] == board.RowMarks.Length)
                                {
                                    Console.WriteLine($"Bingo in a row! With n{bingoNumber}. Row numbers: ");
                                    board.PrintRow(i);
                                    Console.Write("\n\n");
                                    board.HasWon = true;
                                    if (part == Part.ONE) return (board, bingoNumber);
                                    if (boards.All(b => b.HasWon)) return (board, bingoNumber);
                                }
                                if (board.ColMarks[j] == board.ColMarks.Length)
                                {
                                    Console.WriteLine($"Bingo in a col! With n{bingoNumber}. Column numbers: ");
                                    board.PrintColumn(j);
                                    Console.Write("\n\n");
                                    board.HasWon = true;
                                    if (part == Part.ONE) return (board, bingoNumber);
                                    if (boards.All(b => b.HasWon)) return (board, bingoNumber);
                                }
                            }
                        }
                    }
                }
            }
            return (null, 0);
        }

        private static void InitializeBoard(List<Board> boards, in string[] lines)
        {
            bool initializeRowsCols = false;
            int boardLine = 0;
            Board? board = null;
            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i] == "")
                {
                    if (board != null) boards.Add(board);
                    board = new();
                    boardLine = 0;
                    board.Id = i;
                    initializeRowsCols = true;
                    continue;
                }

                if (board == null) continue;

                string[] boardNumbers = lines[i].Split(' ');

                if (initializeRowsCols)
                {
                    board.Values = new BingoNumber[boardNumbers.Length][];
                    board.RowMarks = new int[boardNumbers.Length];
                    board.ColMarks = new int[boardNumbers.Length];
                    initializeRowsCols = false;
                }

                for (int j = 0; j < boardNumbers.Length; j++)
                {
                    board.Values[boardLine] ??= new BingoNumber[boardNumbers.Length];
                    board.Values[boardLine][j] = FileUtils.CastToValue<int>(boardNumbers[j]);
                }
                boardLine++;
            }
            if (board != null && !boards.Contains(board)) boards.Add(board);
        }
    }
}
