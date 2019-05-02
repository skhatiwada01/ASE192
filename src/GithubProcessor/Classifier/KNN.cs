using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GithubProcessor.Classifier
{
    public class KNN
    {
        public void Run()
        {
            Predict();
            Analyze();
        }

        public const int DimensionCount = 500;

        public void Predict()
        {
            var trainingIssues = File.ReadAllLines(@"C:\Research\20580498\Classification\AreaPmi\IssueUsedTrainingDataset.txt").ToList();
            var issueTags = File.ReadAllLines(@"C:\Research\20580498\Classification\AreaPmi\IssueTagsCleaned.txt").Select(x => x.SplitWith("##")).ToDictionary(x => x[0], x => int.Parse(x[1].SplitWith(",")[0]));
            var dimesions = new HashSet<string>(GetSimilarityData($@"C:\Research\20580498\Classification\SingleTagsSourceSimilarityPmi\Similarity\Pmi_{trainingIssues.First()}.txt").Keys.Take(DimensionCount));

            var outputs = trainingIssues.Select(x => issueTags[x]).ToArray();
            var distinctOutputs = outputs.Distinct().ToList();
            var outputsDictionaryMappingIndexTag = distinctOutputs.Select((x, index) => new { x, index }).ToDictionary(x => x.index, x => x.x);
            var outputsDictionaryMappingTagIndex = outputsDictionaryMappingIndexTag.ToDictionary(x => x.Value, x => x.Key);

            var formattedOutputs = outputs.Select(x => outputsDictionaryMappingTagIndex[x]).ToArray();
            var inputs = trainingIssues.Select(x => GetSimilarityData(x, dimesions)).ToArray();
            var k = issueTags.Values.Distinct().Count();

            var knn = new Accord.MachineLearning.KNearestNeighbors(k);
            var trainedKnn = knn.Learn(inputs, formattedOutputs);
            
            var testingIssues = File.ReadAllLines(@"C:\Research\20580498\Classification\AreaPmi\IssueUsedTestingDataset.txt").ToList();
            var results = new List<string>();
            int counter = 0;
            foreach(var testingIssue in testingIssues)
            {
                System.Console.WriteLine(counter++);
                var coordinate = GetSimilarityData(testingIssue, dimesions);
                var decision = trainedKnn.Decide(coordinate);
                results.Add(testingIssue + "##" + outputsDictionaryMappingIndexTag[decision]);
            }

            File.WriteAllLines(@"C:\Research\20580498\Classification\AreaPmi\KnnResult_Pmi.txt", results);
        }

        public Dictionary<string, double> GetSimilarityData(string filePath)
        {
            return File.ReadAllLines(filePath)
                .Select(x => x.SplitWith(" "))
                .ToDictionary(x => x[0], x => double.Parse(x[1]));
        }

        public double[] GetSimilarityData(string issue, HashSet<string> filesToInclude)
        {
            var read = File.ReadAllLines($@"C:\Research\20580498\Classification\SingleTagsSourceSimilarityPmi\Similarity\Pmi_{issue}.txt")
                .Select(x => x.SplitWith(" "))
                .ToDictionary(x => x[0], x => double.Parse(x[1]));
            return filesToInclude.Select(x => read[x]).ToArray();
        }

        public void Analyze()
        {
            var issueTagsCleanedFilePath =  @"C:\Research\20580498\Classification\AreaPmi\IssueTagsCleaned.txt";
            var allIssueTags = File.ReadAllLines(issueTagsCleanedFilePath).Select(x => x.SplitWith("##")).ToDictionary(x => x[0], x => new HashSet<int>(x[1].SplitWith(",").Select(int.Parse).ToList()));

            var predictionIssueTag = File.ReadAllLines(@"C:\Research\20580498\Classification\AreaPmi\Prediction.txt").Select(x => x.SplitWith("##")).ToDictionary(x => x[0], x => int.Parse(x[1]));

            double count = predictionIssueTag.Count(x => allIssueTags[x.Key].Contains(x.Value));

            Console.WriteLine("Count: " + count);
            Console.WriteLine("Total: " + predictionIssueTag.Count);
            Console.WriteLine("P: " + count / predictionIssueTag.Count);
        }
    }
}
