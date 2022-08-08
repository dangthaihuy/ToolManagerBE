using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VnCompany.WebApp.Models
{
    public class WordsCheckingModel
    {
        public string SearchChannel { get; set; }

        [Required(ErrorMessage = "必須の項目です。")]
        public string Keyword { get; set; }
        public string Address { get; set; }
        public int PassedPercent { get; set; }
        public int CurrentThreads { get; set; }
        public int DelayTime { get; set; }
        public string QuoteType { get; set; }
        public bool BreakWithSigns { get; set; }
        public string Seperators { get; set; }
        public int WordsLimit { get; set; }

        public List<WordsCheckingResultItemModel> Results { get; set; }
        public string SessionName { get; set; }
        public string AddressSession { get; set; }
        public string KeywordPattern { get; set; }
        public string AddressPattern { get; set; }

        public string SearchType { get; set; }
        public List<string> Patterns { get; set; }
        public int ShowGoogleResults { get; set; }

        public List<string> IgnoreStrings { get; set; }

        public int MatchedResultCount { get; set; }

        public WordsCheckingModel()
        {
            IgnoreStrings = new List<string>();
            IgnoreStrings.Add("TEL");
            IgnoreStrings.Add("tel");
            IgnoreStrings.Add("FAX");
            IgnoreStrings.Add("fax");
            IgnoreStrings.Add("電話番号");
            IgnoreStrings.Add("代表番号");
            IgnoreStrings.Add("部署");
            IgnoreStrings.Add("代表取締役社長");
            IgnoreStrings.Add("専務取締役");
            IgnoreStrings.Add("常務取締役");
            IgnoreStrings.Add("本部長");
            IgnoreStrings.Add("部長");
            IgnoreStrings.Add("次長");
            IgnoreStrings.Add("課長");
            IgnoreStrings.Add("係長");
        }
    }

    public class WordsCheckingResultItemModel
    {
        public string Type { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
    }

    public class WordsCheckingDataModel
    {
        public string AllSentences { get; set; }
        public int TotalNotExists { get; set; }
        public int TotalExisted { get; set; }
        public int TotalSentences { get; set; }
        public float PercentResult { get; set; }

    }

    public class SearchInputImportModel
    {
        public IFormFile ImportFile { get; set; }
    }

    public class SearchResultModel
    {
        public int Idx { get; set; }
        public string Link { get; set; }
        public string Desc { get; set; }
    }
}
