using Autofac;
using Manager.DataLayer.Entities;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.DataLayer.Stores.System;
using Manager.SharedLibs;
using Manager.WebApp.Helpers.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly ILogger<ConversationController> _logger;
        public ConversationController(ILogger<ConversationController> logger)
        {

            storeConversation = Startup.IocContainer.Resolve<IStoreConversation>();
            storeUser = Startup.IocContainer.Resolve<IAPIStoreUser>();
            _logger = logger;

        }

        [HttpGet]
        [Route("getbyid")]
        public async Task<IActionResult> GetById(string id)
        {
            if (id == null)
            {
                return BadRequest(new { error = new { message = "Not found" } });
            }
            try
            {
                var list = storeConversation.GetById(id);
                var data = new List<IdentityConversation>();
                if (list != null)
                {
                    foreach(var item in list)
                    {
                        var Item = item;
                        var ConversationInfo = storeUser.GetById(Convert.ToString(Item.ReceiverId));
                        if (ConversationInfo != null)
                        {
                            Item.Receiver = ConversationInfo;
                            data.Add(Item);
                        }
                    }
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogDebug("Could not login: " + ex.ToString());
            }
            return BadRequest(new { error = new { message = "Not found" } });
        }
    }
}
