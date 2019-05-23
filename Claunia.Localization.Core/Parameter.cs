using System;

namespace Claunia.Localization.Core
{
    /// <summary>
    ///     Explanation of a parameter in a software string to help the translator understand the context and for translation
    ///     helping software to be able to provide appropriate examples
    /// </summary>
    public class Parameter
    {
        string                description;
        int                   index;
        Language              language;
        long?                 maximum;
        long?                 minimum;
        internal EventHandler Modified;
        string                type;

        /// <summary>
        ///     Index of parameter in source message
        /// </summary>
        public int Index
        {
            get => index;
            set
            {
                index = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Programming language for interpretation of messages
        /// </summary>
        public Language Language
        {
            get => language;
            set
            {
                language = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Simple (single-word) description of parameter type
        /// </summary>
        public string Type
        {
            get => type;
            set
            {
                type = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Minimum value the parameter can get
        /// </summary>
        public long? Minimum
        {
            get => minimum;
            set
            {
                minimum = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Maximum value the parameter can get
        /// </summary>
        public long? Maximum
        {
            get => maximum;
            set
            {
                maximum = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Description of the parameter
        /// </summary>
        public string Description
        {
            get => description;
            set
            {
                description = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}