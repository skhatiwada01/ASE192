using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GithubProcessor.Models;

namespace GithubProcessor.BugLocalization
{
    public class Pmi : BaseLocalization
    {
        public Pmi(string sourceFilePath, string outputFolderPath)
            : base(sourceFilePath, outputFolderPath)
        {

        }

        public void Execute(string queryFilePath)
        {
            var queryText = File.ReadAllLines(queryFilePath).Where(x => x.Length > 2).ToList();
            Execute(queryText, "");
        }

        public void Execute(List<string> queryText, string appendTextToFileName)
        {
            ExecuteBase();

            var tssDocumentDictionary = new MyDoubleDictionary();

            // Create list of word contained in query
            var distinctQueryWordList = queryText.Distinct().ToList(); // DISTINCT HERE but since its calculating PMI done remove it
            var nPmiMatrix = new MyAnyDictionary<MyDoubleDictionary>();
            int n = CodeFilesWithContent.Count;

            // Compute pmi for each word in WordAndContainingFiles and unique words in query
            foreach (var queryWordW2 in distinctQueryWordList)
            {
                var nPmiDictionary = new MyDoubleDictionary();

                foreach (var sourceWordW1 in WordAndContainingFiles.Keys)
                {
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
                nPmiMatrix.Add(queryWordW2, nPmiDictionary);
            }

            var queryWordListForTss = queryText.ToList();
            int totalNumberOfDocumentInSource = CodeFilesWithContent.Count;
            foreach (var sourceFileWithWords in CodeFilesWithContent)
            {
                var sourceWords = sourceFileWithWords.Value.ToList();
                double sumQueryTimeIdf = 0.0;
                double sumQueryIdf = 0.0;

                foreach (var queryWord in queryWordListForTss)
                {
                    double maxSim = -1;
                    foreach (var sourceWord in sourceWords)
                    {
                        double currentnPmi = nPmiMatrix[queryWord][sourceWord];
                        if (maxSim < currentnPmi)
                            maxSim = currentnPmi;
                    }

                    // if term does not occur in any corpus then its only in use case hence -1
                    double idf = 0;
                    if (WordAndContainingFiles.ContainsKey(queryWord))
                        idf = Math.Log10((double)totalNumberOfDocumentInSource / WordAndContainingFiles[queryWord].Count);

                    sumQueryIdf += idf;
                    sumQueryTimeIdf += (maxSim * idf);
                }

                double sumCorpusTimeIdf = 0.0;
                double sumCorpusIdf = 0.0;

                foreach (string sourceWord in sourceWords)
                {
                    double maxSim = -1;
                    foreach (string useCaseWord in queryWordListForTss)
                    {
                        double currentNPmi = nPmiMatrix[useCaseWord][sourceWord];
                        if (maxSim < currentNPmi)
                            maxSim = currentNPmi;
                    }

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
