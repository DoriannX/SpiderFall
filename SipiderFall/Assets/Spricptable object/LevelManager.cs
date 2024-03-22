using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelManager", menuName = "ScriptableObjects/LevelManager", order = 1)]
public class LevelManager : ScriptableObject
{
    public bool ActiveTuto;
    public int EnemyAmount;
    public int LongRangeEnemyAmount;
    public int FlyingEnemyAmount;
    public int MapSize;
    int ActualLevel = 1;

    public bool HasToReset;
    public void ResetLevel()
    {
        Debug.Log("reset level");
        PlayerPrefs.SetInt("EnemyAmount", 10);
        PlayerPrefs.SetInt("LongRangeEnemyAmount", 5);
        PlayerPrefs.SetInt("FlyingEnemyAmount", 5);
        PlayerPrefs.SetInt("MapSize", 100);
        PlayerPrefs.SetInt("ActualLevel", 1);
        HasToReset = false;
    }

    public void OnEnable()
    {
        ActiveTuto = (PlayerPrefs.GetInt("ActiveTuto") == 1) ? true : false;
        EnemyAmount = PlayerPrefs.GetInt("EnemyAmount");
        LongRangeEnemyAmount = PlayerPrefs.GetInt("LongRangeEnemyAmount");
        FlyingEnemyAmount = PlayerPrefs.GetInt("FlyingEnemyAmount");
        MapSize = PlayerPrefs.GetInt("MapSize");
        ActualLevel = PlayerPrefs.GetInt("ActualLevel");
        Debug.Log(ActualLevel);
    }

    public void NextLevel()
    {
        ActiveTuto = false;
        EnemyAmount += 5;
        LongRangeEnemyAmount += 3;
        FlyingEnemyAmount += 5;
        MapSize += 20;
        ActualLevel ++;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("next level");
    }

    public int GetActualLevel()
    {
        return ActualLevel;
    }

    private void OnDestroy()
    {
        if (ActiveTuto)
            PlayerPrefs.SetInt("ActiveTuto", 1);
        else
            PlayerPrefs.SetInt("ActiveTuto", 0);
        PlayerPrefs.SetInt("EnemyAmount", EnemyAmount);
        PlayerPrefs.SetInt("LongRangeEnemyAmount", LongRangeEnemyAmount);
        PlayerPrefs.SetInt("FlyingEnemyAmount", FlyingEnemyAmount);
        PlayerPrefs.SetInt("MapSize", MapSize);
        PlayerPrefs.SetInt("ActualLevel", ActualLevel);
    }
}
