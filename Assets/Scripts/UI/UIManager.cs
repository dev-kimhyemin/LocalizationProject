using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameMenuObject;
    [SerializeField] private List<Button> localeChangeButtons;


    private void Start()
    {
        InputController.OnEscapePressed += OnEscapePressed;
        
        for (int i = 0; i < localeChangeButtons.Count; ++i)
        {
            int targetLocaleIndex = i;
            localeChangeButtons[i].onClick.AddListener(delegate{ChangeLocale(targetLocaleIndex);});
        }
    }

    private void OnEscapePressed()
    {
        gameMenuObject.SetActive(!gameMenuObject.activeSelf);
    }

    private static void ChangeLocale(int localeIndex)
    {
        LocalizationManager.ChangeLocale(localeIndex);
    }
}
