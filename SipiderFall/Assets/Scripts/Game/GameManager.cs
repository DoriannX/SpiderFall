using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        StartCoroutine(ResetInput());
    }

    IEnumerator ResetInput()
    {
        _input.enabled = false;
        yield return new WaitForSecondsRealtime(.5f);
        _input.enabled = true;
    }
    
}
