namespace Manager.WebApp.Models.Business
{
    public class ConversationModel
    {
        public int Id { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public int CreateBy { get; set; }
        public int DeleteByUser1 { get; set; }
        public int DeleteByUser2 { get; set; }

    }
}
