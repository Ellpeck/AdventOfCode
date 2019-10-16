using System.Collections.Generic;

namespace AdventOfCode._2018 {
    public class Day1 : ISolution {

        public string GetName() {
            return "Chronal Calibration";
        }

        public string SolvePart1(string input) {
            var sum = 0;
            foreach (var change in GetFrequencies(input))
                sum += change;
            return sum.ToString();
        }

        public string SolvePart2(string input) {
            var sum = 0;
            var already = new HashSet<int>();
            while (true) {
                foreach (var change in GetFrequencies(input)) {
                    sum += change;
                    if (already.Contains(sum)) {
                        return sum.ToString();
                    } else {
                        already.Add(sum);
                    }
                }
            }
        }

        private static IEnumerable<int> GetFrequencies(string input) {
            foreach (var line in input.Split('\n'))
                yield return int.Parse(line);
        }

    }
}