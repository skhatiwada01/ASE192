using Bluebit.MatrixLibrary;
using GithubProcessor.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GithubProcessor.BugLocalization
{
    public class Lsi : BaseLocalization
    {
        private Dictionary<int, Matrix> _uk;
        private Dictionary<int, Matrix> _sk;
        private Dictionary<int, Matrix> _vkTranspose;

        public string LogAdd { get; set; }

        public readonly List<int> LsiKs = new List<int>() { 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550, 600, 650, 700, 750, 800, 850, 900 };

        private string _outputLsiFolderPath;

        public Lsi(string sourceFolderPath, string outputFolderPath)
            : base(sourceFolderPath, outputFolderPath)
        {
            _outputLsiFolderPath = outputFolderPath;
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
            Log("SVD: " + LogAdd);

            // create the matrix
            int totalNumberOfSourceFiles = TfDictionary.Count;
            int totalDistinctTermsInAllSourceFiles = IdfDictionary.Count;
            Dictionary<string, int> allSourceFilesWithIndex = TfDictionary.Keys.Select((x, index) => new { Name = x, Index = index }).ToDictionary(x => x.Name, x => x.Index);
            Dictionary<string, int> allSourceWordsWithIndex = IdfDictionary.Keys.Select((x, index) => new { Name = x, Index = index }).ToDictionary(x => x.Name, x => x.Index);

            double[,] sourceMatrix = new double[totalDistinctTermsInAllSourceFiles, totalNumberOfSourceFiles]; // row, col row is word col docs

            foreach (var fileNameWithTfDictionary in TfDictionary)
            {
                int fileIndex = allSourceFilesWithIndex[fileNameWithTfDictionary.Key];
                foreach (var fileWordWithTf in fileNameWithTfDictionary.Value)
                {
                    sourceMatrix[allSourceWordsWithIndex[fileWordWithTf.Key], fileIndex] = fileWordWithTf.Value;
                }
            }

            // create matrix
            Matrix generalMatrix = new Matrix(sourceMatrix);

            // singular value decomposition
            SVD svd = new SVD(generalMatrix);

            _uk = new Dictionary<int, Matrix>();
            _sk = new Dictionary<int, Matrix>();
            _vkTranspose = new Dictionary<int, Matrix>();

            LsiKs.Where(x => x <= svd.S.Cols).ToList().ForEach(k =>
            {
                Log("SVD " + k + ": " + LogAdd);
                _uk.Add(k, new Matrix(svd.U.ToArray(), svd.U.Rows, k));
                _sk.Add(k, new Matrix(svd.S.ToArray(), k, k));
                _vkTranspose.Add(k, new Matrix(svd.VH.ToArray(), k, svd.VH.Cols));
            });

            // create one for query as well
            double[,] queryMatrixTranspose = new double[1, totalDistinctTermsInAllSourceFiles];
            queryTfDictionary.Keys.ToList().ForEach(queryWord =>
            {
                if (allSourceWordsWithIndex.ContainsKey(queryWord))
                    queryMatrixTranspose[0, allSourceWordsWithIndex[queryWord]] = queryMatrixTranspose[0, allSourceWordsWithIndex[queryWord]] + 1;
            });

            var outputResultFolderPath = _outputLsiFolderPath + @"\Lsi\";
            if (!Directory.Exists(outputResultFolderPath))
                Directory.CreateDirectory(outputResultFolderPath);

            var ks = _uk.Keys.Where(x => !File.Exists(outputResultFolderPath + x + ".txt")).ToList();

            foreach (var k in ks)
            {
                var uk = _uk[k];
                var sk = _sk[k];
                var vkTranspose = _vkTranspose[k];

                var q = new Matrix(queryMatrixTranspose);
                var qv = q * uk * sk.Inverse();
                var qDoubles = qv.RowVector(0).ToArray().ToList();

                var similarityList = allSourceFilesWithIndex.Select(doc => new KeyValuePair<string, double>(doc.Key, Helper.GetSimilarity(qDoubles, vkTranspose.ColVector(doc.Value).ToArray().ToList())));
                var similarityDictionary = similarityList.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                WriteDocumentVectorToFileOrderedDescending(@"Lsi\Lsi_" + k + appendTextToFileName, similarityDictionary);
            }
        }
    }
}
