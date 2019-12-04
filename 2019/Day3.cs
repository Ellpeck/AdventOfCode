using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode._2019 {
    public class Day3 : ISolution {

        public string GetName() {
            return "Crossed Wires";
        }

        public string SolvePart1(string input) {
            var lines = input.Split("\n");
            var wire1 = ParseWire(lines[0]).ToArray();
            var wire2 = ParseWire(lines[1]).ToArray();

            var closest = Point.Empty;
            for (var i = 0; i < wire1.Length - 1; i++) {
                for (var j = i; j < wire2.Length - 1; j++) {
                    var inter = GetIntersection(wire1[i], wire1[i + 1], wire2[j], wire2[j + 1]);
                    if (!inter.HasValue || inter.Value == Point.Empty)
                        continue;
                    if (closest == Point.Empty || Manhattan(Point.Empty, inter.Value) < Manhattan(Point.Empty, closest))
                        closest = inter.Value;
                }
            }
            return Manhattan(Point.Empty, closest).ToString();
        }

        public string SolvePart2(string input) {
            var lines = input.Split("\n");
            var wire1 = ParseWire(lines[0]).ToArray();
            var wire2 = ParseWire(lines[1]).ToArray();

            var least = int.MaxValue;
            var steps1 = 0;
            for (var i = 0; i < wire1.Length - 1; i++) {
                var steps2 = 0;
                for (var j = 0; j < wire2.Length - 1; j++) {
                    var inter = GetIntersection(wire1[i], wire1[i + 1], wire2[j], wire2[j + 1]);
                    if (inter.HasValue && inter.Value != Point.Empty) {
                        var total = steps1 + steps2 + Manhattan(wire1[i], inter.Value) + Manhattan(wire2[j], inter.Value);
                        if (total < least)
                            least = total;
                    }
                    steps2 += Manhattan(wire2[j], wire2[j + 1]);
                }
                steps1 += Manhattan(wire1[i], wire1[i + 1]);
            }
            return least.ToString();
        }

        private static IEnumerable<Point> ParseWire(string input) {
            yield return Point.Empty;
            
            var instructions = input.Split(",");
            var curr = new Point();
            foreach (var instruction in instructions) {
                var dir = instruction[0];
                var amount = int.Parse(instruction.Substring(1));
                switch (dir) {
                    case 'R':
                        curr.Offset(amount, 0);
                        break;
                    case 'L':
                        curr.Offset(-amount, 0);
                        break;
                    case 'U':
                        curr.Offset(0, amount);
                        break;
                    case 'D':
                        curr.Offset(0, -amount);
                        break;
                }
                yield return curr;
            }
        }

        private static Point? GetIntersection(Point wire11, Point wire12, Point wire21, Point wire22) {
            var w11 = new Point(Math.Min(wire11.X, wire12.X), Math.Min(wire11.Y, wire12.Y));
            var w12 = new Point(Math.Max(wire11.X, wire12.X), Math.Max(wire11.Y, wire12.Y));
            var w21 = new Point(Math.Min(wire21.X, wire22.X), Math.Min(wire21.Y, wire22.Y));
            var w22 = new Point(Math.Max(wire21.X, wire22.X), Math.Max(wire21.Y, wire22.Y));

            if (w11.X == w12.X) {
                if (w21.X == w22.X)
                    return null;
                if (w21.X > w11.X || w22.X < w11.X)
                    return null;
                if (w11.Y > w21.Y || w12.Y < w21.Y)
                    return null;
                return new Point(w11.X, w21.Y);
            } else {
                if (w21.Y == w22.Y)
                    return null;
                if (w21.Y > w11.Y || w22.Y < w11.Y)
                    return null;
                if (w11.X > w21.X || w12.X < w21.X)
                    return null;
                return new Point(w21.X, w11.Y);
            }
        }

        private static int Manhattan(Point p1, Point p2) {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }

    }
}