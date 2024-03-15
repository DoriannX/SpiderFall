using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{

    //Components
    [SerializeField] GameObject _tile;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _wall;
    Transform _transform;
    Transform _playerTransform;

    //map
    float _seed;
    List<Transform> _wallsTransform = new List<Transform>();
    int[,] map;

    //enemies
    [SerializeField] int _enemyAmount;

    private void Awake()
    {
        _transform = transform;

    }
    private void Start()
    {
        if (_player)
            _playerTransform = _player.transform;
        else
            Debug.LogError("You forgot to put the player in the serialize field in ProceduralGeneration script");
        if (_wall)
        {
            _wallsTransform.Add(Instantiate(_wall, new Vector3(-Camera.main.orthographicSize / 2, 0), Quaternion.identity, _transform).transform);
            _wallsTransform.Add(Instantiate(_wall, new Vector3(Camera.main.orthographicSize / 2, 0), Quaternion.identity, _transform).transform);
        }
        else
            Debug.LogError("You forgot to put the wall in the serialize field in ProceduralGeneration script");

        _seed = Random.Range(-100000, 100000);

        GenerationMap();
        EnemyManager.Instance.SpawnEnemies();
    }

    private void FixedUpdate()
    {
        WallFollowPlayer();
    }

    public void GenerationMap()
    {
        if (_player)
        {
            if (_tile)
            {
                for (int y = (int)_playerTransform.position.y; y > -1000; y--)
                {
                    for (int x = 0; x < Camera.main.orthographicSize; x++)
                    {
                        if (Mathf.PerlinNoise(x / 10f + _seed, y / 10f + _seed) >= .65f)
                        {
                            Instantiate(_tile, new Vector3(x - Camera.main.orthographicSize / 2, y), Quaternion.identity, _transform);
                        }
                    }
                }
            }
            else
                Debug.LogError("You forgot to put the tile in the serialize field in ProceduralGeneration script");
        }
        else
            Debug.LogError("You forgot to put the player in the serialize field in ProceduralGeneration script");
    }

    private void WallFollowPlayer()
    {
        if (_wall)
        {
            foreach (Transform wall in _wallsTransform)
            {
                wall.position = new Vector3(wall.position.x, _playerTransform.position.y);
            }
        }
        else
            Debug.LogError("You forgot to put the wall in the serialize field in ProceduralGeneration script");
    }
}
