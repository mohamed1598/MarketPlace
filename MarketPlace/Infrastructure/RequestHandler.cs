using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MarketPlace.Infrastructure
{
    public class RequestHandler
    {
        public static async Task<IActionResult> HandleRequest<T>(T request,Func<T,Task> handler)
        {
            try
            {
                await handler(request);
                return new OkResult();
            }catch(Exception ex)
            {
                return new BadRequestObjectResult(new
                {
                    error = ex.Message,
                    ex.StackTrace
                });
            }
        }
        public static async Task<IActionResult> HandleQuery<T>(
            Func<Task<T>> query)
        {
            try
            {
                return new OkObjectResult(await query());
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new
                {
                    error = e.Message,
                    stackTrace = e.StackTrace
                });
            }
        }
    }
}
