using UnityEngine;

public class GatheredEnemyCounter : MonoBehaviour
{
    int _gatheredEnemy = 0;

    public int GetGatheredEnemyAmount()
    {
        return _gatheredEnemy;
    }

    public void AddGatheredEnemy()
    {
        _gatheredEnemy++;
    }
}
