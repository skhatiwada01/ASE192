using System.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GithubProcessor.Models;

namespace GithubProcessor.BugLocalization
{
    public class PmiValueCache : BaseLocalization
    {
        private string _appendLog;
        public PmiValueCache(string sourceFilePath, string outputFolderPath, string appendLog)
            : base(sourceFilePath, outputFolderPath)
        {
            _appendLog = appendLog;
        }

        public void Execute(List<string> queryTextList, string cacheOutputFolderPath)
        {
            ExecuteBase();

            if (!Directory.Exists(cacheOutputFolderPath))
            {
                Directory.CreateDirectory(cacheOutputFolderPath);
                Thread.Sleep(100);
            }

            var tssDocumentDictionary = new MyDoubleDictionary();

            // Create list of word contained in query
            int n = CodeFilesWithContent.Count;

            // Compute pmi for each word in WordAndContainingFiles and unique words in query
            int counter = 0;
            foreach (var queryText in queryTextList.Distinct())
            {
                var queryWordW2 = queryText;

                Log($"{_appendLog} Creating {++counter} of {queryTextList.Count}: {queryWordW2}");

                var outputFilePath = Path.Combine(cacheOutputFolderPath, $"_{queryWordW2}.txt");
                if (File.Exists(outputFilePath))
                    continue;

                var nPmiDictionary = new MyDoubleDictionary();

                foreach (var sourceText in WordAndContainingFiles.Keys)
                {
                    var sourceWordW1 = sourceText;
                    bool sourceContainsQueryWord = WordAndContainingFiles.ContainsKey(queryWordW2);

                    int countW1 = WordAndContainingFiles[sourceWordW1].Count;
                    int countW2 = sourceContainsQueryWord ? WordAndContainingFiles[queryWordW2].Count : 0;
                    int countW1W2 = sourceContainsQueryWord ? WordAndContainingFiles[sourceWordW1].Intersect(WordAndContainingFiles[queryWordW2]).Count() : 0;

                    double nPmi;
                    if (countW1W2 == 0)
                    {
                        nPmi = -1;
                    }
                    else if (countW1 == countW1W2 && countW2 == countW1W2)
                    {
                        nPmi = 1;
                    }
                    else
                    {
                        nPmi = (Math.Log10((double)countW1 / n * countW2 / n) / Math.Log10((double)countW1W2 / n)) - 1;
                    }
                    nPmiDictionary.Add(sourceWordW1, nPmi);
                }

                var print = nPmiDictionary.Where(x => x.Value > -1).ToDictionary(x => x.Key, x => x.Value.ToString("#.000"));
                File.WriteAllText(outputFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(print));
            }
        }
    }
}
