using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018 {
    public class Day8 : ISolution {

        public string GetName() {
            return "Memory Maneuver";
        }

        public string SolvePart1(string input) {
            var root = GetRoot(input);
            var sum = 0;
            root.AndChildren(n => {
                foreach (var data in n.Metadata)
                    sum += data;
            });
            return sum.ToString();
        }

        public string SolvePart2(string input) {
            var root = GetRoot(input);
            return root.Value().ToString();
        }

        private static Node GetRoot(string input) {
            return new Node(input.Split(" ").Select(int.Parse).ToList());
        }

        private class Node {

            public int Length;
            public List<int> Metadata = new List<int>();
            public List<Node> Children = new List<Node>();

            public Node(List<int> data) {
                var childAmount = data[0];
                var metaAmount = data[1];

                var subData = data.Skip(2).ToList();
                for (var i = 0; i < childAmount; i++) {
                    var child = new Node(subData);
                    this.Children.Add(child);
                    subData = subData.Skip(child.Length).ToList();
                }

                for (var i = 0; i < metaAmount; i++) {
                    this.Metadata.Add(subData[i]);
                }

                this.Length = metaAmount + 2;
                foreach (var child in this.Children)
                    this.Length += child.Length;
            }

            public void AndChildren(Action<Node> action) {
                action(this);
                foreach (var child in this.Children)
                    child.AndChildren(action);
            }

            public int Value() {
                if (this.Children.Count <= 0)
                    return this.Metadata.Sum();

                var sum = 0;
                foreach (var meta in this.Metadata) {
                    var childIndex = meta - 1;
                    if (childIndex < 0 || childIndex >= this.Children.Count)
                        continue;
                    var child = this.Children[childIndex];
                    sum += child.Value();
                }
                return sum;
            }

        }

    }
}