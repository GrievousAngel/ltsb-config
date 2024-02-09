using ConfigAdmin;
using ConfigAdmin.Features.Index;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();
ConfigureApplication(app);
app.Run();

return;

void ConfigureServices(WebApplicationBuilder builder)
{
    var services = builder.Services;

    services.AddRazorPages();
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetIndex.Handler>());
    services.AddServices(builder.Configuration);
}

void ConfigureApplication(WebApplication webApplication)
{
    if (!webApplication.Environment.IsDevelopment())
    {
        webApplication
            .UseExceptionHandler("/Error")
            .UseHsts();
    }

    webApplication
        .UseStaticFiles()
        .UseRouting()
        .UseAuthorization();
    webApplication.MapRazorPages();
}