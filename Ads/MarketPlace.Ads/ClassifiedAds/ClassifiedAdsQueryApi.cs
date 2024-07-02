using MarketPlace.Ads.Projections;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using MarketPlace.RavenDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Ads.ClassifiedAds
{
    [ApiController, Route("/ad")]
    public class ClassifiedAdsQueryApi : ControllerBase
    {
        readonly IAsyncDocumentSession _session;

        public ClassifiedAdsQueryApi(IAsyncDocumentSession session)
            => _session = session;

        [HttpGet]
        public Task<ActionResult<ReadModels.ClassifiedAdDetails>> Get(
            [FromQuery] QueryModels.GetPublicClassifiedAd request)
            => _session.RunApiQuery(s => s.Query(request));
    }
}
