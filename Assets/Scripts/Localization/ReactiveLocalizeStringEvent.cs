using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ReactiveLocalizeStringEvent))]
public class ReactiveLocalizeStringEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var stringEvent = target as ReactiveLocalizeStringEvent;
        if (!stringEvent) return;
        
        if(GUILayout.Button("Component Setting"))
            stringEvent.ComponentSetting();
    }
}

#endif

/// <summary>
/// 게임 실행 시점, 언어 변경 시점을 탐지하여 자동으로 텍스트의 폰트를 변경하는 클래스
/// </summary>
/// 현재 권장되는 TMP_FontAsset의 경우 지정된 문자들로 Atlas를 미리 생성하여 사용하므로
/// 폰트 에셋의 크기를 작게 유지하면서도 현지화 기능을 강력하게 유지할 수 있도록 하는 클래스

/// LocalizeStringEvent를 상속하므로 아래의 두 가지 방식 모두를 사용해 텍스트를 띄울 수 있음
/// 1. Dynamic Text를 이용해 문자열 직접 지정
/// 2. String Reference에서 Localization Table, Table Entry를 지정
public class ReactiveLocalizeStringEvent : LocalizeStringEvent
{
    [SerializeField] private TMP_Text textField;


    private void Start()
    {
        // Localization Setting에서 언어가 변경된 시점에 폰트를 변경하도록 처리
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        
        ChangeFont();
    }

    private void OnLocaleChanged(Locale newLocale)
    {
        ChangeFont();
    }

    private void ChangeFont()
    {
        if (!Application.isPlaying) return;
        
        // 일정 시간 간격을 두고 여러 개의 다이얼로그가 자동으로 송출되는 경우에도 안정적으로 동작할 수 있도록
        // 폰트가 변경될 때 텍스트를 지움
        // (폰트가 변경되었다는 것은 설정 언어가 변경되었다는 의미)
        textField.text = "";
        
        var currentLocaleFont = LocalizationManager.Instance.GetCurrentLocaleFont();

        if (currentLocaleFont)
        {
            textField.font = currentLocaleFont;
        }
    }

    internal void ComponentSetting()
    {
        TryGetComponent(out textField);
    }
}
