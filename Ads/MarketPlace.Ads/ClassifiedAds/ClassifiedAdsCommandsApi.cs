using MarketPlace.Ads.Domain.ClassifiedAds;
using MarketPlace.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MarketPlace.Ads.Messages.Commands;

namespace MarketPlace.Ads.ClassifiedAds
{
    [ApiController, Route("api/ad")]
    public class ClassifiedAdsCommandsApi : CommandApi<ClassifiedAd>
    {
        public ClassifiedAdsCommandsApi(
            ClassifiedAdsCommandService applicationService)
            : base(applicationService) { }

        [HttpPost]
        public Task<IActionResult> Post(V1.Create command)
            => HandleCommand(command, cmd => cmd.OwnerId = cmd.OwnerId);

        [Route("title"), HttpPut]
        public Task<IActionResult> Put(V1.ChangeTitle command)
            => HandleCommand(command);

        [Route("text"), HttpPut]
        public Task<IActionResult> Put(V1.UpdateText command)
            => HandleCommand(command);

        [Route("price"), HttpPut]
        public Task<IActionResult> Put(V1.UpdatePrice command)
            => HandleCommand(command);

        [Route("requestpublish"), HttpPut]
        public Task<IActionResult> Put(V1.RequestToPublish command)
            => HandleCommand(command);

        [Route("publish"), HttpPut]
        public Task<IActionResult> Put(V1.Publish command)
            => HandleCommand(command);

        [Route("delete"), HttpPost]
        public Task<IActionResult> Delete(V1.Delete command)
            => HandleCommand(command);

        [Route("image"), HttpPost]
        public Task<IActionResult> Post(V1.UploadImage command)
            => HandleCommand(command);
    }
}
