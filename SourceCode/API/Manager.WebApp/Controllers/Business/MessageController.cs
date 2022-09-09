using Autofac;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.SharedLibs;
using Manager.WebApp.Helpers;
using Manager.WebApp.Models.Business;
using Manager.WebApp.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly IStoreConversation storeConversation;
        private readonly ILogger<MessageController> _logger;
        public MessageController(ILogger<MessageController> logger)
        {
            storeConversation = Startup.IocContainer.Resolve<IStoreConversation>();
            storeMessage = Startup.IocContainer.Resolve<IStoreMessage>();
            storeMessageAttachment = Startup.IocContainer.Resolve<IStoreMessageAttachment>();
            _logger = logger;

        }

        [HttpGet]
        [Route("getbypage")]
        public ActionResult GetByPage(int conversationId, int page, string keyword, int pageSize)
        {
            pageSize = pageSize > 0 ? pageSize : 50;
            int currentPage = page > 0 ? page : 1;

            List<IdentityMessage> list = new List<IdentityMessage>();
            var filter = new IdentityMessageFilter();
            try
            {
                filter.CurrentPage = currentPage;
                filter.PageSize = pageSize;
                filter.ConversationId = conversationId;
                filter.Keyword = keyword;

                list = storeMessage.GetByPage(filter);

                foreach(var item in list)
                {
                    if(item.Type == 2)
                    {
                        item.Attachments = storeMessageAttachment.GetByMessageId(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not getbypage: " + ex.ToString());
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("getbysearch")]
        public ActionResult GetBySearch(int conversationId, string keyword)
        {

            List<IdentityMessage> list = new List<IdentityMessage>();
            var filter = new IdentityMessageFilter();
            try
            {
                filter.ConversationId = conversationId;
                filter.Keyword = keyword;
                filter.PageSize = 50;

                list = storeMessage.GetBySearch(filter);

                foreach (var item in list)
                {
                    if (item.Type == 2)
                    {
                        item.Attachments = storeMessageAttachment.GetByMessageId(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not getbypage: " + ex.ToString());
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
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not deletemessage: " + ex.ToString());
            }
            return Ok(model.Id);
        }



        [HttpGet]
        [Route("getimportant")]
        public ActionResult GetImportant(int conversationId, int page, string keyword, int pageSize)
        {
            pageSize = pageSize != 0 ? pageSize : 50;
            var currentPage = page != 0 ? page : 1;

            List<IdentityMessage> list = new List<IdentityMessage>();
            var filter = new IdentityMessageFilter();
            try
            {
                if (keyword == null)
                {
                    keyword = "";
                }
                filter.CurrentPage = currentPage;
                filter.PageSize = pageSize;
                filter.ConversationId = conversationId;
                filter.Keyword = keyword;

                list = storeMessage.GetImportant(filter);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not getimportant: " + ex.ToString());
            }
            return Ok(list);
        }

        [HttpPost]
        [Route("changeimportant")]
        public ActionResult ChangeImportant(MessageCheckImportantModel model)
        {
            if (model.Id == 0)
            {
                return BadRequest("Message is empty");
            }
            else
            {
                try
                {
                    var res = storeMessage.ChangeImportant(model.Id, model.Important);


                }
                catch (Exception ex)
                {
                    _logger.LogDebug("Could not changeimportant: " + ex.ToString());
                }
            }

            return Ok();
        }

        

        [HttpPost]
        [Route("sendtogroup")]
        public void SendGroup(SendMessageModel model)
        {
            
            try
            {
                var identityMessage = new IdentityMessage();
                identityMessage.ConversationId = model.GroupId;
                identityMessage.Message = model.Message;
                identityMessage.SenderId = model.SenderId;
                identityMessage.CreateDate = DateTime.Now;

                var con = new IdentityConversation();
                con.Id = model.GroupId;
                var messageSuccess = storeMessage.Insert(identityMessage);
                ConversationHelpers.ClearCache(model.GroupId);

                //var connectedUsers = MessengerHelpers.GetAllUsersFromCache();
                //var listUserInGroup = GroupChatHelpers.GetGroupInfo(con);
                //foreach (var user in listUserInGroup.Member)
                //{
                //    var userConnect = connectedUsers.FirstOrDefault(x => x.Id == user.Id);
                //    if (userConnect != null)
                //    {
                //        foreach (var senderConn in userConnect.Connections)
                //        {
                //            //Clients.Client(senderConn.ConnectionId).SendAsync("ReceiveMessage", GroupId, SenderId, Message, IdentityMessage.CreateDate);
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not SendGroup: " + ex.ToString());
            }
        }


        [HttpPost]
        [Route("sendprivatemessage")]
        public void SendPrivateMessage(SendMessageModel model)
        {
            try
            {

                var con = storeConversation.GetDetail(model.SenderId, model.ReceiverId);

                var msg = new IdentityMessage();
                msg.ConversationId = con.Id;
                msg.Message = model.Message;
                msg.Type = 1;
                msg.SenderId = Utils.ConvertToInt32(model.SenderId);
                msg.ReceiverId = Utils.ConvertToInt32(model.ReceiverId);
                msg.CreateDate = DateTime.Now;
                msg.Id = storeMessage.Insert(msg);
                ConversationHelpers.ClearCache(con.Id);

                //Send notification to user
                NotifNewPrivateMessage(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not SendToUser: " + ex.ToString());
            }
        }

        [HttpPost]
        [Route("sendgroupmessage")]
        public void SendGroupMessage(SendMessageModel model)
        {
            try
            {

                var msg = new IdentityMessage();
                msg.ConversationId = model.ConversationId;
                msg.Message = model.Message;
                msg.Type = 1;
                msg.SenderId = model.SenderId;
                msg.CreateDate = DateTime.Now;
                msg.Id = storeMessage.Insert(msg);

                var identityCon = new IdentityConversation();
                identityCon.Id = model.ConversationId;

                ConversationHelpers.ClearCache(model.ConversationId);

                //Send notification to user
                NotifNewGroupMessage(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not SendGroupMessage: " + ex.ToString());
            }
        }

        [HttpPost]
        [Route("sendfilemessage")]
        public async void SendFileMessage([FromForm] SendMessageModel model)
        {
            try
            {                
                if (model.Files.HasData())
                {
                    IdentityMessage msg = new IdentityMessage();
                    msg.ConversationId = model.ConversationId;
                    msg.SenderId = model.SenderId;
                    msg.ReceiverId = model.ReceiverId;
                    msg.Message = "";
                    msg.Type = 2;
                    msg.CreateDate = DateTime.Now;
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
                        NotifNewGroupMessage(msg);
                    }
                    else
                    {
                        NotifNewPrivateMessage(msg);
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not SendFileMessage: " + ex.ToString());
            }
        }


        #region Helpers

        private void NotifNewPrivateMessage(IdentityMessage msg)
        {
            try
            {
                var apiPrivateMsg = new SendMessageModel();
                apiPrivateMsg = msg.MappingObject<SendMessageModel>();
                apiPrivateMsg.CreateDate = DateTime.UtcNow;

                var connBuilder = new HubConnectionBuilder();
                connBuilder.WithUrl(string.Format("{0}/chat", SystemSettings.MessengerCloud));
                connBuilder.WithAutomaticReconnect(); //I don't think this is totally required, but can't hurt either

                var conn = connBuilder.Build();

                //Start the connection
                var t = conn.StartAsync();

                //Wait for the connection to complete
                t.Wait();

                _logger.LogError("Begin Invoke SendToUser");

                //Make your call - but in this case don't wait for a response 
                conn.InvokeAsync("SendToUser", apiPrivateMsg);

            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to NotifNewPrivateMessage because: {0}", ex.ToString());
                _logger.LogError(strError);
            }
        }

        private void NotifNewGroupMessage(IdentityMessage msg)
        {
            try
            {
                var apiGroupMsg = new SendMessageModel();
                apiGroupMsg = msg.MappingObject<SendMessageModel>();
                apiGroupMsg.CreateDate = DateTime.Now;

                var connBuilder = new HubConnectionBuilder();
                connBuilder.WithUrl(string.Format("{0}/chat", SystemSettings.MessengerCloud));
                connBuilder.WithAutomaticReconnect(); //I don't think this is totally required, but can't hurt either

                var conn = connBuilder.Build();

                //Start the connection
                var t = conn.StartAsync();

                //Wait for the connection to complete
                t.Wait();

                //Make your call - but in this case don't wait for a response 
                conn.InvokeAsync("SendToGroup", apiGroupMsg);

            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to NotifNewGroupMessage because: {0}", ex.ToString());
                _logger.LogError(strError);
            }
        }

        

        #endregion
    }
}
