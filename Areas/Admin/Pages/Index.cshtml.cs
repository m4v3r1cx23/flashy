// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flashy.Areas.Admin;

public class IndexModel : PageModel
{
    [TempData] public string StatusMessage { get; set; }
}