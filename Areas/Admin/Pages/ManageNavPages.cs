// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Flashy.Areas.Admin;

public static class ManageNavPages
{
    public static string Index => "Index";
    public static string Users => "Users";
    public static string Roles => "Roles";
    public static string FlashCards => "FlashCards";
    public static string Decks => "Decks";

    public static string IndexNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, Index);
    }

    public static string UsersNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, Users);
    }

    public static string RolesNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, Roles);
    }

    public static string FlashCardsNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, FlashCards);
    }

    public static string DecksNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, Decks);
    }

    public static string PageNavClass(ViewContext viewContext, string page)
    {
        string activePage = viewContext.ViewData["ActivePage"] as string
                            ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

        return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
    }
}