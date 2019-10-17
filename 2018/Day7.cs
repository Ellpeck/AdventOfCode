using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode._2018 {
    public class Day7 : ISolution {

        public string GetName() {
            return "The Sum of Its Parts";
        }

        public string SolvePart1(string input) {
            var (steps, dependencies) = Parse(input);
            var order = new StringBuilder();
            while (steps.Count > 0) {
                foreach (var step in steps) {
                    if (!dependencies.TryGetValue(step, out var deps) || !deps.Any(d => steps.Contains(d))) {
                        order.Append(step);
                        steps.Remove(step);
                        break;
                    }
                }
            }
            return order.ToString();
        }

        public string SolvePart2(string input) {
            var (steps, dependencies) = Parse(input);
            const int workers = 5;
            var seconds = 0;
            var workerSecsRemaining = new int[workers];
            var workerCurrSteps = new char[workers];
            while (steps.Count > 0) {
                for (var i = 0; i < workers; i++) {
                    workerSecsRemaining[i]--;
                    if (workerSecsRemaining[i] > 0)
                        continue;
                    var finishedStep = workerCurrSteps[i];
                    if (finishedStep != default) {
                        steps.Remove(finishedStep);
                        workerCurrSteps[i] = default;
                    }

                    foreach (var step in steps) {
                        if (workerCurrSteps.Contains(step))
                            continue;
                        if (!dependencies.TryGetValue(step, out var deps) || !deps.Any(d => steps.Contains(d))) {
                            workerSecsRemaining[i] = 60 + (step - 'A' + 1);
                            workerCurrSteps[i] = step;
                            break;
                        }
                    }
                }
                seconds++;
            }
            return (seconds - 1).ToString();
        }

        private static (List<char> steps, Dictionary<char, List<char>> dependencies) Parse(string input) {
            var steps = new List<char>();
            var dependencies = new Dictionary<char, List<char>>();

            var matches = Regex.Matches(input, @"Step ([A-Z]) must be finished before step ([A-Z]) can begin\.");
            foreach (Match match in matches) {
                var first = match.Groups[1].Value[0];
                var second = match.Groups[2].Value[0];

                if (!steps.Contains(first))
                    steps.Add(first);
                if (!steps.Contains(second))
                    steps.Add(second);

                if (!dependencies.ContainsKey(second))
                    dependencies[second] = new List<char>();
                dependencies[second].Add(first);
            }

            steps.Sort((s1, s2) => s1.CompareTo(s2));
            return (steps, dependencies);
        }

    }
}