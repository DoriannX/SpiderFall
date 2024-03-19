using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{

    //Components
    [SerializeField] GameObject _blocPrefab;
    float _mapSize;
    Transform _transform;
    Transform _playerTransform;
    [SerializeField] Transform _finishLineTransform;

    //map
    float _seed;
    List<Transform> _wallsTransform = new List<Transform>();
    int[,] map;

    public static ProceduralGeneration Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        _transform = transform;

        

    }
    private void Start()
    {
        _mapSize = GameManager.Instance.Level.MapSize;

        _playerTransform = Player.Instance.gameObject.transform;
        _seed = Random.Range(-100000, 100000);

        GenerationMap();
        CreateWalls();
    }

    public float GetMapSize()
    {
        return _mapSize;
    }

    private void CreateWalls()
    {
        if (_blocPrefab)
        {
            _wallsTransform.Add(Instantiate(_blocPrefab, new Vector3((int) Tools.GetScreenSize().x / 2+1, -_mapSize/2), Quaternion.identity, _transform).transform);
            _wallsTransform.Add(Instantiate(_blocPrefab, new Vector3((int)-Tools.GetScreenSize().x / 2-1, -_mapSize/2), Quaternion.identity, _transform).transform);
        }
        else
            Debug.LogError("You forgot to put the wall in the serialize field in ProceduralGeneration script");
        
        foreach (Transform wall in _wallsTransform)
        {
            WallManager.Instance.AddWalls(wall.gameObject);
            if (wall.TryGetComponent<DestructibleGround>(out DestructibleGround destructibleWall))
                Destroy(destructibleWall);
            wall.localScale = new Vector3(wall.localScale.x, _mapSize + Tools.GetScreenSize().x*2, wall.localScale.z);

        }
    }

    public void GenerationMap()
    {
        WallManager.Instance.ResetMap();
        if (_blocPrefab)
        {
            for (int y = (int)_playerTransform.position.y - (int)(Tools.GetScreenSize().y/2); y > -_mapSize - (int)(Tools.GetScreenSize().y/2); y--)
            {
                for (int x = 0; x < Camera.main.orthographicSize; x++)
                {
                    if (Mathf.PerlinNoise(x / 10f + _seed, y / 10f + _seed) >= .65f)
                    {
                        WallManager.Instance.AddMapBlocs(Instantiate(_blocPrefab, new Vector3((int)x - (int)Tools.GetScreenSize().x / 2, y), Quaternion.identity, _transform));
                    }
                }
            }
            if (_finishLineTransform)
            {
                _finishLineTransform.localScale = new Vector3(Tools.GetScreenSize().x, _finishLineTransform.localScale.y);
                _finishLineTransform.position = new Vector3(0, -_mapSize - (int)(Tools.GetScreenSize().y / 2));
            }
            else
                Debug.LogError("no finish line transform in ProceduralGeneration");
            EnemyManager.Instance.SpawnEnemies();
        }
        else
            Debug.LogError("You forgot to put the tile in the serialize field in ProceduralGeneration script");

    }

    
}
