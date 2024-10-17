using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어 입력 처리 및 입력 이벤트 관리 클래스
/// </summary>
/// Player Input에서 Send Messages 사용
public class InputController : MonoBehaviour
{
    public static event Action OnEscapePressed;
    
    
    private void OnEscape(InputValue value)
    {
        OnEscapePressed?.Invoke();
    }
}
