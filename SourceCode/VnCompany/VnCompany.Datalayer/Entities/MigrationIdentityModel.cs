using System;
using System.Collections.Generic;

namespace VnCompany.DataLayer.Entities
{
    public class MigrationEntityExamModel
    {
        public string _id { get; set; }
        public int viewCount { get; set; }
        public List<string> tryId { get; set; }
        public List<int> sentences { get; set; }
        public int examNumber { get; set; }
        public string examTitle { get; set; }
        public int examLevel { get; set; }
        public decimal totalMark { get; set; }
        public decimal examDurationPart1 { get; set; }
        public decimal examDurationPart2 { get; set; }
        public decimal passMark { get; set; }
        public string examType { get; set; }
        public int examSkill { get; set; }
        public string examSoundUrl { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public string author { get; set; }
        public int __v { get; set; }
    }

    public class MigrationEntityAnswerModel
    {
        public string _id { get; set; }
        public int answerOrder { get; set; }
        public int __v { get; set; }
        public string answerDetail { get; set; }
        public bool isTrue { get; set; }
        public int question { get; set; }
    }

    public class MigrationEntityGrammarDetailModel
    {
        public List<string> examples { get; set; }
        public string synopsis { get; set; }
        public string explain { get; set; }
        public string mean { get; set; }
        public string note { get; set; }
    }

    public class MigrationEntityGrammarModel
    {
        public string _id { get; set; }
        public string category { get; set; }
        public List<MigrationEntityGrammarDetailModel> detail { get; set; }
        public int level { get; set; }
        public string @struct { get; set; }
        public string struct_vi { get; set; }
        public int __v { get; set; }
        public int grammar { get; set; }
    }

    public class MigrationEntityGrammarExampleModel
    {
        public string _id { get; set; }
        public string content { get; set; }
        public string content_html { get; set; }
        public string mean { get; set; }
        public string trans { get; set; }
        public int __v { get; set; }
    }

    public class MigrationEntityKanjiModel
    {
        public string _id { get; set; }
        public int level { get; set; }
        public string word { get; set; }
        public string cn_mean { get; set; }
        public string vi_mean { get; set; }
        public string radical { get; set; }
        public string detail { get; set; }
        public string onjomi { get; set; }
        public string kunjomi { get; set; }
        public int numstroke { get; set; }
        public string note { get; set; }
        public string remember { get; set; }
        public string remember_jp { get; set; }
        public string image { get; set; }
        public int __v { get; set; }
    }

    public class MigrationEntityQuestionModel
    {
        public string _id { get; set; }
        public int questionOrder { get; set; }
        public int __v { get; set; }
        public decimal mark { get; set; }
        public string questionDetail { get; set; }
        public string questionSoundUrl { get; set; }
        public int sentence { get; set; }
        public decimal soundDuration { get; set; }
        public int time { get; set; }
    }

    public class MigrationEntitySentenceModel
    {
        public string _id { get; set; }
        public int sentenceOrder { get; set; }
        public int __v { get; set; }
        public int level { get; set; }
        public int mondai { get; set; }
        public int part { get; set; }
        public string sentenceDetail { get; set; }
        public string sentenceSoundUrl { get; set; }
        public int skill { get; set; }
        public decimal soundDuration { get; set; }
        public decimal totalMark { get; set; }
        public int totalQuestion { get; set; }
        public decimal totalTime { get; set; }
        public int grammar { get; set; }
    }

    public class MigrationEntitySessionViewExamModel
    {
        public int examNumber { get; set; }
        public DateTime? updatedAt { get; set; }
        public DateTime? createdAt { get; set; }
    }

    public class MigrationEntitySessionModel
    {
        public string _id { get; set; }
        public int tryExamLeft { get; set; }
        public string ipAddress { get; set; }
        public string userAgent { get; set; }
        public List<MigrationEntitySessionViewExamModel> viewExams { get; set; }
        public string devicePlatform { get; set; }
        public string deviceUid { get; set; }
        public int __v { get; set; }
    }

    public class MigrationEntityTryExamResultModel
    {
        public bool isTrue { get; set; }
        public decimal mark { get; set; }
        public int question { get; set; }
        public int answer { get; set; }
    }

    public class MigrationEntityTryExamModel
    {
        public string _id { get; set; }
        public List<MigrationEntityTryExamResultModel> results { get; set; }
        public int exam { get; set; }
        public DateTime? updatedAt { get; set; }
        public DateTime? createdAt { get; set; }
    }

    public class MigrationEntityUserModel
    {
        public string _id { get; set; }
        public string password { get; set; }
        public bool isAdmin { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public int ssoId { get; set; }
        public string ssoToken { get; set; }
        public string contactNumber { get; set; }
        public string avatar { get; set; }
        public List<MigrationEntityTryExamModel> tryExams { get; set; }
        public DateTime? birthday { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public int __v { get; set; }
    }

    public class MigrationEntityWordModel
    {
        public string _id { get; set; }
        public string example_a { get; set; }
        public string example_b { get; set; }
        public string example_mean { get; set; }
        public string hira { get; set; }
        public string kanji { get; set; }
        public string mean { get; set; }
        public decimal duration { get; set; }
        public int level { get; set; }
        public string chap { get; set; }
        public string section { get; set; }
        public int __v { get; set; }
    }

    public class MigrationEntityWordChapModel
    {
        public string _id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int level { get; set; }
        public int __v { get; set; }
    }

    public class MigrationEntityWordSectionModel
    {
        public string _id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int level { get; set; }
        public int audio_id { get; set; }
        public string chap { get; set; }
        public int __v { get; set; }
    }

    public class MigrationEntityTrackModel
    {
        public string _id { get; set; }
        public string hash { get; set; }
        public string email { get; set; }
        public string reference_link { get; set; }
        public string ip { get; set; }
        public string device_type { get; set; }
        public DateTime? clicked_date { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public int __v { get; set; }
    }

    public class MigrationEntityPushTokenModel
    {
        public string _id { get; set; }
        public string pushToken { get; set; }
        public string userId { get; set; }
        public string user { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }
}