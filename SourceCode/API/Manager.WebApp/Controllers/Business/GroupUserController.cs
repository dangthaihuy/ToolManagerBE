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
using System.Threading.Tasks;

namespace Manager.WebApp.Controllers.Business
{
    [Route("api/chat/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupUserController : ControllerBase
    {
        private readonly IStoreGroup storeGroup;
        private readonly IAPIStoreUser storeUser;
        private readonly IStoreMessage storeMessage;
        private readonly ILogger<GroupUserController> _logger;
        public GroupUserController(ILogger<GroupUserController> logger)
        {

            storeGroup = Startup.IocContainer.Resolve<IStoreGroup>();
            storeUser = Startup.IocContainer.Resolve<IAPIStoreUser>();
            storeMessage = Startup.IocContainer.Resolve<IStoreMessage>();
            _logger = logger;

        }

        [HttpPost]
        [Route("insert")]
        public ActionResult Insert(GroupUserModel model)
        {
            try
            {
                var idenMessage = new IdentityMessage();
                idenMessage.Users = new List<IdentityInformationUser>();

                idenMessage.ConversationId = model.GroupId;
                idenMessage.Type = EnumMessageType.Noti;
                idenMessage.CreateDate = DateTime.Now;

                int check = 0;

                foreach (string item in model.UsersId)
                {
                    check++;
                    /*var res = storeGroup.Insert(model.GroupId, Utils.ConvertToInt32(item));*/
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
                NotifNewGroupMessage(idenMessage);

                var res = storeMessage.Insert(idenMessage);
                GroupChatHelpers.ClearCache(model.GroupId);
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not insert group-user: " + ex.ToString());

                return StatusCode(500, new { message = "Server error: Insert" });
            }

            return Ok(new {success = true});
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult Delete(GroupUserModel model)
        {
            try
            {
                var idenMessage = new IdentityMessage();
                idenMessage.Users = new List<IdentityInformationUser>();

                idenMessage.ConversationId = model.GroupId;
                idenMessage.Type = EnumMessageType.Noti;
                idenMessage.CreateDate = DateTime.Now;

                int check = 0;

                foreach (string item in model.UsersId)
                {
                    check++;

                    /*var res = storeGroup.Delete(model.GroupId, Utils.ConvertToInt32(item));*/
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
                NotifNewGroupMessage(idenMessage);


                var res = storeMessage.Insert(idenMessage);
                GroupChatHelpers.ClearCache(model.GroupId);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not delete user from group: " + ex.ToString());

                return StatusCode(500, new { message = "Server error: Delete" });
            }

            return Ok(new { success = true });
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
    }
}
