using System.Collections.Generic;
using System.Linq;
using GithubProcessor.Models;

namespace GithubProcessor.BugLocalization
{
    public class Vsm : BaseLocalization
    {
        public Vsm(string sourceFolderPath, string outputFolderPath)
            : base(sourceFolderPath, outputFolderPath)
        {

        }

        private void ExecuteSub(MyDoubleDictionary queryTfIdfDictionary, string appendTextToFileName = "")
        {
            // max frequency
            double maxFrequency = queryTfIdfDictionary.Max(x => x.Value);

            // now multiply each by idf to get tfidf for query
            foreach (var queryWordWithTf in queryTfIdfDictionary.ToList())
            {
                queryTfIdfDictionary[queryWordWithTf.Key] = IdfDictionary.ContainsKey(queryWordWithTf.Key)
                    ? (queryWordWithTf.Value / maxFrequency) * IdfDictionary[queryWordWithTf.Key]
                    : 0;
            }

            // Calculate Similarity
            var similarityDictionary = new MyDoubleDictionary();

            // compute similarity of fileText with each _codeFiles
            foreach (var codeFileWithTfIdfDictionary in TfIdfDictionary)
            {
                double cosineSimilarityWithUseCase = Helper.GetSimilarity(queryTfIdfDictionary, codeFileWithTfIdfDictionary.Value);
                similarityDictionary.Add(codeFileWithTfIdfDictionary.Key, cosineSimilarityWithUseCase);
            }

            // WRITE TO FILE
            WriteDocumentVectorToFileOrderedDescending("Vsm" + appendTextToFileName, similarityDictionary);
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
    }
}
