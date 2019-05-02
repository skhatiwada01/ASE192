using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GithubProcessor.Models;

namespace GithubProcessor.BugLocalization
{
    public class BaseLocalization
    {
        private readonly string _sourceFilePath;
        private readonly string _outputFolderPath;
        public BaseLocalization(string sourceFilePath, string outputFolderPath)
        {
            _sourceFilePath = sourceFilePath;
            _outputFolderPath = outputFolderPath;
        }

        public void ExecuteBase()
        {
            ProcessSourceCode();
        }

        public MyDoubleDictionary GetQueryIntDictionary(string queryFilePath)
        {
            var queryText = File.ReadAllLines(queryFilePath).Where(x => x.Length > 2).ToList();
            return GetQueryIntDictionary(queryText);
        }

        public MyDoubleDictionary GetQueryIntDictionary(List<string> queryText)
        {
            var queryTfIdfDictionary = new MyDoubleDictionary();
            queryText.ForEach(queryTfIdfDictionary.IncreaseCount);
            return queryTfIdfDictionary;
        }

        public Dictionary<string, List<string>> CodeFilesWithContent;

        public MyDoubleDictionary IdfDictionary;
        public Dictionary<string, MyDoubleDictionary> TfDictionary;
        public Dictionary<string, MyDoubleDictionary> TfIdfDictionary;
        public Dictionary<string, List<string>> WordAndContainingFiles;

        private void ProcessSourceCode()
        {
            // Read all files
            CodeFilesWithContent = new Dictionary<string, List<string>>();
            foreach (var line in File.ReadAllLines(_sourceFilePath))
            {
                var lineSplit = line.SplitWith("##");
                string[] text = lineSplit[1].SplitWith(",").Where(x => x.Length > 2).ToArray();
                CodeFilesWithContent.Add(lineSplit[0], text.Take(text.Length/50).ToList());
            }

            // compute tf and idf
            TfDictionary = new Dictionary<string, MyDoubleDictionary>();
            IdfDictionary = new MyDoubleDictionary();
            TfIdfDictionary = new Dictionary<string, MyDoubleDictionary>();
            foreach (var fileAndItsWords in CodeFilesWithContent)
            {
                var fileTfDictionary = new MyDoubleDictionary();

                // for each word in the file add 1 to the count
                foreach (string word in fileAndItsWords.Value)
                {
                    fileTfDictionary.IncreaseCount(word);
                }

                // save tf result for the file
                TfDictionary.Add(fileAndItsWords.Key, fileTfDictionary);

                // for each DISTINCT word found in the file increase the idf by 1. At this point idf holds document frequency
                foreach (var wordAndItsCount in fileTfDictionary)
                {
                    IdfDictionary.IncreaseCount(wordAndItsCount.Key);
                }
            }

            // change df to idf
            int totalNumberOfDocuments = CodeFilesWithContent.Count;
            foreach (var wordAndItsDocumentCount in IdfDictionary.ToList()) // to list so that we can change the dictionary
            {
                IdfDictionary[wordAndItsDocumentCount.Key] = Math.Log10(totalNumberOfDocuments / wordAndItsDocumentCount.Value);
            }

            // update tfidf for each file
            foreach (var sourceFileWithTfDictionary in TfDictionary)
            {
                var fileTfIdfDictionary = new MyDoubleDictionary();
                foreach (var wordWithTfCount in sourceFileWithTfDictionary.Value)
                {
                    fileTfIdfDictionary.Add(wordWithTfCount.Key, wordWithTfCount.Value * IdfDictionary[wordWithTfCount.Key]);
                }
                TfIdfDictionary.Add(sourceFileWithTfDictionary.Key, fileTfIdfDictionary);
            }

            WordAndContainingFiles = new Dictionary<string, List<string>>();
            foreach (var sourceFileWithWords in CodeFilesWithContent)
            {
                sourceFileWithWords.Value.Distinct().ToList().ForEach(word =>
                {
                    if (!WordAndContainingFiles.ContainsKey(word))
                        WordAndContainingFiles.Add(word, new List<string>());
                    WordAndContainingFiles[word].Add(sourceFileWithWords.Key);
                });
            }
        }

        public void Log(string text)
        {
            Console.WriteLine(text);
        }

        protected void WriteDocumentVectorToFileOrderedDescending(string fileName, Dictionary<string, double> vector, bool asInt = false)
        {
            string pattern = asInt ? "##" : "##.00000";
            File.WriteAllLines($@"{_outputFolderPath}\{fileName}.txt", vector.ToList().OrderByDescending(x => x.Value).Select(x => x.Key + " " + x.Value.ToString(pattern)));
        }
    }
}
