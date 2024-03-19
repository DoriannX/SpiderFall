using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerInput _input;
    GameObject _player;
    public LevelManager Level;

    public static GameManager Instance;



    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        _input = GetComponent<PlayerInput>();
        _player = Player.Instance.gameObject;

        if (Level.HasToReset)
            Level.Reset();
    }

    private void Start()
    {
        StartCoroutine(ResetInput());
        FinishLevelDetecter.Instance.LevelFinished.AddListener(ChoseLevel);
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
        Level.ActiveTuto = false;
        Level.EnemyAmount += 5;
        Level.MapSize += 20;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    
}
