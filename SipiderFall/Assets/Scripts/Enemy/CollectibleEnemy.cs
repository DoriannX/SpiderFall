using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class CollectibleEnemy : MonoBehaviour
{

    //Components
    Transform _playerTransform;
    Rigidbody2D _rb;
    Transform _transform;
    Collider2D _collider;
    [SerializeField] LineController _rope;

    //Attraction
    bool _gathered = false;
    [SerializeField] float _ropeSize;
    [SerializeField] float _enemyMass;
    [SerializeField] LayerMask _notTouchingDeadEnemies;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = transform;
        _collider = GetComponentInChildren<Collider2D>();
        _playerTransform = Player.Instance.gameObject.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!_gathered && collision.gameObject.transform.parent.gameObject == Player.Instance.gameObject)
        {
            _collider.isTrigger = false;
            _rb.isKinematic = false;
            _rb.excludeLayers = _notTouchingDeadEnemies;
            _rb.mass = _enemyMass;
            _rb.gravityScale = 10f;
            _gathered=true;

            if(_playerTransform.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                DistanceJoint2D joint = gameObject.AddComponent<DistanceJoint2D>();
                joint.connectedBody = rb;
                joint.autoConfigureDistance = false;
                joint.distance = _ropeSize;
                joint.maxDistanceOnly = true;
            }
            if (_rope)
            {
                LineController rope = Instantiate(_rope, _playerTransform);
                rope.SetUpLine(new Transform[] { _playerTransform, _transform });
            }
            else
                Debug.LogError("not rope in CollectibleEnemy");
            if(_playerTransform.gameObject.TryGetComponent<GatheredEnemyCounter>(out GatheredEnemyCounter gatheredEnemyCounter)){
                gatheredEnemyCounter.AddGatheredEnemy();
            }
        }
    }
}
