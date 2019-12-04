using System;
using System.Linq;

namespace AdventOfCode._2019 {
    public class Day1 : ISolution {

        public string GetName() {
            return "The Tyranny of the Rocket Equation";
        }

        public string SolvePart1(string input) {
            var nums = input.Split("\n").Select(int.Parse);
            var total = 0;
            foreach (var num in nums)
                total += num / 3 - 2;
            return total.ToString();
        }

        public string SolvePart2(string input) {
            var nums = input.Split("\n").Select(int.Parse);
            var total = 0;
            foreach (var num in nums)
                CalculateTotalFuel(num, ref total);
            return total.ToString();
        }

        private static void CalculateTotalFuel(int num, ref int total) {
            var fuel = num / 3 - 2;
            if (fuel > 0) {
                total += fuel;
                CalculateTotalFuel(fuel, ref total);
            }
        }

    }
}