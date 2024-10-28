using System;
using System.Collections.Generic;
using System.Linq;
using JsonExporter.Contract;
using JsonExporter.Model;
using JsonExporter.Repository;
using StardewValley;

namespace JsonExporter.Util;

public static class TranslationHelper
{
    private static void WithAllLanguages(Action<string> callback)
    {
        var languageCodes =
            (LocalizedContentManager.LanguageCode[])Enum.GetValues(typeof(LocalizedContentManager.LanguageCode));

        foreach (var languageCode in languageCodes)
        {
            if (
                    languageCode is LocalizedContentManager.LanguageCode.zh or LocalizedContentManager.LanguageCode.th
                    or LocalizedContentManager.LanguageCode.ko or LocalizedContentManager.LanguageCode.mod
                )
                // skip some languages
                continue;

            try
            {
                LocalizedContentManager.CurrentLanguageCode = languageCode;
                Game1.game1.TranslateFields();

                callback(Enum.GetName(typeof(LocalizedContentManager.LanguageCode), languageCode) ?? "none");
            }
            catch
            {
            }
        }
    }

    public static void TranslateAll(IEnumerable<ITranslatable> ts)
    {
        WithAllLanguages((languageCode) =>
        {
            foreach (var t in ts) t.PopulateDisplayName(languageCode);
        });
    }

    public static void Reset()
    {
        LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.en;
        Game1.game1.TranslateFields();
    }
}