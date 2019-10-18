using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    public static class Program {

        public static void Main(string[] args) {
            var allSolutions = new Dictionary<int, List<ISolution>>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes()) {
                if (!type.IsClass)
                    continue;
                if (!type.GetInterfaces().Contains(typeof(ISolution)))
                    continue;

                var inst = (ISolution) Activator.CreateInstance(type);
                var year = inst.GetYear();

                if (!allSolutions.ContainsKey(year))
                    allSolutions.Add(year, new List<ISolution>());
                allSolutions[year].Add(inst);
            }
            foreach (var solutions in allSolutions.Values)
                solutions.Sort((s1, s2) => s1.GetDay().CompareTo(s2.GetDay()));

            var commands = new Dictionary<Regex, Action<Match>>();
            // Solve a year
            commands.Add(new Regex(@"^\d+$"), match => {
                var year = int.Parse(match.Value);
                if (!allSolutions.TryGetValue(year, out var solutions)) {
                    Console.WriteLine($"The year {year} hasn't been solved yet");
                    return;
                }
                foreach (var solution in solutions)
                    Solve(solution);
            });
            // Solve a specific day
            commands.Add(new Regex(@"^(\d+)/(\d+)$"), match => {
                var year = int.Parse(match.Groups[1].Value);
                var day = int.Parse(match.Groups[2].Value);
                if (!allSolutions.TryGetValue(year, out var solutions)) {
                    Console.WriteLine($"The year {year} hasn't been solved yet");
                    return;
                }
                var solution = solutions.Find(s => s.GetDay() == day);
                if (solution == null) {
                    Console.WriteLine($"The day {day} of {year} hasn't been solved yet");
                    return;
                }
                Solve(solution);
            });
            // Solve latest
            commands.Add(new Regex("^latest$"), match => {
                var highestYear = allSolutions.Keys.Max();
                var solutions = allSolutions[highestYear];
                var solution = solutions.OrderBy(s => s.GetDay()).Last();
                Solve(solution);
            });
            // Solve all
            commands.Add(new Regex("^all$"), match => {
                foreach (var entry in allSolutions.OrderBy(entry => entry.Key)) {
                    foreach (var solution in entry.Value) {
                        Solve(solution);
                    }
                }
            });
            // List all
            commands.Add(new Regex("^list$"), match => {
                foreach (var (key, value) in allSolutions.OrderBy(entry => entry.Key)) {
                    Console.WriteLine($"Year {key}:");
                    foreach (var solution in value)
                        Console.WriteLine($"  Day {solution.GetDay()}: {solution.GetName()}");
                }
            });

            if (args.Length == 1) {
                foreach (var (regex, command) in commands) {
                    var match = regex.Match(args[0]);
                    if (match.Success) {
                        command(match);
                        return;
                    }
                }
            }
            Console.WriteLine("Usage: dotnet run <[year] | [year]/[day] | latest | all | list>");
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