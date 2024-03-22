using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerInput _input;
    public LevelManager Level;
    [SerializeField] private TextMeshProUGUI _actualLevelText;

    public static GameManager Instance;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        _input = GetComponent<PlayerInput>();
        if (Level.HasToReset)
            Level.ResetLevel();
        Level.AwakeLevel();
    }

    private void Start()
    {
        StartCoroutine(ResetInput());
        RefreshLevel();
    }

    IEnumerator ResetInput()
    {
        _input.enabled = false;
        yield return new WaitForSecondsRealtime(.5f);
        _input.enabled = true;
    }

    public void NextLevel()
    {
        Level.NextLevel();
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void RefreshLevel()
    {
        _actualLevelText.text = "Level : " + Level.GetActualLevel();
    }

}
