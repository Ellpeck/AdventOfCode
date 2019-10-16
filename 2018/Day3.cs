using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2018 {
    public class Day3 : ISolution {

        public string GetName() {
            return "No Matter How You Slice It";
        }

        public string SolvePart1(string input) {
            var claims = GetClaims(input).ToArray();
            var width = claims.Max(c => c.X + c.Width);
            var height = claims.Max(c => c.Y + c.Height);

            var grid = new int[width, height];
            var moreThanOne = 0;
            foreach (var claim in claims) {
                for (var xOff = 0; xOff < claim.Width; xOff++) {
                    for (var yOff = 0; yOff < claim.Height; yOff++) {
                        var x = claim.X + xOff;
                        var y = claim.Y + yOff;
                        grid[x, y]++;
                        if (grid[x, y] == 2)
                            moreThanOne++;
                    }
                }
            }
            return moreThanOne.ToString();
        }

        public string SolvePart2(string input) {
            var claims = GetClaims(input).ToArray();
            foreach (var claim1 in claims) {
                var overlaps = false;
                foreach (var claim2 in claims) {
                    if (claim1 == claim2)
                        continue;
                    var rect1 = new Rectangle(claim1.X, claim1.Y, claim1.Width, claim1.Height);
                    var rect2 = new Rectangle(claim2.X, claim2.Y, claim2.Width, claim2.Height);
                    if (rect1.IntersectsWith(rect2)) {
                        overlaps = true;
                        break;
                    }
                }
                if (!overlaps)
                    return claim1.Id.ToString();
            }
            return "";
        }

        private static IEnumerable<Claim> GetClaims(string input) {
            var matches = Regex.Matches(input, @"#(\d*) @ (\d*),(\d*): (\d*)x(\d*)");
            foreach (Match match in matches) {
                yield return new Claim(int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value),
                    int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value));
            }
        }

        private class Claim {

            public readonly int Id;
            public readonly int X;
            public readonly int Y;
            public readonly int Width;
            public readonly int Height;

            public Claim(int id, int x, int y, int width, int height) {
                this.Id = id;
                this.X = x;
                this.Y = y;
                this.Width = width;
                this.Height = height;
            }

        }

    }
}