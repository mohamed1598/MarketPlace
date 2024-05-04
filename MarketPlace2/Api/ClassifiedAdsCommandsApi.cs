using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Api
{
    public class ClassifiedAdsCommandsApi:Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post (Contracts.ClassifiedAds.V1.Create request){
            //handle request here

            return Ok();
        }
    }
}
