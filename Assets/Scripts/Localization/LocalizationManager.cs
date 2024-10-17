using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalizationManager : Singleton<LocalizationManager>
{
    [SerializeField] private List<TMP_FontAsset> fonts;


    public static void ChangeLocale(int localeIndex)
    {
        int availableLocalesCount = LocalizationSettings.AvailableLocales.Locales.Count;

        if (localeIndex < 0 || localeIndex >= availableLocalesCount)
        {
            return;
        }

        Locale targetLocale = LocalizationSettings.AvailableLocales.Locales[localeIndex];
        LocalizationSettings.SelectedLocale = targetLocale;
    }
    
    public static string GetLocalizedString(int tableReferenceIndex, int tableEntryIndex)
    {
        string resultString = "";
        
        LocalizedString localizedString = new LocalizedString()
        {
            TableReference = GetTableReferenceString(tableReferenceIndex),
            TableEntryReference = $"{tableEntryIndex:D3}"
        };

        var stringOperation = localizedString.GetLocalizedStringAsync();

        if (stringOperation is { IsDone: true, Status: AsyncOperationStatus.Succeeded })
        {
            resultString = stringOperation.Result;
        }

        return resultString;
    }

    private static string GetTableReferenceString(int tableReferenceIndex)
    {
        return string.Concat("Table_", $"{tableReferenceIndex:D2}");
    }
    
    public TMP_FontAsset GetCurrentLocaleFont()
    {
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            bool canGetFont =
                LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[i] &&
                i < fonts.Count;

            if (canGetFont)
            {
                return fonts[i];
            }
        }

        return null;
    }
}
