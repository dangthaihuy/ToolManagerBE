using Autofac;
using Manager.DataLayer.Entities;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.DataLayer.Stores.System;
using Manager.SharedLibs;
using Manager.WebApp.Helpers;
using Manager.WebApp.Helpers.Business;
using Manager.WebApp.Models.Business;
using Manager.WebApp.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Manager.WebApp.Controllers.Business
{
    [Route("api/chat/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupUserController : ControllerBase
    {
        private readonly IStoreGroup storeGroup;
        private readonly IAPIStoreUser storeUser;
        private readonly IStoreMessage storeMessage;
        private readonly ILogger<GroupUserController> _logger;
        public GroupUserController(ILogger<GroupUserController> logger)
        {

            storeGroup = Startup.IocContainer.Resolve<IStoreGroup>();
            storeUser = Startup.IocContainer.Resolve<IAPIStoreUser>();
            storeMessage = Startup.IocContainer.Resolve<IStoreMessage>();
            _logger = logger;

        }

        [HttpPost]
        [Route("insert")]
        public ActionResult Insert(GroupUserModel model)
        {
            try
            {
                var idenMessage = new IdentityMessage();
                idenMessage.Users = new List<IdentityInformationUser>();

                idenMessage.ConversationId = model.GroupId;
                idenMessage.Type = EnumMessageType.Noti;
                idenMessage.CreateDate = DateTime.Now;

                int check = 0;

                foreach (string item in model.UsersId)
                {
                    check++;
                    var res = storeGroup.Insert(model.GroupId, Utils.ConvertToInt32(item));
                    var user = storeUser.GetById(item);

                    idenMessage.Users.Add(user);
                    if (check != model.UsersId.Count)
                    {
                        idenMessage.Message = idenMessage.Message + user.Fullname + ", ";
                    }
                    else
                    {
                        idenMessage.Message = idenMessage.Message + user.Fullname + " ";
                    }

                }

                idenMessage.Message += "được thêm vào nhóm";
                idenMessage.Id = storeMessage.Insert(idenMessage);

                MessengerHelpers.NotifNewGroupMessage(idenMessage);
                GroupChatHelpers.ClearCache(model.GroupId);
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not insert group-user: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

            return Ok(new {success = true});
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult Delete(GroupUserModel model)
        {
            try
            {
                var idenMessage = new IdentityMessage();
                idenMessage.Users = new List<IdentityInformationUser>();

                idenMessage.ConversationId = model.GroupId;
                idenMessage.Type = EnumMessageType.Noti;
                idenMessage.CreateDate = DateTime.Now;

                int check = 0;

                foreach (string item in model.UsersId)
                {
                    check++;

                    var res = storeGroup.Delete(model.GroupId, Utils.ConvertToInt32(item));
                    var user = storeUser.GetById(item);

                    idenMessage.Users.Add(user);
                    if(check != model.UsersId.Count)
                    {
                        idenMessage.Message = idenMessage.Message + user.Fullname + ", " ;
                    }
                    else
                    {
                        idenMessage.Message = idenMessage.Message + user.Fullname + " ";
                    }

                }

                idenMessage.Message += "đã bị xóa khỏi nhóm";
                idenMessage.Id = storeMessage.Insert(idenMessage);

                MessengerHelpers.NotifNewGroupMessage(idenMessage);
                GroupChatHelpers.ClearCache(model.GroupId);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not delete user from group: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

            return Ok(new { success = true });
        }


        
    }
}
