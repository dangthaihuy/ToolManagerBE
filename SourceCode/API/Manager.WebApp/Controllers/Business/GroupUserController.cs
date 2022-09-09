﻿using Autofac;
using Manager.DataLayer.Stores.Business;
using Manager.SharedLibs;
using Manager.WebApp.Helpers.Business;
using Manager.WebApp.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Manager.WebApp.Controllers.Business
{
    [Route("api/chat/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupUserController : ControllerBase
    {
        private readonly IStoreGroup storeGroup;
        private readonly ILogger<GroupUserController> _logger;
        public GroupUserController(ILogger<GroupUserController> logger)
        {

            storeGroup = Startup.IocContainer.Resolve<IStoreGroup>();
            _logger = logger;

        }

        [HttpPost]
        [Route("insert")]
        public ActionResult Insert(GroupUserModel model)
        {
            try
            {
                foreach(string item in model.UsersId)
                {
                    var res = storeGroup.Insert(model.GroupId, Utils.ConvertToInt32(item));
                    

                }
                GroupChatHelpers.ClearCache(model.GroupId);
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not insert group-user: " + ex.ToString());

                return StatusCode(500);
            }

            return Ok(new {success = true});
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult Delete(GroupUserModel model)
        {
            try
            {
                foreach (string item in model.UsersId)
                {
                    var res = storeGroup.Delete(model.GroupId, Utils.ConvertToInt32(item));


                }
                GroupChatHelpers.ClearCache(model.GroupId);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not delete user from group: " + ex.ToString());

                return StatusCode(500);
            }

            return Ok(new { success = true });
        }

    }
}
