using Cinemachine;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] float _healthpoint = 10;
    [SerializeField] float _borderDetecterRange = .1f;
    [SerializeField] float _groundRange = .1f;
    [SerializeField] float _moveForce = 1;
    [SerializeField] float _maxVelocity = 1;
    Vector3 _direction = Vector3.right;
    CinemachineImpulseSource _impuleSource;
    Transform _transform;
    Rigidbody2D _rb;


    private void Awake()
    {
        _impuleSource = GetComponent<CinemachineImpulseSource>();
        _transform = transform;
        _rb = GetComponent<Rigidbody2D>();
    }
    public void TakeDamage(float damage)
    {
        print(damage);
        _healthpoint -= damage;
        if( _healthpoint <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        _impuleSource.GenerateImpulse(new Vector3(1, 1));

        Handheld.Vibrate();
        Destroy(gameObject);
    }

    private void Update()
    {
        Patrol();
    }

    public void Patrol()
    {
        RaycastHit2D leftBorderDetecter = Physics2D.Raycast(_transform.position + new Vector3(0, -.6f, 0) + Vector3.left/2, Vector3.down * _borderDetecterRange, _borderDetecterRange);
        RaycastHit2D rightBorderDetecter = Physics2D.Raycast(_transform.position + new Vector3(0, -.6f, 0) + Vector3.right/2, Vector3.down * _borderDetecterRange, _borderDetecterRange);
        RaycastHit2D leftWallDetecter = Physics2D.Raycast(_transform.position + new Vector3(-.6f, 0, 0), Vector3.left * _borderDetecterRange, _borderDetecterRange);
        RaycastHit2D rightWallDetecter = Physics2D.Raycast(_transform.position + new Vector3(.6f, 0, 0), Vector3.right * _borderDetecterRange, _borderDetecterRange);

        Debug.DrawRay(_transform.position + new Vector3(-.6f, 0, 0), Vector3.left * _borderDetecterRange, Color.blue);
        Debug.DrawRay(_transform.position + new Vector3(.6f, 0, 0), Vector3.right * _borderDetecterRange, Color.blue);
        Debug.DrawRay(_transform.position + new Vector3(0, -.6f, 0) + Vector3.left/2, Vector3.down * _borderDetecterRange, Color.blue);
        Debug.DrawRay(_transform.position + new Vector3(0, -.6f, 0) + Vector3.right/2, Vector3.down * _borderDetecterRange, Color.blue);

        


        if (_direction == Vector3.left && (!leftBorderDetecter.collider || leftWallDetecter.collider))
        {
            _direction = Vector3.right;
        }if (_direction == Vector3.right && (!rightBorderDetecter.collider || rightWallDetecter.collider))
        {
            _direction = Vector3.left;
        }
        if (_rb.velocity.x < _maxVelocity)
            _rb.AddForce(_direction * _moveForce);
        else
            _rb.velocity = new Vector3(_maxVelocity, _rb.velocity.y);

    }
}
