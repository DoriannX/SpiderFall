using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Vector3 _direction = Vector3.right;
    EnemyBorderDetecter _borderDetecter;
    [SerializeField] float _maxVelocity = 1;
    [SerializeField] EnemyLongRangePlayerDetecter _longRangePlayerDetecter;
    bool _playerDetected = false;
    Rigidbody2D _rb;
    Enemy _enemy;
    Transform _transform;



    private void Awake()
    {
        _borderDetecter = GetComponent<EnemyBorderDetecter>();
        _rb = GetComponent<Rigidbody2D>();
        _enemy = GetComponent<Enemy>();
        _transform = transform;
    }

    private void Start()
    {
        if (_enemy.CurrentEnemyType == Enemy.EnemyType.FlyingEnemy)
            _longRangePlayerDetecter.PlayerDetectedEvent.AddListener(StartFollowPlayer);
    }
    private void FixedUpdate()
    {
        Patrol();
        FollowPlayer();
       
    }
    private void Update()
    {
        if(_enemy.CurrentEnemyType == Enemy.EnemyType.FlyingEnemy)
            _direction = _borderDetecter.DetectOnlyWall(_direction);
        else
            _direction = _borderDetecter.DetectBorderAndWall(_direction);
    }

    private void Patrol()
    {
        if(!_playerDetected || _enemy.CurrentEnemyType != Enemy.EnemyType.FlyingEnemy)
        {
            Vector2 targetVelocity = _direction * _maxVelocity;

            Vector3 velocityChange = targetVelocity - _rb.velocity;

            Vector3 acceleration = velocityChange / Time.fixedDeltaTime;

            acceleration = Vector3.ClampMagnitude(acceleration, _maxVelocity);

            _rb.AddForce(acceleration, ForceMode2D.Force);
        }
    }

    public void StartFollowPlayer()
    {
        _playerDetected = true;
    }

    private void FollowPlayer()
    {
        if (_playerDetected)
        {
            _transform.right = Player.Instance.transform.position - _transform.position;
            Vector2 targetVelocity = _transform.right * _maxVelocity;
            Vector3 velocityChange = targetVelocity - _rb.velocity;
            Vector3 acceleration = velocityChange / Time.fixedDeltaTime;
            acceleration = Vector3.ClampMagnitude(acceleration, _maxVelocity);
            _rb.AddForce(acceleration, ForceMode2D.Force);
        }
    }

    
}