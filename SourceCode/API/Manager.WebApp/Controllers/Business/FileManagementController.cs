using Autofac;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.DataLayer.Stores.System;
using Manager.SharedLibs;
using Manager.WebApp.Helpers;
using Manager.WebApp.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    public class FileManagementController : ControllerBase
    {
        private readonly ILogger<FileManagementController> _logger;
        private readonly IApiStoreUser storeUser;
        private readonly IStoreFileManagement storeFileManagement;
        private IHostingEnvironment Environment;

        public FileManagementController(ILogger<FileManagementController> logger, IHostingEnvironment environment)
        {
            storeUser = Startup.IocContainer.Resolve<IApiStoreUser>();
            storeFileManagement = Startup.IocContainer.Resolve<IStoreFileManagement>();
            _logger = logger;
            Environment = environment;
        }


        // XỬ LÝ FOLDER

        [HttpPost]
        [Route("insert_folder")]
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
        [Route("delete_folder")]
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
        [Route("update_folder")]
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
        [Route("insert_file")]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [RequestSizeLimit(409715200)]
        public async Task<ActionResult> InsertFile([FromForm] FileModel model)
        {
            try
            {
                var listFile = new List<IdentityFile>();
                foreach(var item in Request.Form.Files)
                {
                    var res = 1;
                }
                             
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

                            /*identity.Id = storeFileManagement.InsertFile(identity);*/
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
        [Route("UploadChunks")]
        public async Task<IActionResult> UploadChunksFile(string id, string fileName)
        {
            try
            {
                string wwwPath = this.Environment.WebRootPath;
                string dicPath = wwwPath + $@"\{fileName}";
                string newpath = Path.Combine(dicPath, fileName + id);

                if (!System.IO.Directory.Exists(dicPath))
                {
                    System.IO.Directory.CreateDirectory(dicPath);
                }

                using (FileStream fs = System.IO.File.Create(newpath))
                {
                    byte[] bytes = new byte[1048576 * 100];
                    int byteRead = 0;

                    while ((byteRead = await Request.Body.ReadAsync(bytes, 0, bytes.Length)) > 0)
                    {
                        fs.Write(bytes, 0, byteRead);
                    }
                }


            }
            catch (Exception ex)
            {
                _logger.LogError("Could not SendFileMessage: " + ex.ToString());
            }

            return Ok(new { isSuccess = true });
        }

        [HttpPost]
        [Route("UploadComplete")]
        public async Task<IActionResult> UploadComplete(string fileName)
        {
            try
            {
                string wwwPath = this.Environment.WebRootPath;
                string dicPath = wwwPath + $@"\{fileName}";
                string filePath = wwwPath + $@"\Files";
                string newpath = Path.Combine(filePath, fileName);

                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }

                foreach (string file in Directory.GetFiles(dicPath, "*.*"))
                {
                    if (!System.IO.File.Exists(newpath))
                    {
                        using (FileStream fs = System.IO.File.Create(newpath))
                        {
                            using (FileStream fs1 = System.IO.File.Open(file, FileMode.Open))
                            {
                                byte[] bytes = new byte[fs1.Length];
                                int byteRead = 0;

                                while ((byteRead = fs1.Read(bytes, 0, bytes.Length)) > 0)
                                {
                                    fs.Write(bytes, 0, byteRead);
                                }
                            }
                        }
                    }
                    else
                    {
                        using (FileStream fs = System.IO.File.Open(newpath, FileMode.Append))
                        {
                            using (FileStream fs1 = System.IO.File.Open(file, FileMode.Open))
                            {
                                byte[] bytes = new byte[fs1.Length];
                                int byteRead = 0;

                                while ((byteRead = fs1.Read(bytes, 0, bytes.Length)) > 0)
                                {
                                    fs.Write(bytes, 0, byteRead);
                                }
                            }
                        }

                    }
                }

                System.IO.Directory.Delete(dicPath);

            }
            catch (Exception ex)
            {
                _logger.LogError("Could not SendFileMessage: " + ex.ToString());
            }

            return Ok(0);
        }


        [HttpPost]
        [Route("delete_file")]
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
