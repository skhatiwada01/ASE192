using System.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace GithubProcessor.Seer
{
    public class BugLocalizer
    {
        public void brt(ILog logger)
        {
            new DirectoryInfo(@"C:\Research\Localization\Original\SEERS\2_bug_localization_data\brt_data\all_queries").GetDirectories().ToList().ForEach(dirInfo => {
                var dirName = dirInfo.Name.SplitWith("-")[0];
                var contents = File.ReadAllLines(dirInfo.FullName + @"\bug-reports.json");

                var dirPath = @"C:\Research\Localization\Process\Seers\brt\" + dirName + @"\";
                Directory.CreateDirectory(dirPath);
                dirPath = dirPath + @"Issues\";
                Directory.CreateDirectory(dirPath);
                var counter = contents.Length;
                foreach (var content in contents)
                {
                    logger.Log("Remaining: " + counter--);
                    var json = JsonConvert.DeserializeObject<BugReport>(content);

                    var issueDirPath = dirPath + json.key + @"\";
                    Directory.CreateDirectory(issueDirPath);

                    File.WriteAllText(issueDirPath + "OriginalBugReport.txt", json.title + Environment.NewLine + json.original_text);
                    File.WriteAllLines(issueDirPath + "OriginalRelevantFiles.txt", json.fixed_files);
                }
            });
        }

        public void d4j(ILog logger)
        {
            new DirectoryInfo(@"C:\Research\Localization\Original\SEERS\2_bug_localization_data\d4j_data\all_queries\preprocessed").GetFiles().ToList().ForEach(fileInfo =>
            {
                var name = fileInfo.Name.Replace(".json.prep", "");
                var dirPath = @"C:\Research\Localization\Process\Seers\d4j\" + name + @"\";
                Directory.CreateDirectory(dirPath);
                dirPath = dirPath + @"Issues\";
                Directory.CreateDirectory(dirPath);

                var contents = File.ReadAllLines(fileInfo.FullName);
                var counter = contents.Length;
                foreach (var content in contents)
                {
                    logger.Log("Remaining: " + counter--);
                    var json = JsonConvert.DeserializeObject<BugReport1>(content);

                    var issueDirPath = dirPath + json.bug_id + @"\";
                    Directory.CreateDirectory(issueDirPath);

                    File.WriteAllText(issueDirPath + "OriginalBugReport.txt", json.text);
                    File.WriteAllLines(issueDirPath + "OriginalRelevantFiles.txt", json.class_goldset);
                }
            });
        }

        public void lb(ILog logger)
        {
            new DirectoryInfo(@"C:\Research\Localization\Original\SEERS\2_bug_localization_data\lb_data\all_queries").GetDirectories().ToList().ForEach(dirInfo =>
            {
                var name = dirInfo.Name;
                var dirPath = @"C:\Research\Localization\Process\Seers\lb\" + name + @"\";
                Directory.CreateDirectory(dirPath);
                dirPath = dirPath + @"Issues\";
                Directory.CreateDirectory(dirPath);

                var contents = File.ReadAllLines(dirInfo.FullName + @"\bug-reports.json");
                var counter = contents.Length;
                foreach (var content in contents)
                {
                    logger.Log("Remaining: " + counter--);
                    var json = JsonConvert.DeserializeObject<BugReport>(content);

                    var issueDirPath = dirPath + json.key + @"\";
                    Directory.CreateDirectory(issueDirPath);

                    File.WriteAllText(issueDirPath + "OriginalBugReport.txt", json.title + Environment.NewLine + json.original_text);
                    File.WriteAllLines(issueDirPath + "OriginalRelevantFiles.txt", json.fixed_files);
                }
            });
        }

        public void qq(ILog logger)
        {
            new DirectoryInfo(@"C:\Research\Localization\Original\SEERS\2_bug_localization_data\qq_data\all_queries\not_preprocessed").GetFiles().ToList().ForEach(fileInfo =>
            {
                var name = fileInfo.Name.Replace(".json", "");//.SplitWith("-")[0];
                var dirPath = @"C:\Research\Localization\Process\Seers\qq\" + name + @"\";
                Directory.CreateDirectory(dirPath);
                dirPath = dirPath + @"Issues\";
                Directory.CreateDirectory(dirPath);

                var contents = File.ReadAllLines(fileInfo.FullName);
                var counter = contents.Length;
                foreach (var content in contents)
                {
                    logger.Log("Remaining: " + counter--);
                    var json = JsonConvert.DeserializeObject<BugReport1>(content);

                    var issueDirPath = dirPath + json.id + @"\";
                    Directory.CreateDirectory(issueDirPath);

                    File.WriteAllText(issueDirPath + "OriginalBugReport.txt", json.text);
                    File.WriteAllLines(issueDirPath + "OriginalRelevantFiles.txt", json.method_goldset.Select(x => x.SplitWith(":")[0]).Distinct());
                }
            });
        }

        public void brt_FileList()
        {
            var infos = File.ReadAllLines(@"C:\Research\Localization\Original\SEERS\2_bug_localization_data\brt_data\code_corpus\preprocessed\swt-3.1-source-code.json");
            var directoryPath = @"C:\Research\Localization\Process\Seers\brt\swt\";

            //var infos = File.ReadAllLines(@"C:\Research\Localization\Original\SEERS\2_bug_localization_data\brt_data\code_corpus\preprocessed\eclipse-3.1-source-code.json");
            //var directoryPath = @"C:\Research\Localization\Process\Seers\brt\eclipse\";

            var fileList = new List<string>();
            var counter = 1;
            Directory.CreateDirectory(directoryPath + @"Source\");
            foreach(var info in infos)
            {
                Console.WriteLine(counter + " of " + infos.Length);

                var json = JsonConvert.DeserializeObject<FileInfo>(info);
                counter++;
                fileList.Add(counter + "," + json.file_path);
                File.WriteAllLines(directoryPath + $@"Source\{counter}.txt", json.text.SplitWith(" "));
            }
            File.WriteAllLines(directoryPath + @"FileList.txt", fileList);
        }

        public void d4j_FileList()
        {
            int counter = 1;
            var allSources = new DirectoryInfo(@"C:\Research\Localization\Original\SEERS\2_bug_localization_data\d4j_data\code_corpus\preprocessed").GetFiles().ToList();
            allSources.AsParallel().ForAll(fileInfo =>
            {
                Console.WriteLine(counter++ + " of " + allSources.Count);
                var bugName = fileInfo.Name.Substring(5).Replace(".json.prep", "");
                var systemName = fileInfo.Name.Substring(0, 4);
                var infos = File.ReadAllLines(fileInfo.FullName).ToList().Select(info =>
                {
                    var json = JsonConvert.DeserializeObject<D4JSource>(info);
                    return new { FileName = json.qname.SplitWith(":")[0], Text = json.text };
                })
                .GroupBy(x => x.FileName)
                .ToList()
                .Select((x, index) => new { Index = index + 1, FileName = x.Key, Text = string.Join(" ", x.Select(y => y.Text)) })
                .ToList();

                var dirPath = @"C:\Research\Localization\Process\Seers\d4j\";
                if (systemName == "Lang")
                    dirPath += @"lang\";
                else if (systemName == "Math")
                    dirPath += @"math\";
                else if (systemName == "Time")
                    dirPath += @"time\";
                else
                    throw new Exception();

                dirPath += @"Issues\" + bugName + @"\";

                File.WriteAllLines(dirPath + @"\FileList.txt", infos.Select(x => x.Index + "," + x.FileName));

                var sourceDirPath = dirPath + @"Source\";
                if (Directory.Exists(sourceDirPath))
                {
                    Directory.Delete(sourceDirPath, true);
                    Thread.Sleep(100);
                }
                Directory.CreateDirectory(sourceDirPath);
                
                foreach (var info in infos)
                {
                    File.WriteAllLines(sourceDirPath + info.Index + ".txt", info.Text.SplitWith(" "));
                }
            });
        }

        public void lb_FileList()
        {
            var allSources = new DirectoryInfo(@"C:\Research\Localization\Original\SEERS\2_bug_localization_data\lb_data\code_corpus\preprocessed").GetDirectories().ToList();
            foreach(var source in allSources)
            {
                var infos = File.ReadAllLines(source.FullName + @"\source-code.json");

                var name = source.Name;
                var directoryPath = @"C:\Research\Localization\Process\Seers\lb\" + name + @"\";
                int counter = 0;
                var fileList = new List<string>();
                Directory.CreateDirectory(directoryPath + @"Source\");
                foreach (var info in infos)
                {
                    counter++;
                    Console.WriteLine(counter + " of " + infos.Length);

                    var json = JsonConvert.DeserializeObject<FileInfo>(info);
                    
                    fileList.Add(counter + "," + json.file_path.ToLowerInvariant());
                    File.WriteAllLines(directoryPath + $@"Source\{counter}.txt", json.text.SplitWith(" "));
                }
                File.WriteAllLines(directoryPath + @"FileList.txt", fileList);
            }
        }

        public void qq_FileList()
        {
            int counter = 1;
            var allSources = new DirectoryInfo(@"C:\Research\Localization\Original\SEERS\2_bug_localization_data\qq_data\code_corpus\preprocessed").GetFiles().ToList();
            allSources.ForEach(fileInfo =>
            {
                Console.WriteLine(counter++ + " of " + allSources.Count);

                var systemName = fileInfo.Name.Replace(".json.prep", "");
                var dirPath = @"C:\Research\Localization\Process\Seers\qq\" + systemName + @"\";

                var infos = File.ReadAllLines(fileInfo.FullName).ToList().Select(info =>
                {
                    var json = JsonConvert.DeserializeObject<D4JSource>(info);
                    return new { FileName = json.qname.SplitWith(":")[0], Text = json.text };
                })
                .GroupBy(x => x.FileName)
                .ToList()
                .Select((x, index) => new { Index = index + 1, FileName = x.Key, Text = string.Join(" ", x.Select(y => y.Text)) })
                .ToList();

                File.WriteAllLines(dirPath + @"\FileList.txt", infos.Select(x => x.Index + "," + x.FileName.ToLowerInvariant()));

                var sourceDirPath = dirPath + @"Source\";
                if (Directory.Exists(sourceDirPath))
                {
                    Directory.Delete(sourceDirPath, true);
                    Thread.Sleep(100);
                }
                Directory.CreateDirectory(sourceDirPath);

                foreach (var info in infos)
                {
                    File.WriteAllLines(sourceDirPath + info.Index + ".txt", info.Text.SplitWith(" "));
                }
            });
        }

        public void brt_eclipse_Link()
        {
            // eclipse
            var fileList = File.ReadAllLines(@"C:\Research\Localization\Process\Seers\brt\eclipse\FileList.txt")
                .Select(x => x.SplitWith(","))
                .Select(x => new { FileName = x[1].ToLowerInvariant().Replace(@"/", "."), Index = x[0]})
                .ToList();
            var issueDirInfos = new DirectoryInfo(@"C:\Research\Localization\Process\Seers\brt\eclipse\Issues").GetDirectories().ToList();
            var counter = issueDirInfos.Count;
            foreach(var issueDirInfo in issueDirInfos)
            {
                Console.WriteLine(counter--);
                File.WriteAllLines(issueDirInfo.FullName + @"\BugReport.txt", File.ReadAllText(issueDirInfo.FullName + @"\OriginalBugReport.txt").CamelCaseSplit().RemoveStopWords().Stem().RemoveStopWords().Where(x => x.Length > 2));

                var originalRelList = File.ReadAllLines(issueDirInfo.FullName + @"\OriginalRelevantFiles.txt").Select(x => x.ToLowerInvariant()).ToList();
                var newList = new List<string>();
                foreach(var originalRel in originalRelList)
                {
                    string index = null;
                    var javaName = originalRel + ".java";
                    foreach(var file in fileList)
                    {
                        if (file.FileName.EndsWith(javaName))
                        {
                            index = file.Index;
                            break;
                        }
                    }
                    if (index == null)
                        throw new Exception();
                    newList.Add(index);
                }
                File.WriteAllLines(issueDirInfo.FullName + @"\RelevantList.txt", newList);
            }
        }

        public void brt_swt_Link()
        {
            // swt
            var fileList = File.ReadAllLines(@"C:\Research\Localization\Process\Seers\brt\swt\FileList.txt")
                .Select(x => x.SplitWith(","))
                .Select(x => new { FileName = x[1].ToLowerInvariant().Replace(@"/", "."), Index = x[0] })
                .ToList();
            var issueDirInfos = new DirectoryInfo(@"C:\Research\Localization\Process\Seers\brt\swt\Issues").GetDirectories().ToList();
            var counter = issueDirInfos.Count;
            foreach (var issueDirInfo in issueDirInfos)
            {
                Console.WriteLine(counter--);
                File.WriteAllLines(issueDirInfo.FullName + @"\BugReport.txt", File.ReadAllText(issueDirInfo.FullName + @"\OriginalBugReport.txt").CamelCaseSplit().RemoveStopWords().Stem().RemoveStopWords().Where(x => x.Length > 2));

                var originalRelList = File.ReadAllLines(issueDirInfo.FullName + @"\OriginalRelevantFiles.txt").Select(x => x.ToLowerInvariant()).ToList();
                var newList = new List<string>();
                foreach (var originalRel in originalRelList)
                {
                    string index = null;
                    var javaName = originalRel + ".java";
                    foreach (var file in fileList)
                    {
                        if (file.FileName.EndsWith(javaName))
                        {
                            index = file.Index;
                            break;
                        }
                    }
                    if (index == null)
                        throw new Exception();
                    newList.Add(index);
                }
                File.WriteAllLines(issueDirInfo.FullName + @"\RelevantList.txt", newList);
            }
        }

        public void d4j_Link()
        {
            new DirectoryInfo(@"C:\Research\Localization\Process\Seers\d4j").GetDirectories().ToList().ForEach(projectDirInfo =>
            {
                var issueDirInfos = new DirectoryInfo(projectDirInfo.FullName + @"\Issues\").GetDirectories().ToList();
                var counter = issueDirInfos.Count;
                foreach(var issueDirInfo in issueDirInfos)
                {
                    Console.WriteLine(counter--);

                    File.WriteAllLines(issueDirInfo.FullName + @"\BugReport.txt", File.ReadAllText(issueDirInfo.FullName + @"\OriginalBugReport.txt").CamelCaseSplit().RemoveStopWords().Stem().RemoveStopWords().Where(x => x.Length > 2));

                    var fileList = File.ReadAllLines(issueDirInfo.FullName + @"\FileList.txt")
                        .Select(x => x.SplitWith(","))
                        .Select(x => new { FileName = x[1].ToLowerInvariant(), Index = x[0] })
                        .ToList();
                    var newT = File.ReadAllText(issueDirInfo.FullName + @"\OriginalRelevantFiles.txt").SplitWith(";").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Replace("\r\n", "")).ToList();
                    File.WriteAllLines(issueDirInfo.FullName + @"\OriginalRelevantFiles.txt", newT);
                    var originalRelList = File.ReadAllLines(issueDirInfo.FullName + @"\OriginalRelevantFiles.txt").Select(x => x.ToLowerInvariant()).ToList();

                    var newList = new List<string>();
                    foreach (var originalRel in originalRelList)
                    {
                        string index = null;
                        foreach (var file in fileList)
                        {
                            if (file.FileName == originalRel)
                            {
                                index = file.Index;
                                break;
                            }
                        }
                        if (index == null)
                            throw new Exception();
                        newList.Add(index);
                    }
                    File.WriteAllLines(issueDirInfo.FullName + @"\RelevantList.txt", newList);
                }
            });
        }

        public void lb_Link()
        {
            new DirectoryInfo(@"C:\Research\Localization\Process\Seers\lb").GetDirectories().ToList().ForEach(projectDirInfo =>
            {
                var fileList = File.ReadAllLines(projectDirInfo.FullName + @"\FileList.txt")
                    .Select(x => x.SplitWith(","))
                    .Select(x => new { FileName = x[1].ToLowerInvariant(), Index = x[0] })
                    .ToList();

                var issueDirInfos = new DirectoryInfo(projectDirInfo.FullName + @"\Issues\").GetDirectories().ToList();
                var counter = issueDirInfos.Count;
                foreach (var issueDirInfo in issueDirInfos)
                {
                    Console.WriteLine(counter--);

                    File.WriteAllLines(issueDirInfo.FullName + @"\BugReport.txt", File.ReadAllText(issueDirInfo.FullName + @"\OriginalBugReport.txt").CamelCaseSplit().RemoveStopWords().Stem().RemoveStopWords().Where(x => x.Length > 2));
                    
                    var originalRelList = File.ReadAllLines(issueDirInfo.FullName + @"\OriginalRelevantFiles.txt").ToList();

                    var filteredOriginalList = new List<string>();
                    var newList = new List<string>();

                    foreach (var originalRelUpper in originalRelList)
                    {
                        var originalRel = originalRelUpper.ToLowerInvariant();
                        string index = null;
                        foreach (var file in fileList)
                        {
                            if (file.FileName == originalRel)
                            {
                                index = file.Index;
                                break;
                            }
                        }
                        if (index != null)
                        {
                            newList.Add(originalRel);
                            filteredOriginalList.Add(originalRelUpper);
                        }
                    }
                    if (newList.Count == 0)
                        throw new Exception();
                    File.WriteAllLines(issueDirInfo.FullName + @"\OriginalRelevantFiles.txt", filteredOriginalList);
                    File.WriteAllLines(issueDirInfo.FullName + @"\RelevantList.txt", newList);
                }
            });
        }

        public void qq_Link()
        {
            new DirectoryInfo(@"C:\Research\Localization\Process\Seers\qq").GetDirectories().ToList().ForEach(projectDirInfo =>
            {
                var fileList = File.ReadAllLines(projectDirInfo.FullName + @"\FileList.txt")
                    .Select(x => x.SplitWith(","))
                    .Select(x => new { FileName = x[1].ToLowerInvariant(), Index = x[0] })
                    .ToList();

                var issueDirInfos = new DirectoryInfo(projectDirInfo.FullName + @"\Issues\").GetDirectories().ToList();
                var counter = issueDirInfos.Count;
                foreach (var issueDirInfo in issueDirInfos)
                {
                    Console.WriteLine(counter--);

                    File.WriteAllLines(issueDirInfo.FullName + @"\BugReport.txt", File.ReadAllText(issueDirInfo.FullName + @"\OriginalBugReport.txt").CamelCaseSplit().RemoveStopWords().Stem().RemoveStopWords().Where(x => x.Length > 2));

                    var originalRelList = File.ReadAllLines(issueDirInfo.FullName + @"\OriginalRelevantFiles.txt").ToList();

                    var filteredOriginalList = new List<string>();
                    var newList = new List<string>();

                    foreach (var originalRelUpper in originalRelList)
                    {
                        var originalRel = originalRelUpper.ToLowerInvariant();
                        string index = null;
                        foreach (var file in fileList)
                        {
                            if (file.FileName == originalRel)
                            {
                                index = file.Index;
                                break;
                            }
                        }
                        if (index != null)
                        {
                            newList.Add(originalRel);
                            filteredOriginalList.Add(originalRelUpper);
                        }
                    }
                    if (newList.Count == 0)
                        throw new Exception();
                    File.WriteAllLines(issueDirInfo.FullName + @"\OriginalRelevantFiles.txt", filteredOriginalList);
                    File.WriteAllLines(issueDirInfo.FullName + @"\RelevantList.txt", newList);
                }
            });
        }

        public class FileInfo
        {
            public string file_path;

            public string text;
        }

        public class BugReport
        {
            public string original_text;

            public string[] fixed_files;

            public string key;

            public string title;
        }

        public class BugReport1
        {
            public string text;

            public string[] class_goldset;

            public string[] method_goldset;

            public int id;

            public string bug_id;
        }

        public class D4JSource
        {
            public string text;

            public string qname;
        }
    }
}
