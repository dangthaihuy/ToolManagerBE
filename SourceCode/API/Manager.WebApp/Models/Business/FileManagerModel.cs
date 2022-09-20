using System;

namespace Manager.WebApp.Models.Business
{
    public class FolderModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int CreateById { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FolderId { get; set; }
        public string Path { get; set; }
        public int CreateById { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class UserFolderModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FolderId { get; set; }
    }

    public class UserFileModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FileId { get; set; }
    }
}
