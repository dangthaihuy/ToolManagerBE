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
        List<int> DeleteProject(IdentityProject identity);
        IdentityProject UpdateProject(IdentityProject identity);
        List<int> GetProjectByUserId(int id);
        IdentityProject GetProjectById(int id);
        int InsertUserToProject(IdentityUserProject identity);
        int DeleteUserInProject(IdentityUserProject identity);
        int UpdateUserInProject(IdentityUserProject identity);
        List<IdentityProjectAttachment> GetAttachmentByProjectId(int id);


        int InsertTask(IdentityTask identity);
        IdentityTask UpdateTask(IdentityTask identity);
        List<string> DeleteTask(int id);
        List<int> GetTaskByUserId(int id);
        IdentityTask GetTaskById(int id);
        List<int> GetTaskByProjectId(int id);
        List<int> GetTaskIdByFeatureId(int id);
        List<int> GetUserByProjectId(int id);
        int InsertUserToTask(IdentityUserProject id);
        int DeleteUserInTask(IdentityUserProject identity);
        List<IdentityProjectAttachment> GetAttachmentByTaskId(int id);
        IdentityProjectAttachment DeleteAttachmentById(int id);

        int GetRoleUser(IdentityUserProject identity);

        IdentityFeature InsertFeature(IdentityFeature identity);
        List<string> DeleteFeature(int id);
        IdentityFeature UpdateFeature(IdentityFeature identity);
        List<int> GetChild(int parentId);
        IdentityFeature GetFeatureById(int id);
        List<int> GetFeatureByProjectId(int id);
        List<int> GetSubFeature(int id);

        int InsertFile(IdentityProjectAttachment identity);
        string DeleteFile(int id);
        List<IdentityProjectAttachment> GetAttachmentByFeatureId(IdentityProjectAttachment identity);
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
        public List<int> DeleteProject(IdentityProject identity)
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
        public List<IdentityProjectAttachment> GetAttachmentByProjectId(int id)
        {
            return r.GetAttachmentByProjectId(id);
        }



        public int InsertTask(IdentityTask identity)
        {
            return r.InsertTask(identity);
        }
        public List<string> DeleteTask(int id)
        {
            return r.DeleteTask(id);
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
        public List<int> GetTaskIdByFeatureId(int id)
        {
            return r.GetTaskIdByFeatureId(id);
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
        public List<IdentityProjectAttachment> GetAttachmentByTaskId(int id)
        {
            return r.GetAttachmentByTaskId(id);
        }
        public IdentityProjectAttachment DeleteAttachmentById(int id)
        {
            return r.DeleteAttachmentById(id);
        }


        public int GetRoleUser(IdentityUserProject identity)
        {
            return r.GetRoleUser(identity);
        }


        public IdentityFeature InsertFeature(IdentityFeature identity)
        {
            return r.InsertFeature(identity);
        }
        public List<string> DeleteFeature(int id)
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
        public List<int> GetSubFeature(int id)
        {
            return r.GetSubFeature(id);
        }

        public int InsertFile(IdentityProjectAttachment identity)
        {
            return r.InsertFile(identity);
        }
        public string DeleteFile(int id)
        {
            return r.DeleteFile(id);
        }

        public List<IdentityProjectAttachment> GetAttachmentByFeatureId(IdentityProjectAttachment identity)
        {
            return r.GetAttachmentByFeatureId(identity);
        }

    }

}
