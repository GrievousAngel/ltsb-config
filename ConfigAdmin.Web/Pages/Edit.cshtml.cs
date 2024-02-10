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

    public PageData Data { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(string name)
    {
        var response = await mediator.Send(new GetEdit.Request { Name = name });
        if (response == null)
        {
            return NotFound();
        }

        Data = new PageData
               {
                   Defaults = response.Defaults,
                   FileLastModified = response.FileLastModified,
                   Name = response.Name,
                   Properties = response.Properties
               };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(PageData data)
    {
        var response = await mediator.Send(
            new PostEdit.Request
            {
                FileLastModified = data.FileLastModified,
                Name = data.Name,
                Properties = data.Properties
            });

        if (response.Success)
        {
            return RedirectToPage(nameof(Index));
        }

        ModelState.AddModelError("", response.ErrorMessage!);
        Data = data with
               {
                   Defaults = response.Defaults,
                   FileLastModified = response.FileLastModified
               };

        return Page();
    }

    public record PageData
    {
        public required Dictionary<string, string> Defaults { get; set; }

        public long FileLastModified { get; set; }

        public required string Name { get; set; }

        public required Dictionary<string, string> Properties { get; set; }
    }
}