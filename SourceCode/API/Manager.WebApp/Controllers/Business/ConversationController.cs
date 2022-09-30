﻿using Autofac;
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
using System.IO;
using System.Threading.Tasks;

namespace Manager.WebApp.Controllers.Business
{
    [Route("api/chat/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversationController : ControllerBase
    {
        private readonly IApiStoreUser storeUser;
        private readonly IStoreConversation storeConversation;
        private readonly IStoreConversationUser storeConversationUser;
        private readonly IStoreMessage storeMessage;
        private readonly IStoreMessageAttachment storeMessageAttachment;
        private readonly ILogger<ConversationController> _logger;
        public ConversationController(ILogger<ConversationController> logger)
        {

            storeUser= Startup.IocContainer.Resolve<IApiStoreUser>();
            storeConversation = Startup.IocContainer.Resolve<IStoreConversation>();
            storeConversationUser = Startup.IocContainer.Resolve<IStoreConversationUser>();
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
                return BadRequest(new { apiMessage = new { type = "error", message = "conversation101" } });
            }
            try
            {
                var listSolo = storeConversation.GetById(id);
                var data = new List<IdentityConversation>();
                if (listSolo != null)
                {
                    foreach(var item in listSolo)
                    {
                        var receiverInfo = storeUser.GetById(Convert.ToString(item.ReceiverId));
                        var lastMessage = ConversationHelpers.GetLastMessage(item);
                        if (receiverInfo != null)
                        {
                            item.Receiver = receiverInfo;
                            item.LastMessageId = lastMessage.Id; 
                            item.LastMessage = lastMessage.Message;
                            item.LastTime = lastMessage.CreatedDate;
                            data.Add(item);
                        }
                    }
                }
                var listGroup = storeConversation.GetGroupByUserId(id);
                if (listGroup != null)
                {
                    foreach (var item in listGroup)
                    {
                        IdentityConversationUser groupInfo = storeConversationUser.GetById(Convert.ToString(item.Id));
                        groupInfo.Member = storeConversationUser.GetUserById(item.Id);
                        var lastMessage = ConversationHelpers.GetLastMessage(item);

                        if (groupInfo != null)
                        {
                            item.Group = groupInfo;
                            item.Type = EnumMessageType.Attachment;
                            data.Add(item);
                            if(lastMessage != null)
                            {
                                item.LastMessageId = lastMessage.Id;
                                item.LastMessage = lastMessage.Message;
                                item.LastTime = lastMessage.CreatedDate;
                            }
                        }
                    }

                }

                foreach(var conversation in data)
                {
                    conversation.IsRead = storeConversationUser.GetIsRead(conversation);
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not getbyid conversation: " + ex.ToString());

                return StatusCode(500 ,new { apiMessage = new { type = "error", code = "server001" } });

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
                    if (newConversation.ReceiverId > 0)
                    {

                        var receiver = storeConversationUser.Insert(res, newConversation.ReceiverId, newConversation.Type);
                    }
                    var creator = storeConversationUser.Insert(res, newConversation.CreatedBy, newConversation.Type);
                    var files = Directory.CreateDirectory(String.Concat("wwwroot\\Media\\Message\\Attachments\\", res));
                }

                if (model.MemberGroup.HasData())
                {
                    res = storeConversation.InsertGroup(newConversation);

                    var files = Directory.CreateDirectory(String.Concat("wwwroot\\Media\\Message\\Attachments\\", res));

                    var idenMessage = new IdentityMessage();
                    idenMessage.ConversationId = res;
                    idenMessage.Type = EnumMessageType.Noti;
                    idenMessage.Message = "Nhóm mới đã được tạo";

                    var insertMess = storeMessage.Insert(idenMessage);

                    foreach(string item in model.MemberGroup)
                    {
                        var insertMember  = storeConversationUser.Insert(res, Utils.ConvertToInt32(item), newConversation.Type);
                    }
                    var creator = storeConversationUser.Insert(res, newConversation.CreatedBy, newConversation.Type);
                }
                


                return Ok(new { ConversationId = res });
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not insert conversation: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });

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

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });

            }
        }

        [HttpGet]
        [Route("getfile")]
        public ActionResult GetFile(int conversationId, int page, int pageSize)
        {
            if(conversationId <= 0)
            {
                return BadRequest(new { apiMessage = new { type = "error", message = "conversation002" } });
            }
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

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });

            }
            return Ok(list);
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update([FromForm] ConversationUpdateModel model)
        {
            try
            {
                var identity = model.MappingObject<IdentityConversationUpdate>();
                

                if (model != null)
                {
                    
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        var attachmentFolder = string.Format("Avatars/Conversations/{0}", identity.Id);
                        var filePath = FileUploadHelper.UploadFile(file, attachmentFolder);

                        await Task.FromResult(filePath);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            identity.AvatarPath = filePath;
                            identity.Avatar = Request.Form.Files[0].FileName;
                        }
                    }

                    var updateConversation = storeConversation.Update(identity);

                    return Ok(new {conversation = updateConversation, apiMessage = new { type = "success", code = "conversation003" } });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Could not update: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }

            return Ok(new { apiMessage = new { type = "error", code = "conversation103" } });
        }
    }
}
