﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.WebApp.Helpers.Business
{
    public static class EnumFormatInfoCacheKeys
    {
        public static string Conversation = "CONVERSATION_{0}";
    }

    public static class EnumListCacheKeys
    {
        public static string Account = "ACCOUNT";
    }

    public enum EnumGender
    {
        Male = 0,
        Female = 1
    }
}
