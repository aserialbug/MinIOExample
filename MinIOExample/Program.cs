using MinIOExample.Application;
using MinIOExample.Extensions;
using MinIOExample.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = long.MaxValue;
});
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers(options => options.AddFilters());
builder.Services.AddCustomSwaggerGen();

var app = builder.Build();
app.UseCustomSwagger();
app.UseRouting();
app.UseEndpoints(config => config.MapControllers());

app.Run();