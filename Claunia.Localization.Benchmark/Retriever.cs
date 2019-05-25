using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using Claunia.Localization.Benchmark.Resources;
using Claunia.Localization.Core;
using Claunia.Localization.Parsers;
using Mono.Unix;

namespace Claunia.Localization.Benchmark
{
    public class Retriever
    {
        readonly Core.Localization          localization;
        readonly Dictionary<string, string> localizationIndex;

        public Retriever()
        {
            localization =
                GetText.Parse(Path.Combine(".", "gettext", "es_ES.po"), Encoding.GetEncoding("iso8859-1"));
            localizationIndex = localization.GetIndex("es_ES");
            Catalog.Init("catalog", "./gettext");
        }

        [Benchmark]
        public void MonoCatalog()
        {
            Catalog.GetString("Use MPEG video from the DVD version instead of lower resolution AVI");
            Catalog.GetString("Failed to load saved game from file.");
            Catalog.GetString("Engine does not support debug level '%s'");
            Catalog.GetString("Select an action and click 'Map'");
            Catalog.GetString(""                                                                              +
                              "Subtitles are enabled, but subtitling in King's Quest 7 was unfinished and "   +
                              "disabled in the release version of the game. ScummVM allows the subtitles to " +
                              "be re-enabled, but because they were removed from the original game, they do " +
                              "not always render properly or reflect the actual game speech. This is not a "  +
                              "ScummVM bug -- it is a problem with the game's assets.");
            Catalog.GetString("Preferred Dev.:");
            Catalog.GetString("This does not exist");
        }

        [Benchmark]
        public void Indexed()
        {
            localizationIndex.TryGetValue("Use MPEG video from the DVD version instead of lower resolution AVI", out _);
            localizationIndex.TryGetValue("Failed to load saved game from file.",                                out _);
            localizationIndex.TryGetValue("Engine does not support debug level '%s'",                            out _);
            localizationIndex.TryGetValue("Select an action and click 'Map'",                                    out _);
            localizationIndex
               .TryGetValue("" + "Subtitles are enabled, but subtitling in King's Quest 7 was unfinished and " + "disabled in the release version of the game. ScummVM allows the subtitles to " + "be re-enabled, but because they were removed from the original game, they do " + "not always render properly or reflect the actual game speech. This is not a " + "ScummVM bug -- it is a problem with the game's assets.",
                            out _);
            localizationIndex.TryGetValue("Preferred Dev.:",     out _);
            localizationIndex.TryGetValue("This does not exist", out _);
        }

        [Benchmark]
        public void DotNetResources()
        {
            string str1 = LocalizedResources.Use_MPEG_VIDEO;
            string str2 = LocalizedResources.FailedToLoad;
            string str3 = LocalizedResources.EngineDoesNotSupport;
            string str4 = LocalizedResources.SekectActionClickMap;
            string str5 = LocalizedResources.EnabledSubtitles;
            string str6 = LocalizedResources.PreferredDev;
        }

        [Benchmark]
        public void Linq()
        {
            GetUsingLinq("Use MPEG video from the DVD version instead of lower resolution AVI");
            GetUsingLinq("Failed to load saved game from file.");
            GetUsingLinq("Engine does not support debug level '%s'");
            GetUsingLinq("Select an action and click 'Map'");
            GetUsingLinq(""                                                                              +
                         "Subtitles are enabled, but subtitling in King's Quest 7 was unfinished and "   +
                         "disabled in the release version of the game. ScummVM allows the subtitles to " +
                         "be re-enabled, but because they were removed from the original game, they do " +
                         "not always render properly or reflect the actual game speech. This is not a "  +
                         "ScummVM bug -- it is a problem with the game's assets.");
            GetUsingLinq("Preferred Dev.:");
            GetUsingLinq("This does not exist");
        }

        [Benchmark]
        public void Loop()
        {
            GetUsingLoops("Use MPEG video from the DVD version instead of lower resolution AVI");
            GetUsingLoops("Failed to load saved game from file.");
            GetUsingLoops("Engine does not support debug level '%s'");
            GetUsingLoops("Select an action and click 'Map'");
            GetUsingLoops(""                                                                              +
                          "Subtitles are enabled, but subtitling in King's Quest 7 was unfinished and "   +
                          "disabled in the release version of the game. ScummVM allows the subtitles to " +
                          "be re-enabled, but because they were removed from the original game, they do " +
                          "not always render properly or reflect the actual game speech. This is not a "  +
                          "ScummVM bug -- it is a problem with the game's assets.");
            GetUsingLoops("Preferred Dev.:");
            GetUsingLoops("This does not exist");
        }

        string GetUsingLinq(string key)
        {
            Message message = localization.Messages.FirstOrDefault(m => m.Id == key);

            if(message is null) return key;

            LocalizedString localized = message.Translations.FirstOrDefault(l => l.Locale == "es_ES");

            if(!(localized is null)) return localized.Singular;

            int underscore = "es_ES".IndexOf('_');

            if(underscore < 0) return key;

            localized = message.Translations.FirstOrDefault(l => l.Locale == "es_ES".Substring(0, 2));

            return localized?.Singular ?? key;
        }

        string GetUsingLoops(string key)
        {
            Message message = null;

            foreach(Message msg in localization.Messages)
            {
                if(msg.Id != key) continue;

                message = msg;
                break;
            }

            if(message is null) return key;

            LocalizedString sameLocale    = null;
            LocalizedString majorLocale   = null;
            LocalizedString similarLocale = null;

            switch(message.Translations.Count)
            {
                case 0: return key;
                case 1: return message.Translations[0].Singular;
            }

            if("es_ES".Length >= 5 && "es_ES"[2] == '_')
            {
                string subLocale = new string(new[] {"es_ES"[0], "es_ES"[1]});

                foreach(LocalizedString translation in message.Translations)
                {
                    if(translation.Locale == "es_ES")
                    {
                        sameLocale = translation;
                        break;
                    }

                    if(translation.Locale.Length >= 5            && translation.Locale[0] == subLocale[0] &&
                       translation.Locale[1]     == subLocale[1] && similarLocale is null) similarLocale = translation;
                    else if(translation.Locale == subLocale && majorLocale is null) majorLocale          = translation;
                }

                return sameLocale?.Singular ?? majorLocale?.Singular ?? similarLocale?.Singular;
            }

            foreach(LocalizedString translation in message.Translations)
            {
                if(translation.Locale == "es")
                {
                    sameLocale = translation;
                    break;
                }

                if(translation.Locale.Length >= 5       && translation.Locale[0] == "es"[0] &&
                   translation.Locale[1]     == "es"[1] && similarLocale is null) similarLocale = translation;
            }

            return sameLocale?.Singular ?? similarLocale?.Singular;
        }
    }
}