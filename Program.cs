using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    public static class Program {

        private static readonly Dictionary<int, List<ISolution>> AllSolutions = new Dictionary<int, List<ISolution>>();

        public static void Main(string[] args) {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes()) {
                if (!type.IsClass)
                    continue;
                if (!type.GetInterfaces().Contains(typeof(ISolution)))
                    continue;

                var inst = (ISolution) Activator.CreateInstance(type);
                var year = inst.GetYear();

                if (!AllSolutions.ContainsKey(year))
                    AllSolutions.Add(year, new List<ISolution>());
                AllSolutions[year].Add(inst);
            }
            foreach (var solutions in AllSolutions.Values)
                solutions.Sort((s1, s2) => s1.GetDay().CompareTo(s2.GetDay()));

            if (args.Length == 1) {
                var arg = args[0];
                if (SolveAllYear(arg))
                    return;
                if (SolveSpecific(arg))
                    return;
                if (SolveLatest(arg))
                    return;
                if (SolveAll(arg))
                    return;
                if (List(arg))
                    return;
            }
            Console.WriteLine("Usage: dotnet run <[year] | [year]/[day] | latest | all | list>");
        }

        private static bool SolveAllYear(string arg) {
            if (!int.TryParse(arg, out var year))
                return false;
            if (!AllSolutions.TryGetValue(year, out var solutions)) {
                Console.WriteLine($"The year {year} hasn't been solved yet");
                return true;
            }
            foreach (var solution in solutions)
                Solve(solution);
            return true;
        }

        private static bool SolveSpecific(string arg) {
            var match = Regex.Match(arg, @"(\d*)/(\d*)");
            if (!int.TryParse(match.Groups[1].Value, out var year))
                return false;
            if (!int.TryParse(match.Groups[2].Value, out var day))
                return false;
            if (!AllSolutions.TryGetValue(year, out var solutions)) {
                Console.WriteLine($"The year {year} hasn't been solved yet");
                return true;
            }
            var solution = solutions.Find(s => s.GetDay() == day);
            if (solution == null) {
                Console.WriteLine($"The day {day} of {year} hasn't been solved yet");
                return true;
            }
            Solve(solution);
            return true;
        }

        private static bool SolveLatest(string arg) {
            if (!arg.Equals("latest"))
                return false;
            var highestYear = AllSolutions.Keys.Max();
            var solutions = AllSolutions[highestYear];
            var solution = solutions.OrderBy(s => s.GetDay()).Last();
            Solve(solution);
            return true;
        }

        private static bool SolveAll(string arg) {
            if (!arg.Equals("all"))
                return false;
            foreach (var entry in AllSolutions.OrderBy(entry => entry.Key)) {
                foreach (var solution in entry.Value) {
                    Solve(solution);
                }
            }
            return true;
        }

        private static bool List(string arg) {
            if (!arg.Equals("list"))
                return false;
            foreach (var (key, value) in AllSolutions.OrderBy(entry => entry.Key)) {
                Console.WriteLine($"Year {key}:");
                foreach (var solution in value)
                    Console.WriteLine($"  Day {solution.GetDay()}: {solution.GetName()}");
            }
            return true;
        }

        private static void Solve(ISolution solution) {
            var year = solution.GetYear();
            var day = solution.GetDay();
            string input;
            using (var stream = new FileInfo($"{year}/Input/{day}.txt").OpenText())
                input = stream.ReadToEnd();
            Console.WriteLine($"--- Year {year}, Day {day}: {solution.GetName()} ---");
            PrintPart(1, solution.SolvePart1(input));
            PrintPart(2, solution.SolvePart2(input));
            Console.WriteLine();
        }

        private static void PrintPart(int part, string solution) {
            if (solution.Contains('\n')) {
                Console.WriteLine($"Part {part}:");
                Console.WriteLine(solution);
            } else {
                Console.WriteLine($"Part {part}: {solution}");
            }
        }

    }
}