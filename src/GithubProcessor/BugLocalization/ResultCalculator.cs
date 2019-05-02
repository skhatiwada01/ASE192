using System.Collections.Generic;
using System.IO;
using System.Linq;
using GithubProcessor.Models;

namespace GithubProcessor.BugLocalization
{
    public class ResultCalculator
    {
        public static void Analyze(string datasetFolderPath, string methodName, string localizationResultFolderRelativePath, string datasetResultRelativeFilePath, ILog logger = null)
        {
            var datasetMetricResults = new DatasetMetricResult();
            var issues = new DirectoryInfo(datasetFolderPath).GetDirectories().ToList();
            var total = issues.Count;
            int counter = 0;
            foreach (var issueDirectoryInfo in issues)
            {
                logger?.Log(++counter + " of " + total);
                var issueFolderPath = issueDirectoryInfo.FullName + @"\";
                
                var similarityFilePath = $@"{issueFolderPath}{localizationResultFolderRelativePath}{methodName}.txt";
                if (!File.Exists(similarityFilePath))
                    continue;
                var similarityList = File.ReadAllLines(similarityFilePath).Select(x => x.SplitWith(" ")[0]).ToList();

                var relevantList = SerializerHelper.DeserializeFromXmlFile<List<IndexedFile>>(issueFolderPath + "RelevantList.txt").Select(x => x.Index.ToString()).ToList();

                var metricResult = GetMetricNumbers(similarityList, relevantList);
                metricResult.SerializeToXmlFile($@"{issueFolderPath}{localizationResultFolderRelativePath}{methodName}_Result.txt");

                datasetMetricResults.AddMetricResult(metricResult);
            }

            datasetMetricResults.GeneratePercents();
            datasetMetricResults.SerializeToXmlFile($@"{datasetFolderPath}{datasetResultRelativeFilePath}");
        }

        public static MetricResult GetMetricNumbers(IReadOnlyCollection<string> similarityList, IReadOnlyCollection<string> relevanceList)
        {
            // top N
            var top01MatchesForBug = similarityList.Take(1).Any(top1 => relevanceList.Any(r => top1 == r)) ? 1 : 0;
            var top05MatchesForBug = (top01MatchesForBug == 1 || similarityList.Take(5).Any(top5 => relevanceList.Any(r => top5 == r))) ? 1 : 0;
            var top10MatchesForBug = (top05MatchesForBug == 1 || similarityList.Take(10).Any(top10 => relevanceList.Any(r => top10 == r))) ? 1 : 0;

            var indexedSimilarityList = similarityList.Select((value, index) => new { FileNum = value, Index = index }).ToList();

            // RR
            var firstMachedObject = indexedSimilarityList.FirstOrDefault(simFileWithIndex => relevanceList.Any(y => y == simFileWithIndex.FileNum));
            var rr = 1.0 / (firstMachedObject?.Index + 1) ?? 0;

            // AP
            double sumPrecision = 0.0;
            indexedSimilarityList.ForEach(simFileWithIndex =>
            {
                // If pos(i) is 0, it wont contribute to sum of AP
                if (relevanceList.All(relFile => relFile != simFileWithIndex.FileNum))
                    return;

                // calculate precision
                int matchAtThisPoint = relevanceList.Intersect(similarityList.Take(simFileWithIndex.Index + 1)).Count(); // how many match between rel file and sim file at this point
                double precision = (double)matchAtThisPoint / (simFileWithIndex.Index + 1); //prec is total match / total file found similar (at this point)
                sumPrecision += precision;
            });
            double ap = sumPrecision / relevanceList.Count;

            return new MetricResult()
            {
                Top01 = top01MatchesForBug,
                Top05 = top05MatchesForBug,
                Top10 = top10MatchesForBug,
                Rr = rr,
                Ap = ap
            };
        }
    }
}
