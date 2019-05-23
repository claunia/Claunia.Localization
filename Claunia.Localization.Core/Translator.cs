using System;

namespace Claunia.Localization.Core
{
    public class Translator
    {
        string                email;
        internal EventHandler Modified;
        string                name;
        string                nativeName;

        internal Translator(int id)
        {
            Id = id;
        }

        /// <summary>
        ///     Translator ID, unique in the corresponding localization, and sequential
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Translator e-mail
        /// </summary>
        public string Email
        {
            get => email;
            set
            {
                email = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Translator name, in ASCII, english form
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                name = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Translator full name in native form
        /// </summary>
        public string NativeName
        {
            get => nativeName ?? Name;
            set
            {
                if(value == name) return;

                nativeName = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}