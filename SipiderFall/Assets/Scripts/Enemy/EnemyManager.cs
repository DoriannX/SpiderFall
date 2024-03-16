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
    [SerializeField] int _iterMax = 1;

    
 
    private void Awake()
    {
        _transform = transform;
        if(Instance == null)
            Instance = this;
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
            int i = 0;
            while (_enemyAmount < _maxEnemyAmount)
            {

                bool spawn = true;
                Vector3 randomPos = walls[(int)Random.Range(0, walls.Count - 1)].transform.position + Vector3.up;
                Collider2D[] hit = Physics2D.OverlapCircleAll(randomPos, .1f);

                foreach(Collider2D hit2d in hit)
                {
                    if(hit2d != null)
                    {
                        spawn = false;
                    }
                }
                if (spawn)
                {
                    _enemyList.Add(Instantiate(_enemy, randomPos, Quaternion.identity, _transform));
                    _enemyAmount++;
                }
                i++;
                if(i >= _iterMax)
                {
                    break;
                }
            }
        }
        else
            Debug.LogError("enemy is not in serialize field");
    }
}
