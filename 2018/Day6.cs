using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018 {
    public class Day6 : ISolution {

        public string GetName() {
            return "Chronal Coordinates";
        }

        public string SolvePart1(string input) {
            var grid = Parse(input).grid;

            var counts = new Dictionary<int, int>();
            var highest = 0;
            for (var x = 0; x < grid.GetLength(0); x++) {
                for (var y = 0; y < grid.GetLength(1); y++) {
                    var pos = grid[x, y];
                    if (pos == 0)
                        continue;

                    if (x == 0 || x == grid.GetLength(0) - 1 || y == 0 || y == grid.GetLength(1) - 1) {
                        counts[pos] = -1;
                        continue;
                    }

                    var curr = counts.ContainsKey(pos) ? counts[pos] : 0;
                    if (curr == -1)
                        continue;
                    counts[pos] = curr + 1;
                    if (curr + 1 > highest) {
                        highest = curr + 1;
                    }
                }
            }

            return highest.ToString();
        }

        public string SolvePart2(string input) {
            var parsed = Parse(input);

            var totalBelow = 0;
            for (var x = 0; x < parsed.grid.GetLength(0); x++) {
                for (var y = 0; y < parsed.grid.GetLength(1); y++) {
                    var total = 0;
                    foreach (var coord in parsed.coords) {
                        var dist = Manhattan(coord.X, coord.Y, x, y);
                        total += dist;
                    }
                    if (total < 10000)
                        totalBelow++;
                }
            }
            return totalBelow.ToString();
        }

        private static (int[,] grid, List<Coord> coords) Parse(string input) {
            var lines = input.Split('\n');
            var coords = new List<Coord>();
            foreach (var line in lines) {
                var split = line.Split(',');
                coords.Add(new Coord(coords.Count + 1, int.Parse(split[0]), int.Parse(split[1])));
            }

            var width = coords.Max(c => c.X);
            var height = coords.Max(c => c.Y);
            var grid = new int[width, height];

            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    var bestDist = 0;
                    Coord bestCoord = null;
                    foreach (var coord in coords) {
                        var dist = Manhattan(coord.X, coord.Y, x, y);
                        if (bestCoord == null || dist <= bestDist) {
                            bestDist = dist;
                            bestCoord = coord;
                        }
                    }
                    if (bestCoord != null) {
                        foreach (var coord in coords) {
                            if (coord == bestCoord)
                                continue;
                            var dist = Manhattan(coord.X, coord.Y, x, y);
                            if (dist == bestDist) {
                                grid[x, y] = 0;
                                bestCoord = null;
                                break;
                            }
                        }
                        if (bestCoord != null)
                            grid[x, y] = bestCoord.Id;
                    }
                }
            }
            return (grid, coords);
        }

        private static int Manhattan(int x1, int y1, int x2, int y2) {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        private class Coord {

            public readonly int Id;
            public readonly int X;
            public readonly int Y;

            public Coord(int id, int x, int y) {
                this.Id = id;
                this.X = x;
                this.Y = y;
            }

        }

    }
}