using System;

namespace AdventOfCode._2018 {
    public class Day11 : ISolution {

        public string GetName() {
            return "Chronal Charge";
        }

        public string SolvePart1(string input) {
            var maxPart1 = GetMaxPower(input, 3);
            return $"{maxPart1.maxPowerX},{maxPart1.maxPowerY}";
        }

        public string SolvePart2(string input) {
            var maxPower = int.MinValue;
            var maxPowerX = 0;
            var maxPowerY = 0;
            var maxPowerArea = 0;
            for (var area = 1; area <= 300; area++) {
                var maxPart2 = GetMaxPower(input, area);
                if (maxPart2.maxPower > maxPower) {
                    maxPower = maxPart2.maxPower;
                    maxPowerX = maxPart2.maxPowerX;
                    maxPowerY = maxPart2.maxPowerY;
                    maxPowerArea = area;
                }
                Console.WriteLine($"The max power after {area} is {maxPowerX},{maxPowerY},{maxPowerArea} with power {maxPower}");
            }
            return $"{maxPowerX},{maxPowerY},{maxPowerArea}";
        }

        private static (int maxPowerX, int maxPowerY, int maxPower) GetMaxPower(string input, int area) {
            var serial = int.Parse(input);
            var powerGrid = new int[300, 300];
            for (var x = 0; x < 300; x++) {
                for (var y = 0; y < 300; y++) {
                    powerGrid[x, y] = GetPower(serial, x, y);
                }
            }

            var maxPower = int.MinValue;
            var maxPowerX = 0;
            var maxPowerY = 0;
            for (var x = 0; x <= 300 - area; x++) {
                for (var y = 0; y <= 300 - area; y++) {
                    var power = 0;
                    for (var xOff = 0; xOff < area; xOff++) {
                        for (var yOff = 0; yOff < area; yOff++) {
                            power += powerGrid[x + xOff, y + yOff];
                        }
                    }
                    if (power > maxPower) {
                        maxPower = power;
                        maxPowerX = x;
                        maxPowerY = y;
                    }
                }
            }
            return (maxPowerX, maxPowerY, maxPower);
        }

        private static int GetPower(int serial, int x, int y) {
            var rack = x + 10;
            var power = rack * y;
            power += serial;
            power *= rack;

            if (power < 100)
                return 0;

            var powerSt = power.ToString();
            var hundred = int.Parse(powerSt.Substring(powerSt.Length - 3, 1));
            return hundred - 5;
        }

    }
}