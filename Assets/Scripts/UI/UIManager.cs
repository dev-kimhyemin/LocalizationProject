using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 내 UI 관리 클래스
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameMenuObject;
    [SerializeField] private List<Button> localeChangeButtons;


    private void Start()
    {
        InputController.OnEscapePressed += OnEscapePressed;
        
        // 언어 변경 버튼에 순차적으로 사용 가능한 언어 변경 기능 할당
        for (int i = 0; i < localeChangeButtons.Count; ++i)
        {
            int targetLocaleIndex = i;
            localeChangeButtons[i].onClick.AddListener(delegate{ChangeLocale(targetLocaleIndex);});
        }
    }

    // esc 키 입력 시 게임 메뉴 활성화 상태 토글
    private void OnEscapePressed()
    {
        gameMenuObject.SetActive(!gameMenuObject.activeSelf);
    }

    private static void ChangeLocale(int localeIndex)
    {
        LocalizationManager.ChangeLocale(localeIndex);
    }
}
