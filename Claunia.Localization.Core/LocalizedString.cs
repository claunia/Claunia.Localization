using System;
using System.Collections.ObjectModel;

namespace Claunia.Localization.Core
{
    /// <summary>
    ///     Represent a localized string
    /// </summary>
    public sealed class LocalizedString
    {
        readonly ObservableCollection<Plural> plurals;
        string                                comments;
        string                                locale;
        internal EventHandler                 Modified;
        string                                singular;
        int?                                  translator;

        internal LocalizedString()
        {
            plurals                   =  new ObservableCollection<Plural>();
            plurals.CollectionChanged += (sender, args) => Modified?.Invoke(this, EventArgs.Empty);
            Plurals                   =  new ReadOnlyObservableCollection<Plural>(plurals);
        }

        /// <summary>
        ///     Contains the singular, or only, text for this string
        /// </summary>
        public string Singular
        {
            get => singular;
            set
            {
                singular = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Which locale this string is in. Format is ll_CC with ll as ISO 639-2 language code and CC as ISO 3166 two letter
        ///     uppercase country code. _CC can be omitted.
        /// </summary>
        public string Locale
        {
            get => locale;
            set
            {
                locale = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     ID of the translator, in this localization, that wrote this string
        /// </summary>
        public int? Translator
        {
            get => translator;
            set
            {
                translator = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Translator comments
        /// </summary>
        public string Comments
        {
            get => comments;
            set
            {
                comments = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Plural forms for this string
        /// </summary>
        public ReadOnlyObservableCollection<Plural> Plurals { get; }

        public override string ToString() => singular;

        /// <summary>
        ///     Initializes a new plural in this message
        /// </summary>
        /// <returns>The newly initialized plural</returns>
        public Plural NewPlural()
        {
            Plural parameter = new Plural();
            parameter.Modified += Modified;
            plurals.Add(parameter);

            return parameter;
        }

        /// <summary>
        ///     Removes a plural
        /// </summary>
        /// <param name="parameter">The plural to remove</param>
        /// <returns><c>true</c> if the plural string has been removed successfully, <c>false</c> otherwise</returns>
        public bool RemovePlural(Plural parameter) => plurals.Remove(parameter);
    }
}