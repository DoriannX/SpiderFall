using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{
    PlayerInput _input;
    public LevelManager Level;
    [SerializeField] private TextMeshProUGUI _actualLevelText;

    public static GameManager Instance;



    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        _input = GetComponent<PlayerInput>();

        if (Level.HasToReset)
            Level.Reset();
    }

    private void Start()
    {
        StartCoroutine(ResetInput());
        FinishLevelDetecter.Instance.LevelFinished.AddListener(ChoseLevel);
        RefreshLevel();
    }

    IEnumerator ResetInput()
    {
        _input.enabled = false;
        yield return new WaitForSecondsRealtime(.5f);
        _input.enabled = true;
    }

    private void ChoseLevel()
    {
        if (FinishLevelDetecter.Instance.Won)
            NextLevel();
        else
            RestartLevel();
    }

    public void NextLevel()
    {
        Level.NextLevel();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void RefreshLevel()
    {
        _actualLevelText.text = "Level : " + Level.ActualLevel;
    }

}
