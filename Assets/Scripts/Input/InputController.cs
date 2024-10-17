using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static event Action OnEscapePressed;
    
    
    private void OnEscape(InputValue value)
    {
        OnEscapePressed?.Invoke();
    }
}
