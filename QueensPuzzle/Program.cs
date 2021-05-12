using System;
using System.Collections.Generic;
using System.Linq;

namespace QueensPuzzle
{
    public class Program
    {
        public static void Main()
        {
            new QueensPuzzle(8, 8).Solution();
            Console.ReadKey();
        }
    }
    /// <summary>
    /// 皇后問題
    /// </summary>
    public class QueensPuzzle
    {
        /// <summary>
        /// 空座標
        /// </summary>
        public List<(int, int)> EmptyPosition { get; set; }
        /// <summary>
        /// 已放置座標
        /// </summary>
        public List<(int, int)> PlacedPosition { get; set; }
        /// <summary>
        /// 皇后數量
        /// </summary>
        private int QueensQuantity { get; set; }
        /// <summary>
        /// 邊界極限
        /// </summary>
        private int BordersMax { get; set; }

        /// <summary>
        /// 建立皇后問題
        /// </summary>
        /// <param name="queenQuantity">皇后數量</param>
        /// <param name="gridMax">方格邊界</param>
        public QueensPuzzle(int queenQuantity, int gridMax)
        {
            EmptyPosition = Enumerable.Range(1, gridMax)
                .SelectMany(x => Enumerable.Range(1, gridMax)
                .Select(y => (x, y))).ToList();
            PlacedPosition = new List<(int, int)>();
            QueensQuantity = queenQuantity;
            BordersMax = gridMax;
        }

        /// <summary>
        /// 建立皇后問題
        /// </summary>
        /// <param name="puzzle">皇后問題</param>
        /// <param name="position">座標</param>
        /// <returns></returns>
        public QueensPuzzle(QueensPuzzle puzzle, (int, int) position)
        {
            EmptyPosition = new List<(int, int)>(puzzle.EmptyPosition);
            PlacedPosition = new List<(int, int)>(puzzle.PlacedPosition);
            QueensQuantity = puzzle.QueensQuantity;
            BordersMax = puzzle.BordersMax;

            PlacedPosition.Add(position);
            var pX = position.Item1;
            var pY = position.Item2;
            EmptyPosition.RemoveAll(x => x.Item1 == pX);
            EmptyPosition.RemoveAll(x => x.Item2 == pY);
            EmptyPosition.RemoveAll(x => (x.Item1 - x.Item2) == (pX - pY));
            EmptyPosition.RemoveAll(x => (x.Item1 + x.Item2) == (pX + pY));
        }

        /// <summary>
        /// 完成 => 皇后數量=已放置數量
        /// </summary>
        private bool Finished => PlacedPosition.Count() == QueensQuantity;
        /// <summary>
        /// 未完成 => 空格>=皇后數量-已放置數量
        /// </summary>
        private bool Unfinished => QueensQuantity - PlacedPosition.Count() >= 0;

        /// <summary>
        /// 印出結果
        /// </summary>
        public void Print()
        {
            for (var xAxis = 1; xAxis <= BordersMax; xAxis++)
            {
                for (var yAxis = 1; yAxis <= BordersMax; yAxis++)
                {
                    //皇后座標
                    if (PlacedPosition.Contains((xAxis, yAxis)))
                    {
                        Console.Write("Q");
                    }
                    //空座標
                    else if (EmptyPosition.Contains((xAxis, yAxis)))
                    {
                        Console.Write("-");
                    }
                    //不可用座標
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 解法
        /// </summary>
        public void Solution()
        {
            GetSolution(this);
        }

        /// <summary>
        /// 解法編號
        /// </summary>
        private int count = 1;
        /// <summary>
        /// 取得解法
        /// </summary>
        /// <param name="puzzle"></param>
        public void GetSolution(QueensPuzzle puzzle)
        {
            foreach (var position in puzzle.EmptyPosition
                .Where(x => x.Item1 > puzzle.PlacedPosition.Count())
                .OrderBy(x => x))
            {
                //將每個皇后放置後的狀況視為分支
                var branch = new QueensPuzzle(puzzle, position);
                if (branch.Finished)
                {
                    Console.WriteLine($"// Solution {count++}");
                    branch.Print();
                }
                else if (branch.Unfinished)
                {
                    GetSolution(branch);
                }
            }
        }
    }
}
