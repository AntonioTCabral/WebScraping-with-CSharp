using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebScraping.Core.Entities;
using WebScraping.Core.Models;


namespace WebScraping.Web.Controllers
{
    [ApiController]
    [Route("api/WebScrapingController")]
    public class WebScrapingController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;
        public WebScrapingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<List<ItemDescriptionResult>> Get(string userGit = "AntonioTCabral", string repoGit = "React_Training")
        {
            ConcurrentBag<ItemDescription> itemsDescription = new ConcurrentBag<ItemDescription>();
            var baseUrl = "https://github.com";
            var path = $"/{userGit}/{repoGit}";
            Scrap.getUrlContent(baseUrl, path, itemsDescription);
            var result = Scrap.getResultList(itemsDescription.ToList());

            return result;
        }
    }
}