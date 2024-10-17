using TMPro;
using UnityEngine;

[RequireComponent(typeof(ReactiveLocalizeStringEvent))]
public class GameCharacterTextBalloon : TextMeshPro
{
    protected override void Awake()
    {
        base.Awake();

        text = "";
    }

    public void ShowDialogue(string dialogueString)
    {
        text = dialogueString;
    }

    public void ResetTextBalloonStatus()
    {
        text = "";
    }
}
