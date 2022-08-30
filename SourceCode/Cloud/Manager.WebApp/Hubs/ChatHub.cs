using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.WebApp.Helpers.Business;
using Manager.WebApp.Hubs.Common;
using Manager.DataLayer.Entities.Business;
using Manager.SharedLibs;
using Manager.DataLayer.Stores.Business;
using Autofac;
using Serilog;
using Microsoft.AspNetCore.Http;
using System.IO;
using Manager.WebApp.Helpers;
using Manager.WebApp.Models.Business;
using Newtonsoft.Json;

namespace Manager.WebApp.Hubs
{

    public class ChatHub : BaseMessengerHub
    {
        private IStoreConversation storeConversation = Startup.IocContainer.Resolve<IStoreConversation>();
        private IStoreMessage storeMessage = Startup.IocContainer.Resolve<IStoreMessage>();
        
        private ILogger _logger = Log.ForContext(typeof(ConversationHelpers));

        

        [HubMethodName("SendToGroup")]
        public void SendGroup(SendMessageModel model)
        {
            try
            {
                IdentityConversation idenConversation = new IdentityConversation();
                idenConversation.Id = model.ConversationId;
                ConversationHelpers.ClearCache(model.ConversationId);

                var connectedUsers = MessengerHelpers.GetAllUsersFromCache();
                var listUserInGroup = GroupChatHelpers.GetGroupInfo(idenConversation);
                foreach (var user in listUserInGroup.Member)
                {
                    var userConnect = connectedUsers.FirstOrDefault(x => x.Id == user.Id);
                    if (userConnect != null)
                    {
                        foreach (var senderConn in userConnect.Connections)
                        {
                            Clients.Client(senderConn.ConnectionId).SendAsync("ReceiveMessage", model);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Error("Could not GetBaseInfo: " + ex.ToString());
            }



            


            //Lấy người gửi trong cache

        }



        //[HubMethodName("SendToUser")]
        //public void SendRedirect(string SenderId, string ReceiverId, string Message)
        //{
        //    var senderId = Utils.ConvertToInt32(SenderId);
        //    var receiverId = Utils.ConvertToInt32(ReceiverId);
        //    var Conversation = storeConversation.GetDetail(senderId, receiverId);

        //    var IdentityMessage = new IdentityMessage();
        //    IdentityMessage.ConversationId = Conversation.Id;
        //    IdentityMessage.Message = Message;
        //    IdentityMessage.SenderId = Utils.ConvertToInt32(SenderId);
        //    IdentityMessage.ReceiverId = Utils.ConvertToInt32(ReceiverId);
        //    IdentityMessage.CreateDate = DateTime.Now;


        //    try
        //    {
        //        var MessageSuccess = storeMessage.Insert(IdentityMessage);
        //        ConversationHelpers.ClearCache(Conversation.Id);
        //    }
        //    catch (Exception ex)
        //    {

        //        _logger.Error("Could not GetBaseInfo: " + ex.ToString());
        //    }

        //    var connectedUsers = MessengerHelpers.GetAllUsersFromCache();
        //    //Lấy người gửi trong cache
        //    var fromUser = connectedUsers.FirstOrDefault(x => x.Id == Utils.ConvertToInt32(SenderId));
        //    //Lấy người nhận trong cache
        //    var toUser = connectedUsers.FirstOrDefault(x => x.Id == Utils.ConvertToInt32(ReceiverId));
        //    if (connectedUsers != null && connectedUsers.Count > 0)
        //    {
        //        if (fromUser != null && fromUser.Connections.HasData())
        //        {
        //            foreach (var senderConn in fromUser.Connections)
        //            {
        //                Clients.Client(senderConn.ConnectionId).SendAsync("ReceiveMessage", Conversation.Id, Utils.ConvertToInt32(SenderId), Message, IdentityMessage.CreateDate);
        //            }
        //        }

        //        if (toUser != null && toUser.Connections.HasData())
        //        {
        //            foreach (var receiverConn in toUser.Connections)
        //            {
        //                // Broad cast message
        //                Clients.Client(receiverConn.ConnectionId).SendAsync("ReceiveMessage", Conversation.Id, Utils.ConvertToInt32(SenderId), Message, IdentityMessage.CreateDate);
        //            }
        //        }
        //    }

        //}

        [HubMethodName("SendToUser")]
        public void SendToUser(SendMessageModel model)
        {
            try
            {
                _logger.Error("Begin SendToUser");

                var connectedUsers = MessengerHelpers.GetAllUsersFromCache();

                model.CreateDate = DateTime.Now;

                //Lấy người gửi trong cache
                var fromUser = connectedUsers.FirstOrDefault(x => x.Id == model.SenderId);
                //Lấy người nhận trong cache
                var toUser = connectedUsers.FirstOrDefault(x => x.Id == model.ReceiverId);

                var conversationId = Utils.ConvertToInt32(model.ConversationId);
                if (connectedUsers != null && connectedUsers.Count > 0)
                {
                    _logger.Error("SendToUsers: " + JsonConvert.SerializeObject(connectedUsers));

                    if (fromUser != null && fromUser.Connections.HasData())
                    {
                        foreach (var senderConn in fromUser.Connections)
                        {
                            Clients.Client(senderConn.ConnectionId).SendAsync("ReceiveMessage", model);
                        }
                    }

                    if (toUser != null && toUser.Connections.HasData())
                    {
                        foreach (var receiverConn in toUser.Connections)
                        {
                            // Broad cast message
                            Clients.Client(receiverConn.ConnectionId).SendAsync("ReceiveMessage", model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.Error("Could not SendToUser: " + ex.ToString());
            }
        }
    }
}
