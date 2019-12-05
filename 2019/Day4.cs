using System;
using System.Linq;

namespace AdventOfCode._2019 {
    public class Day4 : ISolution {

        public string GetName() {
            return "Secure Container";
        }

        public string SolvePart1(string input) {
            return Compute(input, 6);
        }

        public string SolvePart2(string input) {
            return Compute(input, 2);
        }

        private static string Compute(string input, int maxGroupSize) {
            var minmax = input.Split("-").Select(int.Parse).ToArray();
            var total = 0;
            for (var pass = minmax[0]; pass < minmax[1]; pass++) {
                var passStrg = pass.ToString();
                var match = false;
                // There should be a group greater than 1 and less than the max size
                for (var i = 0; i < passStrg.Length;) {
                    var group = 1;
                    for (var j = 1; j < passStrg.Length - i; j++) {
                        if (passStrg[i] == passStrg[i + j]) {
                            group++;
                        }
                    }
                    if (group >= 2 && group <= maxGroupSize)
                        match = true;
                    i += group;
                }
                // Each number should be greater or equal the next
                for (var i = 0; i < passStrg.Length - 1; i++) {
                    if (passStrg[i] > passStrg[i + 1]) {
                        match = false;
                        break;
                    }
                }
                if (match)
                    total++;
            }
            return total.ToString();
        }

    }
}