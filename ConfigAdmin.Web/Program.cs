using ConfigAdmin;
using ConfigAdmin.Features.Index;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var app = builder.Build();
ConfigureApplication(app);
app.Run();

return;

void ConfigureServices(IServiceCollection services)
{
    services.AddRazorPages();
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetIndex.Handler>());
    services.AddServices();
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