using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2019 {
    public class Day2 : ISolution {

        public string GetName() {
            return "1202 Program Alarm";
        }

        public string SolvePart1(string input) {
            return RunSimulation(input, 12, 2).ToString();
        }

        public string SolvePart2(string input) {
            for (var one = 0; one < 100; one++) {
                for (var two = one; two < 100; two++) {
                    var res = RunSimulation(input, one, two);
                    if (res == 19690720)
                        return (100 * one + two).ToString();
                }
            }
            throw new InvalidDataException();
        }

        private static int RunSimulation(string input, int one, int two) {
            var raw = input.Split(",").Select(int.Parse).ToArray();
            raw[1] = one;
            raw[2] = two;

            for (var i = 0; i < raw.Length; i += 4) {
                var opcode = (Opcode) raw[i];
                if (opcode == Opcode.Halt)
                    return raw[0];

                var input1 = raw[i + 1];
                var input2 = raw[i + 2];
                var output = raw[i + 3];
                switch (opcode) {
                    case Opcode.Add:
                        raw[output] = raw[input1] + raw[input2];
                        break;
                    case Opcode.Multiply:
                        raw[output] = raw[input1] * raw[input2];
                        break;
                }
            }
            throw new InvalidDataException();
        }

        private enum Opcode {

            Add = 1,
            Multiply = 2,
            Halt = 99

        }

    }
}