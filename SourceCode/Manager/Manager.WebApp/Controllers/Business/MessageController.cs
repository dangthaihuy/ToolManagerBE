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
        [Route("getbypage")]
        public async Task<IActionResult> GetByPage(int ConversationId, int Page, string Keyword, int pageSize)
        {
            int PageSize = pageSize == null ? pageSize : 10 ;
            int CurrentPage = Page == null ? pageSize : 1;

            List<IdentityMessage> list = new List<IdentityMessage>();
            var filter = new IdentityMessageFilter();
            try
            {
                if (Keyword == null)
                {
                    Keyword = "";
                }
                filter.CurrentPage = CurrentPage;
                filter.PageSize = PageSize;
                filter.ConversationId = ConversationId;
                filter.Keyword = Keyword;

                list = storeMessage.GetByPage(filter);
            }
            catch(Exception ex)
            {
                _logger.LogDebug("Could not login: " + ex.ToString());
            }
            return Ok(list);
        }
    }
}
