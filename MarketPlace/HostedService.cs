using MarketPlace.Infrastructure;

namespace MarketPlace
{
    public class HostedService : BackgroundService
    {
        private readonly ProjectionManager _projectionManager;

        public HostedService(ProjectionManager projectionManager)
        {
            _projectionManager = projectionManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           await _projectionManager.Start();
        }
    }
}
