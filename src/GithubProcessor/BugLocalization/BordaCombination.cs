using System.Linq;
using System.IO;

namespace GithubProcessor.BugLocalization
{
    public class BordaCombination
    {
        private readonly string _resultFolderPath;

        public BordaCombination(string resultFolderPath)
        {
            _resultFolderPath = resultFolderPath;
        }

        public void Execute(string method1, string method2)
        {
            var method1Similarity = File.ReadAllLines($@"{_resultFolderPath}\{method1}.txt").Select(x => x.SplitWith(" ")).ToDictionary(x => x[0], x => double.Parse(x[1]));
            if (method1.ToUpperInvariant() == "PMI")
                method1Similarity = method1Similarity.ToDictionary(x => x.Key, x => (x.Value + 1.0) / 2.0);

            int m1 = method1Similarity.Count(x => x.Value > 0);
            var method1Ranks = method1Similarity.OrderByDescending(x => x.Value).Select((x, index) => new { Key = x.Key, Rank = index + 1 }).ToDictionary(x => x.Key, x => m1 - x.Rank);

            var method2Similarity = File.ReadAllLines($@"{_resultFolderPath}\{method2}.txt").Select(x => x.SplitWith(" ")).ToDictionary(x => x[0], x => double.Parse(x[1]));
            if (method2.ToUpperInvariant() == "PMI")
                method2Similarity = method2Similarity.ToDictionary(x => x.Key, x => (x.Value + 1.0) / 2.0);

            int m2 = method2Similarity.Count(x => x.Value > 0);
            var method2Ranks = method2Similarity.OrderByDescending(x => x.Value).Select((x, index) => new { Key = x.Key, Rank = index + 1 }).ToDictionary(x => x.Key, x => m2 - x.Rank);

            var combinedSimilarity = method1Ranks.Keys.ToDictionary(x => x, x => (method1Ranks.ContainsKey(x) ? method1Ranks[x] : 0) + (method2Ranks.ContainsKey(x) ? method2Ranks[x] : 0));

            File.WriteAllLines($@"{_resultFolderPath}\Borda_{method1}_{method2}.txt", combinedSimilarity.ToList().OrderByDescending(x => x.Value).Select(x => x.Key + " " + x.Value.ToString()));
        }
    }
}
