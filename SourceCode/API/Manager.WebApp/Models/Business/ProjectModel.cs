using System;
using System.Collections.Generic;

namespace Manager.WebApp.Models.Business
{
    public class ProjectModel
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


    }

    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public int FeatureId { get; set; }
        public int Process { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public int Assignee { get; set; }
        public int MessageId { get; set; }
        public int Status { get; set; }

    }

    public class UserProjectModel
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public int TaskId { get;set; }
        public int Role { get;set; }
    }


    public class FeatureModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public int ParentId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }

    }

    public class ProjectAttachmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public int TaskId { get; set; }
        public int FeatureId { get; set; }
        public string Path { get; set; }

    }
}
