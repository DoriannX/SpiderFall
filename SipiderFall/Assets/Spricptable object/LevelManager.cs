using UnityEngine;

[CreateAssetMenu(fileName = "LevelManager", menuName = "ScriptableObjects/LevelManager", order = 1)]
public class LevelManager : ScriptableObject
{
    public bool ActiveTuto;
    public int EnemyAmount;
    public int MapSize;

    public bool HasToReset;

    public void Reset()
    {
        HasToReset = false;
        EnemyAmount = 10;
        MapSize = 75;
    }
}
