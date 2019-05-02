using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using GithubProcessor.Models;

namespace GithubProcessor
{
    public static class Helper
    {
        #region Files

        public static void CopyTo(this DirectoryInfo copyFrom, string copyTo)
        {
            if (Directory.Exists(copyTo))
            {
                throw new Exception("Directory Exists");
            }

            Directory.CreateDirectory(copyTo);
            Thread.Sleep(500);
            var sourcePath = copyFrom.FullName;

            foreach (string dirPath in Directory.GetDirectories(copyFrom.FullName, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, copyTo));
            }

            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, copyTo), true);
            }
        }

        public static List<string> GetFilesWithSearch(this DirectoryInfo directory, List<string> extensions)
        {
            if (directory.Name.StartsWith(".git"))
                return new List<string>();

            var files = directory.GetFiles()
                .Where(x => extensions == null || extensions.Any(e => x.Extension == "." + e))
                .Where(x => !x.Name.StartsWith(".git"))
                .Select(x => x.FullName)
                .ToList();

            var subDirs = directory.GetDirectories().ToList();
            foreach (var subDir in subDirs)
            {
                files.AddRange(subDir.GetFilesWithSearch(extensions));
            }

            return files;
        }

        #endregion Files

        #region Split

        public static HashSet<string> EnglishWords = new HashSet<string>(Properties.Resources.EnglishWords.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).Select(x => new PorterStemmer().GetStemmedText(x)).Distinct().Select(x => x.ToLowerInvariant()));
        public static HashSet<string> StopWords = new HashSet<string>(Properties.Resources.StopWords.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).Select(x => new PorterStemmer().GetStemmedText(x)).Distinct().Select(x => x.ToLowerInvariant()));

        private static readonly Regex CamelcaseSplitterOnSymbols = new Regex("[^a-zA-Z]+");
        private static readonly Regex CamelcaseSplitterOnTransition = new Regex("(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z](?:s[a-z]|[abcdefghijklmnopqrtuvwxyz]))");
        private static readonly Regex LeadingTrailingNonAlphabetRegex = new Regex("[^A-Za-z]+|[^A-Za-z]+$");

        public static IEnumerable<string> CamelCaseSplit(this string identifier)
        {
            var splitsOnSymbols = CamelcaseSplitterOnSymbols.Split(identifier);
            foreach (string symbolSplit in splitsOnSymbols)
            {
                string trailingRemoved = LeadingTrailingNonAlphabetRegex.Replace(symbolSplit, "");
                if (trailingRemoved.Length == 0)
                    continue;

                foreach (string splitsOnTransition in CamelcaseSplitterOnTransition.Split(symbolSplit))
                {
                    string split = splitsOnTransition.Trim().ToLowerInvariant();
                    if (split == "")
                        continue;

                    yield return split;
                }
            }
        }

        public static IEnumerable<string> CamelCaseSplitNoTransition(this string identifier)
        {
            var splitsOnSymbols = CamelcaseSplitterOnSymbols.Split(identifier);
            foreach (string symbolSplit in splitsOnSymbols)
            {
                string trailingRemoved = LeadingTrailingNonAlphabetRegex.Replace(symbolSplit, "");
                if (trailingRemoved.Length == 0)
                    continue;

                yield return trailingRemoved;
            }
        }

        public static IEnumerable<string> RemoveStopWords(this IEnumerable<string> texts)
        {
            return texts.Where(x => !StopWords.Contains(x));
        }

        public static IEnumerable<string> Stem(this IEnumerable<string> texts)
        {
            return texts.Select(x => new PorterStemmer().GetStemmedText(x));
        }

        public static string[] SplitWith(this string text, string separator)
        {
            return text.Split(new[] { separator }, StringSplitOptions.None);
        }

        #endregion Split

        #region Math

        public static double GetSimilarity(MyDoubleDictionary vector1, MyDoubleDictionary vector2)
        {
            double length1 = GetLength(vector1);
            double length2 = GetLength(vector2);

            double dotProduct = vector1.Where(wordWithCount => vector2.ContainsKey(wordWithCount.Key)).Sum(wordWithCount => (wordWithCount.Value * vector2[wordWithCount.Key]));

            return vector2.Count == 0 ? 0 : dotProduct / (length1 * length2);
        }

        public static double GetSimilarity(Dictionary<string, double> vector1, Dictionary<string, double> vector2)
        {
            double length1 = GetLength(vector1);
            double length2 = GetLength(vector2);

            double dotProduct = vector1.Where(wordWithCount => vector2.ContainsKey(wordWithCount.Key)).Sum(wordWithCount => (wordWithCount.Value * vector2[wordWithCount.Key]));

            return vector2.Count == 0 ? 0 : dotProduct / (length1 * length2);
        }

        public static double GetSimilarity(Dictionary<string, int> vector1, Dictionary<string, int> vector2)
        {
            double length1 = GetLength(vector1);
            double length2 = GetLength(vector2);

            double dotProduct = vector1.Where(wordWithCount => vector2.ContainsKey(wordWithCount.Key)).Sum(wordWithCount => (wordWithCount.Value * vector2[wordWithCount.Key]));

            return vector2.Count == 0 ? 0 : dotProduct / (length1 * length2);
        }

        public static double MyAverage(this IEnumerable<double> enumerable)
        {
            var list = enumerable.ToList();
            var x = list.Count == 0 ? 0 : list.Sum() / list.Count;
            return x;
        }

        public static double GetSimilarity(IReadOnlyList<double> a1, IReadOnlyList<double> a2)
        {
            double dotProduct = 0;
            double aSum = 0, bSum = 0;

            for (int i = 0; i < a1.Count; i++)
            {
                dotProduct += a1[i] * a2[i];
                aSum += Math.Pow(a1[i], 2);
                bSum += Math.Pow(a2[i], 2);
            }

            return dotProduct / (Math.Sqrt(aSum) * Math.Sqrt(bSum));
        }

        public static double GetLength(MyDoubleDictionary vector)
        {
            double length = Math.Sqrt(vector.Sum(x => Math.Pow(x.Value, 2)));
            return length;
        }

        public static double GetLength(Dictionary<string, int> vector)
        {
            double length = Math.Sqrt(vector.Sum(x => Math.Pow(x.Value, 2)));
            return length;
        }

        public static double GetLength(Dictionary<string, double> vector)
        {
            double length = Math.Sqrt(vector.Sum(x => Math.Pow(x.Value, 2)));
            return length;
        }

        public static double[] JensenSum(this double[] vector1, double[] vector2)
        {
            if (vector1.Length != vector2.Length)
                throw new Exception("Length " + vector1.Length + " does not match vector 2 length: " + vector2.Length);

            double[] result = new double[vector1.Length];
            for (int i = 0; i < vector1.Length; i++)
                result[i] = (vector1[i] + vector2[i]) / 2;

            return result;
        }

        public static double JensenEntropy(this double[] vector)
        {
            return -1.0 * vector.ToList().Select(x => x * (x == 0 ? 0 : Math.Log(x))).Sum();
        }

        public static double MyMax(this IEnumerable<double> list, double defaultVal)
        {
            return list.Any() ? list.Max() : defaultVal;
        }

        #endregion Math
    }
}
