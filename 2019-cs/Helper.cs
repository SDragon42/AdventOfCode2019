﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent_of_Code
{
    static class Helper
    {
        /// <summary>
        /// Gets a single digit from the specified number (right to left).
        /// </summary>
        /// <param name="value">The number value.</param>
        /// <param name="position">The digit position, from right to left.</param>
        /// <returns></returns>
        public static int GetDigitRight(int value, int position)
        {
            var modifier = Convert.ToInt32(Math.Pow(10, position - 1));
            var result = (value / modifier) % 10;
            return result;
            //if (value == 0)
            //    return 0;
            //var numDigits = Convert.ToInt32(Math.Floor(Math.Log10(value) + 1));
            //if (position < 1)
            //    position = 1;
            //var offset = numDigits - position + 1;
            //var result = Math.Truncate(value / Math.Pow(10, numDigits - offset))
            //          - (Math.Truncate(value / Math.Pow(10, numDigits - offset + 1)) * 10);
            //return Convert.ToInt32(result);
        }
        /// <summary>
        /// Gets a single digit from the specified number (right to left).
        /// </summary>
        /// <param name="value">The number value.</param>
        /// <param name="position">The digit position, from right to left.</param>
        /// <returns></returns>
        public static int GetDigitRight(long value, int position)
        {
            var modifier = Convert.ToInt32(Math.Pow(10, position - 1));
            var result = (value / modifier) % 10;
            return (int)result;
        }

        /// <summary>
        /// Gets a single digit from the specified number (left to right).
        /// </summary>
        /// <param name="value">The number value.</param>
        /// <param name="position">The digit position, from left to right.</param>
        /// <returns></returns>
        public static int GetDigitLeft(int value, int position)
        {
            var result = Math.Truncate(value / Math.Pow(10, 6 - position)) - (Math.Truncate(value / Math.Pow(10, 6 - position + 1)) * 10);
            return Convert.ToInt32(result);
        }


        /// <summary>
        /// Returns a list of all possible combinations of the item list.
        /// (item list only tested with unique values)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sourced and modified from:
        /// https://stackoverflow.com/questions/5132758/words-combinations-without-repetition
        /// </remarks>
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items)
        {
            if (items.Count() == 1)
            {
                yield return new T[] { items.First() };
                yield break;
            }

            foreach (var item in items)
            {
                var nextItems = items.Where(i => !i.Equals(item));
                foreach (var result in GetPermutations(nextItems))
                    yield return new T[] { item }.Concat(result);
            }
        }

        public static string GetFileContent(string filename)
        {
            var fullPath = Path.Combine(@"..\..\..\Data", filename);
            var content = File.ReadAllText(fullPath);
            return content;
        }

        public static string[] GetFileContentAsLines(string filename)
        {
            var fullPath = Path.Combine(@"..\..\..\Data", filename);
            var lines = File.ReadAllLines(fullPath);
            return lines;
        }


        /// <summary>
        /// Finds the Greatest Common Factor between two numbers.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://stackoverflow.com/a/20824923/6136
        /// </remarks>
        public static long FindGCF(long a, long b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Find the Least Common Multiple of two numbers.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://stackoverflow.com/a/20824923/6136
        /// </remarks>
        public static long FindLCM(long a, long b)
        {
            return (a / FindGCF(a, b)) * b;
        }



        public static void DrawPointGrid2D<TValue>(IDictionary<Point, TValue> gridData, Func<TValue, string> DrawElementMethod = null, Rectangle? minArea = null)
        {
            var minX = gridData.Keys.Select(h => h.X).Min();
            var maxX = gridData.Keys.Select(h => h.X).Max();
            var minY = gridData.Keys.Select(h => h.Y).Min();
            var maxY = gridData.Keys.Select(h => h.Y).Max();

            if (minArea.HasValue)
            {
                var xDiff = minArea.Value.Width - Math.Abs(maxX - minX);
                var yDiff = minArea.Value.Height - Math.Abs(maxY - minY);

                if (xDiff > 0)
                {
                    var xPart = xDiff / 2;
                    minX -= xPart;
                    maxX += xDiff = xPart;
                }

                if (yDiff > 0)
                {
                    var yPart = yDiff / 2;
                    minY -= yPart;
                    maxY += yDiff = yPart;
                }
            }

            var text = new StringBuilder();
            for (int y = maxY; y >= minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var key = new Point(x, y);
                    var value = default(TValue);
                    if (gridData.ContainsKey(key))
                        value = gridData[key];

                    var element = DrawElementMethod?.Invoke(value) ?? value?.ToString() ?? " ";
                    text.Append(element);
                    //text.Append(DrawElementMethod(value));
                }
                text.AppendLine();
            }

            Console.WriteLine(text.ToString());
        }

        public static void DrawScreenGrid2D<TValue>(IDictionary<Point, TValue> gridData, Func<TValue, string> DrawElementMethod = null)
        {
            var minX = gridData.Keys.Select(h => h.X).Min();
            var maxX = gridData.Keys.Select(h => h.X).Max();
            var minY = gridData.Keys.Select(h => h.Y).Min();
            var maxY = gridData.Keys.Select(h => h.Y).Max();

            var text = new StringBuilder();
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var key = new Point(x, y);
                    var value = default(TValue);
                    if (gridData.ContainsKey(key))
                        value = gridData[key];

                    var element = DrawElementMethod?.Invoke(value) ?? value.ToString();
                    text.Append(element);
                    //text.Append(DrawElementMethod(value));
                }
                text.AppendLine();
            }

            Console.WriteLine(text.ToString());
        }
    }
}
