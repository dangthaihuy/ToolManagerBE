﻿namespace Manager.WebApp.Models.Business
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Page { get; set; }
        public string Keyword { get; set; }
    }

    public class MessageCheckImportantModel
    {
        public int Id { get; set; }
        public int Important { get; set; }
    }
}
