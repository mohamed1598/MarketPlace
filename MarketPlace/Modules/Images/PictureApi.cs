﻿using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Modules.Images
{
    [ApiController, Route("image")]
    public class PictureApi : ControllerBase
    {
        readonly ImageQueryService _queryService;

        public PictureApi(ImageQueryService queryService)
            => _queryService = queryService;

        [HttpGet]
        public Task<byte[]> GetFile(string file) => _queryService.GetFile(file);
    }
}
