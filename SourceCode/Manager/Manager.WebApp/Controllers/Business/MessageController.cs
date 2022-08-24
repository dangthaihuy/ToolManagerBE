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



    }
}
