using System.Linq;
using System.IO;

namespace GithubProcessor.BugLocalization
{
    public class LambdaCombination
    {
        private readonly string _resultFolderPath;

        public LambdaCombination(string resultFolderPath)
        {
            _resultFolderPath = resultFolderPath;
        }

        public void Execute(string method1, string method2, double lambda)
        {
            var method1Similarity = File.ReadAllLines($@"{_resultFolderPath}\{method1}.txt").Select(x => x.SplitWith(" ")).ToDictionary(x => x[0], x => double.Parse(x[1]));
            if (method1.ToUpperInvariant() == "PMI")
                method1Similarity = method1Similarity.ToDictionary(x => x.Key, x => (x.Value + 1.0) / 2.0);
            //var min1 = method1Similarity.Select(x => x.Value).Min();
            //var max1 = method1Similarity.Select(x => x.Value).Max();
            //method1Similarity = method1Similarity.ToDictionary(x => x.Key, x => (x.Value - min1) / (max1 - min1));

            var method2Similarity = File.ReadAllLines($@"{_resultFolderPath}\{method2}.txt").Select(x => x.SplitWith(" ")).ToDictionary(x => x[0], x => double.Parse(x[1]));
            if (method2.ToUpperInvariant() == "PMI")
                method2Similarity = method2Similarity.ToDictionary(x => x.Key, x => (x.Value + 1.0) / 2.0);
            //var min2 = method2Similarity.Select(x => x.Value).Min();
            //var max2 = method2Similarity.Select(x => x.Value).Max();
            //method2Similarity = method2Similarity.ToDictionary(x => x.Key, x => (x.Value - min2) / (max2 - min2));

            var combinedSimilarity = method1Similarity.ToDictionary(x => x.Key, x => x.Value * lambda + method2Similarity[x.Key] * (1.0 - lambda));

            File.WriteAllLines($@"{_resultFolderPath}\Lambda_{method1}_{method2}.txt", combinedSimilarity.ToList().OrderByDescending(x => x.Value).Select(x => x.Key + " " + x.Value.ToString()));
        }
    }
}