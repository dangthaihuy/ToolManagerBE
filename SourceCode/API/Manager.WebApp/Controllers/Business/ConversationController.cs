﻿using Autofac;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
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
using System.IO;

namespace Manager.WebApp.Controllers.Business
{
    [Route("api/chat/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversationController : ControllerBase
    {
        private readonly IStoreConversation storeConversation;
        private readonly IStoreGroup storeGroup;
        private readonly IStoreMessage storeMessage;
        private readonly IStoreMessageAttachment storeMessageAttachment;
        private readonly ILogger<ConversationController> _logger;
        public ConversationController(ILogger<ConversationController> logger)
        {

            storeConversation = Startup.IocContainer.Resolve<IStoreConversation>();
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
                return BadRequest(new { error = new { message = "Not found id" } });
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
                        var lastMessage = ConversationHelpers.GetLastMessage(item);
                        if (receiverInfo != null)
                        {
                            item.Receiver = receiverInfo;
                            item.LastMessage = lastMessage.Message; 
                            item.LastTime = lastMessage.CreateDate;
                            data.Add(item);
                        }
                    }
                }
                var listGroup = storeConversation.GetGroupByUserId(id);
                if (listGroup != null)
                {
                    foreach (var item in listGroup)
                    {
                        var groupInfo = GroupChatHelpers.GetGroupInfo(item);
                        var lastMessage = ConversationHelpers.GetLastMessage(item);

                        if (groupInfo != null)
                        {
                            item.Group = groupInfo;
                            item.LastMessage = lastMessage.Message;
                            item.LastTime = lastMessage.CreateDate;
                            item.Type = 2;
                            data.Add(item);
                        }
                    }

                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not getbyid conversation: " + ex.ToString());

                return StatusCode(500 ,new { apiMessage = new { type = "error", code = "common001" } });

            }
        }

        [HttpPost]
        [Route("insert")]
        public ActionResult Insert(ConversationModel model)
        {
            var newConversation = model.MappingObject<IdentityConversationDefault>();
            

            try
            {

                var res= new int();
                if (model.MemberGroup == null)
                {
                    res = storeConversation.Insert(newConversation);

                    var files = Directory.CreateDirectory(String.Concat("wwwroot\\Media\\Message\\Attachments\\", res));
                }
                if (model.MemberGroup.HasData())
                {
                    res = storeConversation.InsertGroup(newConversation);

                    var files = Directory.CreateDirectory(String.Concat("wwwroot\\Media\\Message\\Attachments\\", res));

                    var idenMessage = new IdentityMessage();
                    idenMessage.ConversationId = res;
                    idenMessage.Type = EnumMessageType.Noti;
                    idenMessage.CreateDate = DateTime.Now;
                    idenMessage.Message = "Nhóm mới đã được tạo";

                    var creator = storeGroup.Insert(res, model.CreatorId);
                    var insertMess = storeMessage.Insert(idenMessage);


                    foreach(string item in model.MemberGroup)
                    {
                        var insertMember  = storeGroup.Insert(res, Utils.ConvertToInt32(item));
                    }
                }
                return Ok(new { ConversationId = res });
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not insert conversation: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "common001" } });

            }

        }

        [HttpPost]
        [Route("delete")]
        public ActionResult Delete(ConversationModel model)
        {
            var identity = model.MappingObject<IdentityConversationDefault>();
            string filePath = "wwwroot\\Media\\Message\\Attachments\\";
            try
            {
                var deleteConversation = storeConversation.Delete(identity.Id);
                Directory.Delete(String.Concat(filePath, Convert.ToString(model.Id)));
                
                return Ok(model.Id);
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not delete conversation: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "common001" } });

            }
        }

        [HttpGet]
        [Route("getfile")]
        public ActionResult GetFile(int conversationId, int page, int pageSize)
        {
            var list = new List<IdentityMessageAttachment>();
            var filter = new IdentityMessageFilter();
            try
            {
                pageSize = pageSize > 0 ? pageSize : 20;
                var currentPage = page > 0 ? page : 1;

                filter.CurrentPage = currentPage;
                filter.PageSize = pageSize;
                filter.ConversationId = conversationId;

                list = storeMessageAttachment.GetByConId(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not Get file by conid: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "common001" } });

            }
            return Ok(list);
        }

        
    }
}
