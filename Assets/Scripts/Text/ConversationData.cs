using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;

[CustomEditor(typeof(ConversationData))]
public class ConversationDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var dataClass = target as ConversationData;
        if (!dataClass) return;
        
        if(GUILayout.Button("Load From CSV"))
            dataClass.LoadFromCsv();
    }
}

#endif


[Serializable]
public struct CharacterDialogueDataSet
{
    public int CharacterIndex => characterIndex;
    public int DialogueIndex => dialogueIndex;
    
    [SerializeField] private int characterIndex;
    [SerializeField] private int dialogueIndex;

    public CharacterDialogueDataSet(int characterIndex, int dialogueIndex)
    {
        this.characterIndex = characterIndex;
        this.dialogueIndex = dialogueIndex;
    }
}


/// <summary>
/// 게임 캐릭터간의 대화에 필요한 데이터 관리 클래스
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject/ConversationData")]
public class ConversationData : ScriptableObject
{
    public int TableIndex => tableIndex;
    public List<CharacterDialogueDataSet> Dialogues => dialogues;

    // string 타입의 식별자를 따로 둔 것은 많은 수의 다이얼로그 세트들이 동일한 Localization Table을 사용할 수 있도록 하기 위함
    // 예를 들어 하나의 캐릭터에 관련된 다이얼로그를 모두 하나의 테이블에 넣고 그중 일부를 끊어 사용하는 방식으로 통합 관리할 수 있음
    [SerializeField] private string identifier;
    
    // Localization Table 인덱스
    [SerializeField] private int tableIndex;
    [SerializeField] private List<CharacterDialogueDataSet> dialogues;

    [Header("Editor")] 
    [SerializeField] private string filePath;


    public bool IsMatched(string requestedIdentifier)
    {
        return identifier == requestedIdentifier;
    }
    
    // 내부 개발 툴
    // 엑셀, 구글 시트, 넘버스 등으로 작업한 데이터를 ScriptableObject로 편리하게 import 할 수 있음
    internal void LoadFromCsv()
    {
        dialogues.Clear();

        string[] lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            string[] values = line.Split(',');

            if (values.Length >= 2 && 
                int.TryParse(values[0], out int characterIndex) &&
                int.TryParse(values[1], out int dialogueIndex))
            {
                var newDataSet = new CharacterDialogueDataSet(characterIndex, dialogueIndex);
                dialogues.Add(newDataSet);
            }
        }
    }
}
