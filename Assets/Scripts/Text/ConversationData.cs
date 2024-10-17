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


[CreateAssetMenu(menuName = "ScriptableObject/ConversationData")]
public class ConversationData : ScriptableObject
{
    public int TableIndex => tableIndex;
    public List<CharacterDialogueDataSet> Dialogues => dialogues;

    [SerializeField] private string identifier;
    [SerializeField] private int tableIndex;
    [SerializeField] private List<CharacterDialogueDataSet> dialogues;

    [Header("Editor")] 
    [SerializeField] private string filePath;


    public bool IsMatched(string requestedIdentifier)
    {
        return identifier == requestedIdentifier;
    }
    
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
