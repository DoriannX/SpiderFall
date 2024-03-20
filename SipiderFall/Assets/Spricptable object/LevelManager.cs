using UnityEngine;

[CreateAssetMenu(fileName = "LevelManager", menuName = "ScriptableObjects/LevelManager", order = 1)]
public class LevelManager : ScriptableObject
{
    public bool ActiveTuto;
    public int EnemyAmount;
    public int MapSize;
    [HideInInspector] public int ActualLevel = 1;

    public bool HasToReset;

    public void Reset()
    {
        HasToReset = false;
        EnemyAmount = 10;
        MapSize = 75;
        ActualLevel = 1;
    }

    public void NextLevel()
    {
        ActiveTuto = false;
        EnemyAmount += 5;
        MapSize += 20;
        ActualLevel += 1;
    }
}
