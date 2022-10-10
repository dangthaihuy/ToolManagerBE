using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.WebApp.Helpers
{
    public static class EnumFormatInfoCacheKeys
    {
        public static string Conversation = "CONVERSATION_{0}_{1}";
        public static string ConversationGroup = "CONVERSATION_GROUP_{0}";
        public static string Message = "MESSAGE_{0}";
        public static string ConversationLastMessage = "CONVERSATIONLASTMESSAGE_{0}";
        public static string GroupOfUser = "GROUPOFUSER_{0}";
        public static string UsersInGroup = "USERSINGROUP_{0}";

        public static string Project = "PROJECT_{0}";
        public static string Task = "TASK_{0}";
        public static string Feature = "FEATURE_{0}";

        public static string User = "USER_{0}";


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

    public static class EnumCommonCode
    {
        public static int Success = 1;
        public static int Error = -1;
        public static int Error_Info_NotFound = -2;
    }

    public static class EnumMessageType
    {
        public static int Text = 1;
        public static int Attachment = 2;
        public static int Noti = 3;
        public static int Read = 4;
        public static int TaskNoti = 5;
    }

    public static class EnumConversationType
    {
        public static int Solo = 1;
        public static int Group = 2;
    }


}
