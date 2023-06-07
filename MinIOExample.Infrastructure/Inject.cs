using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinIOExample.Application.Interfaces;
using MinIOExample.Infrastructure.Context;
using MinIOExample.Infrastructure.Repositories;
using MinIOExample.Infrastructure.Settings;

namespace MinIOExample.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        
        serviceCollection.Configure<MinioSettings>(
            configuration.GetSection(MinioSettings.SectionName));
        

        serviceCollection.AddScoped<IFileMetadataRepository, FileMetadataRepository>();
        serviceCollection.AddScoped<IFileContentRepository, FileContentRepository>();
        serviceCollection.AddScoped<IBusinessTransactionContext, BusinessTransactionContext>();

        serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("ApplicationDbContext")));
        
        return serviceCollection;
    }
}