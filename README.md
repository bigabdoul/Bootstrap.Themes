# Bootstrap.Themes

This project is a collection of customized Twitter's Bootstrap themes that may be
included into your projects. The themes support Bootstrap version 3 and some of
version 4.

## Integrating Bootstrap.Themes into an ASP.NET (Core) MVC Project

Integration is simple:

### On the server

```C#
using Bootstrap.Themes;
#if ASPNETCORE // Make sure this conditional compilation symbol is defined if it's an ASP.NET Core project
using Microsoft.AspNetCore.Mvc;
#else
using System.Web.Mvc;
#endif
using System;

namespace {APP NAMESPACE}.Controllers
{
    public class BootstrapController : Controller
    {
        /// <summary>
        /// Returns a Bootstrap CSS theme file.
        /// </summary>
        /// <param name="id">The identifier of the default built-in theme to return.</param>
        /// <param name="theme">The name of another built-in theme to return. This has a higher precedence over <paramref name="id"/>.</param>
        /// <returns></returns>
        public IActionResult Themes(BuiltInThemeName? id, [FromQuery(Name = "theme")] string theme)
        {
            if (!string.IsNullOrEmpty(theme) && Enum.TryParse<BuiltInThemeName>(theme, true, out var result))
            {
                id = result;
            }

            if (id.HasValue && Theme.FromResource(id.Value, out var t))
            {
                return File(System.Text.Encoding.UTF8.GetBytes(t.Content), "text/css");
            }

            return NotFound();
        }

    }
}
```

### In an HTML view

```HTML
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    ...
    <link href="~/bootstrap/themes/@ViewBag.Theme" rel="stylesheet" />
</head>
<body>
</body>
</html>
```

`@ViewBag.Theme` should be a valid enumeration value of type
`Bootstrap.Themes.BuiltInThemeName`.

For a working sample, checkout the https://github.com/bigabdoul/Polib.Net repo.
A sample project of its own will be soon included into the current repo.
