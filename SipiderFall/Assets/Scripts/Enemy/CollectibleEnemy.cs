using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class CollectibleEnemy : MonoBehaviour
{

    //Components
    Transform _player;
    Rigidbody2D _rb;
    Transform _transform;
    Collider2D _collider;

    //Attraction
    [SerializeField] float _attractionForce = 10;
    [SerializeField] float _maxDistance = 10;
    float _actualDistance;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = transform;
        _collider = GetComponentInChildren<Collider2D>();
        _player = Player.Instance.gameObject.transform;
    }

    private void FixedUpdate()
    {
        _actualDistance = Vector2.Distance(_player.position, _transform.position);
        _rb.AddForce((_player.position - _transform.position) * _attractionForce * Time.fixedDeltaTime);
        if(_actualDistance < _maxDistance)
        {
            _rb.AddForce(-_rb.velocity * Time.smoothDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.transform.parent.gameObject == Player.Instance.gameObject)
        {
            gameObject.transform.parent = collision.gameObject.transform;
            _rb.isKinematic = false;
            _rb.gravityScale = 0;
            Destroy(_collider);
        }
    }
}
