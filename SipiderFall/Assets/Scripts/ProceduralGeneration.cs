using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    float _seed;
    [SerializeField] GameObject _tile;
    [SerializeField] GameObject _player;
    Transform _playerTransform;
    [SerializeField] GameObject _wall;
    List<Transform> _wallsTransform = new List<Transform>();
    Transform _transform;
    int[,] map;
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

        _seed = Random.Range(0, 100000);
        GenerationMap();
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
                    for (int x = 0; x < Camera.main.orthographicSize * 2; x++)
                    {
                        if (Mathf.PerlinNoise(x / 10f + _seed, y / 10f + _seed) >= .65f)
                        {
                            Instantiate(_tile, new Vector3(x - Camera.main.orthographicSize, y), Quaternion.identity, _transform);
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
