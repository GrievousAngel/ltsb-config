using ConfigAdmin.Features.Index;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConfigAdminWeb.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> logger;
    private readonly IMediator mediator;

    public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public PageData Data { get; private set; } = null!;

    public async Task OnGetAsync()
    {
        var response = await mediator.Send(new GetIndex.Request());
        Data = new PageData { Servers = response.Servers };
    }

    public record PageData
    {
        public required Dictionary<string, Dictionary<string, string>> Servers { get; set; }
    }
}