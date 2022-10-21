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
    public class ConversationUserController : ControllerBase
    {
        private readonly IStoreConversationUser storeConversationUser;
        private readonly IStoreConversation storeConversation;
        private readonly IApiStoreUser storeUser;
        private readonly IStoreMessage storeMessage;
        private readonly ILogger<ConversationUserController> _logger;
        public ConversationUserController(ILogger<ConversationUserController> logger)
        {

            storeConversationUser = Startup.IocContainer.Resolve<IStoreConversationUser>();
            storeConversation = Startup.IocContainer.Resolve<IStoreConversation>();
            storeUser = Startup.IocContainer.Resolve<IApiStoreUser>();
            storeMessage = Startup.IocContainer.Resolve<IStoreMessage>();
            _logger = logger;

        }

        [HttpPost]
        [Route("insert")]
        public ActionResult Insert(ConversationUserModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "ConversationUser001" };
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
                    var user = storeUser.GetById(item);
                    idenMessage.Users.Add(user);
                    var res = storeConversationUser.Insert(model.ConversationId, Utils.ConvertToInt32(item), EnumConversationType.Group);
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
                returnModel.Type = "error";
                returnModel.Code = "server001";
                _logger.LogDebug("Could not insert group-user: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }

            return Ok(new { apiMessage = returnModel });
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult Delete(ConversationUserModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "ConversationUser002" };
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
                idenMessage.UserIdsDeleted = model.UsersId;

                MessengerHelpers.NotifNewGroupMessage(idenMessage);
                
                foreach (string item in model.UsersId)
                {
                    var res = storeConversationUser.Delete(model.ConversationId, Utils.ConvertToInt32(item));
                }

                GroupChatHelpers.ClearCache(model.ConversationId);


            }
            catch (Exception ex)
            {
                returnModel.Type = "error";
                returnModel.Code = "server001";
                _logger.LogDebug("Could not delete user from group: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }

            return Ok(new { apiMessage = returnModel });
        }

        [HttpPost]
        [Route("update_read")]
        public ActionResult UpdateRead(ConversationUserModel model)
        {
            var returnModel = new ReturnMessageModel { Type = "success", Code = "ConversationUser003" };
            try
            {
                var identity = model.MappingObject<IdentityConversationUser>();
                var readBy = storeConversationUser.UpdateRead(identity);
                var receiver = storeConversation.GetReceiverById(identity);

                var msg = new IdentityMessage();
                msg.ConversationId = identity.ConversationId;
                msg.SenderId = identity.UserId;
                msg.Message = "đã xem";
                msg.Type = EnumMessageType.Read;

                if (receiver == 0)
                {
                    MessengerHelpers.NotifNewGroupMessage(msg);
                }
                else
                {
                    msg.ReceiverId = receiver;
                    MessengerHelpers.NotifNewPrivateMessage(msg);
                }
                

                return Ok(new {conversationId = model.ConversationId ,readBy = readBy, apiMessage = returnModel });
            }
            catch(Exception ex)
            {
                returnModel.Type = "error";
                returnModel.Code = "server001";
                _logger.LogDebug("Could not update read: " + ex.ToString());
                return StatusCode(500, new { apiMessage = returnModel });
            }
        }
    }
}
