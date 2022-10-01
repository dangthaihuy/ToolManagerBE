using Autofac;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
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
    public class MessageController : ControllerBase
    {
        private readonly IStoreMessage storeMessage;
        private readonly IStoreMessageAttachment storeMessageAttachment;
        private readonly IStoreConversationUser storeConversationUser;
        private readonly IStoreConversation storeConversation;
        private readonly ILogger<MessageController> _logger;
        public MessageController(ILogger<MessageController> logger)
        {
            storeConversation = Startup.IocContainer.Resolve<IStoreConversation>();
            storeMessage = Startup.IocContainer.Resolve<IStoreMessage>();
            storeMessageAttachment = Startup.IocContainer.Resolve<IStoreMessageAttachment>();
            storeConversationUser = Startup.IocContainer.Resolve<IStoreConversationUser>();
            _logger = logger;

        }

        [HttpGet]
        [Route("getbypage")]
        public ActionResult GetByPage(int conversationId, int messageId, int pageSize, int direction, bool isMore = false)
        {
            if(conversationId <= 0)
            {
                return BadRequest(new { apiMessage = new { type = "error", code = "message101" } });
            }

            pageSize = pageSize > 0 ? pageSize : 20;
            var currentPage = 1;
            var messageList = new List<IdentityMessage>();
            var readBy = new List<int>();
            var filter = new IdentityMessageFilter();
            try
            {
                filter.Id = messageId;
                filter.PageSize = pageSize;
                filter.CurrentPage = currentPage;
                filter.Direction = direction;
                filter.ConversationId = conversationId;
                filter.IsMore = isMore;

                var list = storeMessage.GetByPage(filter);
                readBy = storeConversationUser.GetUsersReadConversation(conversationId);

                if (list.HasData())
                {
                    foreach (var item in list)
                    {
                        var message = MessengerHelpers.GetBaseInfo(item.Id);
                        
                        if (message != null)
                        {
                            message.PageIndex = item.PageIndex;
                            messageList.Add(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not getbypage: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
            return Ok(new { messageList = messageList, readBy = readBy });
        }

        [HttpGet]
        [Route("getbysearch")]
        public ActionResult GetBySearch(int conversationId, string keyword)
        {
            if (conversationId <= 0)
            {
                return BadRequest(new { apiMessage = new { type = "error", code = "message102" } });
            }

            List<IdentityMessage> list = new List<IdentityMessage>();
            var filter = new IdentityMessageFilter();
            try
            {
                filter.ConversationId = conversationId;
                filter.Keyword = keyword;
                filter.PageSize = 20;

                list = storeMessage.GetBySearch(filter);

                foreach (var item in list)
                {
                    if (item.Type == EnumMessageType.Attachment)
                    {
                        item.Attachments = storeMessageAttachment.GetByMessageId(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not getbypage: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
            return Ok(list);
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult DeleteMessage(MessageModel model)
        {
            try
            {
                var identity = model.MappingObject<IdentityMessage>();
                var attachments = storeMessageAttachment.GetByMessageId(identity);
                foreach(var item in attachments)
                {
                    System.IO.File.Delete(String.Concat("wwwroot", item.Path));
                }
                var res = storeMessage.DeleteMessage(identity);
                MessengerHelpers.ClearCacheBaseInfo(model.Id);

            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not deletemessage: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
            return Ok(model.Id);
        }



        [HttpGet]
        [Route("get_important")]
        public ActionResult GetImportant(int conversationId, int page, string keyword, int pageSize)
        {
            if (conversationId <= 0)
            {
                return BadRequest(new { apiMessage = new { type = "error", code = "message103" } });
            }

            pageSize = pageSize != 0 ? pageSize : 50;
            var currentPage = page != 0 ? page : 1;

            List<IdentityMessage> list = new List<IdentityMessage>();
            var filter = new IdentityMessageFilter();
            try
            {
                filter.CurrentPage = currentPage;
                filter.PageSize = pageSize;
                filter.ConversationId = conversationId;
                filter.Keyword = keyword;

                list = storeMessage.GetImportant(filter);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not getimportant: " + ex.ToString());

                return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
            }
            return Ok(list);
        }

        [HttpPost]
        [Route("change_important")]
        public ActionResult ChangeImportant(MessageCheckImportantModel model)
        {
            if (model.Id == 0)
            {
                return BadRequest(new { apiMessage = new { type = "error", code = "message104" } });
            }
            else
            {
                try
                {
                    var res = storeMessage.ChangeImportant(model.Id, model.Important);

                    var message = storeMessage.GetById(model.Id);

                    return Ok(message);
                }
                catch (Exception ex)
                {
                    _logger.LogDebug("Could not changeimportant: " + ex.ToString());

                    return StatusCode(500, new { apiMessage = new { type = "error", code = "server001" } });
                }
            }

           
        }


        


















            //API chat real time

        [HttpPost]
        [Route("send_private_message")]
        public void SendPrivateMessage(SendMessageModel model)
        {
            try
            {

                var con = storeConversation.GetDetail(model.SenderId, model.ReceiverId);

                var msg = model.MappingObject<IdentityMessage>();

                if(msg != null)
                {
                    msg.ConversationId = con.Id;
                    msg.Type = EnumMessageType.Text;
                    msg.ReplyMessage = storeMessage.GetReplyMessageById(model.ReplyMessageId);
                    msg.Id = storeMessage.Insert(msg);

                    //Send notification to user
                    MessengerHelpers.NotifNewPrivateMessage(msg);
                }
                
                ConversationHelpers.ClearCache(con.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not SendToUser: " + ex.ToString());


            }
        }

        [HttpPost]
        [Route("send_group_message")]
        public void SendGroupMessage(SendMessageModel model)
        {
            try
            {

                var msg = model.MappingObject<IdentityMessage>();
                if(msg != null)
                {
                    msg.Type = EnumMessageType.Text;
                    msg.ReplyMessage = storeMessage.GetReplyMessageById(model.ReplyMessageId);
                    msg.Id = storeMessage.Insert(msg);

                    ConversationHelpers.ClearCache(model.ConversationId);

                    //Send notification to user
                    MessengerHelpers.NotifNewGroupMessage(msg);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not SendGroupMessage: " + ex.ToString());


            }
        }

        [HttpPost]
        [Route("send_file_message")]
        public async void SendFileMessage([FromForm] SendMessageModel model)
        {
            try
            {                
                if (model.Files.HasData())
                {

                    IdentityMessage msg = model.MappingObject<IdentityMessage>();
                    
                    msg.Message = "";
                    msg.Type = EnumMessageType.Attachment;
                    msg.ReplyMessage = storeMessage.GetReplyMessageById(model.ReplyMessageId);
                    msg.Attachments = new List<IdentityMessageAttachment>();

                    foreach (var formFile in model.Files)
                    {
                        if (formFile.Length > 0)
                        {
                            var attachmentFolder = string.Format("Message/Attachments/{0}", msg.ConversationId);

                            var filePath = FileUploadHelper.UploadFile(formFile, attachmentFolder);

                            await Task.FromResult(filePath);

                            if (!string.IsNullOrEmpty(filePath))
                            {
                                var msgAttach = new IdentityMessageAttachment();
                                msgAttach.Name = formFile.FileName;
                                msgAttach.MessageId = msg.Id;
                                msgAttach.Path = filePath;

                                //Add attachment to list
                                msg.Attachments.Add(msgAttach);
                            }
                        }
                    }

                    //Insert message
                    msg.Id = storeMessage.Insert(msg);

                    //Clear cache last message
                    ConversationHelpers.ClearCache(model.ConversationId);

                    //Send notification
                    if(model.ReceiverId == 0)
                    {
                        MessengerHelpers.NotifNewGroupMessage(msg);
                    }
                    else
                    {
                        MessengerHelpers.NotifNewPrivateMessage(msg);
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not SendFileMessage: " + ex.ToString());
            }
        }


        
    }
}
