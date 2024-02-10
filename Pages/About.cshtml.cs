using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Docs.Samples;

namespace Flashy.Pages;

public class AboutModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public AboutModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        ViewData["routeInfo"] = PageContext.ToCtxStringP();
    }
}