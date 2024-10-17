using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// 게임 내 텍스트 송출
/// </summary>
public class GameTextHandler : MonoBehaviour
{
    public List<GameCharacterTextBalloon> TextBalloons { private get; set; }
    
    // UI Text 여부에 관계없이 적용할 수 있도록 TMP_Text 사용
    [SerializeField] private TMP_Text subtitleTextField;

    private List<ConversationData> _conversationDataSet;
    private IEnumerator _currentSubtitleCoroutine;
    private IEnumerator _currentConversationCoroutine;


    private void Awake()
    {
        subtitleTextField.gameObject.SetActive(false);

        LoadConversationDataSet();
    }

    // 본 프로젝트의 ConversationData의 수는 매우 적을 것으로 예상되므로
    // 런타임에 Resources 폴더로부터 ConversationData를 로드하는 방식을 채택함
    // 실제 프로젝트에서는 아래와 같은 방식으로 최적화 할 수 있음
    // 1. 실행 시 유니티 에디터인 경우에만 런타임으로 로드하고 빌드 프로세스에 아래 메서드의 동작을 추가
    // 2. ConversationData 리스트를 직렬화하고 아래 메서드의 동작을 에디터 기능(개발 툴)으로 변경
    // 3. ConversationData 리스트를 관리하는 클래스를 별도로 생성
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
    // Subtitle 송출 도중에 실행 요청이 들어와도 동작하도록 구현
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
    // Conversation 송출 도중에 실행 요청이 들어와도 동작하도록 구현
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

    // ConversationData에서 지정된 캐릭터가 지정된 대사를 송출함
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
