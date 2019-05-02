using System.Linq;
using System.IO;

namespace GithubProcessor.BugLocalization
{
    public class AdditionCombination
    {
        private readonly string _resultFolderPath;

        public AdditionCombination(string resultFolderPath)
        {
            _resultFolderPath = resultFolderPath;
        }

        public void Execute(string method1, string method2)
        {
            var method1Similarity = File.ReadAllLines($@"{_resultFolderPath}\{method1}.txt").Select(x => x.SplitWith(" ")).ToDictionary(x => x[0], x => double.Parse(x[1]));
            if (method1.ToUpperInvariant() == "PMI")
                method1Similarity = method1Similarity.ToDictionary(x => x.Key, x => (x.Value + 1.0) / 2.0);

            var method2Similarity = File.ReadAllLines($@"{_resultFolderPath}\{method2}.txt").Select(x => x.SplitWith(" ")).ToDictionary(x => x[0], x => double.Parse(x[1]));
            if (method2.ToUpperInvariant() == "PMI")
                method2Similarity = method2Similarity.ToDictionary(x => x.Key, x => (x.Value + 1.0) / 2.0);

            var combinedSimilarity = method1Similarity.ToDictionary(x => x.Key, x => x.Value * 0.5 + method2Similarity[x.Key] * 0.5);

            File.WriteAllLines($@"{_resultFolderPath}\Addition_{method1}_{method2}.txt", combinedSimilarity.ToList().OrderByDescending(x => x.Value).Select(x => x.Key + " " + x.Value.ToString()));
        }
    }
}