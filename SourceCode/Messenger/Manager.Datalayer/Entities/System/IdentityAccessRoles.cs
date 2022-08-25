using System;
using System.Collections.Generic;

namespace Manager.DataLayer.Entities
{
    public class IdentityAccessRoles
    {
        public virtual string RoleId { get; set; }
        public virtual string AccessId { get; set; }
        public virtual int OperationId { get; set; }

        public virtual string RoleName { get; set; }
        public virtual string AccessName { get; set; }
        public virtual string AccessDescription { get; set; }

        public List<IdentityOperation> OperationsList { get; set; }
    }

    public class IdentityAccess
    {
        public virtual string Id { get; set; }
        public virtual string AccessName { get; set; }
        public virtual bool Active { get; set; }
        public virtual string Description { get; set; }

        public List<IdentityAccessRoles> RolesList { get; set; }

        public List<IdentityOperation> OperationsList { get; set; }

        public List<IdentityAccessLang> LangList { get; set; }

        public IdentityAccess()
        {
            LangList = new List<IdentityAccessLang>();
            OperationsList = new List<IdentityOperation>();
            RolesList = new List<IdentityAccessRoles>();
        }
    }

    public class IdentityAccessLang
    {
        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual string AccessId { get; set; }
        public virtual string LangCode { get; set; }
    }
    public class IdentityOperationLang
    {
        public virtual int Id { get; set; }
        public virtual string OperationName { get; set; }
        public virtual int OperationId { get; set; }
        public virtual string LangCode { get; set; }
    }

    public class IdentityAccessList
    {
        public List<IdentityAccess> AccessList { get; set; }
    }

    public class IdentityPermission
    {
        public string Action { get; set; }
        public string Controller { get; set; }
    }

    //public class IdentityOperation
    //{
    //    //public virtual int Id { get; set; }
    //    //public virtual string OperationName { get; set; }
    //    //public virtual bool Enabled { get; set; }
    //    //public virtual string AccessId { get; set; }
    //    //public virtual string ActionName { get; set; }

    //    ////Extends
    //    //public virtual string AccessName { get; set; }
    //}
}
