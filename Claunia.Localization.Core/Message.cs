using System;
using System.Collections.ObjectModel;

namespace Claunia.Localization.Core
{
    /// <summary>
    ///     Represent a message and its translations
    /// </summary>
    public sealed class Message
    {
        readonly ObservableCollection<Parameter>       parameters;
        readonly ObservableCollection<LocalizedString> translations;
        string                                         comments;
        string                                         context;
        string                                         id;
        internal EventHandler                          Modified;
        string                                         previousContext;
        string                                         reference;

        internal Message()
        {
            Source                         =  new LocalizedString();
            Source.Modified                += Modified;
            translations                   =  new ObservableCollection<LocalizedString>();
            translations.CollectionChanged += (sender, args) => Modified?.Invoke(this, EventArgs.Empty);
            parameters                     =  new ObservableCollection<Parameter>();
            parameters.CollectionChanged   += (sender, args) => Modified?.Invoke(this, EventArgs.Empty);
            Translations                   =  new ReadOnlyObservableCollection<LocalizedString>(translations);
            Parameters                     =  new ReadOnlyObservableCollection<Parameter>(parameters);
        }

        /// <summary>
        ///     Describes the context where this message occurs
        /// </summary>
        public string Context
        {
            get => context;
            set
            {
                context = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Describes the context where the previous version of this message occurred
        /// </summary>
        public string PreviousContext
        {
            get => previousContext;
            set
            {
                previousContext = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Comments added by the original writer of the message
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
        ///     Source code line reference or equivalent
        /// </summary>
        public string Reference
        {
            get => reference;
            set
            {
                reference = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Unique identifier of this message
        /// </summary>
        public string Id
        {
            get => id;
            set
            {
                id = value;
                Modified?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Original locale of this message
        /// </summary>
        public LocalizedString Source { get; }
        /// <summary>
        ///     List of translations
        /// </summary>
        public ReadOnlyObservableCollection<LocalizedString> Translations { get; }
        /// <summary>
        ///     List of parameters
        /// </summary>
        public ReadOnlyObservableCollection<Parameter> Parameters { get; }

        public override string ToString() => Source.Singular;

        /// <summary>
        ///     Initializes a new localized string in this message
        /// </summary>
        /// <returns>The newly initialized localized string</returns>
        public LocalizedString NewLocalizedString()
        {
            LocalizedString localizedString = new LocalizedString();
            localizedString.Modified += Modified;
            translations.Add(localizedString);

            return localizedString;
        }

        /// <summary>
        ///     Removes a localized string
        /// </summary>
        /// <param name="localizedString">The localized string to remove</param>
        /// <returns><c>true</c> if the localized string has been removed successfully, <c>false</c> otherwise</returns>
        public bool RemoveLocalizedString(LocalizedString localizedString) => translations.Remove(localizedString);

        /// <summary>
        ///     Initializes a new parameter in this message
        /// </summary>
        /// <returns>The newly initialized parameter</returns>
        public Parameter NewParameter()
        {
            Parameter parameter = new Parameter();
            parameter.Modified += Modified;
            parameters.Add(parameter);

            return parameter;
        }

        /// <summary>
        ///     Removes a parameter
        /// </summary>
        /// <param name="parameter">The parameter to remove</param>
        /// <returns><c>true</c> if the parameter string has been removed successfully, <c>false</c> otherwise</returns>
        public bool RemoveParameter(Parameter parameter) => parameters.Remove(parameter);
    }
}