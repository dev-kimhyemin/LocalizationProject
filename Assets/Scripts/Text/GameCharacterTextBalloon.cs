using TMPro;
using UnityEngine;

/// <summary>
/// 게임 캐릭터들이 가지고 있는 대화창 클래스
/// </summary>
// 오브젝트 생성 시 ReactiveLocalizeStringEvent 컴포넌트가 자동으로 부착되도록 설정
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
