using Autofac;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.DataLayer.Stores.System;
using Manager.SharedLibs;
using Manager.WebApp.Helpers;
using Manager.WebApp.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

    public class FileManagementController : ControllerBase
    {
        private readonly ILogger<FileManagementController> _logger;
        private readonly IAPIStoreUser storeUser;
        private readonly IStoreFileManagement storeFileManagement;

        public FileManagementController(ILogger<FileManagementController> logger)
        {
            storeUser = Startup.IocContainer.Resolve<IAPIStoreUser>();
            storeFileManagement = Startup.IocContainer.Resolve<IStoreFileManagement>();
            _logger = logger;
        }


        // XỬ LÝ FOLDER

        [HttpPost]
        [Route("insertfolder")]
        public ActionResult InsertFolder(FolderModel model)
        {
            try
            {

                var identity = model.MappingObject<IdentityFolder>();
                var res = storeFileManagement.InsertFolder(identity);

                if (res > 0)
                {
                    return Ok(new { apiMessage = new { type = "success", code = "folder001" } });
                }

                return Ok(new { apiMessage = new { type = "error", code = "folder101" } });
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not insert folder: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }

        [HttpPost]
        [Route("deletefolder")]
        public ActionResult DeleteFolder(FolderModel model)
        {
            try
            {
                var identity = model.MappingObject<IdentityFolder>();

                
                DeleteChild(identity);

                return Ok(new { apiMessage = new { type = "success", code = "folder002" } });
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not delete folder: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }

        [HttpPost]
        [Route("updatefolder")]
        public ActionResult UpdateFolder(FolderModel model)
        {
            try
            {
                var identity = model.MappingObject<IdentityFolder>();
                var res = storeFileManagement.UpdateFolder(identity);



                return Ok(new {folder = res, apiMessage = new { type = "success", code = "folder003" } });
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not update folder: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }




        // XỬ LÝ FILE
        [HttpPost]
        [Route("insertfile")]
        public async Task<ActionResult> InsertFile([FromForm] FileModel model)
        {
            try
            {
                var listFile = new List<IdentityFile>();
                             
                if (Request.Form.Files.Count > 0)
                {
                    foreach (var file in Request.Form.Files)
                    {
                        var attachmentFolder = string.Format("Files");
                        var filePath = FileUploadHelper.UploadFile(file, attachmentFolder);

                        await Task.FromResult(filePath);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            var identity = new IdentityFile();

                            identity.Name = file.FileName;
                            identity.Path = filePath;
                            identity.FolderId = model.FolderId;

                            identity.Id = storeFileManagement.InsertFile(identity);
                            listFile.Add(identity);
                        }

                    }

                    return Ok(new {listFile= listFile, apiMessage = new { type = "success", code = "file001" } });
                }
                  
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not insert file: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

            return Ok(new { apiMessage = new { type = "error", code = "file101" } });
        }

        [HttpPost]
        [Route("deletefile")]
        public ActionResult DeleteFile(FileModel model)
        {
            try
            {
                var identity = model.MappingObject<IdentityFile>();
                var file = storeFileManagement.GetFileById(identity);

                var res = storeFileManagement.DeleteFile(identity);

                if(file.Path != null)
                {
                    System.IO.File.Delete(String.Concat("wwwroot", file.Path));
                }

                return Ok(new { apiMessage = new { type = "success", code = "file002" } });
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not delete folder: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

            return Ok(new { apiMessage = new { type = "error", code = "file102" } });
        }


        private void DeleteChild(IdentityFolder parent)
        {
            try
            {
                var child = storeFileManagement.GetChild(parent);

                if (child.HasData())
                {
                    foreach(var item in child)
                    {
                        DeleteChild(item);
                    }
                }
                var listFile = storeFileManagement.GetFileByFolderId(parent);
                var res = storeFileManagement.DeleteFolder(parent);

                if (listFile.HasData())
                {
                    foreach (var file in listFile)
                    {
                        System.IO.File.Delete(String.Concat("wwwroot", file.Path));
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
