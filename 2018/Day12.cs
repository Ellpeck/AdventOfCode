using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode._2018 {
    public class Day12 : ISolution {

        public string GetName() {
            return "Subterranean Sustainability";
        }

        public string SolvePart1(string input) {
            return Solve(input, 20);
        }

        public string SolvePart2(string input) {
            return Solve(input, 50000000000);
        }

        private static string Solve(string input, long generations) {
            var initial = Regex.Match(input, "initial state: (.*)").Groups[1];
            var rules = Regex.Matches(input, "([.#]*) => ([.#]*)");

            var initialState = new State {
                Left = 0,
                Right = initial.Length
            };
            for (var i = 0; i < initial.Length; i++) {
                var c = initial.Value[i];
                if (c.Equals('#'))
                    initialState.Plants.Add(i);
            }

            return Simulate(initialState, rules, generations);
        }

        private static string Simulate(State state, MatchCollection rules, long generations) {
            var gensRemain = generations;
            while (gensRemain > 0) {
                gensRemain--;

                var lastState = state;
                state = SimulateOne(state, rules);

                if (lastState.Display() == state.Display()) {
                    Console.WriteLine("Reached a stable position, speeding up simulation...");

                    var totalMove = (state.Left - lastState.Left) * gensRemain;
                    var newState = new State();
                    foreach (var plant in state.Plants)
                        newState.Plants.Add(plant + totalMove);
                    state = newState;
                    break;
                }
            }

            var sum = state.Plants.Sum();
            return sum.ToString();
        }

        private static State SimulateOne(State state, MatchCollection rules) {
            var nextState = new State();
            for (var x = state.Left - 2; x <= state.Right + 2; x++) {
                if (WillHavePlant(state, rules, x)) {
                    if (x < nextState.Left)
                        nextState.Left = x;
                    if (x > nextState.Right)
                        nextState.Right = x;
                    nextState.Plants.Add(x);
                }
            }
            return nextState;
        }

        private static bool WillHavePlant(State state, MatchCollection rules, long index) {
            var five = new StringBuilder();
            for (var off = -2; off <= 2; off++) {
                five.Append(state.Plants.Contains(off + index) ? "#" : ".");
            }
            var fiveStr = five.ToString();

            foreach (Match rule in rules) {
                var pre = rule.Groups[1];
                if (pre.Value.Equals(fiveStr)) {
                    return rule.Groups[2].Value.Equals("#");
                }
            }
            return false;
        }

        private class State {

            public readonly ISet<long> Plants = new HashSet<long>();
            public long Left = long.MaxValue;
            public long Right = long.MinValue;

            public string Display() {
                var s = new StringBuilder();
                for (var x = this.Left; x <= this.Right; x++) {
                    s.Append(this.Plants.Contains(x) ? "#" : ".");
                }
                return s.ToString();
            }

        }

    }
}