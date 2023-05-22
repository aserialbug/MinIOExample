using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinIOExample.Application.Services;
using MinIOExample.Application.Settings;

namespace MinIOExample.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<UploadRestrictionsSettings>(
            configuration.GetSection(UploadRestrictionsSettings.SectionName));
        serviceCollection.Configure<RemoveTempObjectSettings>(
            configuration.GetSection(RemoveTempObjectSettings.SectionName));

        serviceCollection.AddHostedService<RemoveTmpObjectsService>();
        serviceCollection.AddScoped<FileService>();
        return serviceCollection;
    }
}