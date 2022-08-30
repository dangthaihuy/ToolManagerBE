﻿using Autofac;
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
    [Route("api/chat/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversationController : ControllerBase
    {
        private readonly IStoreConversation storeConversation;
        private readonly IAPIStoreUser storeUser;
        private readonly IStoreGroup storeGroup;
        private readonly IStoreMessage storeMessage;
        private readonly IStoreMessageAttachment storeMessageAttachment;
        private readonly ILogger<ConversationController> _logger;
        public ConversationController(ILogger<ConversationController> logger)
        {

            storeConversation = Startup.IocContainer.Resolve<IStoreConversation>();
            storeUser = Startup.IocContainer.Resolve<IAPIStoreUser>();
            storeGroup = Startup.IocContainer.Resolve<IStoreGroup>();
            storeMessage = Startup.IocContainer.Resolve<IStoreMessage>();
            storeMessageAttachment = Startup.IocContainer.Resolve<IStoreMessageAttachment>();
            _logger = logger;

        }

        [HttpGet]
        [Route("getbyid")]
        public ActionResult GetById(string id)
        {
            if (id == null)
            {
                return BadRequest(new { error = new { message = "Not found" } });
            }
            try
            {
                var listSolo = storeConversation.GetById(id);
                var data = new List<IdentityConversation>();
                if (listSolo != null)
                {
                    foreach(var item in listSolo)
                    {
                        var receiverInfo = ConversationHelpers.GetReceiverInfo(item);
                        var LastMessage = ConversationHelpers.GetLastMessage(item);
                        if (receiverInfo != null)
                        {
                            item.Receiver = receiverInfo;
                            item.LastMessage = LastMessage.Message; 
                            item.LastTime = LastMessage.CreateDate;
                            data.Add(item);
                        }
                    }
                }
                var listGroup = storeConversation.GetGroupByUserId(id);
                if (listGroup != null)
                {
                    foreach (var item in listGroup)
                    {
                        var GroupInfo = GroupChatHelpers.GetGroupInfo(item);
                        var LastMessage = ConversationHelpers.GetLastMessage(item);

                        if (GroupInfo != null)
                        {
                            item.Group = GroupInfo;
                            item.LastMessage = LastMessage.Message;
                            item.LastTime = LastMessage.CreateDate;
                            item.Type = 2;
                            data.Add(item);
                        }
                    }

                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not get message: " + ex.ToString());
            }
            return BadRequest(new { error = new { message = "Not found" } });
        }

        [HttpPost]
        [Route("insert")]
        public ActionResult Insert(ConversationModel model)
        {
            var NewConversation = model.MappingObject<IdentityConversationDefault>();
            try
            {
                var res= new int();
                if (model.MemberGroup == null)
                {
                    res = storeConversation.Insert(NewConversation);
                }
                if (model.MemberGroup.HasData())
                {
                    res = storeConversation.InsertGroup(NewConversation);
                    var Creator = storeGroup.Insert(res, model.CreatorId);
                    foreach(string item in model.MemberGroup)
                    {
                        var InsertMember  = storeGroup.Insert(res, Utils.ConvertToInt32(item));
                    }
                }
                MessengerHelpers.ClearCache();
                return Ok(new { ConversationId = res });
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not insert conversation: " + ex.ToString());
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult Delete(ConversationModel model)
        {
            var identity = model.MappingObject<IdentityConversationDefault>();
            try
            {
                var deleteConversation = storeConversation.Delete(identity.Id);
                var deleteGroupUser = storeGroup.DeleteByGrpId(identity.Id);
                var deleteMessage = storeMessage.DeleteByConId(identity.Id);
                var deleteMessageAttachment = storeMessageAttachment.DeleteByConId(identity.Id);

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not delete conversation: " + ex.ToString());
            }
            return BadRequest();
        }
    }
}
