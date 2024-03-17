using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{

    //Components
    [SerializeField] GameObject _blocPrefab;
    [SerializeField] GameObject _player;
    [SerializeField] float _mapSize;
    Transform _transform;
    Transform _playerTransform;
    [SerializeField] Transform _finishLineTransform;

    //map
    float _seed;
    List<Transform> _wallsTransform = new List<Transform>();
    int[,] map;

    private void Awake()
    {
        _transform = transform; 
        if (_player)
            _playerTransform = _player.transform;
        else
            Debug.LogError("You forgot to put the player in the serialize field in ProceduralGeneration script");

    }
    private void Start()
    {

        CreateWalls();

        _seed = Random.Range(-100000, 100000);

        GenerationMap();

        EnemyManager.Instance.SpawnEnemies();
    }

    private void CreateWalls()
    {
        if (_blocPrefab)
        {
            _wallsTransform.Add(Instantiate(_blocPrefab, new Vector3(-Camera.main.orthographicSize / 2, 0), Quaternion.identity, _transform).transform);
            _wallsTransform.Add(Instantiate(_blocPrefab, new Vector3(Camera.main.orthographicSize / 2, 0), Quaternion.identity, _transform).transform);
        }
        else
            Debug.LogError("You forgot to put the wall in the serialize field in ProceduralGeneration script");
        
        foreach (Transform wall in _wallsTransform)
        {
            WallManager.Instance.AddWalls(wall.gameObject);
            if (wall.TryGetComponent<DestructibleGround>(out DestructibleGround destructibleWall))
                Destroy(destructibleWall);
            wall.localScale = new Vector3(wall.localScale.x, Camera.main.orthographicSize * Screen.width / Screen.height, wall.localScale.z);

        }
    }

    public void GenerationMap()
    {
        if (_player)
        {
            if (_blocPrefab)
            {
                for (int y = (int)_playerTransform.position.y; y > -_mapSize; y--)
                {
                    for (int x = 0; x < Camera.main.orthographicSize; x++)
                    {
                        if (Mathf.PerlinNoise(x / 10f + _seed, y / 10f + _seed) >= .65f)
                        {
                            WallManager.Instance.AddMapBlocs(Instantiate(_blocPrefab, new Vector3(x - Camera.main.orthographicSize / 2, y), Quaternion.identity, _transform));
                        }
                    }
                }
                if (_finishLineTransform)
                {
                    _finishLineTransform.localScale = new Vector3(Camera.main.orthographicSize * (Screen.width / Screen.height), _finishLineTransform.localScale.y);
                    _finishLineTransform.position = new Vector3(0, -_mapSize);
                }
                else
                    Debug.LogError("no finish line transform in ProceduralGeneration");
            }
            else
                Debug.LogError("You forgot to put the tile in the serialize field in ProceduralGeneration script");
        }
        else
            Debug.LogError("You forgot to put the player in the serialize field in ProceduralGeneration script");

    }

    
}
