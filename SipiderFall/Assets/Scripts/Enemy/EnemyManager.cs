using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{

    //Enemy
    private List<GameObject> _enemyList = new List<GameObject>();
    public int EnemyAmount;
    int _maxEnemyAmount;

    //Components
    Transform _transform;
    [SerializeField] GameObject _enemy;

    //while 
    [SerializeField] int _iterMax = 1;

    //Instance
    public static EnemyManager Instance;
    public bool IsTuto = true;
    public bool IsTutoDie = true;

    public UnityEvent EnemySpawned;



    private void Awake()
    {
        _transform = transform;
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _maxEnemyAmount = GameManager.Instance.Level.EnemyAmount;
    }

    public void ResetEnemies()
    {
        foreach(GameObject enemy in _enemyList.ToList())
        {
            Destroy(enemy);
            _enemyList.Remove(enemy);
        }
    }

    public void SpawnEnemies()
    {
        ResetEnemies();
        List<GameObject> walls = WallManager.Instance.GetBlocsMap();
        if (_enemy)
        {
            int i = 0;
            while (EnemyAmount < _maxEnemyAmount)
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
                    EnemyAmount++;
                }
                i++;
                if(i >= _iterMax)
                {
                    break;
                }
            }
            EnemySpawned.Invoke();
        }
        else
            Debug.LogError("enemy is not in serialize field");
    }
}
