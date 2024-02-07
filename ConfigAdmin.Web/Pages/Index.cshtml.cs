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

    public GetIndex.Result Data { get; private set; }

    public async Task OnGetAsync() => Data = await mediator.Send(new GetIndex.Request());
}