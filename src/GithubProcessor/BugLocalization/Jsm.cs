using System.Collections.Generic;
using System.Linq;
using GithubProcessor.Models;

namespace GithubProcessor.BugLocalization
{
    public class Jsm : BaseLocalization
    {
        public Jsm(string sourceFolderPath, string outputFolderPath)
            : base(sourceFolderPath, outputFolderPath)
        {

        }

        public void Execute(List<string> queryWords, string appendTextToFileName)
        {
            ExecuteBase();
            var queryTfIdfDictionary = GetQueryIntDictionary(queryWords);
            ExecuteSub(queryTfIdfDictionary, appendTextToFileName);
        }

        public void Execute(string queryFilePath)
        {
            ExecuteBase();
            var queryTfIdfDictionary = GetQueryIntDictionary(queryFilePath);
            ExecuteSub(queryTfIdfDictionary);
        }

        private void ExecuteSub(MyDoubleDictionary queryTfDictionary, string appendTextToFileName = "")
        {
            // create the vector for each source code
            var allUniqueWordsInSourceAndQuery = IdfDictionary.Keys.Union(queryTfDictionary.Keys).Distinct().ToList();
            int allUniqueWordsInSourceAndQueryCount = allUniqueWordsInSourceAndQuery.Count;

            var sourceVectors = new Dictionary<string, double[]>();
            TfDictionary.ToList().ForEach(fileWithTfCount =>
            {
                MyDoubleDictionary tfDictionary = fileWithTfCount.Value;
                int totalWordsInFile = CodeFilesWithContent[fileWithTfCount.Key].Count;

                double[] vector = new double[allUniqueWordsInSourceAndQueryCount];
                int counter = 0;
                allUniqueWordsInSourceAndQuery.ForEach(uniqueWord =>
                {
                    vector[counter] = tfDictionary.ContainsKey(uniqueWord)
                        ? tfDictionary[uniqueWord] / totalWordsInFile
                        : 0;
                    counter++;
                });

                sourceVectors.Add(fileWithTfCount.Key, vector);
            });

            // create the vector for query
            double[] queryVector = new double[allUniqueWordsInSourceAndQueryCount];
            int queryCounter = 0;
            var queryHashSet = new HashSet<string>(queryTfDictionary.Keys);
            var totalQueryWordCount = queryTfDictionary.Sum(x => x.Value);
            allUniqueWordsInSourceAndQuery.ForEach(uniqueWord =>
            {
                queryVector[queryCounter] = queryHashSet.Contains(uniqueWord)
                    ? (double)queryTfDictionary[uniqueWord] / totalQueryWordCount
                    : 0;
                queryCounter++;
            });

            // calculate H(p), H(q) and H(p + q)
            MyDoubleDictionary similarityDictionary = new MyDoubleDictionary();
            sourceVectors.ToList().ForEach(sourceFileWithVector =>
            {
                var p = sourceFileWithVector.Value;
                var sumEntropy = (p.JensenSum(queryVector)).JensenEntropy();
                var pEntropy = 1.0 / 2 * p.JensenEntropy();
                var qEntropy = 1.0 / 2 * queryVector.JensenEntropy();

                var jensenDivergence = sumEntropy - pEntropy - qEntropy;
                var jensenSimilarity = 1 - jensenDivergence;

                similarityDictionary.Add(sourceFileWithVector.Key, jensenSimilarity);
            });

            // WRITE TO FILE
            WriteDocumentVectorToFileOrderedDescending("Jsm" + appendTextToFileName, similarityDictionary);
        }

    }
}
