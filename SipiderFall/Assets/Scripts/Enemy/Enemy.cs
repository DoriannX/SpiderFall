using Cinemachine;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] float _healthpoint = 10;
    [SerializeField] float _borderDetecterRange = .1f;
    [SerializeField] float _groundRange = .1f;
    [SerializeField] float _moveForce = 1;
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
        RaycastHit2D leftBorderDetecter = Physics2D.Raycast(_transform.position + new Vector3(0, -.6f, 0) + Vector3.left, Vector3.down * _borderDetecterRange, _borderDetecterRange);
        RaycastHit2D rightBorderDetecter = Physics2D.Raycast(_transform.position + new Vector3(0, -.6f, 0) + Vector3.right, Vector3.down * _borderDetecterRange, _borderDetecterRange);
        RaycastHit2D leftWallDetecter = Physics2D.Raycast(_transform.position + new Vector3(-.6f, 0, 0), Vector3.left * _borderDetecterRange, _borderDetecterRange);
        RaycastHit2D rightWallDetecter = Physics2D.Raycast(_transform.position + new Vector3(.6f, 0, 0), Vector3.right * _borderDetecterRange, _borderDetecterRange);

        Debug.DrawRay(_transform.position + new Vector3(-.6f, 0, 0), Vector3.left * _borderDetecterRange, Color.blue);
        Debug.DrawRay(_transform.position + new Vector3(.6f, 0, 0), Vector3.right * _borderDetecterRange, Color.blue);
        Debug.DrawRay(_transform.position + new Vector3(0, -.6f, 0) + Vector3.left, Vector3.down * _borderDetecterRange, Color.blue);
        Debug.DrawRay(_transform.position + new Vector3(0, -.6f, 0) + Vector3.right, Vector3.down * _borderDetecterRange, Color.blue);

        Vector3 direction = Vector3.right;
        if ((rightBorderDetecter.collider || leftWallDetecter.collider) && direction == Vector3.left)
        {
            direction = Vector3.right;
        }if((leftBorderDetecter.collider || rightWallDetecter.collider) && direction == Vector3.right)
        {
            direction = Vector3.left;
        }
        _rb.velocity = direction * _moveForce;

    }
}
