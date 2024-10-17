using UnityEngine;

public class GameCharacter : MonoBehaviour
{
    public int CharacterIndex => characterIndex;
    public GameCharacterTextBalloon TextBalloon => textBalloon;
    
    [SerializeField] private int characterIndex;
    [SerializeField] private GameCharacterTextBalloon textBalloon;
}
