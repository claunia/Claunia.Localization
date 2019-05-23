using System;

namespace Claunia.Localization.Core
{
    /// <summary>
    ///     Contains the plural forms of a string
    /// </summary>
    public class Plural
    {
        int                   items;
        internal EventHandler Modified;
        string                text;

        /// <summary>
        ///     Minimum number (inclusive) of items named to use this plural form
        /// </summary>
        public int Items
        {
            get => items;
            set
            {
                items = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Plural form of the string
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                text = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}