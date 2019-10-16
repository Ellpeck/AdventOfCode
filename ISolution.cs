namespace AdventOfCode {
    public interface ISolution {

        string GetName();

        string SolvePart1(string input);

        string SolvePart2(string input);

    }

    public static class SolutionExtensions {

        public static int GetYear(this ISolution solution) {
            var nsp = solution.GetType().Namespace;
            return int.Parse(nsp.Substring(nsp.Length - 4, 4));
        }

        public static int GetDay(this ISolution solution) {
            return int.Parse(solution.GetType().Name.Substring(3));
        }

    }
}