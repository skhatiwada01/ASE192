using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GithubProcessor.Models;

namespace GithubProcessor.BugLocalization
{
    public class PmiUsingCache : BaseLocalization
    {
        private string _appendLog;

        public PmiUsingCache(string sourceFilePath, string outputFolderPath, string appendLog)
            : base(sourceFilePath, outputFolderPath)
        {
            _appendLog = appendLog;
            ExecuteBase();
        }

        public void Execute(List<string> queryTexts, string pmiCacheFolderPath, string appendTextToFileName)
        {
            var queryTextsDistinct = queryTexts.Distinct().ToList();
            var tssDocumentDictionary = new MyDoubleDictionary();

            // Create list of word contained in query
            var nPmiMatrix = new Dictionary<string, Dictionary<string, double>>();
            foreach (var queryText in queryTextsDistinct)
            {
                var pmiDictionaryString = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.Combine(pmiCacheFolderPath, $"_{queryText}.txt")));
                var pmiDictionary = pmiDictionaryString.ToDictionary(x => x.Key, x => double.Parse(x.Value));
                nPmiMatrix.Add(queryText, pmiDictionary);
            }

            // Compute pmi for each word in WordAndContainingFiles and unique words in query
            int totalNumberOfDocumentInSource = CodeFilesWithContent.Count;
            int counter = 0;
            foreach (var sourceFileWithWords in CodeFilesWithContent)
            {
                counter++;
                Log($"{_appendLog} Running PMI: {counter} of {CodeFilesWithContent.Count}");

                var sourceWords = sourceFileWithWords.Value.ToList();
                double sumQueryTimeIdf = 0.0;
                double sumQueryIdf = 0.0;

                var queryTextsDistinctMaxSimDictionary = new Dictionary<string, double>();

                foreach (var queryWord in queryTexts)
                {
                    if (!queryTextsDistinctMaxSimDictionary.ContainsKey(queryWord))
                    {
                        double maxSimCurrent = sourceWords.AsParallel().Select(x => nPmiMatrix[queryWord].ContainsKey(x) ? nPmiMatrix[queryWord][x] : -1).MyMax(-1);
                        queryTextsDistinctMaxSimDictionary.Add(queryWord, maxSimCurrent);
                    }

                    double maxSim = queryTextsDistinctMaxSimDictionary[queryWord];

                    // if term does not occur in any corpus then its only in use case hence -1
                    double idf = 0;
                    if (WordAndContainingFiles.ContainsKey(queryWord))
                        idf = Math.Log10((double)totalNumberOfDocumentInSource / WordAndContainingFiles[queryWord].Count);

                    sumQueryIdf += idf;
                    sumQueryTimeIdf += (maxSim * idf);
                }

                double sumCorpusTimeIdf = 0.0;
                double sumCorpusIdf = 0.0;

                var sourceWordsAsIntsMaxSimDictionary = new Dictionary<string, double>();
                foreach (var sourceWord in sourceWords)
                {
                    if (!sourceWordsAsIntsMaxSimDictionary.ContainsKey(sourceWord))
                    {
                        double maxSimCurrent = queryTextsDistinct.AsParallel().Select(x => nPmiMatrix[x].ContainsKey(sourceWord) ? nPmiMatrix[x][sourceWord] : -1).MyMax(-1);
                        sourceWordsAsIntsMaxSimDictionary[sourceWord] = maxSimCurrent;
                    }

                    var maxSim = sourceWordsAsIntsMaxSimDictionary[sourceWord];

                    // sourceWord has to be in IdfDictionary
                    double idf = Math.Log10((double)totalNumberOfDocumentInSource / WordAndContainingFiles[sourceWord].Count);

                    sumCorpusIdf += idf;
                    sumCorpusTimeIdf += (maxSim * idf);
                }

                double tss = sumQueryIdf == 0 || sumCorpusIdf == 0 ? -1 : (1.0 / 2) * ((sumQueryTimeIdf / sumQueryIdf) + (sumCorpusTimeIdf / sumCorpusIdf));
                tssDocumentDictionary.Add(sourceFileWithWords.Key, tss);
            }

            // WRITE TO FILE
            WriteDocumentVectorToFileOrderedDescending("Pmi" + appendTextToFileName, tssDocumentDictionary);
        }
    }
}
