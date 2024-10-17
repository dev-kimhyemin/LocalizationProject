using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// 현지화 텍스트 관리 및 처리, 언어 설정 관리 클래스
/// </summary>
public class LocalizationManager : Singleton<LocalizationManager>
{
    // 언어별로 폰트 하나만 사용함을 가정하고 유니크함을 인덱스로만 보장하는 리스트 1개 사용
    [SerializeField] private List<TMP_FontAsset> fonts;


    /// <summary>
    /// Localization Setting의 Locale 설정에 따라 현재 Locale을 변경
    /// </summary>
    /// <param name="localeIndex">Localization Settings의 Available Locale 참조</param>
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
    
    /// <summary>
    /// 현재 설정된 Locale에 맞는 현지화된 문자열 반환
    /// </summary>
    /// <param name="tableReferenceIndex">숫자 형식의 인덱스</param>
    /// <param name="tableEntryIndex">숫자 형식의 인덱스</param>
    /// <returns></returns>
    public static string GetLocalizedString(int tableReferenceIndex, int tableEntryIndex)
    {
        string resultString = "";
        
        LocalizedString localizedString = new LocalizedString()
        {
            TableReference = GetTableReferenceString(tableReferenceIndex),
            TableEntryReference = $"{tableEntryIndex:D3}"
        };

        var stringOperation = localizedString.GetLocalizedStringAsync();

        // Localization Table을 Preload로 설정한 후 Async를 사용하고 있음
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
    
    /// <summary>
    /// 현재 설정된 언어에 맞는 폰트 에셋 반환
    /// </summary>
    /// <returns>TMP_FontAsset</returns>
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
