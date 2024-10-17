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


public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameTextHandler gameTextHandler;
    [SerializeField] private List<GameCharacter> characters;


    protected override void Awake()
    {
        base.Awake();

        InitializeCharacterTextBalloons();
    }

    private void InitializeCharacterTextBalloons()
    {
        var textBalloons = new List<GameCharacterTextBalloon>();
        
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
