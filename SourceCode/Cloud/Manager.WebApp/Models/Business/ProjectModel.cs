using System;
using System.Collections.Generic;

namespace Manager.WebApp.Models.Business
{
    public class ProjectModel
    {

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
        public List<int> MemberIds { get; set; }
        public int Status { get; set; }
    }

    public class NotificationModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CreatedBy { get; set; }
        public string Content { get; set; }
        public int ProjectId { get; set; }
        public int FeatureId { get; set; }
        public int TaskId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
    }
}
