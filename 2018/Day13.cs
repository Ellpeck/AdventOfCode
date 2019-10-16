using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018 {
    public class Day13 : ISolution {

        private static readonly Direction[] AllDirections = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToArray();
        private List<Cart> carts;
        private List<Cart> crashes;
        private int width;
        private int height;
        private char[,] tracks;

        public string GetName() {
            return "Mine Cart Madness";
        }

        public string SolvePart1(string input) {
            this.Initialize(input);
            while (true) {
                this.Step();
                if (this.crashes.Count > 0) {
                    var first = this.crashes[0];
                    return $"{first.X},{first.Y}";
                }
            }
        }

        public string SolvePart2(string input) {
            this.Initialize(input);
            while (this.carts.Count > 1)
                this.Step();
            var remain = this.carts[0];
            return $"{remain.X},{remain.Y}";
        }

        private void Initialize(string input) {
            var lines = input.Split('\n');
            this.width = lines.Max(s => s.Length);
            this.height = lines.Length;
            this.carts = new List<Cart>();
            this.crashes = new List<Cart>();
            this.tracks = new char[this.width, this.height];

            for (var y = 0; y < this.height; y++) {
                var line = lines[y];
                for (var x = 0; x < line.Length; x++) {
                    var ch = line[x];
                    if (GetCartDirection(ch, out var cartDir)) {
                        this.tracks[x, y] = cartDir <= Direction.Down ? '|' : '-';
                        this.carts.Add(new Cart {
                            X = x,
                            Y = y,
                            Direction = cartDir
                        });
                    } else {
                        this.tracks[x, y] = line[x];
                    }
                }
            }
        }

        private void Step() {
            foreach (var cart in this.carts.OrderBy(cart => cart.X + cart.Y * this.width)) {
                cart.MoveOne(this.tracks);

                if (cart.Collides(this.carts, out var other)) {
                    this.crashes.Add(cart);
                    this.crashes.Add(other);
                }
            }
            foreach (var crashed in this.crashes)
                this.carts.Remove(crashed);
        }

        private static bool GetCartDirection(char cart, out Direction direction) {
            switch (cart) {
                case '^':
                    direction = Direction.Up;
                    return true;
                case 'v':
                    direction = Direction.Down;
                    return true;
                case '<':
                    direction = Direction.Left;
                    return true;
                case '>':
                    direction = Direction.Right;
                    return true;
            }
            direction = default;
            return false;
        }

        private static (int x, int y) GetDirectionOffset(Direction direction) {
            switch (direction) {
                case Direction.Up:
                    return (0, -1);
                case Direction.Down:
                    return (0, 1);
                case Direction.Left:
                    return (-1, 0);
                case Direction.Right:
                    return (1, 0);
            }
            return default;
        }

        private static Direction GetClockwiseDirection(Direction direction, bool ccw) {
            var index = (int) direction;
            if (ccw) {
                index--;
                if (index < 0)
                    index = AllDirections.Length - 1;
            } else {
                index++;
                if (index >= AllDirections.Length)
                    index = 0;
            }
            return AllDirections[index];
        }

        public class Cart {

            public int X;
            public int Y;
            public Direction Direction;
            private int turnsTaken;

            public void MoveOne(char[,] tracks) {
                var (xOff, yOff) = GetDirectionOffset(this.Direction);
                this.X += xOff;
                this.Y += yOff;
                this.Direction = this.GetNextDirection(tracks[this.X, this.Y]);
            }

            private Direction GetNextDirection(char nextTrack) {
                switch (nextTrack) {
                    case '/':
                        switch (this.Direction) {
                            case Direction.Right:
                                return Direction.Up;
                            case Direction.Up:
                                return Direction.Right;
                            case Direction.Left:
                                return Direction.Down;
                            case Direction.Down:
                                return Direction.Left;
                        }
                        break;
                    case '\\':
                        switch (this.Direction) {
                            case Direction.Up:
                                return Direction.Left;
                            case Direction.Left:
                                return Direction.Up;
                            case Direction.Down:
                                return Direction.Right;
                            case Direction.Right:
                                return Direction.Down;
                        }
                        break;
                    case '+':
                        this.turnsTaken++;
                        switch (this.turnsTaken) {
                            case 3:
                                this.turnsTaken = 0;
                                return GetClockwiseDirection(this.Direction, false);
                            case 1:
                                return GetClockwiseDirection(this.Direction, true);
                        }
                        break;
                }
                return this.Direction;
            }

            public bool Collides(IEnumerable<Cart> carts, out Cart colliding) {
                foreach (var cart in carts) {
                    if (cart == this)
                        continue;
                    if (cart.X == this.X && cart.Y == this.Y) {
                        colliding = cart;
                        return true;
                    }
                }
                colliding = null;
                return false;
            }

        }

        public enum Direction {

            Up,
            Right,
            Down,
            Left

        }

    }
}