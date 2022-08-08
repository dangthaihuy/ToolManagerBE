﻿using Manager.SharedLibs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Entities
{
    [Serializable]
    public class CommonIdentity
    {
        [JsonIgnore]
        [ObjectDictionaryIgnore]
        public string Keyword { get; set; }

        [JsonIgnore]
        [ObjectDictionaryIgnore]
        public int TotalCount { get; set; }

        [JsonIgnore]
        [ObjectDictionaryIgnore]
        public DateTime? FromDate { get; set; }

        [JsonIgnore]
        [ObjectDictionaryIgnore]
        public DateTime? ToDate { get; set; }

        [JsonIgnore]
        [ObjectDictionaryIgnore]
        public int PageIndex { get; set; }

        [JsonIgnore]
        [ObjectDictionaryIgnore]
        public int PageSize { get; set; }

        [JsonIgnore]
        [ObjectDictionaryIgnore]
        public string SortField { get; set; }

        [JsonIgnore]
        [ObjectDictionaryIgnore]
        public string SortType { get; set; }

        [JsonIgnore]
        [ObjectDictionaryIgnore]
        public string ExceptIds { get; set; }
    }
}
