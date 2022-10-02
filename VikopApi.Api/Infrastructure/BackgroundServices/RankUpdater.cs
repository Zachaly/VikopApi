using VikopApi.Application.User;

namespace VikopApi.Api.Infrastructure.BackgroundServices
{
    public class RankUpdater : BackgroundService
    {
        private const int delay = 1000 * 60 * 60 * 24; // 24 hours
        private readonly IServiceProvider _serviceProvider;

        public RankUpdater(IServiceProvider serviceProvider) : base()
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var updateRank = scope.ServiceProvider.GetRequiredService<UpdateRanks>();
                while (!stoppingToken.IsCancellationRequested)
                {
                    var res = await updateRank.Execute();
                    Console.WriteLine($"Ranks updated: {res}");
                    await Task.Delay(delay, stoppingToken);
                }
            }
        }
    }
}
