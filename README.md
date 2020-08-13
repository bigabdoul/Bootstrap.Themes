# Bootstrap.Themes

A collection of customized Twitter's Bootstrap themes that may be included into your
projects. The themes support Bootstrap version 3 and some of version 4.

It contains CSS files embedded as static resources that are served to client applications
on demand.

## Integrating Bootstrap.Themes into an ASP.NET (Core) MVC Project

Integration is simple:

### On the server

```C#
using Bootstrap.Themes;
// Depending upon the target Framework
#if NETCORE
using Microsoft.AspNetCore.Mvc; // .NET Core
#else
using System.Web.Mvc;           // .NET Framework 3.5 or later
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
        public IActionResult Themes(BuiltInThemeName? id,
#if NETCORE
        [FromQuery(Name = "theme")]
#endif
        string theme)
        {
            if (!string.IsNullOrEmpty(theme) && Enum.TryParse<BuiltInThemeName>(theme, true, out var result))
            {
                id = result;
            }

            if (id.HasValue && Theme.FromResource(id.Value, out var t))
            {
                return File(System.Text.Encoding.UTF8.GetBytes(t.Content), "text/css");
            }
#if NETCORE
            return NotFound();
#else
            return new HttpNotFoundResult();
#endif
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
