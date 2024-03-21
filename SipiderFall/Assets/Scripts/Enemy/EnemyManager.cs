using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{

    //Enemy
    public int EnemyAmount;

    //Components
    Transform _transform;
    [SerializeField] GameObject _enemy;
    [SerializeField] GameObject _longRangeEnemy;
    [SerializeField] GameObject _flyingEnemy;

    //while 
    [SerializeField] int _iterMax = 1;

    //Instance
    public static EnemyManager Instance;

    [HideInInspector] public UnityEvent EnemySpawned;

    private void Awake()
    {
        _transform = transform;
        if(Instance == null)
            Instance = this;

    }

    public void SpawnEnemy(Enemy.EnemyType typeToSpawn)
    {
        int maxAmount = 0;
        GameObject enemyToSpawn = null;
        switch (typeToSpawn)
        {
            case Enemy.EnemyType.NormalEnemy: 
                maxAmount = GameManager.Instance.Level.EnemyAmount;
                enemyToSpawn = _enemy;
                break;
            case Enemy.EnemyType.LongRangeEnemy:
                maxAmount = GameManager.Instance.Level.LongRangeEnemyAmount;
                enemyToSpawn = _longRangeEnemy;
                break;
            case Enemy.EnemyType.FlyingEnemy:
                maxAmount = GameManager.Instance.Level.FlyingEnemyAmount;
                enemyToSpawn = _flyingEnemy;
                break;
        }
        List<GameObject> walls = WallManager.Instance.GetBlocsMap();
        if (enemyToSpawn)
        {
            for (int i = 0; i < maxAmount; i++)
            {
                bool spawn = true;
                int j = 0;
                while (spawn)
                {
                    Vector3 randomPos = Vector3.zero;
                    if (typeToSpawn != Enemy.EnemyType.FlyingEnemy)
                        randomPos = walls[Random.Range(0, walls.Count - 1)].transform.position + Vector3.up;
                    else
                        randomPos = new Vector3Int((int)Random.Range(-Tools.GetScreenSize().x / 2, Tools.GetScreenSize().x/2), Random.Range((int)Player.Instance.transform.position.y - (int)(Tools.GetScreenSize().y / 2), -GameManager.Instance.Level.MapSize));
                    Collider2D[] hits = Physics2D.OverlapCircleAll(randomPos, .1f);
                    if (hits.Length <= 0)
                    {
                        Instantiate(enemyToSpawn, randomPos, Quaternion.identity, _transform);
                        EnemyAmount++;
                        spawn = false;
                    }
                    j++;
                    if (j > _iterMax)
                    {
                        print("failed");
                        break;
                    }
                }


            }
        }
        else
            Debug.LogError("No long range enemy in EnemyManager");
    }

    public void SpawnEnemies()
    {
        if (_enemy)
        {
            SpawnEnemy(Enemy.EnemyType.NormalEnemy);
            SpawnEnemy(Enemy.EnemyType.LongRangeEnemy);
            SpawnEnemy(Enemy.EnemyType.FlyingEnemy);
            EnemySpawned?.Invoke();
        }
        else
            Debug.LogError("enemy is not in serialize field");
    }

    
}
