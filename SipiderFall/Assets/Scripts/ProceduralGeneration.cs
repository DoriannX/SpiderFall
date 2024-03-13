using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] float seed;
    [SerializeField] GameObject _tile;
    [SerializeField] GameObject _player;
    Transform _playerTransform;
    [SerializeField] GameObject _wall;
    List<Transform> _wallsTransform = new List<Transform>();
    int[,] map;

    private void Start()
    {
        _playerTransform = _player.transform;
        _wallsTransform.Add(Instantiate(_wall, new Vector3(-Camera.main.orthographicSize/2, 0), Quaternion.identity).transform);
        _wallsTransform.Add(Instantiate(_wall, new Vector3(Camera.main.orthographicSize/2, 0), Quaternion.identity).transform);
        seed = Random.Range(0, 100000);
        GenerationMap();
    }

    private void FixedUpdate()
    {
        WallFollowPlayer();
    }

    public void GenerationMap()
    {
        for (int y = (int)_playerTransform.position.y; y > -1000; y--)
        {
            for (int x = 0; x < Camera.main.orthographicSize*2; x++)
            {
                if(Mathf.PerlinNoise(x/10f + seed, y/10f + seed) >= .65f)
                {
                    Instantiate(_tile, new Vector3(x- Camera.main.orthographicSize, y), Quaternion.identity);
                }
            }
        }
    }

    private void WallFollowPlayer()
    {
        foreach(Transform wall in _wallsTransform)
        {
            wall.position = new Vector3(wall.position.x, _playerTransform.position.y);
        }
    }
}
