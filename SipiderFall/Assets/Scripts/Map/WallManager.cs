using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    //map
    List<GameObject> _mapBlocs = new List<GameObject>();
    List<GameObject> _walls = new List<GameObject>();

    //Instance
    public static WallManager Instance;

    //player
    Transform _playerTransform;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _playerTransform = Player.Instance.gameObject.transform;
    }

    public void AddMapBlocs(GameObject mapBloc)
    {
        _mapBlocs.Add(mapBloc);
    }

    public void AddWalls(GameObject wall)
    {
        _walls.Add(wall);
    }

    public List<GameObject> GetBlocsMap()
    {
        return _mapBlocs;
    }
    private void FixedUpdate()
    {
        //WallFollowPlayer();
    }

    private void WallFollowPlayer()
    {
        foreach (GameObject wall in _walls)
        {
            wall.transform.position = new Vector3(wall.transform.position.x, _playerTransform.position.y);
        }
    }
}