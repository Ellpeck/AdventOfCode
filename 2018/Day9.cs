using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2018 {
    public class Day9 : ISolution {

        public string GetName() {
            return "Marble Mania";
        }

        public string SolvePart1(string input) {
            return Solve(input, 1).ToString();
        }

        public string SolvePart2(string input) {
            return Solve(input, 100).ToString();
        }

        private static long Solve(string input, int multiplier) {
            var regex = Regex.Match(input, "(\\d*) players; last marble is worth (\\d*) points");
            var playerAmount = int.Parse(regex.Groups[1].Value);
            var last = int.Parse(regex.Groups[2].Value);
            return Play(playerAmount, last * multiplier);
        }

        private static long Play(int playerAmount, int last) {
            var players = new long[playerAmount];
            var curr = new Marble {Value = 0};
            curr.CounterClockwise = curr;
            curr.Clockwise = curr;

            var marbleCounter = 1;
            var currPlayer = 1;
            while (marbleCounter <= last) {
                if (marbleCounter % 23 == 0) {
                    players[currPlayer] += marbleCounter;

                    for (var i = 0; i < 7; i++)
                        curr = curr.CounterClockwise;
                    players[currPlayer] += curr.Value;

                    curr.CounterClockwise.Clockwise = curr.Clockwise;
                    curr.Clockwise.CounterClockwise = curr.CounterClockwise;
                    curr = curr.Clockwise;
                } else {
                    var placed = new Marble {Value = marbleCounter};
                    var slot = curr.Clockwise;
                    placed.CounterClockwise = slot;
                    placed.Clockwise = slot.Clockwise;
                    slot.Clockwise.CounterClockwise = placed;
                    slot.Clockwise = placed;
                    curr = placed;
                }

                marbleCounter++;
                currPlayer = (currPlayer + 1) % players.Length;
            }
            return players.Max();
        }

        private class Marble {

            public int Value;
            public Marble CounterClockwise;
            public Marble Clockwise;

        }

    }
}