using System;

namespace AdventOfCode._2018 {
    public class Day5 : ISolution {

        public string GetName() {
            return "Alchemical Reduction";
        }

        public string SolvePart1(string input) {
            return React(input).Length.ToString();
        }

        public string SolvePart2(string input) {
            var lowest = int.MaxValue;
            for (var x = 'a'; x <= 'z'; x++) {
                var without = input.Replace(x.ToString(), "").Replace(char.ToUpperInvariant(x).ToString(), "");
                var result = React(without);
                if (result.Length < lowest)
                    lowest = result.Length;
            }
            return lowest.ToString();
        }

        private static string React(string input) {
            while (true) {
                var change = false;

                for (var i = 0; i < input.Length - 1; i++) {
                    var curr = input[i];
                    var next = input[i + 1];
                    if (char.ToLowerInvariant(curr) != char.ToLowerInvariant(next))
                        continue;
                    if (curr == next)
                        continue;
                    input = input.Remove(i, 2);
                    change = true;
                }

                if (!change)
                    break;
            }
            return input;
        }

    }
}