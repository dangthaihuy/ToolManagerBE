using Autofac;
using Manager.DataLayer.Entities;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.DataLayer.Stores.System;
using Manager.SharedLibs;
using Manager.WebApp.Helpers;
using Manager.WebApp.Helpers.Business;
using Manager.WebApp.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Manager.WebApp.Controllers.Business
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IStoreProject storeProject;
        private readonly string filePath;

        public ProjectController(ILogger<ProjectController> logger)
        {
            storeProject = Startup.IocContainer.Resolve<IStoreProject>();
            _logger = logger;
        }

        [HttpPost]
        [Route("insert_project")]
        public ActionResult InsertProject(ProjectModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "error", Code = "project101" };
            try
            {
                var identity = model.MappingObject<IdentityProject>();
                var res = storeProject.InsertProject(identity);
                if (res > 0)
                {
                    returnModel.Type = "project001";
                    returnModel.Code = "success";
                    return Ok(new {projectId = res, apiMessage = returnModel });
                }

                return Ok(new { apiMessage = new { type = "error", code = "project101" } });
            }
            catch (Exception ex)
            {
                returnModel.Code = "server001";
                _logger.LogDebug("Could not insert project: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        [HttpPost]
        [Route("delete_project")]
        public ActionResult DeleteProject(ProjectModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "project002" };
            try
            {
                var identity = model.MappingObject<IdentityProject>();
                var featureInProject = storeProject.DeleteProject(identity);

                foreach(var feature in featureInProject)
                {
                    DeleteChild(feature);
                }

                //Clear Cache
                ProjectHelpers.ClearCacheBaseInfoProject(model.Id);

                return Ok(new { id = model.Id, apiMessage = returnModel });
            }
            catch (Exception ex)
            {
                returnModel.Code = "server001";
                returnModel.Type = "error";
                _logger.LogDebug("Could not delete project: " + ex.ToString());
                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }

        [HttpPost]
        [Route("update_project")]
        public async Task<ActionResult> UpdateProject([FromForm] ProjectModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "project003" };
            try
            {
                var identity = model.MappingObject<IdentityProject>();
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    var attachmentFolder = string.Format("Project/{0}/Avatar", identity.Id);
                    var filePath = FileUploadHelper.UploadFile(file, attachmentFolder);

                    await Task.FromResult(filePath);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        identity.Avatar = filePath;
                    }
                }

                var res = storeProject.UpdateProject(identity);
                //Clear Cache
                ProjectHelpers.ClearCacheBaseInfoProject(model.Id);

                return Ok(new { project = res, apiMessage = returnModel });
            }
            catch (Exception ex)
            {
                returnModel.Code = "server001";
                returnModel.Type = "error";
                _logger.LogDebug("Could not update project: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        [HttpGet]
        [Route("get_project_by_userid")]
        public ActionResult GetProjectByUserId(int id)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "projectxxx" };
            try
            {
                var res = new List<IdentityProject>();
                var listProject = storeProject.GetProjectByUserId(id);
                foreach(int item in listProject)
                {
                    var project =  ProjectHelpers.GetBaseInfoProject(item);
                    if(project != null)
                    {
                        res.Add(project);
                    }
                }

                return Ok(new { listProject = res, apiMessage = returnModel });
            }
            catch(Exception ex)
            {
                returnModel.Code = "server001";
                returnModel.Type = "error";
                _logger.LogDebug("Could not get project by user id: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        [HttpGet]
        [Route("get_project_by_id")]
        public ActionResult GetProjectById(int id)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "projectxxx" };
            try
            {
                var res = ProjectHelpers.GetBaseInfoProject(id);
                var listFeatureId = storeProject.GetFeatureByProjectId(id);

                if (listFeatureId.HasData())
                {
                    res.Features = new List<IdentityFeature>();
                    foreach (var featureId in listFeatureId)
                    {
                        var idenFeature = ProjectHelpers.GetBaseInfoFeature(featureId);
                        if (idenFeature != null)
                        {
                            res.Features.Add(idenFeature);
                        }
                    }
                }

                var listUserId = storeProject.GetUserByProjectId(id);
                if (listUserId.HasData())
                {
                    var userProject = new IdentityUserProject();
                    res.Members = new List<IdentityInformationUser>();
                    foreach(var userId in listUserId)
                    {
                        var idenUser = UserHelpers.GetBaseInfo(userId);
                        if (idenUser != null)
                        {
                            userProject.UserId = userId;
                            userProject.ProjectId = id;

                            idenUser.Role = storeProject.GetRoleUser(userProject);
                            res.Members.Add(idenUser);
                        }
                    }
                }

                var idenAttach = new IdentityProjectAttachment();
                idenAttach.ProjectId = id;
                idenAttach.FeatureId = 0;
                res.Files = storeProject.GetAttachmentByFeatureId(idenAttach);

                return Ok(new { project = res, apiMessage = returnModel });
            }
            catch (Exception ex)
            {
                returnModel.Code = "server001";
                returnModel.Type = "error";
                _logger.LogDebug("Could not get project by id: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }

        }

        [HttpPost]
        [Route("add_user_to_project")]
        public ActionResult AddUserToProject(UserProjectModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "projectxxx" };
            try
            {
                var identity = model.MappingObject<IdentityUserProject>();
                var res = storeProject.InsertUserToProject(identity);

                return Ok(new { apiMessage = returnModel });
            }
            catch(Exception ex)
            {
                returnModel.Code = "server001";
                returnModel.Type = "error";
                _logger.LogDebug("Could not add user to project: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        [HttpPost]
        [Route("delete_user_in_project")]
        public ActionResult DeleteUserInProject(UserProjectModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "error", Code = "projectxxx" };
            try
            {
                if (model == null)
                {
                    return Ok(new { apiMessage = returnModel });
                }

                var identity = model.MappingObject<IdentityUserProject>();
                var res = storeProject.DeleteUserInProject(identity);

                returnModel.Type = "success";
                return Ok(new { userId = model.UserId, apiMessage = returnModel });
            }
            catch(Exception ex)
            {
                returnModel.Code = "server001";
                returnModel.Type = "error";
                _logger.LogDebug("Could not delete user in project: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        [HttpPost]
        [Route("update_user_in_project")]
        public ActionResult UpdateUserInProject(UserProjectModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "error", Code = "projectxxx" };
            try
            {
                if (model == null)
                {
                    return Ok(new { apiMessage = returnModel });
                }

                var identity = model.MappingObject<IdentityUserProject>();
                var res = storeProject.UpdateUserInProject(identity);
                
            }
            catch (Exception ex)
            {
                returnModel.Code = "server001";
                _logger.LogDebug("Could not add user to project: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
            returnModel.Type = "success";
            returnModel.Code = "projectxxx";
            return Ok(new { apiMessage = returnModel });
        }
        [HttpGet]
        [Route("get_attachment_in_project")]
        public ActionResult GetAttachmentInProject(int id)
        {
            var returnModel = new ReturnMessageModel { Type = "error", Code = "projectxxx" };
            try
            {
                if (id == 0)
                {
                    return Ok(new { apiMessage = returnModel });
                }

                var res = storeProject.GetAttachmentByProjectId(id);
                if (res == null)
                {
                    return Ok(new { apiMessage = returnModel });
                }

                returnModel.Type = "success";
                returnModel.Code = "projectxxx";
                return Ok(new { attachments = res, apiMessage = returnModel });
            }
            catch (Exception ex)
            {
                returnModel.Code = "server001";
                _logger.LogDebug("Could not get attachment in task: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        [HttpPost]
        [Route("insert_task")]
        public async Task<ActionResult> InsertTask([FromForm]TaskModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "taskxxx" };
            try
            {
                var identity = model.MappingObject<IdentityTask>();
                
                if (Request.Form.Files.Count > 0)
                {
                    foreach (var file in Request.Form.Files)
                    {
                        var attachmentFolder = string.Format("Project/{0}/Attachment", identity.ProjectId);
                        if (!System.IO.Directory.Exists(attachmentFolder))
                        {
                            System.IO.Directory.CreateDirectory(attachmentFolder);
                        }

                        var filePath = FileUploadHelper.UploadFile(file, attachmentFolder);

                        await Task.FromResult(filePath);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            if (identity.Files == null)
                            {
                                identity.Files = new List<IdentityProjectAttachment>();
                            }

                            identity.Files.Add(new IdentityProjectAttachment { Name = file.FileName, Path = filePath });
                        }
                    }
                }
                var newTaskId = storeProject.InsertTask(identity);
                var task = ProjectHelpers.GetBaseInfoTask(newTaskId);
                return Ok(new { task = task, apiMessage = returnModel });
            }
            catch (Exception ex)
            {
                returnModel.Code = "server001";
                returnModel.Type = "error";
                _logger.LogDebug("Could not insert task: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        [HttpPost]
        [Route("delete_task")]
        public ActionResult DeleteTask(TaskModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "taskxxx" };
            try
            {
                var identity = model.MappingObject<IdentityTask>();
                var attachment = storeProject.DeleteTask(identity.Id);

                foreach (var item in attachment)
                {
                    if (!System.IO.Directory.Exists(String.Concat("wwwroot", item)))
                    {
                        System.IO.File.Delete(String.Concat("wwwroot", item));
                    }
                }

                //Clear Cache
                ProjectHelpers.ClearCacheBaseInfoTask(model.Id);
                return Ok(new { taskId = model.Id, apiMessage = returnModel });
            }
            catch (Exception ex)
            {
                returnModel.Code = "server001";
                returnModel.Type = "error";
                _logger.LogDebug("Could not delete task: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        [HttpPost]
        [Route("update_task")]
        public async Task<ActionResult> UpdateTask([FromForm] TaskModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "taskxxx" };
            try
            {
                var identity = model.MappingObject<IdentityTask>();

                if (Request.Form.Files.Count > 0)
                {
                    foreach(var file in Request.Form.Files)
                    {
                        var attachmentFolder = string.Format("Project/{0}/Attachment", identity.ProjectId);
                        if (!System.IO.Directory.Exists(attachmentFolder))
                        {
                            System.IO.Directory.CreateDirectory(attachmentFolder);
                        }

                        var filePath = FileUploadHelper.UploadFile(file, attachmentFolder);

                        await Task.FromResult(filePath);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            if(identity.Files == null)
                            {
                                identity.Files = new List<IdentityProjectAttachment>();
                            }

                            identity.Files.Add(new IdentityProjectAttachment{Name= file.FileName, Path= filePath});
                        }
                    }
                    
                }

                var res = storeProject.UpdateTask(identity);
                res.Files = storeProject.GetAttachmentByTaskId(res.Id);
                //Clear Cache
                ProjectHelpers.ClearCacheBaseInfoTask(model.Id);

                return Ok(new {task=res, apiMessage = returnModel });
            }
            catch(Exception ex)
            {
                returnModel.Code = "server001";
                returnModel.Type = "error";
                _logger.LogDebug("Could not update task: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        [HttpGet]
        [Route("get_task_by_id")]
        public ActionResult GetTaskById(int id)
        {
            var returnModel = new ReturnMessageModel { Type = "error", Code = "taskxxx" };
            try
            {
                if (id == 0)
                {
                    return Ok(new { apiMessage = returnModel });
                }

                var task = ProjectHelpers.GetBaseInfoTask(id);
                
                if (task.Id > 0)
                {
                    task.Files = storeProject.GetAttachmentByTaskId(task.Id);
                    returnModel.Code = "taskxxx";
                    returnModel.Type = "success";

                    return Ok(new { task = task, apiMessage = returnModel });
                }

                return Ok(new { apiMessage = returnModel });
            }
            catch (Exception ex)
            {
                returnModel.Code = "serrver001";
                _logger.LogDebug("Could not get task by id: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }
        [HttpGet]
        [Route("get_task_by_userid")]
        public ActionResult GetTaskByUserId(int id)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "taskxxx" };
            try
            {
                var res = new List<IdentityTask>();
                var listProject = storeProject.GetTaskByUserId(id);

                foreach (int item in listProject)
                {
                    var task = ProjectHelpers.GetBaseInfoTask(item);
                    if (task != null)
                    {
                        res.Add(task);
                    }
                }

                return Ok(new { listTask = res, apiMessage = returnModel });
            }
            catch (Exception ex)
            {
                returnModel.Code = "server001";
                returnModel.Type = "error";
                _logger.LogDebug("Could not get task by user id: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }

        }

        

        [HttpPost]
        [Route("add_user_to_task")]
        public ActionResult AddUserToTask(UserProjectModel model)
        {
            try
            {
                if (model == null)
                {
                    return Ok(new { apiMessage = new { type = "error", code = "taskxxx" } });
                }

                var identity = model.MappingObject<IdentityUserProject>();
                var res = storeProject.InsertUserToTask(identity);

                ProjectHelpers.ClearCacheBaseInfoTask(model.TaskId);
                return Ok(new { apiMessage = new { type = "success", code = "taskxxx" } });
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not add user to task: " + ex.ToString());
                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }

        [HttpPost]
        [Route("delete_user_in_task")]
        public ActionResult DeleteUserInTask(UserProjectModel model)
        {
            try
            {
                if (model == null)
                {
                    return Ok(new { apiMessage = new { type = "error", code = "taskxxx" } });
                }

                var identity = model.MappingObject<IdentityUserProject>();
                var res = storeProject.DeleteUserInTask(identity);

                ProjectHelpers.ClearCacheBaseInfoTask(model.TaskId);
                return Ok(new { apiMessage = new { type = "success", code = "taskxxx" } });
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not delete user in task: " + ex.ToString());
                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }


        [HttpGet]
        [Route("get_attachment_in_task")]
        public ActionResult GetAttachmentInTask(int id)
        {
            var returnModel = new ReturnMessageModel { Type = "error", Code = "taskxxx" };
            try
            {
                if (id == 0)
                {
                    return Ok(new { apiMessage = returnModel });
                }

                var res = storeProject.GetAttachmentByTaskId(id);
                if(res == null)
                {
                    return Ok(new { apiMessage = returnModel });
                }

                returnModel.Type = "success";
                returnModel.Code = "taskxxx";
                return Ok(new { attachments = res, apiMessage = returnModel });
            }
            catch (Exception ex)
            {
                returnModel.Type = "error";
                returnModel.Code = "server001";
                _logger.LogDebug("Could not get attachment in task: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        [HttpPost]
        [Route("delete_attachment_by_id")]
        public ActionResult DeleteAttachmentById(ProjectAttachmentModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "error", Code = "taskxxx" };
            try
            {
                var identity = model.MappingObject<IdentityProjectAttachment>();
                var attachment = storeProject.DeleteAttachmentById(identity.Id);
                if(attachment != null)
                {
                    if (!System.IO.Directory.Exists(String.Concat("wwwroot", attachment.Path)))
                    {
                        System.IO.File.Delete(String.Concat("wwwroot", attachment.Path));
                    }

                    returnModel.Type = "success";
                    return Ok(new { attachmentId = attachment.Id, apiMessage = returnModel });
                }

                return Ok(new { apiMessage = returnModel });
            }
            catch(Exception ex)
            {
                returnModel.Type = "error";
                returnModel.Code = "server001";
                _logger.LogDebug("Could not delete attachment in task: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        

        //FEATURE
        [HttpPost]
        [Route("insert_feature")]
        public ActionResult InsertFeature(FeatureModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "error", Code = "featurexxx" };
            try
            {
                var identity = model.MappingObject<IdentityFeature>();
                var res = storeProject.InsertFeature(identity);

                if (res != null)
                {
                    returnModel.Type = "success";
                    return Ok(new {feature = res, apiMessage = returnModel });
                }

                return Ok(new { apiMessage = returnModel });
            }
            catch (Exception ex)
            {
                returnModel.Type = "error";
                returnModel.Code = "server001";
                _logger.LogDebug("Could not insert feature: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        [HttpPost]
        [Route("delete_feature")]
        public ActionResult DeleteFeature(FeatureModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "featurexxx" };
            try
            {
                var identity = model.MappingObject<IdentityFeature>();

                DeleteChild(identity.Id);

                return Ok(new { id = model.Id, apiMessage = returnModel });
            }
            catch (Exception ex)
            {
                returnModel.Type = "error";
                returnModel.Code = "server001";
                _logger.LogDebug("Could not delete feature: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        [HttpPost]
        [Route("update_feature")]
        public ActionResult UpdateFeature(FeatureModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "featurexxx" };
            try
            {
                var identity = model.MappingObject<IdentityFeature>();
                var res = storeProject.UpdateFeature(identity);

                ProjectHelpers.ClearCacheBaseInfoFeature(model.Id);

                return Ok(new { apiMessage = returnModel });
            }
            catch (Exception ex)
            {
                returnModel.Type = "error";
                returnModel.Code = "server001";
                _logger.LogDebug("Could not update feature: " + ex.ToString());
                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }

        [HttpGet]
        [Route("get_feature_by_id")]
        public ActionResult GetFeatureById(int id)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "featurexxx" };
            try
            {
                var idenAttach = new IdentityProjectAttachment();
                idenAttach.FeatureId = id;
                var res = ProjectHelpers.GetBaseInfoFeature(id);

                if(res.Id > 0)
                {
                    res.SubFeatures = new List<IdentityFeature>();
                    res.Tasks = new List<IdentityTask>();
                    var listSubFeature = storeProject.GetSubFeature(id);
                    foreach(var item in listSubFeature)
                    {
                        var feature = ProjectHelpers.GetBaseInfoFeature(item);
                        res.SubFeatures.Add(feature);
                    }

                    var listTask = storeProject.GetTaskIdByFeatureId(id);
                    foreach (var item in listTask)
                    {
                        var task = ProjectHelpers.GetBaseInfoTask(item);
                        res.Tasks.Add(task);
                    }

                    res.Files = storeProject.GetAttachmentByFeatureId(idenAttach);
                    /*res.SubFeatures = storeProject.GetSubFeature(id);
                    res.Tasks = storeProject.GetTaskIdByFeatureId(id);*/


                    return Ok(new { feature = res, apiMessage = returnModel });
                }

                returnModel.Type = "error";
                returnModel.Code = "featurexxx";
                return Ok(new { apiMessage = returnModel });
            }
            catch(Exception ex)
            {
                returnModel.Type = "error";
                returnModel.Code = "server001";
                _logger.LogDebug("Could not get feature by id: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }

        [HttpPost]
        [Route("insert_file")]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [RequestSizeLimit(409715200)]
        public async Task<ActionResult> InsertFile([FromForm] ProjectAttachmentModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "file001" };
            try
            {
                var listFile = new List<IdentityProjectAttachment>();
                
                if (Request.Form.Files.Count > 0)
                {
                    foreach (var file in Request.Form.Files)
                    {
                        var attachmentFolder = string.Format("Project/{0}/Attachment", model.ProjectId);
                        var filePath = FileUploadHelper.UploadFile(file, attachmentFolder);

                        await Task.FromResult(filePath);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            var identity = model.MappingObject<IdentityProjectAttachment>();
                            identity.Name = file.FileName;
                            identity.Path = filePath;

                            identity.Id = storeProject.InsertFile(identity);
                            listFile.Add(identity);
                        }

                    }

                    return Ok(new { listFile = listFile, apiMessage = returnModel });
                }
            }
            catch (Exception ex)
            {
                returnModel.Type = "error";
                returnModel.Code = "server001";
                _logger.LogDebug("Could not insert file: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }

            return Ok(new { apiMessage = new { type = "error", code = "file101" } });
        }

        [HttpPost]
        [Route("delete_file")]
        public ActionResult DeleteFile(ProjectAttachmentModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "file002" };
            try
            {
                var path = storeProject.DeleteFile(model.Id);

                if (path != null)
                {
                    System.IO.File.Delete(String.Concat("wwwroot", path));
                    return Ok(new { apiMessage = returnModel });
                }

                
            }
            catch (Exception ex)
            {
                returnModel.Type = "error";
                returnModel.Code = "server001";
                _logger.LogDebug("Could not delete file: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }

            returnModel.Type = "error";
            returnModel.Code = "file102";
            return Ok(new { apiMessage = returnModel });
        }

        [HttpGet]
        [Route("download_file")]
        [Authorize]
        public IActionResult DownloadFile(string path)
        {
            string physicalPath = "wwwroot/test.pdf";
            byte[] pdfBytes = System.IO.File.ReadAllBytes(physicalPath);
            MemoryStream ms = new MemoryStream(pdfBytes);
            return new FileStreamResult(ms, "application/pdf");
        }

        private void DeleteChild(int parentId)
        {
            try
            {
                var child = storeProject.GetChild(parentId);

                if (child.HasData())
                {
                    foreach (var item in child)
                    {
                        DeleteChild(item);
                    }
                }

                var attachments = storeProject.DeleteFeature(parentId);
                if (attachments.HasData())
                {
                    foreach (var item in attachments)
                    {
                        if (!System.IO.Directory.Exists(String.Concat("wwwroot", item)))
                        {
                            System.IO.File.Delete(String.Concat("wwwroot", item));
                        }
                    }
                }
                ProjectHelpers.ClearCacheBaseInfoFeature(parentId);

                var listTaskId = storeProject.GetTaskIdByFeatureId(parentId);
                //Xóa task trong feature , quan hệ task user
                if (listTaskId.HasData())
                {
                    foreach (var taskId in listTaskId)
                    {
                        var deleteTask = storeProject.DeleteTask(taskId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not delete child: " + ex.ToString());
            }
        }
    }
}
