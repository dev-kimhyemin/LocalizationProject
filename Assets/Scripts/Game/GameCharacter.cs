using UnityEngine;

/// <summary>
/// 게임 내 캐릭터 클래스
/// </summary>
public class GameCharacter : MonoBehaviour
{
    public int CharacterIndex => characterIndex;
    public GameCharacterTextBalloon TextBalloon => textBalloon;
    
    [SerializeField] private int characterIndex;
    [SerializeField] private GameCharacterTextBalloon textBalloon;
}
