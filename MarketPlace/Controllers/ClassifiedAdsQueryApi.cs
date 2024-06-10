using MarketPlace.ClassifiedAd;
using MarketPlace.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;

namespace MarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassifiedAdsQueryApi : ControllerBase
    {
        private readonly IAsyncDocumentSession _session;

        public ClassifiedAdsQueryApi(IAsyncDocumentSession session)
        {
            _session = session;
        }

        [HttpGet]
        public Task<IActionResult> Get([FromQuery]QueryModels.GetPublicClassifiedAd request)
            => RequestHandler.HandleQuery(() => _session.Query(request));
    }
}
