//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using VnCompany.WebApp.Helpers;
//using VnCompany.WebApp.Models;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading;
//using System.Threading.Tasks;
//using VnCompany.SharedLibs;

//namespace NetCoreMVC.Services
//{
//    public class WorkerSearching : BackgroundService
//    {
//        private readonly ILogger<WorkerSearching> _logger;
//        private readonly IConfiguration _configuration;
//        private readonly IWebHostEnvironment _env;

//        public WorkerSearching(ILogger<WorkerSearching> logger, IConfiguration configuration, IWebHostEnvironment env)
//        {
//            _logger = logger;
//            _configuration = configuration;
//            _env = env;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                var enabled = Utils.ConvertToBoolean(_configuration["WorkerSearchingEnabled"]);
//                var setting = CommonHelpers.GetSystemSettings();
//                if (setting != null)
//                {
//                    enabled = setting.Enabled;
//                }

//                if (enabled)
//                {
//                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

//                    await WorkerProcessing();

//                    var delayTime = Utils.ConvertToInt32(_configuration["WorkerSearchingDelay"], 30000);
//                    if (delayTime == 0)
//                        delayTime = 30000;

//                    Random rnd = new Random();
//                    delayTime = rnd.Next(1, 10);

//                    delayTime = delayTime * 60 * 1000;

//                    await Task.Delay(delayTime, stoppingToken);
//                }
//            }
//        }

//        private async Task WorkerProcessing()
//        {
//            try
//            {
//                var target = GetFirstUnSearchTarget();
//                if (target != null)
//                {
//                    WordsCheckingModel model = new WordsCheckingModel();
//                    model.BreakWithSigns = true;
//                    model.SearchChannel = "google";
//                    model.WordsLimit = 1000;
//                    model.CurrentThreads = 1;
//                    model.Keyword = target.Name.StringNormally();
//                    model.Address = target.Address.StringNormally();
//                    model.SearchType = EnumSearchType.Keyword;

//                    model = BeginWordsChecking(model);
//                    if(model.MatchedResultCount == 0 && !string.IsNullOrEmpty(model.Address))
//                    {                        
//                        Random rnd = new Random();
//                        int delayTime = rnd.Next(1, 10);
//                        delayTime = delayTime * 1000;

//                        Thread.Sleep(delayTime);

//                        //Try to search with keyword and address if has no results
//                        model.SearchType = EnumSearchType.Address;
//                        model = BeginWordsChecking(model);
//                    }

//                    await Task.FromResult(model);

//                    target.MatchedResultCount = model.MatchedResultCount;
//                    target.SessionName = model.SessionName;
//                    target.WasSearched = true;                    

//                    //Marked the target was searched
//                    UpdateSearchedTargetResult(target);

//                    ClearInputAndResultFiles();
//                }
//                else
//                {
//                    _logger.LogInformation("All targets was searched or list is empty !");
//                }                
//            }
//            catch (Exception ex)
//            {
//                string strError = "Failed for WorkerProcessing: " + ex.ToString();
//                _logger.LogError(strError);
//            }
//        }

//        #region Helpers

//        private SearchInputItemModel GetFirstUnSearchTarget()
//        {
//            SearchInputItemModel info = null;
//            try
//            {
//                var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), ConfigConstants.SearchInputJsonFile);
//                var list = FileHelpers<List<SearchInputItemModel>>.ReadJsonFileToObject(jsonPath);

//                if (list != null && list.Count > 0)
//                    info = list.Where(x=>x.WasSearched == false).OrderBy(x=>x.CreatedDate).FirstOrDefault();
//            }
//            catch (Exception ex)
//            {
//                string strError = "Failed for GetFirstUnSearchTarget: " + ex.ToString();
//                _logger.LogError(strError);
//            }

//            return info;
//        }

//        private void UpdateSearchedTargetResult(SearchInputItemModel searchedInfo)
//        {
//            try
//            {
//                if (searchedInfo != null && !string.IsNullOrEmpty(searchedInfo.Id))
//                {
//                    var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), ConfigConstants.SearchInputJsonFile);
//                    var allItems = FileHelpers<List<SearchInputItemModel>>.ReadJsonFileToObject(jsonPath);

//                    if (allItems != null && allItems.Count > 0)
//                    {
//                        foreach (var item in allItems)
//                        {
//                            if (item.Id == searchedInfo.Id)
//                            {
//                                item.SessionName = searchedInfo.SessionName;
//                                item.MatchedResultCount = searchedInfo.MatchedResultCount;
//                                item.WasSearched = searchedInfo.WasSearched;
//                                item.LastSearched = DateTime.UtcNow;

//                                break;
//                            }
//                        }
//                    }

//                    var msg = string.Format("Session: {0} - The target name [{1}] width address [{2}] was searched. Matched results is [{3}]",
//                        searchedInfo.SessionName, searchedInfo.Name, searchedInfo.Address, searchedInfo.MatchedResultCount
//                    );

//                    _logger.LogInformation(msg);

//                    FileHelpers<List<SearchInputItemModel>>.WriteObjectToJsonFile(allItems, jsonPath);
//                }
//            }
//            catch (Exception ex)
//            {
//                string strError = "Failed for UpdateSearchedTargetResult: " + ex.ToString();
//                _logger.LogError(strError);
//            }
//        }

//        private List<string> GetAllPatterns()
//        {
//            List<string> list = new List<string>();
//            try
//            {
//                var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), ConfigConstants.RegularJsonFile);
//                var allPatterns = FileHelpers<List<RegularItemModel>>.ReadJsonFileToObject(jsonPath);
//                if (allPatterns != null && allPatterns.Count > 0)
//                {
//                    list = allPatterns.Select(x => x.Name).ToList();
//                }
//            }
//            catch (Exception ex)
//            {
//                string strError = "Failed for GetAllPatterns: " + ex.ToString();
//                _logger.LogError(strError);
//            }

//            return list;
//        }

//        private bool IsValidDomain(string domain)
//        {
//            var listDomains = _configuration.GetValue<string>("MyApplication:AllowedDomains");

//            if (!string.IsNullOrEmpty(listDomains))
//            {
//                if (listDomains.Contains("*"))
//                    return true;

//                if (listDomains.Contains(domain))
//                    return true;
//            }

//            return false;
//        }

//        private WordsCheckingModel BeginWordsChecking(WordsCheckingModel model, string fileName = "")
//        {
//            //Create folder first
//            var folderPath = FileHelpers.MakeSureFolderExists(Path.Combine(Directory.GetCurrentDirectory(), ConfigConstants.SearchInputFolder));

//            if (!string.IsNullOrEmpty(folderPath))
//            {
//                try
//                {
//                    if (!string.IsNullOrEmpty(fileName))
//                    {
//                        model.Results = new List<WordsCheckingResultItemModel>();

//                        var pdfRs = GetPdfTextResults(fileName);
//                        if (pdfRs != null && pdfRs.Count > 0)
//                            model.Results.AddRange(pdfRs);

//                        var txtRs = GetTextResults(fileName);
//                        if (txtRs != null && txtRs.Count > 0)
//                            model.Results.AddRange(txtRs);

//                        return model;
//                    }

//                    var fileCreated = 1;
//                    fileName = ProcessDataBeforeSearch(model, out fileCreated);
//                    model.SessionName = fileName;

//                    var delayTime = (model.DelayTime <= 0) ? 0 : model.DelayTime;
//                    var absoluteChar = (string.IsNullOrEmpty(model.QuoteType) ? QuoteType.Quote : model.QuoteType);

//                    List<Task> tasks = new List<Task>();

//                    if (fileCreated > 0)
//                    {
//                        for (int i = 1; i <= fileCreated; i++)
//                        {
//                            var fileNamePart = string.Format("{0}_{1}", fileName, i);
//                            //var t = Task.Run(() => {
//                            //        Process p = new Process();
//                            //        p.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), ConfigConstants.SentenceCheckingExe);

//                            //        var paramsFormat = "{0} {1}";
//                            //        p.StartInfo.Arguments = string.Format(paramsFormat,
//                            //              fileNamePart,
//                            //              delayTime
//                            //            );

//                            //        p.Start();
//                            //        //p.WaitForExit();
//                            //});
//                            //t.Wait();

//                            //var paramsFormat = "/k python {0} {1} {2}";
//                            var paramsFormat = "{0} {1} {2}";

//                            var searchChannel = string.Empty;
//                            tasks.Add(Task.Run(delegate
//                            {
//                                var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), string.Format(ConfigConstants.SentenceCheckingScriptPathFormat, EnumSearchEngine.Google));
//                                var setting = CommonHelpers.GetSystemSettings();
//                                if(setting != null)
//                                {
//                                    if(!string.IsNullOrEmpty(setting.SearchEngine))
//                                        scriptPath = Path.Combine(Directory.GetCurrentDirectory(), string.Format(ConfigConstants.SentenceCheckingScriptPathFormat, setting.SearchEngine));
//                                }

//                                //ProcessStartInfo myProcess = new ProcessStartInfo("cmd.exe");
//                                ////myProcess.FileName = Path.Combine(Directory.GetCurrentDirectory(), ConfigConstants.SentenceCheckingGooglePython);
//                                //myProcess.Arguments = string.Format(paramsFormat, scriptPath, fileNamePart, delayTime);
//                                //Process.Start(myProcess).WaitForExit();

//                                System.Diagnostics.Process process = new System.Diagnostics.Process();
//                                process.StartInfo = new System.Diagnostics.ProcessStartInfo()
//                                {
//                                    UseShellExecute = false,
//                                    CreateNoWindow = false,
//                                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
//                                    //FileName = "C:\\Program Files\\Python39\\python.exe",
//                                    FileName = _configuration["PythonExePath"],
//                                    Arguments = string.Format(paramsFormat, scriptPath, fileNamePart, delayTime),
//                                    RedirectStandardError = true,
//                                    RedirectStandardOutput = true
//                                };

//                                process.Start();
//                                process.WaitForExit();
//                            }));

//                            //System.Diagnostics.Process process = new System.Diagnostics.Process();
//                            //process.StartInfo = new System.Diagnostics.ProcessStartInfo()
//                            //{
//                            //    UseShellExecute = false,
//                            //    CreateNoWindow = false,
//                            //    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
//                            //    FileName = Path.Combine(Directory.GetCurrentDirectory(), "GoogleAutomation.py"),
//                            //    Arguments = string.Format(paramsFormat,
//                            //        fileNamePart,
//                            //        delayTime
//                            //    ),
//                            //    RedirectStandardError = true,
//                            //    RedirectStandardOutput = true
//                            //};
//                            //process.Start();
//                            //string output = process.StandardOutput.ReadToEnd();
//                            //process.WaitForExit();
//                        }

//                        Task.WaitAll(tasks.ToArray());
//                    }

//                    model.Results = new List<WordsCheckingResultItemModel>();

//                    var pdfResults = GetPdfTextResults(fileName);
//                    if (pdfResults != null && pdfResults.Count > 0)
//                        model.Results.AddRange(pdfResults);

//                    var txtResults = GetTextResults(fileName);
//                    if (txtResults != null && txtResults.Count > 0)
//                        model.Results.AddRange(txtResults);

//                    model.MatchedResultCount = CountMatchedResults(model);
//                }
//                catch (Exception ex)
//                {
//                    var strError = string.Format("Could not run SentenceCheckingExe due to: {0}", ex.ToString());
//                    _logger.LogError(strError);
//                }
//            }
//            else
//            {
//                var strError = "Could not create SentenceCheckingExe search results folder";
//                _logger.LogError(strError);
//            }

//            _logger.LogDebug("Return model: " + JsonConvert.SerializeObject(model));

//            return model;
//        }

//        private int CountMatchedResults(WordsCheckingModel model)
//        {
//            var matchedCount = 0;
//            model.Patterns = GetAllPatterns();
//            var hasResults = (model.Results != null && model.Results.Count > 0);
//            var hasPatterns = (model.Patterns != null && model.Patterns.Count > 0);

//            if (hasResults)
//            {
//                var counter = 0;
//                foreach (var item in model.Results)
//                {
//                    counter++;
//                    List<string> allWords = new List<string>();
//                    List<string> validWords = new List<string>();
//                    var hiddenCls = string.Empty;
//                    if (!string.IsNullOrEmpty(item.Content))
//                    {
//                        allWords = item.Content.Split(@"\n").ToList();
//                    }

//                    var hasWords = (allWords != null && allWords.Count > 0);

//                    if (hasWords)
//                    {
//                        foreach (var w in allWords)
//                        {
//                            var vWord = TextHelpers.ReplaceByPattern(w, model.IgnoreStrings);
//                            validWords.Add(vWord);
//                        }
//                    }

//                    var hasValidWords = (validWords != null && validWords.Count > 0);
//                    var mergeStr = new StringBuilder();

//                    if (hasPatterns && hasValidWords)
//                    {
//                        foreach (var p in model.Patterns)
//                        {
//                            var matchedList = validWords.Where(x => x.Contains(p)).ToList();
//                            if (matchedList != null && matchedList.Count > 0)
//                            {
//                                foreach (var m in matchedList)
//                                {
//                                    var matchedStr = TextHelpers.ReplaceByPattern(m, model.IgnoreStrings);
//                                    var subStr = TextHelpers.SubStringFromString(matchedStr, p);

//                                    if (!string.IsNullOrEmpty(subStr))
//                                    {
//                                        mergeStr.AppendLine(subStr);
//                                    }
//                                }
//                            }
//                        }
//                    }

//                    if (mergeStr.Length == 0)
//                    {
//                        continue;
//                    }
//                    else
//                    {
//                        matchedCount++;
//                    }
//                }
//            }

//            return matchedCount;
//        }

//        private List<WordsCheckingResultItemModel> GetPdfTextResults(string fileName)
//        {
//            var list = new List<WordsCheckingResultItemModel>();

//            var resultPath = FileHelpers.MakeSureFolderExists(Path.Combine(Directory.GetCurrentDirectory(), ConfigConstants.SearchResultsFolder));
//            string[] pdfFiles = System.IO.Directory.GetFiles(resultPath, fileName + "*.pdf");
//            if (pdfFiles != null && pdfFiles.Count() > 0)
//            {
//                foreach (var f in pdfFiles)
//                {
//                    var content = FileHelpers.ExtractTextFromPdf(f);
//                    if (!string.IsNullOrEmpty(content))
//                    {
//                        if (System.IO.File.Exists(f))
//                        {
//                            var fileInfo = new FileInfo(f);
//                            if (fileInfo != null && fileInfo.Length > 0)
//                            {
//                                var item = new WordsCheckingResultItemModel();
//                                item.Path = fileInfo.Name;
//                                item.Content = content;
//                                item.Type = EnumResultType.File;

//                                list.Add(item);
//                            }
//                        }
//                    }
//                }
//            }

//            //resultPath = resultPath + "/" + fileName;
//            //var fileNamePart = string.Format("{0}_{1}.pdf", resultPath, 1);
//            //var content = FileHelpers.ParsePdfToTextUsingPDFBox(fileNamePart);

//            return list;
//        }

//        private List<WordsCheckingResultItemModel> GetTextResults(string fileName)
//        {
//            var list = new List<WordsCheckingResultItemModel>();

//            var resultPath = FileHelpers.MakeSureFolderExists(Path.Combine(Directory.GetCurrentDirectory(), ConfigConstants.SearchResultsFolder));
//            string[] files = System.IO.Directory.GetFiles(resultPath, fileName + "*.txt");
//            if (files != null && files.Count() > 0)
//            {
//                foreach (var f in files)
//                {
//                    if (System.IO.File.Exists(f))
//                    {
//                        var fileInfo = new FileInfo(f);
//                        if (fileInfo != null && fileInfo.Length > 0)
//                        {
//                            var content = System.IO.File.ReadAllText(f);
//                            var results = JsonConvert.DeserializeObject<List<SearchResultModel>>(content);

//                            if (results != null && results.Count > 0)
//                            {
//                                foreach (var r in results)
//                                {
//                                    var item = new WordsCheckingResultItemModel();
//                                    item.Path = fileInfo.Name;
//                                    item.Content = r.Desc;
//                                    item.Type = EnumResultType.Text;

//                                    list.Add(item);
//                                }
//                            }
//                            // Read a text file line by line.  
//                            //string[] lines = System.IO.File.ReadAllLines(f);
//                            //if (lines != null && lines.Count() > 0)
//                            //{
//                            //    foreach (var line in lines)
//                            //    {
//                            //        if (!string.IsNullOrEmpty(line))
//                            //        {
//                            //            var content = line.StringNormally();

//                            //            var item = new WordsCheckingResultItemModel();
//                            //            item.Path = fileInfo.Name;
//                            //            item.Content = content;
//                            //            item.Type = EnumResultType.Text;

//                            //            list.Add(item);
//                            //        }
//                            //    }
//                            //}                           
//                        }
//                    }
//                }
//            }

//            //resultPath = resultPath + "/" + fileName;
//            //var fileNamePart = string.Format("{0}_{1}.pdf", resultPath, 1);
//            //var content = FileHelpers.ParsePdfToTextUsingPDFBox(fileNamePart);

//            return list;
//        }

//        private string ProcessDataBeforeSearch(WordsCheckingModel model, out int fileCreated)
//        {
//            fileCreated = 0;
//            var fileName = Guid.NewGuid().ToString();
//            //Create folder first
//            var folderPath = FileHelpers.MakeSureFolderExists(Path.Combine(Directory.GetCurrentDirectory(), ConfigConstants.SearchInputFolder));

//            if (!string.IsNullOrEmpty(folderPath))
//            {
//                try
//                {
//                    var mySentencesArr = new List<string>();
//                    var listOfArrayByLimit = new List<List<string>>();
//                    model.Keyword = model.Keyword.StringNormally();

//                    if (model.SearchType == EnumSearchType.Address)
//                    {
//                        if (!string.IsNullOrEmpty(model.Address))
//                        {
//                            model.Address = model.Address.StringNormally();
//                            model.Address = KeepOneSpace(model.Address);

//                            model.Keyword += " " + model.Address;
//                        }
//                    }
//                    else
//                    {
//                        model.Keyword = "\"" + model.Keyword + "\"";
//                    }

//                    if (model.BreakWithSigns)
//                    {
//                        mySentencesArr = model.Keyword.Split(',').ToList();

//                        var friendlyStr = model.Keyword.Replace(@"\", string.Empty);

//                        string reduceMultiSpace = @"[ ]{1,}";
//                        string reduceMultiTabs = @"[	]{1,}";
//                        friendlyStr = friendlyStr.Replace("\t", " ");
//                        friendlyStr = Regex.Replace(friendlyStr, reduceMultiSpace, " ");
//                        friendlyStr = Regex.Replace(friendlyStr, reduceMultiTabs, " ");
//                        friendlyStr = KeepOneSpace(friendlyStr);

//                        if (model.WordsLimit <= 0)
//                        {
//                            model.WordsLimit = 10;
//                        }

//                        if (string.IsNullOrEmpty(model.Seperators) || model.Seperators == "|")
//                        {
//                            model.Seperators = ",|.|?|!";
//                        }

//                        var newArraySplitByWordLimit = GetLimitSentencesFromDocumentBySigns(friendlyStr, model.WordsLimit, model.Seperators).ToArray();

//                        var splitedArr = newArraySplitByWordLimit.SplitArrayCustom(model.CurrentThreads).ToList();
//                        if (splitedArr != null && splitedArr.Count > 0)
//                        {
//                            foreach (var item in splitedArr)
//                            {
//                                listOfArrayByLimit.Add(item.ToList());
//                            }
//                        }
//                    }
//                    else
//                    {
//                        //var friendlyStr = ReplaceSpecialCharacters(model.Keyword);
//                        var friendlyStr = model.Keyword.Replace(@"\", string.Empty);
//                        if (model.WordsLimit <= 0)
//                        {
//                            model.WordsLimit = 10;
//                        }

//                        var newArraySplitByWordLimit = GetLimitSentencesFromDocument(friendlyStr, model.WordsLimit).ToArray();

//                        var splitedArr = newArraySplitByWordLimit.SplitArrayCustom(model.CurrentThreads).ToList();
//                        if (splitedArr != null && splitedArr.Count > 0)
//                        {
//                            foreach (var item in splitedArr)
//                            {
//                                listOfArrayByLimit.Add(item.ToList());
//                            }
//                        }
//                    }

//                    if (listOfArrayByLimit != null && listOfArrayByLimit.Count > 0)
//                    {
//                        fileCreated = listOfArrayByLimit.Count;
//                        for (int i = 0; i < fileCreated; i++)
//                        {
//                            var fileNamePart = string.Format("{0}_{1}", fileName, i + 1);
//                            var fileContent = string.Empty;
//                            if (listOfArrayByLimit[i] != null && listOfArrayByLimit[i].Count > 0)
//                            {
//                                fileContent = String.Join(Environment.NewLine, listOfArrayByLimit[i]);
//                            }

//                            FileHelpers.WriteTextAsync(folderPath, fileNamePart, fileContent);
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(string.Format("Error ProcessDataBeforeSearch due to: {0}", ex.ToString()));
//                }
//            }
//            else
//            {
//                _logger.LogError("Could not create SentenceCheckingExe search results folder");
//            }

//            return fileName;
//        }

//        public List<string> GetLimitSentencesFromDocument(string sentence, int limit)
//        {
//            //var myStr = @"If you want to handle different types of line breaks in a text, you can use the ability to match more than one string. This will correctly split on either type of line break, and preserve empty lines and spacing in the text
//            //    Sending the correct parameters to the method is a bit awkward because you are using it for something that is a lot simpler than what it's capable of. At least it's there, prior to framework 2 you had to use a regular 


//            //    expression or build your own splitting routine to split on a string
//            //    ";

//            string[] lines = sentence.Split(
//                new[] { "\r\n", "\r", "\n" },
//                StringSplitOptions.RemoveEmptyEntries
//            );

//            var myList = lines.ToList();
//            var newList = new List<string>();

//            foreach (var item in myList)
//            {
//                if (!string.IsNullOrEmpty(item))
//                {
//                    var newItem = item.Trim();
//                    if (!string.IsNullOrEmpty(newItem))
//                    {
//                        newItem = item.Trim();
//                        newList.Add(newItem);
//                    }
//                }
//            }

//            //var listByLimit = new List<string>();
//            //if (newList != null && newList.Count > 0)
//            //{
//            //    foreach (var item in newList)
//            //    {
//            //        var newWordsArr = GetWordsByLimit(item, limit);
//            //        if (newWordsArr != null && newWordsArr.Count > 0)
//            //        {
//            //            listByLimit.AddRange(newWordsArr);
//            //        }
//            //    }
//            //}

//            //return listByLimit;

//            return newList;
//        }

//        public List<string> GetLimitSentencesFromDocumentBySigns(string sentence, int limit, string signs = ",|.|?|!")
//        {

//            string[] lines = sentence.Split(
//                new[] { "\r\n", "\r", "\n" },
//                StringSplitOptions.RemoveEmptyEntries
//            );

//            var myList = lines.ToList();
//            var newList = new List<string>();

//            foreach (var item in myList)
//            {
//                if (!string.IsNullOrEmpty(item))
//                {
//                    var newItem = item.Trim();
//                    if (!string.IsNullOrEmpty(newItem))
//                    {
//                        newItem = item.Trim();
//                        newList.Add(newItem);
//                    }
//                }
//            }

//            //var listByLimit = new List<string>();
//            //if (newList != null && newList.Count > 0)
//            //{
//            //    foreach (var item in newList)
//            //    {
//            //        var newWordsArr = GetWordsWithMultipleSigns(item, limit, signs);
//            //        if (newWordsArr != null && newWordsArr.Count > 0)
//            //        {
//            //            listByLimit.AddRange(newWordsArr);
//            //        }
//            //    }
//            //}

//            //return listByLimit;

//            return newList;
//        }

//        private string ReplaceSpecialCharacters(string content)
//        {
//            if (!string.IsNullOrEmpty(content))
//            {
//                content = Regex.Replace(content, "[^\\w\\._]", "");
//            }

//            return content;
//        }

//        private List<string> GetWordsByLimit(string sentence, int limit)
//        {
//            var result = new List<string>();
//            var rootArr = new List<string>();
//            if (!string.IsNullOrEmpty(sentence))
//            {
//                sentence = sentence.Replace(',', ' ');
//                sentence = sentence.Replace('.', ' ');
//                sentence = sentence.Replace('?', ' ');
//                sentence = sentence.Replace('!', ' ');

//                sentence = sentence.Trim();
//                sentence = KeepOneSpace(sentence);

//                rootArr = sentence.Split(' ').ToList();

//                for (int i = 0; i < rootArr.Count; i++)
//                {
//                    var newItem = rootArr.Skip(i * limit).Take(limit);
//                    var newWords = string.Join(" ", newItem);

//                    if (!string.IsNullOrEmpty(newWords))
//                    {
//                        newWords = newWords.Trim();
//                    }

//                    if (!string.IsNullOrEmpty(newWords))
//                    {
//                        //newWords = KeepOneSpace(newWords);
//                        if (IsEnoughWords(newWords, limit))
//                        {
//                            result.Add(newWords);
//                        }
//                    }
//                }
//            }

//            return result;
//        }

//        public List<string> GetWordsWithMultipleSigns(string sentence, int limit, string signs = ",|.")
//        {
//            var result = new List<string>();
//            var rootArr = new List<string>();
//            if (!string.IsNullOrEmpty(sentence))
//            {
//                if (string.IsNullOrEmpty(signs) || signs == "|")
//                {
//                    //Default is "dot"
//                    signs = ".";
//                }

//                //sentence = sentence.Replace(',', ' ');
//                //sentence = sentence.Replace('.', ' ');
//                //sentence = sentence.Replace('?', ' ');
//                //sentence = sentence.Replace('!', ' ');

//                sentence = sentence.Trim();
//                sentence = KeepOneSpace(sentence);

//                var signsArr = signs.Split('|');
//                rootArr = sentence.Split(signsArr, StringSplitOptions.RemoveEmptyEntries).ToList();
//                //rootArr = sentence.Split(' ').ToList();

//                for (int i = 0; i < rootArr.Count; i++)
//                {
//                    if (!string.IsNullOrEmpty(rootArr[i]))
//                    {
//                        rootArr[i] = rootArr[i].Trim();
//                        //newWords = KeepOneSpace(newWords);
//                        if (IsEnoughWords(rootArr[i], limit))
//                        {
//                            result.Add(rootArr[i]);
//                        }
//                    }
//                }
//            }

//            return result;
//        }

//        private string KeepOneSpace(string sentence)
//        {
//            if (!string.IsNullOrEmpty(sentence))
//            {
//                RegexOptions options = RegexOptions.None;
//                Regex regex = new Regex("[ ]{2,}", options);
//                sentence = regex.Replace(sentence, " ");
//            }

//            return sentence;
//        }

//        private bool IsEnoughWords(string sentence, int limit)
//        {
//            //if (!string.IsNullOrEmpty(sentence))
//            //{
//            //    var currentWords = sentence.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
//            //    if (currentWords != null && currentWords.Count >= limit)
//            //        return true;
//            //}

//            //return false;

//            return true;
//        }

//        private void ClearInputAndResultFiles()
//        {
//            try
//            {
//                var inputPath = FileHelpers.MakeSureFolderExists(Path.Combine(Directory.GetCurrentDirectory(), ConfigConstants.SearchInputFolder));
//                var resultPath = FileHelpers.MakeSureFolderExists(Path.Combine(Directory.GetCurrentDirectory(), ConfigConstants.SearchResultsFolder));

//                ClearOldFiles(inputPath);
//                ClearOldFiles(resultPath);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Could not delete the old files because: " + ex.ToString());
//            }
//        }

//        private void ClearOldFiles(string folderPath)
//        {
//            string[] files = Directory.GetFiles(folderPath);
//            var keepResultsInDays = Utils.ConvertToInt32(_configuration["KeepSearchResultInDays"], 3);

//            //Default is 1 week
//            if (keepResultsInDays == 0)
//                keepResultsInDays = 7;

//            foreach (string file in files)
//            {
//                FileInfo fi = new FileInfo(file);
//                if (fi.LastAccessTime < DateTime.Now.AddDays(-1 * keepResultsInDays))
//                    fi.Delete();
//            }
//        }

//        #endregion
//    }
//}
