
namespace VnCompany.WebApp.Helpers
{
    public class QuoteType
    {
        public static string Quote = "quote";
        public static string SquareBracket = "square_bracket";
    }

    public class Seperator
    {
        public static string Dot = "dot";
        public static string Comma = "comma";
        public static string Colon = "colon";
        public static string WordsLimit = "wordslimit";
    }

    public class ConfigConstants {
        public static string SystemSettingsJsonFile = "Data/Actor/Data/system_settings.json";

        public static string SearchInputFolder = "Data/Actor/Data/Input";
        public static string SearchResultsFolder = "Data/Actor/Data/SearchResults";
        public static string RegularJsonFile = "Data/Actor/Data/regular.json";
        public static string SearchInputJsonFile = "Data/Actor/Data/search_input.json";
        public static string GoogleDriveSettingsJsonFile = "Data/Actor/Data/google_drive_settings.json";
        public static string GoogleDriveCredentialsJsonFile = "Data/Actor/Data/google_drive_credentials.json";
        public static string GoogleDriveTokenJsonFile = "Data/Actor/Data/google_drive_token.json";
        public static string SentenceCheckingExe = "Data/Actor/SentenceChecking.exe";
        public static string SentenceCheckingBingExe = "Data/Actor/SentenceChecking_Bing.exe";
        public static string SentenceCheckingGoogleExe = "Data/Actor/SentenceChecking_Google.exe";

        public static string SentenceCheckingGooglePython = "Data/Actor/GoogleAutomation.py";
        public static string SentenceCheckingScriptPathFormat = "Data/Actor/search_engine_{0}.py";
    }
}
