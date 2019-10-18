using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2018 {
    public class Day14 : ISolution {

        public string GetName() {
            return "Chocolate Charts";
        }

        public string SolvePart1(string input) {
            var num = int.Parse(input);
            var scoreboard = Simulate(s => s.Count < num + 10);
            return string.Join("", scoreboard.TakeLast(10));
        }

        public string SolvePart2(string input) {
            var amount = 1000000;
            while (true) {
                var a = amount;
                // It's a lot faster to construct a large scoreboard first and then find the index later
                var scoreboard = Simulate(s => s.Count < a);
                var index = string.Join("", scoreboard).IndexOf(input, StringComparison.Ordinal);
                if (index >= 0)
                    return index.ToString();
                amount *= 10;
                Console.WriteLine("Trying again with " + amount);
            }
        }

        private static List<int> Simulate(Func<List<int>, bool> continueFunc) {
            var scoreboard = new List<int>(new[] {3, 7});
            var current = new[] {0, 1};

            while (continueFunc(scoreboard)) {
                var sum = scoreboard[current[0]] + scoreboard[current[1]];
                foreach (var c in sum.ToString()) {
                    var digit = (int) char.GetNumericValue(c);
                    scoreboard.Add(digit);
                }

                for (var i = 0; i < current.Length; i++) {
                    var next = current[i] + scoreboard[current[i]] + 1;
                    current[i] = next % scoreboard.Count;
                }
            }
            return scoreboard;
        }

    }
}