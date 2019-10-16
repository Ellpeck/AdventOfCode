using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2018 {
    public class Day4 : ISolution {

        public string GetName() {
            return "Repose Record";
        }

        public string SolvePart1(string input) {
            var highest = 0;
            Guard highestGuard = null;
            foreach (var guard in Parse(input).Values) {
                var total = 0;
                foreach (var interval in guard.SleepIntervals)
                    total += interval.Span.Minutes;
                if (total > highest) {
                    highest = total;
                    highestGuard = guard;
                }
            }

            var minutes = new Dictionary<int, int>();
            var highestMinute = 0;
            var highestMinuteAmount = 0;
            foreach (var interval in highestGuard.SleepIntervals) {
                for (var min = interval.Start.Minute; min < interval.End.Minute; min++) {
                    if (!minutes.ContainsKey(min)) {
                        minutes.Add(min, 1);
                    } else {
                        minutes[min]++;
                        if (minutes[min] >= highestMinuteAmount) {
                            highestMinute = min;
                            highestMinuteAmount = minutes[min];
                        }
                    }
                }
            }
            return (highestGuard.Id * highestMinute).ToString();
        }

        public string SolvePart2(string input) {
            Guard highestGuard = null;
            var highestMinute = 0;
            var highestMinuteAmount = 0;
            foreach (var guard in Parse(input).Values) {
                var minutes = new Dictionary<int, int>();
                foreach (var interval in guard.SleepIntervals) {
                    for (var min = interval.Start.Minute; min < interval.End.Minute; min++) {
                        if (!minutes.ContainsKey(min)) {
                            minutes.Add(min, 1);
                        } else {
                            minutes[min]++;
                            if (minutes[min] >= highestMinuteAmount) {
                                highestMinute = min;
                                highestMinuteAmount = minutes[min];
                                highestGuard = guard;
                            }
                        }
                    }
                }
            }
            return (highestGuard.Id * highestMinute).ToString();
        }

        private static Dictionary<int, Guard> Parse(string input) {
            var actions = new List<(DateTime, string)>();
            foreach (Match match in Regex.Matches(input, @"\[(.*)\] (.*)")) {
                var time = DateTime.Parse(match.Groups[1].Value);
                var action = match.Groups[2].Value;
                actions.Add((time, action));
            }

            var guards = new Dictionary<int, Guard>();
            Guard currGuard = null;
            DateTime sleepStart = default;
            foreach (var (time, action) in actions.OrderBy(a => a.Item1)) {
                var match = Regex.Match(action, @"Guard #(\d*)");
                if (match.Success) {
                    var id = int.Parse(match.Groups[1].Value);
                    if (guards.TryGetValue(id, out var guard)) {
                        currGuard = guard;
                    } else {
                        currGuard = new Guard(id);
                        guards[id] = currGuard;
                    }
                } else if (action.Contains("falls asleep")) {
                    sleepStart = time;
                } else if (action.Contains("wakes up")) {
                    currGuard.SleepIntervals.Add(new SleepInterval(sleepStart, time));
                }
            }
            return guards;
        }

        private class Guard {

            public readonly int Id;
            public readonly List<SleepInterval> SleepIntervals = new List<SleepInterval>();

            public Guard(int id) {
                this.Id = id;
            }

        }

        private struct SleepInterval {

            public readonly DateTime Start;
            public readonly DateTime End;
            public TimeSpan Span => this.End - this.Start;

            public SleepInterval(DateTime start, DateTime end) {
                this.Start = start;
                this.End = end;
            }

        }

    }
}