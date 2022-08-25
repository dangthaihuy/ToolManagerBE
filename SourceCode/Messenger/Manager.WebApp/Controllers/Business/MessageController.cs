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

namespace Manager.WebApp.Controllers.Business
{
    [Route("api/chat/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IStoreMessage storeMessage;
        private readonly ILogger<MessageController> _logger;
        public MessageController(ILogger<MessageController> logger)
        {
            storeMessage = Startup.IocContainer.Resolve<IStoreMessage>();
            _logger = logger;

        }

        [HttpGet]
        [Route("getbypage")]
        public ActionResult GetByPage(int conversationId, int page, string keyword, int pageSize)
        {
            int PageSize = pageSize != 0 ? pageSize : 50 ;
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
            catch(Exception ex)
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
            if(model.Id == 0)
            {
                return BadRequest("Message is empty");
            }
            else
            {
                try
                {
                    var res = storeMessage.ChangeImportant(model.Id, model.Important);

                    
                }
                catch(Exception ex)
                {
                    _logger.LogDebug("Could not login: " + ex.ToString());
                }
            }

            return Ok();
        }


        [HttpPost]
        [Route("SendToGroup")]
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
        [Route("SendPrivateMessage")]
        public void SendPrivateMessage(SendPrivateMessageModel model)
        {
            try
            {
                var storeConversation = Startup.IocContainer.Resolve<IStoreConversation>();
                var con = storeConversation.GetDetail(model.SenderId, model.ReceiverId);

                var msg = new IdentityMessage();
                msg.ConversationId = con.Id;
                msg.Message = model.Message;
                msg.SenderId = Utils.ConvertToInt32(model.SenderId);
                msg.ReceiverId = Utils.ConvertToInt32(model.ReceiverId);
                msg.CreateDate = DateTime.UtcNow;

                var MessageSuccess = storeMessage.Insert(msg);
                ConversationHelpers.ClearCache(con.Id);

                //Send notification to user
                NotifNewPrivateMessage(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not SendToUser: " + ex.ToString());
            }
        }

        #region Helpers

        private void NotifNewPrivateMessage(IdentityMessage msg)
        {
            try
            {
                var apiPrivateMsg = new SendPrivateMessageModel();
                apiPrivateMsg = msg.MappingObject<SendPrivateMessageModel>();
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

        #endregion
    }
}
