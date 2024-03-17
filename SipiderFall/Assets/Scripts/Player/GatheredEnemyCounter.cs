using UnityEngine;

public class GatheredEnemyCounter : MonoBehaviour
{
    //The amount of enemy gathered
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
