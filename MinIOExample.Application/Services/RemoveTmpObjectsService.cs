using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MinIOExample.Application.Interfaces;
using MinIOExample.Application.Settings;

namespace MinIOExample.Application.Services;

public class RemoveTmpObjectsService : IHostedService, IDisposable
{
    private readonly Timer _timer;
    private readonly RemoveTempObjectSettings _removeTempObjectSettings;
    private readonly ILogger<RemoveTmpObjectsService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public RemoveTmpObjectsService(
        IOptions<RemoveTempObjectSettings> tempObjectSettings, 
        ILogger<RemoveTmpObjectsService> logger, 
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _removeTempObjectSettings = tempObjectSettings.Value;
        _timer = new Timer(RemoveTmpObjectsWrapper, null, Timeout.Infinite, Timeout.Infinite);
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_removeTempObjectSettings.RemoveTaskIntervalMinutes.HasValue)
        {
            _logger.LogInformation("Cannot start RemoveTmpObjectsService. RemoveIntervalMinutes is null");
            return Task.CompletedTask;
        }
        
        _timer.Change(
            TimeSpan.FromSeconds(5), 
            TimeSpan.FromMinutes(_removeTempObjectSettings.RemoveTaskIntervalMinutes.Value));
        
        return Task.CompletedTask;
    }

    private async void RemoveTmpObjectsWrapper(object? state)
    {
        using var scope = _serviceProvider.CreateScope();

        var metadataRepository = scope
            .ServiceProvider
            .GetRequiredService<IFileMetadataRepository>();

        var contentRepository = scope
            .ServiceProvider
            .GetRequiredService<IFileContentRepository>();

        await RemoveTempObjects(metadataRepository, contentRepository);
    }

    private async Task RemoveTempObjects(
        IFileMetadataRepository metadataRepository, 
        IFileContentRepository contentRepository)
    {
        foreach (var fileMetadata in await metadataRepository.GetTemporaryObjects())
        {
            try
            {
                // Удаляем временные файлы которые храняться
                // больше чем _tempObjectSettings.TempFileLifetimeMinutes
                var storingTime = DateTime.UtcNow - fileMetadata.UploadedAt;
                var allowedStoringTime = TimeSpan.FromMinutes(_removeTempObjectSettings.TempFileLifetimeMinutes);
                if(storingTime <= allowedStoringTime)
                    continue;
                
                await contentRepository.RemoveByIdAsync(fileMetadata.Id);
                await metadataRepository.RemoveAsync(fileMetadata);
                
                _logger.LogInformation(
                    "Temporary stored file Id={FileId}, Name={Name} successfully removed",
                    fileMetadata.Id,
                    fileMetadata.FileName + fileMetadata.FileExt);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error removing temporary file {FileId}.", fileMetadata.Id);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Change(Timeout.Infinite, 0);
        
        return Task.CompletedTask;
    }
    

    public void Dispose()
    {
        _timer.Dispose();
    }
}