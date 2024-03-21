using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Vector3 _direction = Vector3.right;
    EnemyBorderDetecter _borderDetecter;
    [SerializeField] float _maxVelocity = 1;
    Rigidbody2D _rb;
    Enemy _enemy;

    private void Awake()
    {
        _borderDetecter = GetComponent<EnemyBorderDetecter>();
        _rb = GetComponent<Rigidbody2D>();
        _enemy = GetComponent<Enemy>();
    }
    private void FixedUpdate()
    {
        Patrol();
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
        Vector2 targetVelocity = _direction * _maxVelocity;

        Vector3 velocityChange = targetVelocity - _rb.velocity;

        Vector3 acceleration = velocityChange / Time.fixedDeltaTime;

        acceleration = Vector3.ClampMagnitude(acceleration, _maxVelocity);

        _rb.AddForce(acceleration, ForceMode2D.Force);
    }
}