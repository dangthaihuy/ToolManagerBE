namespace Manager.WebApp.Models.Business
{
    public class FolderModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
    }

    public class FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FolderId { get; set; }
        public string Path { get; set; }
    }
}
