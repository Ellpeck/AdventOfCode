using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode._2018 {
    public class Day10 : ISolution {

        public string GetName() {
            return "The Stars Align";
        }

        public string SolvePart1(string input) {
            return Simulate(input).result;
        }

        public string SolvePart2(string input) {
            return Simulate(input).seconds.ToString();
        }

        private static (string result, int seconds) Simulate(string input) {
            var points = new List<Point>();
            var matches = Regex.Matches(input, @"position=<[ ]*(-?\d+),[ ]*(-?\d+)> velocity=<[ ]*(-?\d+),[ ]*(-?\d+)>");
            foreach (Match match in matches) {
                points.Add(new Point(
                    int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
            }

            var seconds = 0;
            while (true) {
                seconds++;

                foreach (var point in points) {
                    point.X += point.VelX;
                    point.Y += point.VelY;
                }

                var minX = int.MaxValue;
                var minY = int.MaxValue;
                var maxX = int.MinValue;
                var maxY = int.MinValue;
                foreach (var point in points) {
                    if (point.X < minX)
                        minX = point.X;
                    if (point.X > maxX)
                        maxX = point.X;
                    if (point.Y < minY)
                        minY = point.Y;
                    if (point.Y > maxY)
                        maxY = point.Y;
                }

                if (maxX - minX <= 500 && maxY - minY <= 500) {
                    var neighbors = 0;
                    foreach (var point in points) {
                        if (points.Any(p => p != point && p.IsNeighboring(point)))
                            neighbors++;
                    }
                    if (neighbors >= points.Count) {
                        var result = new StringBuilder();
                        for (var y = minY; y <= maxY; y++) {
                            for (var x = minX; x <= maxX; x++) {
                                result.Append(points.Any(p => p.X == x && p.Y == y) ? "X" : " ");
                            }
                            result.AppendLine();
                        }
                        return (result.ToString(), seconds);
                    }
                }
            }
        }

        private class Point {

            public int X;
            public int Y;
            public readonly int VelX;
            public readonly int VelY;

            public Point(int x, int y, int velX, int velY) {
                this.X = x;
                this.Y = y;
                this.VelX = velX;
                this.VelY = velY;
            }

            public bool IsNeighboring(Point other) {
                return Math.Abs(other.X - this.X) <= 1 && Math.Abs(other.Y - this.Y) <= 1;
            }

        }

    }
}