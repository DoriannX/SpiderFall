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

    public void Reset()
    {
        HasToReset = false;
        EnemyAmount = 10;
        LongRangeEnemyAmount = 5;
        FlyingEnemyAmount = 5;
        MapSize = 100;
        ActualLevel = 1;
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
}
