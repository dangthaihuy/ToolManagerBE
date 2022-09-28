using Manager.DataLayer.Entities.Business;
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
        int UpdateUserInProject(IdentityUserProject identity);
        

        int InsertTask(IdentityTask identity);
        int DeleteTask(IdentityTask identity);
        IdentityTask UpdateTask(IdentityTask identity);
        List<int> GetTaskByUserId(int id);
        IdentityTask GetTaskById(int id);
        List<int> GetTaskByProjectId(int id);
        List<IdentityTask> GetTaskByFeatureId(int id);
        List<int> GetUserByProjectId(int id);
        int InsertUserToTask(IdentityUserProject id);
        int DeleteUserInTask(IdentityUserProject identity);
        List<string> DeleleAttachmentByTaskId(int id);

        int GetRoleUser(IdentityUserProject identity);

        int InsertFeature(IdentityFeature identity);
        int DeleteFeature(int id);
        IdentityFeature UpdateFeature(IdentityFeature identity);
        List<int> GetChild(int parentId);
        IdentityFeature GetFeatureById(int id);
        List<int> GetFeatureByProjectId(int id);
        List<IdentityFeature> GetSubFeature(int id);
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
        public int UpdateUserInProject(IdentityUserProject identity)
        {
            return r.UpdateUserInProject(identity);
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
        
        public List<IdentityTask> GetTaskByFeatureId(int id)
        {
            return r.GetTaskByFeatureId(id);
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
        public List<string> DeleleAttachmentByTaskId(int id)
        {
            return r.DeleleAttachmentByTaskId(id);
        }


        public int GetRoleUser(IdentityUserProject identity)
        {
            return r.GetRoleUser(identity);
        }




        public int InsertFeature(IdentityFeature identity)
        {
            return r.InsertFeature(identity);
        }
        public int DeleteFeature(int id)
        {
            return r.DeleteFeature(id);
        }
        public IdentityFeature UpdateFeature(IdentityFeature identity)
        {
            return r.UpdateFeature(identity);
        }
        public List<int> GetChild(int parentId)
        {
            return r.GetChild(parentId);
        }
        public IdentityFeature GetFeatureById(int id)
        {
            return r.GetFeatureById(id);
        }
        public List<int> GetFeatureByProjectId(int id)
        {
            return r.GetFeatureByProjectId(id);
        }
        public List<IdentityFeature> GetSubFeature(int id)
        {
            return r.GetSubFeature(id);
        }


    }

}
