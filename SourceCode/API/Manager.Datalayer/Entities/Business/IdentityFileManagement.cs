using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Entities.Business
{
    public class IdentityFolder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int CreateById { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class IdentityFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FolderId { get; set; }
        public string Path { get; set; }
        public int CreateById { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class IdentityUserFolder
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FolderId { get; set; }
    }

    public class IdentityUserFile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FileId { get; set; }
    }
}
