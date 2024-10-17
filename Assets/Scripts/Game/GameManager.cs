using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var manager = target as GameManager;
        if (!manager) return;
        
        if(GUILayout.Button("Start Subtitle"))
            manager.StartSubtitle();
        
        if(GUILayout.Button("Start Conversation"))
            manager.StartConversation();
    }
}

#endif

/// <summary>
/// 게임을 구동하는 데에 필요한 필수 오브젝트들의 관리, 코어 플레이 요소 실행
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameTextHandler gameTextHandler;
    [SerializeField] private List<GameCharacter> characters;


    protected override void Awake()
    {
        base.Awake();

        InitializeCharacterTextBalloons();
    }

    /// <summary>
    /// 게임 실행 시 게임 캐릭터들의 대화창을 GameTextHandler에 전달
    /// </summary>
    private void InitializeCharacterTextBalloons()
    {
        var textBalloons = new List<GameCharacterTextBalloon>();
        
        // 캐릭터 인덱스가 불규칙적으로 할당되어 있는 상황에 대비하여 인덱스를 지정하여 리스트 생성
        foreach (var character in characters)
        {
            textBalloons.Insert(character.CharacterIndex, character.TextBalloon);
        }

        gameTextHandler.TextBalloons = textBalloons;
    }
    
    internal void StartSubtitle()
    {
        gameTextHandler.StartSubtitle();
    }

    internal void StartConversation()
    {
        gameTextHandler.StartConversation("Sample");
    }
}
