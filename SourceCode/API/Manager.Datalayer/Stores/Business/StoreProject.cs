﻿using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Repositories.Business;
using Manager.SharedLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataLayer.Stores.Business
{
    public interface IStoreProject
    {
        int InsertProject(IdentityProject identity);
        int DeleteProject(IdentityProject identity);
        IdentityProject UpdateProject(IdentityProject identity);
        List<int> GetProjectByUserId(int id);
        IdentityProject GetProjectById(int id);
        int InsertUserToProject(IdentityUserProject identity);
        int DeleteUserInProject(IdentityUserProject identity);

        int InsertTask(IdentityTask identity);
        int DeleteTask(IdentityTask identity);
        IdentityTask UpdateTask(IdentityTask identity);
        List<int> GetTaskByUserId(int id);
        IdentityTask GetTaskById(int id);
        List<int> GetTaskByProjectId(int id);

        List<int> GetUserByProjectId(int id);
        int InsertUserToTask(IdentityUserProject id);
        int DeleteUserInTask(IdentityUserProject identity);

    }

    public class StoreProject : IStoreProject
    {
        private readonly string _conStr;
        private RpsProject r;
        public StoreProject() : this("MainDBConn")
        {
        }

        public StoreProject(string connectionStringName)
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
            r = new RpsProject(_conStr);
        }
        public int InsertProject(IdentityProject identity)
        {
            return r.InsertProject(identity);
        }
        public int DeleteProject(IdentityProject identity)
        {
            return r.DeleteProject(identity);
        }
        public IdentityProject UpdateProject(IdentityProject identity)
        {
            return r.UpdateProject(identity);
        }

        public List<int> GetProjectByUserId(int id)
        {
            return r.GetProjectByUserId(id);
        }

        public IdentityProject GetProjectById(int id)
        {
            return r.GetProjectById(id);
        }
        public int InsertUserToProject(IdentityUserProject identity)
        {
            return r.InsertUserToProject(identity);
        }
        public int DeleteUserInProject(IdentityUserProject identity)
        {
            return r.DeleteUserInProject(identity);
        }


        public int InsertTask(IdentityTask identity)
        {
            return r.InsertTask(identity);
        }
        public int DeleteTask(IdentityTask identity)
        {
            return r.DeleteTask(identity);
        }

        public IdentityTask UpdateTask(IdentityTask identity)
        {
            return r.UpdateTask(identity);
        }
        public List<int> GetTaskByUserId(int id)
        {
            return r.GetTaskByUserId(id);
        }
        public IdentityTask GetTaskById(int id)
        {
            return r.GetTaskById(id);
        }

        public List<int> GetTaskByProjectId(int id)
        {
            return r.GetTaskByProjectId(id);
        }


        public List<int> GetUserByProjectId(int id)
        {
            return r.GetUserByProjectId(id);
        }
        public int InsertUserToTask(IdentityUserProject identity)
        {
            return r.InsertUserToTask(identity);
        }
        public int DeleteUserInTask(IdentityUserProject identity)
        {
            return r.DeleteUserInTask(identity);
        }
    }
}
