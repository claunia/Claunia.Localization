using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Claunia.Localization.Core
{
    /// <summary>
    ///     Represents the localization
    /// </summary>
    public class Localization
    {
        readonly Dictionary<string, Dictionary<string, string>> indexes;
        readonly ObservableCollection<Message>                  messages;
        readonly ObservableCollection<Translator>               translators;

        public Localization()
        {
            Creation                      =  DateTime.UtcNow;
            LastWritten                   =  DateTime.UtcNow;
            Project                       =  new Project();
            Project.ProjectModified       += OnModified;
            translators                   =  new ObservableCollection<Translator>();
            Translators                   =  new ReadOnlyObservableCollection<Translator>(translators);
            translators.CollectionChanged += OnModified;
            messages                      =  new ObservableCollection<Message>();
            Messages                      =  new ReadOnlyObservableCollection<Message>(messages);
            messages.CollectionChanged    += OnModified;
            indexes                       =  new Dictionary<string, Dictionary<string, string>>();
        }

        /// <summary>Date this localization has been created</summary>
        public DateTime Creation { get; }
        /// <summary>Date this localization has been modified</summary>
        public DateTime LastWritten { get; private set; }
        /// <summary>Project this localization belongs to</summary>
        public Project Project { get; }
        /// <summary>List of translators that have worked in this localization</summary>
        public ReadOnlyObservableCollection<Translator> Translators { get; }
        /// <summary>
        ///     List of messages present in this localization
        /// </summary>
        public ReadOnlyObservableCollection<Message> Messages { get; }

        void OnModified(object sender, EventArgs args)
        {
            LastWritten = DateTime.UtcNow;
        }

        /// <summary>
        ///     Adds a new translator to the localization
        /// </summary>
        /// <param name="name">Translator full name, english-form, in ASCII</param>
        /// <param name="email">Translator e-mail</param>
        /// <param name="nativeName">Translator full name, in native form</param>
        /// <returns>The new translator</returns>
        public Translator NewTranslator(string name, string email, string nativeName = null)
        {
            int        id         = translators.Count > 0 ? translators.Max(t => t.Id) + 1 : 1;
            Translator translator = new Translator(id) {Name = name, Email = email};
            translator.Modified += OnModified;

            translators.Add(translator);

            return translator;
        }

        /// <summary>
        ///     Removes a translator from the localization
        /// </summary>
        /// <param name="translator">Translator to remove</param>
        /// <returns><c>true</c> if the translator has been removed successfully, <c>false</c> otherwise</returns>
        public bool RemoveTranslator(Translator translator) => !(translator is null) && translators.Remove(translator);

        /// <summary>
        ///     Removes a translator from the localization
        /// </summary>
        /// <param name="id">ID of the translator to remove</param>
        /// <returns><c>true</c> if the translator has been removed successfully, <c>false</c> otherwise</returns>
        public bool RemoveTranslator(int id)
        {
            Translator translator = translators.FirstOrDefault(t => t.Id == id);
            return !(translator is null) && translators.Remove(translator);
        }

        public Message NewMessage()
        {
            Message message = new Message();
            message.Modified += OnModified;

            messages.Add(message);

            return message;
        }

        public bool RemoveMessage(Message message) => !(message is null) && messages.Remove(message);

        public bool RemoveMessage(string id)
        {
            Message message = messages.FirstOrDefault(t => t.Id == id);
            return !(message is null) && messages.Remove(message);
        }

        public Dictionary<string, string> GetIndex(string locale)
        {
            // TODO: Plurals
            if(indexes.TryGetValue(locale, out Dictionary<string, string> existingIndex)) return existingIndex;

            Dictionary<string, string> index = new Dictionary<string, string>();
            foreach(Message message in messages)
            {
                LocalizedString translated = message.Translations.FirstOrDefault(l => l.Locale == locale);

                if(translated is null)
                {
                    // TODO: Requested "es" but only "es_ES" exists
                    int underscore = locale.IndexOf('_');
                    if(underscore > 0)
                        translated = message.Translations.FirstOrDefault(l => l.Locale == locale.Substring(underscore));
                }

                index.Add(message.Id, translated is null ? message.Source.Singular : translated.Singular);
            }

            indexes.Add(locale, index);

            return index;
        }
    }
}