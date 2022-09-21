using Autofac;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.DataLayer.Stores.System;
using Manager.SharedLibs;
using Manager.WebApp.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

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
    }
}
