using System.Collections.Generic;
using System.IO;

namespace Bootstrap.Themes
{
    /// <summary>
    /// Represents a list of <see cref="Theme"/> objects.
    /// </summary>
    public class ThemeList : List<Theme>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="ThemeList"/> class.
        /// </summary>
        public ThemeList()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="ThemeList"/> class with the specified collection.
        /// </summary>
        /// <param name="collection">A collection of <see cref="Theme"/> objects to add to the base list.</param>
        public ThemeList(IEnumerable<Theme> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Returns a collection of Bootstrap <see cref="Theme"/> objects.
        /// </summary>
        /// <param name="path">See <see cref="Theme.FindThemes(string, string, SearchOption)"/>.</param>
        /// <param name="searchPattern">See <see cref="Theme.FindThemes(string, string, SearchOption)"/>.</param>
        /// <param name="searchOption">See <see cref="Theme.FindThemes(string, string, SearchOption)"/>.</param>
        /// <returns></returns>
        public static ThemeList GetThemes(string path, string searchPattern = null, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return new ThemeList(Theme.FindThemes(path, searchPattern, searchOption));
        }
    }
}
