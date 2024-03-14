using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private List<GameObject> _enemyList = new List<GameObject>();
    Transform _transform;
    public static EnemyManager Instance;

    [SerializeField] GameObject _enemy;
    int _enemyAmount;
    [SerializeField] int _maxEnemyAmount;
    [SerializeField] float _enemySpawnRate;
    [SerializeField] GameObject _wallParent;
 
    private void Awake()
    {
        _transform = transform;
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        SpawnEnemies();
    }
    public void SpawnEnemies()
    {
        List<GameObject> walls  = new List<GameObject>();
        if (_wallParent)
        {
            foreach (Transform wall in _wallParent.transform)
            {
                walls.Add(wall.gameObject);
            }
        }
        else
            Debug.LogError("You forgot to put wallParent in EnemyManager");
        
        if (_enemy)
        {
            while (_enemyAmount < _maxEnemyAmount)
            {
                if (Random.Range(0, 1) < _enemySpawnRate / 100)
                {
                    print(_enemySpawnRate / 100);
                    if (_enemyAmount < _maxEnemyAmount)
                    {
                        _enemyList.Add(Instantiate(_enemy, walls[(int)Random.Range(0, walls.Count)].transform.position + Vector3.up, Quaternion.identity, _transform));
                        _enemyAmount++;
                    }
                }
            }
        }
        else
            Debug.LogError("enemy is not in serialize field");
    }
}
