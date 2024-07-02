using MarketPlace.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarketPlace.WebAPI
{
    public abstract class CommandApi<T> : ControllerBase
        where T : AggregateRoot
    {

        protected CommandApi(
            ApplicationService<T> applicationService)
        {
            Service = applicationService;
        }

        ApplicationService<T> Service { get; }

        protected async Task<IActionResult> HandleCommand<TCommand>(
            TCommand command,
            Action<TCommand> commandModifier = null)
        {
            try
            {
                commandModifier?.Invoke(command);
                await Service.Handle(command);
                return new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(
                    new
                    {
                        error = e.Message,
                        stackTrace = e.StackTrace
                    }
                );
            }
        }

        protected Guid GetUserId() => Guid.Parse(User.Identity.Name);
    }
}
