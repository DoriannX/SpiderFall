using System.Collections;
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

    private void Start()
    {
        StartCoroutine(SlowDownTime(0, 3));
    }

    public void StartGame()
    {
        if (!_gameStarted)
        {
            StopAllCoroutines();
            Time.timeScale = 1;
            _gameStarted = true;
            Destroy(_input);
        }
        
    }
    IEnumerator SlowDownTime(float targetTimeScale, float duration)
    {
        float start = Time.timeScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(start, targetTimeScale, elapsed / duration);
            yield return null;
        }

        Time.timeScale = targetTimeScale;
    }
}
