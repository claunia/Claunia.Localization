using System;

namespace Claunia.Localization.Core
{
    public class Project
    {
        string                name;
        internal EventHandler ProjectModified;
        string                url;
        string                version;

        /// <summary>
        ///     Project name
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                name = value;
                ProjectModified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Project version
        /// </summary>
        public string Version
        {
            get => version;
            set
            {
                version = value;
                ProjectModified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Project URL
        /// </summary>
        public string Url
        {
            get => url;
            set
            {
                url = value;
                ProjectModified?.Invoke(this, EventArgs.Empty);
            }
        }

        public override string ToString() => version is null ? name : $"{name} {version}";
    }
}