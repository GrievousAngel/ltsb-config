var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var app = builder.Build();
ConfigureApplication(app);
app.Run();

return;

void ConfigureServices(IServiceCollection services)
{
    services.AddRazorPages();
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