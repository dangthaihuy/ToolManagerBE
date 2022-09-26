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
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.WebApp.Controllers.Business
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IApiStoreUser storeUser;
        private readonly IStoreProject storeProject;

        public ProjectController(ILogger<ProjectController> logger)
        {
            storeUser = Startup.IocContainer.Resolve<IApiStoreUser>();
            storeProject = Startup.IocContainer.Resolve<IStoreProject>();
            _logger = logger;
        }

        [HttpPost]
        [Route("insert_project")]
        public ActionResult InsertProject(ProjectModel model)
        {
            try
            {

                var identity = model.MappingObject<IdentityProject>();
                var res = storeProject.InsertProject(identity);

                if (res > 0)
                {
                    return Ok(new { apiMessage = new { type = "success", code = "project001" } });
                }

                return Ok(new { apiMessage = new { type = "error", code = "project101" } });
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not insert project: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }

        [HttpPost]
        [Route("delete_project")]
        public ActionResult DeleteProject(ProjectModel model)
        {
            try
            {
                var identity = model.MappingObject<IdentityProject>();

                var res = storeProject.DeleteProject(identity);

                //Clear Cache
                ProjectHelpers.ClearCacheBaseInfoProject(model.Id);

                return Ok(new { apiMessage = new { type = "success", code = "project002" } });
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not delete project: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }

        [HttpPost]
        [Route("update_project")]
        public async Task<ActionResult> UpdateProject([FromForm] ProjectModel model)
        {
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

                return Ok(new { project = res, apiMessage = new { type = "success", code = "project003" } });
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not update project: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }

        [HttpGet]
        [Route("get_project_by_userid")]
        public ActionResult GetProjectByUserId(int id)
        {
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
                return Ok(new { listProject = res, apiMessage = new { type = "success", code = "projectxxx" } });
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not get project by user id: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

            
        }

        [HttpGet]
        [Route("get_project_by_id")]
        public ActionResult GetProjectById(int id)
        {
            try
            {
                var res = ProjectHelpers.GetBaseInfoProject(id);
                var listTaskId = storeProject.GetTaskByProjectId(id);

                if (listTaskId.HasData())
                {
                    res.Task = new List<IdentityTask>();
                    foreach (var taskId in listTaskId)
                    {
                        var idenTask = ProjectHelpers.GetBaseInfoTask(taskId);
                        if (idenTask != null)
                        {
                            res.Task.Add(idenTask);
                        }
                    }
                }

                var listUserId = storeProject.GetUserByProjectId(id);
                if (listUserId.HasData())
                {
                    res.User = new List<IdentityInformationUser>();
                    foreach(var userId in listUserId)
                    {
                        var idenUser = UserHelpers.GetBaseInfo(userId);
                        if (idenUser != null)
                        {
                            res.User.Add(idenUser);
                        }
                    }
                }

                return Ok(new { project = res, apiMessage = new { type = "success", code = "projectxxx" } });
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not get project by id: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }


        }


        [HttpPost]
        [Route("insert_task")]
        public ActionResult InsertTask(TaskModel model)
        {
            try
            {

                var identity = model.MappingObject<IdentityTask>();
                var res = storeProject.InsertTask(identity);

                if (res > 0)
                {
                    return Ok(new { apiMessage = new { type = "success", code = "project004" } });
                }

                return Ok(new { apiMessage = new { type = "error", code = "project104" } });
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not insert task: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }

        [HttpPost]
        [Route("delete_task")]
        public ActionResult DeleteTask(TaskModel model)
        {
            try
            {
                var identity = model.MappingObject<IdentityTask>();

                var res = storeProject.DeleteTask(identity);

                //Clear Cache
                ProjectHelpers.ClearCacheBaseInfoTask(model.Id);
                return Ok(new { apiMessage = new { type = "success", code = "project005" } });
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not delete task: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }

        [HttpPost]
        [Route("update_task")]
        public async Task<ActionResult> UpdateTask(TaskModel model)
        {
            try
            {
                var identity = model.MappingObject<IdentityTask>();

                if (Request.Form.Files.Count > 0)
                {
                    foreach(var file in Request.Form.Files)
                    {
                        var attachmentFolder = string.Format("Project/{0}/Attachment", identity.Id);
                        var filePath = FileUploadHelper.UploadFile(file, attachmentFolder);

                        await Task.FromResult(filePath);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            identity.File.Add(new IdentityProjectAttachment{ Path= filePath});
                            
                        }
                    }
                    

                }

                var res = storeProject.UpdateTask(identity);

                //Clear Cache
                ProjectHelpers.ClearCacheBaseInfoTask(model.Id);

                return Ok(new { apiMessage = new { type = "success", code = "project005" } });

            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not update task: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }

        [HttpGet]
        [Route("get_task_by_id")]
        public ActionResult GetTaskById(int id)
        {
            try
            {
                if(id == 0)
                {
                    return Ok(new { apiMessage = new { type = "error", code = "projectxxx" } });
                }
                var task = ProjectHelpers.GetBaseInfoTask(id);

                if (task == null)
                {
                    return Ok(new { apiMessage = new { type = "error", code = "projectxxx" } });
                }

                return Ok(new { task = task, apiMessage = new { type = "success", code = "projectxxx" } });
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not get task by id: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }


        }
        [HttpGet]
        [Route("get_task_by_userid")]
        public ActionResult GetTaskByUserId(int id)
        {
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
                return Ok(new { listTask = res, apiMessage = new { type = "success", code = "projectxxx" } });
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not get task by user id: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }


        }

    }
}
