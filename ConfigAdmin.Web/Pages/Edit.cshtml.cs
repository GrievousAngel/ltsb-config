using ConfigAdmin.Features.Edit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConfigAdminWeb.Pages;

public class EditModel : PageModel
{
    private readonly ILogger<EditModel> logger;
    private readonly IMediator mediator;

    public EditModel(ILogger<EditModel> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public GetEdit.Result Data { get; private set; }

    public async Task<IActionResult> OnGetAsync(GetEdit.Request request)
    {
        Data = await mediator.Send(request);
        if (Data.Properties == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(GetEdit.Result data)
    {
        await mediator.Send(new PostEdit.Request { Properties = data.Properties });

        return RedirectToPage(nameof(Index));
    }
}