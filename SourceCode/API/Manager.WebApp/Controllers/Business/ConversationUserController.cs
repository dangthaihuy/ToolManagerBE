﻿using Autofac;
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
    public class ConversationUserController : ControllerBase
    {
        private readonly IStoreConversationUser storeConversationUser;
        private readonly IApiStoreUser storeUser;
        private readonly IStoreMessage storeMessage;
        private readonly ILogger<ConversationUserController> _logger;
        public ConversationUserController(ILogger<ConversationUserController> logger)
        {

            storeConversationUser = Startup.IocContainer.Resolve<IStoreConversationUser>();
            storeUser = Startup.IocContainer.Resolve<IApiStoreUser>();
            storeMessage = Startup.IocContainer.Resolve<IStoreMessage>();
            _logger = logger;

        }

        [HttpPost]
        [Route("insert")]
        public ActionResult Insert(ConversationUserModel model)
        {
            try
            {
                var idenMessage = new IdentityMessage();
                idenMessage.Users = new List<IdentityInformationUser>();

                idenMessage.ConversationId = model.ConversationId;
                idenMessage.Type = EnumMessageType.Noti;

                int check = 0;

                foreach (string item in model.UsersId)
                {
                    check++;
                    var res = storeConversationUser.Insert(model.ConversationId, Utils.ConvertToInt32(item), EnumConversationType.Group);
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
                GroupChatHelpers.ClearCache(model.ConversationId);
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not insert group-user: " + ex.ToString());
                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

            return Ok(new { apiMessage = new { type = "success", code = "ConversationUser001" } });
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult Delete(ConversationUserModel model)
        {
            try
            {
                var idenMessage = new IdentityMessage();
                idenMessage.Users = new List<IdentityInformationUser>();

                idenMessage.ConversationId = model.ConversationId;
                idenMessage.Type = EnumMessageType.Noti;

                int check = 0;

                foreach (string item in model.UsersId)
                {
                    check++;

                    var res = storeConversationUser.Delete(model.ConversationId, Utils.ConvertToInt32(item));
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
                GroupChatHelpers.ClearCache(model.ConversationId);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not delete user from group: " + ex.ToString());
                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

            return Ok(new { apiMessage = new { type = "success", code = "ConversationUser002" } });
        }

        [HttpPost]
        [Route("update_read")]
        public ActionResult UpdateRead(ConversationUserModel model)
        {
            try
            {
                var identity = model.MappingObject<IdentityConversationUser>();
                var res = storeConversationUser.UpdateRead(identity);

                return Ok(new { apiMessage = new { type = "success", code = "conversationuser001" } });
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not update read: " + ex.ToString());
                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
        }
    }
}