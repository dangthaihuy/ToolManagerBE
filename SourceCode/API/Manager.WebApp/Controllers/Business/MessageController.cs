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
        private readonly IStoreConversation storeConversation;
        private readonly ILogger<MessageController> _logger;
        public MessageController(ILogger<MessageController> logger)
        {
            storeConversation = Startup.IocContainer.Resolve<IStoreConversation>();
            storeMessage = Startup.IocContainer.Resolve<IStoreMessage>();
            _logger = logger;

        }

        [HttpGet]
        [Route("getbypage")]
        public ActionResult GetByPage(int conversationId, int page, string keyword, int pageSize)
        {
            int PageSize = pageSize != 0 ? pageSize : 50;
            int CurrentPage = page != 0 ? page : 1;

            List<IdentityMessage> list = new List<IdentityMessage>();
            var filter = new IdentityMessageFilter();
            try
            {
                if (keyword == null)
                {
                    keyword = "";
                }
                filter.CurrentPage = CurrentPage;
                filter.PageSize = PageSize;
                filter.ConversationId = conversationId;
                filter.Keyword = keyword;

                list = storeMessage.GetByPage(filter);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not login: " + ex.ToString());
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("getimportant")]
        public ActionResult GetImportant(int conversationId, int page, string keyword, int pageSize)
        {
            int PageSize = pageSize != 0 ? pageSize : 50;
            int CurrentPage = page != 0 ? page : 1;

            List<IdentityMessage> list = new List<IdentityMessage>();
            var filter = new IdentityMessageFilter();
            try
            {
                if (keyword == null)
                {
                    keyword = "";
                }
                filter.CurrentPage = CurrentPage;
                filter.PageSize = PageSize;
                filter.ConversationId = conversationId;
                filter.Keyword = keyword;

                list = storeMessage.GetImportant(filter);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not login: " + ex.ToString());
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
                    _logger.LogDebug("Could not login: " + ex.ToString());
                }
            }

            return Ok();
        }


        [HttpPost]
        [Route("sendtogroup")]
        public void SendGroup(int SenderId, int GroupId, string Message)
        {
            var IdentityMessage = new IdentityMessage();
            IdentityMessage.ConversationId = GroupId;
            IdentityMessage.Message = Message;
            IdentityMessage.SenderId = SenderId;
            IdentityMessage.CreateDate = DateTime.Now;

            var con = new IdentityConversation();
            con.Id = GroupId;

            try
            {
                var MessageSuccess = storeMessage.Insert(IdentityMessage);
                ConversationHelpers.ClearCache(GroupId);

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
                msg.SenderId = model.SenderId;
                msg.CreateDate = DateTime.Now;
                msg.Id = storeMessage.Insert(msg);

                var IdentityConversation = new IdentityConversation();
                IdentityConversation.Id = model.ConversationId;

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
                    else if(model.ConversationId == 0)
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
