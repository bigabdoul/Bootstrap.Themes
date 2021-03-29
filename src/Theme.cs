using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using Bootstrap.Themes.Properties;

namespace Bootstrap.Themes
{
    /// <summary>
    /// Represents a Twitter Bootstrap theme.
    /// </summary>
    public class Theme
    {
        /// <summary>
        /// Initializes an instance of the <see cref="Theme"/> class.
        /// </summary>
        public Theme()
        {
        }

        /// <summary>
        /// Gets or sets the identifier for the current theme.
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Gets or sets the current theme name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the name to display for the current theme.
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description for the current theme.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the fully-qualified name, including the path, of the current theme.
        /// </summary>
        public virtual string FileName { get; set; }

        /// <summary>
        /// Gets or sets the content definition of the current theme.
        /// </summary>
        public virtual string Content { get; private set; }

        /// <summary>
        /// Reads all the text contained in the theme found under <see cref="FileName"/>.
        /// </summary>
        /// <returns></returns>
        public virtual string GetContent()
        {
            if (!string.IsNullOrWhiteSpace(Content))
                return Content;

            if (File.Exists(FileName))
                return Content = File.ReadAllText(FileName);

            return null;
        }

        /// <summary>
        /// Returns a collection of Bootstrap <see cref="Theme"/> elements that match the specified search
        /// pattern in the specified directory, using a value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path.
        /// This parameter can contain a combination of valid literal path and wildcard (* and ?).
        /// You can specify multiple search patterns by separating them with the pipe | symbol.
        /// </param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation 
        /// should include all subdirectories or only the current directory.
        /// </param>
        /// <returns>An enumerated collection of <see cref="Theme"/> instances.</returns>
        public static IEnumerable<Theme> FindThemes(string path, string searchPattern = null, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (string.IsNullOrWhiteSpace(searchPattern))
            {
                searchPattern = "*.bootstrap.min.css|bootstrap.css";
            }

            foreach (var pat in searchPattern.Split('|'))
            {
                var sp = pat.Trim();

                if (string.IsNullOrEmpty(sp))
                    continue;

                var files = Directory.GetFiles(path, sp, searchOption);

                if (files.Length == 0)
                    continue;

                var name = string.Empty;
                Func<string, string> GetName = f => name = Path.GetFileNameWithoutExtension(f);

                string DescriptionFromName()
                {
                    var descr = TitleCaseWords(name);
                    return (name.EndsWith(".min") ? "Minified " : "") + $"{descr} Bootstrap CSS Theme.";
                };

                foreach (var f in files)
                {
                    yield return new Theme
                    {
                        FileName = f,
                        Id = f.GetHashCode().ToString("x"),
                        Name = GetName(f), // important to call this func BEFORE the below method calls
                        Description = DescriptionFromName(),
                        DisplayName = TitleCaseWords(name),
                    };
                }
            }
        }

        /// <summary>
        /// Returns a theme with its definition from the resources file.
        /// </summary>
        /// <param name="name">The name of the built-in theme to find.</param>
        /// <param name="theme">Returns the requested theme, if any.</param>
        /// <param name="culture">The culture to use for resource lookup.</param>
        /// <returns></returns>
        public static bool FromResource(BuiltInThemeName name, out Theme theme, System.Globalization.CultureInfo culture = null)
        {
            return FromResource(name, out theme, null, culture);
        }

        /// <summary>
        /// Returns a theme with its definition from the resources file.
        /// </summary>
        /// <param name="name">The name of the built-in theme to find.</param>
        /// <param name="theme">Returns the requested theme, if any.</param>
        /// <param name="fallBackThemeName">The name of the theme to use if <paramref name="name"/>is not found in the resources file.</param>
        /// <param name="culture">The culture to use for resource lookup.</param>
        /// <returns></returns>
        public static bool FromResource(BuiltInThemeName name, out Theme theme, string fallBackThemeName, System.Globalization.CultureInfo culture = null)
        {
            theme = null;
            var resname = name.ToString();
            var content = Resources.ResourceManager.GetString("Bootstrap" + resname, culture);

            if (string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(fallBackThemeName))
            {
                content = Resources.ResourceManager.GetString(fallBackThemeName, culture);
            }

            if (!string.IsNullOrWhiteSpace(content))
                theme = new Theme
                {
                    Id = content.GetHashCode().ToString("x"),
                    Name = resname,
                    Content = content,
                    Description = $"Minified {TitleCaseWords(resname)} Bootstrap CSS theme loaded from the application resources file."
                };

            return theme != null;
        }

        /// <summary>
        /// Returns an array of the application's built-in theme names.
        /// </summary>
        /// <returns></returns>
        public static string[] GetBuiltInThemeNames()
        {
            return Enum.GetNames(typeof(BuiltInThemeName));
        }

        /// <summary>
        /// Returns a collection of the application's built-in theme names according to <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A function that evaluates a condition for including a theme name.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetBuiltInThemeNames(Func<string, bool> predicate)
        {
            var names = Enum.GetNames(typeof(BuiltInThemeName));
            foreach (var name in names)
            {
                if (predicate(name))
                {
                    yield return name;
                }
            }
        }

        static string TitleCaseWords(string s)
        {
            if (s == null)
                return null;

            return Regex.Replace(s, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }
    }
}
