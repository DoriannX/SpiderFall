using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetecter : MonoBehaviour
{
    //Objects
    [HideInInspector] public List<GameObject> DetectedObject;

    //Components
    [SerializeField] PlayerAttack _playerAttack;
    Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        if (_playerAttack)
        {
            _transform.position = _playerAttack.transform.position + Vector3.down / 2 + Vector3.down * _playerAttack.ShotRange / 2;
            _transform.localScale = new Vector3(1, _playerAttack.ShotRange * _playerAttack.transform.localScale.y * 2);
        }
        else
            Debug.LogError("No player attack in ObstacleDetected");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DetectedObject.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (DetectedObject.Contains(collision.gameObject))
        {
            DetectedObject.Remove(collision.gameObject);
        }
    }
}
