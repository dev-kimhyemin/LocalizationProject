using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameTextHandler : MonoBehaviour
{
    public List<GameCharacterTextBalloon> TextBalloons { private get; set; }
    
    [SerializeField] private TMP_Text subtitleTextField;

    private List<ConversationData> _conversationDataSet;
    
    private IEnumerator _currentSubtitleCoroutine;
    private IEnumerator _currentConversationCoroutine;


    private void Awake()
    {
        subtitleTextField.gameObject.SetActive(false);

        LoadConversationDataSet();
    }

    private void LoadConversationDataSet()
    {
        const string filePath = "ScriptableObjects";

        var loadedConversationDataSet = Resources.LoadAll<ConversationData>(filePath);
        Debug.Log($"Loaded ConversationDataSet Count: {loadedConversationDataSet.Length}");
        
        if (loadedConversationDataSet.Length > 0)
        {
            _conversationDataSet = loadedConversationDataSet.ToList();
        }
    }
    
#region Subtitle
    public void StartSubtitle()
    {
        if (_currentSubtitleCoroutine != null)
        {
            StopCoroutine(_currentSubtitleCoroutine); 
        }
        
        subtitleTextField.gameObject.SetActive(true);
        
        _currentSubtitleCoroutine = SubtitleCoroutine();
        StartCoroutine(_currentSubtitleCoroutine);
    }
    
    private IEnumerator SubtitleCoroutine()
    {
        const int subtitleCount = 18;

        for (int i = 0; i < subtitleCount; ++i)
        {
            string subtitleString = LocalizationManager.GetLocalizedString(1, i);
            subtitleTextField.text = subtitleString;
            
            yield return new WaitForSeconds(3f);
        }
        
        OnSubtitleEnded();
    }

    private void OnSubtitleEnded()
    {
        _currentSubtitleCoroutine = null;
        subtitleTextField.gameObject.SetActive(false);
    }
#endregion

#region Conversation
    public void StartConversation(string conversationIdentifier)
    {
        ConversationData targetConversationData = null;
        
        foreach (var conversationData in _conversationDataSet)
        {
            if (conversationData.IsMatched(conversationIdentifier))
            {
                targetConversationData = conversationData;
            }
        }

        if (!targetConversationData) return;

        if (_currentConversationCoroutine != null)
        {
            StopCoroutine(_currentConversationCoroutine);
        }
        
        ResetAllTextBalloons();

        _currentConversationCoroutine = ConversationCoroutine(targetConversationData);
        StartCoroutine(_currentConversationCoroutine);
    }

    private void ResetAllTextBalloons()
    {
        foreach (var textBalloon in TextBalloons)
        {
            textBalloon.ResetTextBalloonStatus();
        }
    }

    private IEnumerator ConversationCoroutine(ConversationData conversationData)
    {
        var dialogues = conversationData.Dialogues;

        foreach (var dialogue in dialogues)
        {
            string dialogueString =
                LocalizationManager.GetLocalizedString(conversationData.TableIndex, dialogue.DialogueIndex);

            TextBalloons[dialogue.CharacterIndex].ShowDialogue(dialogueString);

            yield return new WaitForSeconds(3f);
            
            TextBalloons[dialogue.CharacterIndex].ResetTextBalloonStatus();
        }
        
        OnConversationEnded();
    }

    private void OnConversationEnded()
    {
        _currentConversationCoroutine = null;
        ResetAllTextBalloons();
    }
#endregion
}
