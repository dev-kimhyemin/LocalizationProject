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


public class ReactiveLocalizeStringEvent : LocalizeStringEvent
{
    [SerializeField] private TMP_Text textField;


    private void Start()
    {
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
