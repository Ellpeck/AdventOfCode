using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018 {
    public class Day2 : ISolution {

        public string GetName() {
            return "Inventory Management System";
        }

        public string SolvePart1(string input) {
            var twice = 0;
            var thrice = 0;
            foreach (var line in input.Split('\n')) {
                var letters = new Dictionary<char, int>();
                foreach (var letter in line) {
                    if (!letters.ContainsKey(letter)) {
                        letters.Add(letter, 1);
                    } else {
                        letters[letter]++;
                    }
                }
                if (letters.Values.Contains(2))
                    twice++;
                if (letters.Values.Contains(3))
                    thrice++;
            }
            return (twice * thrice).ToString();
        }

        public string SolvePart2(string input) {
            var lines = input.Split('\n');
            foreach (var line1 in lines) {
                foreach (var line2 in lines) {
                    if (line1.Length != line2.Length)
                        continue;
                    var nonMatching = -1;
                    for (var i = 0; i < line1.Length; i++) {
                        if (line1[i] == line2[i])
                            continue;
                        if (nonMatching >= 0) {
                            goto next;
                        } else {
                            nonMatching = i;
                        }
                    }
                    if (nonMatching >= 0) {
                        return line1.Remove(nonMatching, 1);
                    }
                    next: ;
                }
            }
            return "";
        }

    }
}