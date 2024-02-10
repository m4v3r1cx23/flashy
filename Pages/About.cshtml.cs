using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Docs.Samples;

namespace Flashy.Pages;

public class AboutModel : PageModel
{
    public IActionResult OnGet() =>
            PageContext.MyDisplayRouteInfoRP();
}
