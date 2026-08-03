namespace BACKEND.Helpers
{
    public class BlackListCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BlackListCleanupService> _logger;

        public BlackListCleanupService(
            IServiceScopeFactory scopeFactory, 
            ILogger<BlackListCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BlackListCleanupService iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Ejecuta el cleanup cada 24 horas
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);

                    using var scope = _scopeFactory.CreateScope();
                    
                    var blackList = scope.ServiceProvider.GetRequiredService<BlackListService>();
                    var refreshTokens = scope.ServiceProvider.GetRequiredService<RefreshTokenService>();

                    var cleanedBlackList = await blackList.CleanupExpiredEntriesAsync();
                    var cleanedRefreshTokens = await refreshTokens.CleanupExpiredTokensAsync();

                    if (cleanedBlackList > 0 || cleanedRefreshTokens > 0)
                    {
                        _logger.LogInformation(
                            "Cleanup ejecutado correctamente. BlackList: {B} eliminados. RefreshTokens: {R} eliminados.",
                            cleanedBlackList, cleanedRefreshTokens);
                    }
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ocurrió un error ejecutando el cleanup de tokens en background.");
                }
            }

            _logger.LogInformation("BlackListCleanupService detenido.");
        }
    }
}
