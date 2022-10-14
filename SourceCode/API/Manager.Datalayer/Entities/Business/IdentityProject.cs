using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Entities.Business
{
    public class IdentityProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> MemberIds { get; set; }
        public string Avatar { get; set; }
        public int Process { get; set; }
        public int Status { get; set; }
        public List<IdentityInformationUser> Members { get; set; }
        public List<IdentityFeature> Features { get; set; }
        public List<IdentityProjectAttachment> Files { get; set; }

    }

    public class IdentityTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public int FeatureId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public int Assignee { get; set; }
        public int Process { get; set; }
        public List<int> MemberIds { get; set; }
        public List<IdentityProjectAttachment> Files { get; set; }
        public int MessageId { get; set; }
        public int Status { get; set; }

    }

    public class IdentityProjectAttachment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public int TaskId { get; set; }
        public int FeatureId { get; set; }
        public string Path { get; set; }

    }

    public class IdentityUserProject
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public int TaskId { get; set; }
        public int Role { get; set; }

    }

    public class IdentityFeature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public int ParentId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public List<IdentityTask> Tasks { get; set; }
        public List<IdentityFeature> SubFeatures { get; set; }
        public List<IdentityProjectAttachment> Files { get; set; }

    }

    public class IdentityNotification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CreatedBy { get; set; }
        public string Content { get; set; }
        public int ProjectId { get; set; }
        public int FeatureId { get; set; }
        public int TaskId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Boolean IsRead { get; set; }

    }

    public class IdentitySearchFilter : CommonIdentity
    {
        public int UserId { get; set; }
    }
    
}
