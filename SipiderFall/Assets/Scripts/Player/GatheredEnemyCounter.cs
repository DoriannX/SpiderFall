using UnityEngine;

public class GatheredEnemyCounter : MonoBehaviour
{
    //The amount of enemy gathered
    int _gatheredEnemy = 0;
    public static GatheredEnemyCounter Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public int GetGatheredEnemyAmount()
    {
        return _gatheredEnemy;
    }

    public void AddGatheredEnemy()
    {
        _gatheredEnemy++;
    }
}
