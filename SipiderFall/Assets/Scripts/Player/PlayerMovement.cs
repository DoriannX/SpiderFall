using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using CandyCoded.HapticFeedback;

public class PlayerMovement : MonoBehaviour
{

    UnityEngine.Gyroscope _gyro;
    Rigidbody2D _rb;
    Transform _transform;
    [SerializeField] float _tiltForce = 1;
    [SerializeField] float _jumpForce = 1;
    [SerializeField] float _maxVelocity = 1;
    [SerializeField] float _shotRange = 1;
    [SerializeField] float _radius = 1;
    [SerializeField] int _jumpMulti = 1;
    [SerializeField] float _playerDamage;
    float _groundRange = .1f;
    CinemachineImpulseSource _impuleSource;

    private void Awake()
    {
        _impuleSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        
        _gyro = Input.gyro;
        _gyro.enabled = true;
        _rb = GetComponentInChildren<Rigidbody2D>();
        _transform = transform;
        _groundRange += _transform.localScale.x / 2;
    }

    private void Update()
    {
        _rb.AddForce(new Vector3(Input.acceleration.x * _tiltForce, 0));
        Debug.DrawRay(_transform.position, Vector3.down * _shotRange, Color.green);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            HapticFeedback.LightFeedback();
            foreach (Transform child in _transform)
            {
                child.gameObject.layer = 2;
            }
            bool grounded = Tools.IsGrounded(gameObject, _groundRange);
            
            if (grounded)
            {
                _impuleSource.GenerateImpulse(new Vector3(.5f, .5f));
                Jump(_jumpForce * _jumpMulti);
            }
            else
                Jump(_jumpForce);

            
            if (!grounded)
            {
                Shot();
            }
        }
    }

    private void Shot()
    {
        RaycastHit2D shotHit = Physics2D.Raycast(_transform.position, Vector3.down * _shotRange, _shotRange);
        _impuleSource.GenerateImpulse(new Vector3(.05f, .05f));
        if (shotHit.collider)
        {
            _impuleSource.GenerateImpulse(new Vector3(.25f, .25f));
            if (shotHit.collider.transform.parent.TryGetComponent<DestructibleGround>(out DestructibleGround groundShot))
            {
                groundShot.DestroyGround(_radius);
            }
            if (shotHit.collider.transform.parent.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_playerDamage);
            }
        }

        foreach (Transform child in _transform)
        {
            child.gameObject.layer = 3;
        }
    }

    private void Jump(float force)
    {
        if (_rb.velocity.y < 0)
            _rb.velocity = new Vector3(_rb.velocity.x, 0);
        if (_rb.velocity.y <= _maxVelocity)
            _rb.AddForce(Vector3.up * force);
    }
}
