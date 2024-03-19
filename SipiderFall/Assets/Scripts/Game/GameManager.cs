using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private bool _gameStarted = false;
    PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }
    
}
