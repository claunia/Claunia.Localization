using System;
using System.IO;
using System.Text;
using Claunia.Localization.Core;

namespace Claunia.Localization.Parsers
{
    public static class GetText
    {
        public static Core.Localization Parse(string catalog)
        {
            using(StringReader sr = new StringReader(catalog)) return Parse(sr);
        }

        public static Core.Localization Parse(string path, Encoding encoding)
        {
            using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using(StreamReader sr = new StreamReader(fs, encoding)) return Parse(sr);
            }
        }

        public static Core.Localization Parse(Stream catalog)
        {
            using(StreamReader sr = new StreamReader(catalog)) return Parse(sr);
        }

        public static Core.Localization Parse(TextReader catalog)
        {
            Core.Localization localization = new Core.Localization();

            string     line                = "";
            string     currentMsgId        = "";
            string     currentMsgString    = "";
            Translator currentTranslator   = null;
            string     currentLocale       = null;
            bool       firstEmptyMessageId = true;
            string     currentComment      = "";
            string     extractedComment    = "";
            string     currentReference    = "";
            string     currentContext      = "";
            Language   currentLanguage     = Language.None;

            while(line != null)
            {
                line = catalog.ReadLine();

                if(line is null) break;

                if(line == string.Empty)
                {
                    if(!firstEmptyMessageId)
                    {
                        Message message = localization.NewMessage();

                        message.Comments        = extractedComment;
                        message.Context         = currentContext;
                        message.Reference       = currentReference;
                        message.Source.Singular = currentMsgId;
                        message.Id              = currentContext + currentMsgId;
                        LocalizedString localizedString = message.NewLocalizedString();

                        localizedString.Comments   = currentComment;
                        localizedString.Locale     = currentLocale;
                        localizedString.Singular   = currentMsgString;
                        localizedString.Translator = currentTranslator?.Id;

                        // TODO: Parse per programming language
                    }

                    firstEmptyMessageId = false;
                    currentMsgId        = "";
                    currentMsgString    = "";
                    currentComment      = "";
                    extractedComment    = "";
                    currentReference    = "";
                    currentContext      = "";
                    continue; // TODO
                }

                if(line[0] == '#')
                {
                    if(line.Length == 1)
                    {
                        if(!firstEmptyMessageId) currentComment += Environment.NewLine;
                        continue;
                    }

                    switch(line[1])
                    {
                        case ' ':
                            if(firstEmptyMessageId) break;

                            currentComment += line.Substring(2) + Environment.NewLine;
                            continue;
                        case '.':
                            extractedComment += line.Substring(3) + Environment.NewLine;
                            continue;
                        case ':':
                            currentReference += line.Substring(3) + Environment.NewLine;
                            continue;
                        case ',':
                            string flags = line.Substring(2);

                            switch(flags)
                            {
                                case "fuzzy": continue;
                                case "c-format":
                                    currentLanguage = Language.C;
                                    continue;
                                case "objc-format":
                                    currentLanguage = Language.ObjectiveC;
                                    continue;
                                case "sh-format":
                                    currentLanguage = Language.Shell;
                                    continue;
                                case "python-format":
                                    currentLanguage = Language.Python;
                                    continue;
                                case "python-brace-format":
                                    currentLanguage = Language.PythonBraced;
                                    continue;
                                case "lisp-format":
                                    currentLanguage = Language.Lisp;
                                    continue;
                                case "elisp-format":
                                    currentLanguage = Language.EmacsLisp;
                                    continue;
                                case "librep-format":
                                    currentLanguage = Language.Librep;
                                    continue;
                                case "scheme-format":
                                    currentLanguage = Language.Scheme;
                                    continue;
                                case "smalltalk-format":
                                    currentLanguage = Language.Smalltalk;
                                    continue;
                                case "java-format":
                                    currentLanguage = Language.Java;
                                    continue;
                                case "csharp-format":
                                    currentLanguage = Language.CSharp;
                                    continue;
                                case "awk-format":
                                    currentLanguage = Language.Awk;
                                    continue;
                                case "object-pascal-format":
                                    currentLanguage = Language.ObjectPascal;
                                    continue;
                                case "ycp-format":
                                    currentLanguage = Language.Ycp;
                                    continue;
                                case "tcl-format":
                                    currentLanguage = Language.Tcl;
                                    continue;
                                case "perl-format":
                                    currentLanguage = Language.Perl;
                                    continue;
                                case "perl-brace-format":
                                    currentLanguage = Language.PerlBraced;
                                    continue;
                                case "php-format":
                                    currentLanguage = Language.Php;
                                    continue;
                                case "gcc-internal-format":
                                    currentLanguage = Language.GccInternal;
                                    continue;
                                case "gfc-internal-format":
                                    currentLanguage = Language.GfcInternal;
                                    continue;
                                case "qt-format":
                                    currentLanguage = Language.Qt;
                                    continue;
                                case "qt-plural-format":
                                    currentLanguage = Language.QtPlural;
                                    continue;
                                case "kde-format":
                                    currentLanguage = Language.Kde;
                                    continue;
                                case "boost-format":
                                    currentLanguage = Language.Boost;
                                    continue;
                                case "lua-format":
                                    currentLanguage = Language.Lua;
                                    continue;
                                case "javascript-format":
                                    currentLanguage = Language.JavaScript;
                                    continue;
                                default:
                                    currentLanguage = Language.None;
                                    continue;
                            }

                        case '|': break;
                    }
                }

                if(line.StartsWith("msgid ", StringComparison.Ordinal))
                {
                    currentMsgId += line.Substring(7, line.Length - 8);
                    continue;
                }

                if(line.StartsWith("msgstr ", StringComparison.Ordinal))
                {
                    currentMsgString = line.Substring(7, line.Length - 8);
                    continue;
                }

                if(line[0] == '"')
                {
                    if(currentMsgId != string.Empty)
                    {
                        currentMsgString += line.Substring(1, line.Length - 2);
                        continue;
                    }

                    string projectString = line.Substring(1, line.Length - 2);

                    if(projectString.StartsWith("Project-Id-Version", StringComparison.Ordinal))
                    {
                        string projectIdVersion = projectString.Substring(20, projectString.Length - 20);
                        if(projectIdVersion.EndsWith("\\n", StringComparison.Ordinal))
                            projectIdVersion = projectIdVersion.Substring(0, projectIdVersion.Length - 2);

                        localization.Project.Name = projectIdVersion;
                        continue;
                    }

                    if(projectString.StartsWith("Report-Msgid-Bugs-To", StringComparison.Ordinal))
                    {
                        string projectReportTo = projectString.Substring(22, projectString.Length - 22);
                        if(projectReportTo.EndsWith("\\n", StringComparison.Ordinal))
                            projectReportTo = projectReportTo.Substring(0, projectReportTo.Length - 2);

                        localization.Project.Url = projectReportTo;
                        continue;
                    }

                    if(projectString.StartsWith("Last-Translator", StringComparison.Ordinal))
                    {
                        string lastTranslator = projectString.Substring(17, projectString.Length - 17);
                        if(lastTranslator.EndsWith("\\n", StringComparison.Ordinal))
                            lastTranslator = lastTranslator.Substring(0, lastTranslator.Length - 2);

                        if(lastTranslator[lastTranslator.Length - 1] == '>')
                        {
                            int    emailStart = lastTranslator.LastIndexOf('<');
                            string name       = lastTranslator.Substring(0, emailStart - 1);
                            string email = lastTranslator.Substring(emailStart                         + 1,
                                                                    lastTranslator.Length - emailStart - 2);

                            currentTranslator = localization.NewTranslator(name, email);
                        }
                        else currentTranslator = localization.NewTranslator(lastTranslator, null);

                        continue;
                    }

                    if(projectString.StartsWith("Language", StringComparison.Ordinal))
                    {
                        currentLocale = projectString.Substring(10, projectString.Length - 10);
                        if(currentLocale.EndsWith("\\n", StringComparison.Ordinal))
                            currentLocale = currentLocale.Substring(0, currentLocale.Length - 2);
                    }
                }
            }

            return localization;
        }
    }
}