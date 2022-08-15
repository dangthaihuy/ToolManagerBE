using Autofac;
using Manager.DataLayer.Entities.Business;
using Manager.DataLayer.Stores.Business;
using Manager.SharedLibs;
using Manager.WebApp.Models.Business;
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
        [Route("getlist")]
        public async Task<IActionResult> GetList(string ConversationId, string Page, string Keyword)
        {
            int PageSize = 5;
            List<IdentityMessageFilter> list = new List<IdentityMessageFilter>();

            try
            {
                int CurrentPage = Utils.ConvertToInt32(Page);
                if (CurrentPage == null)
                {
                    CurrentPage = 1;
                }
                if (Keyword == null)
                    Keyword = "";

                list = storeMessage.GetByPage(Utils.ConvertToInt32(ConversationId), Keyword, CurrentPage, PageSize);
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not login: " + ex.ToString());
            }
            return Ok(list);
        }
    }
}
