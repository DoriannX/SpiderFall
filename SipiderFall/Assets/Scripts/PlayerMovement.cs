using UnityEngine;
using UnityEngine.InputSystem;

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
    float _groundRange = .1f;
    

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
            foreach (Transform child in _transform)
            {
                child.gameObject.layer = 2;
            }
            RaycastHit2D hit = Physics2D.Raycast(_transform.position, Vector3.down * _groundRange, _groundRange);
            bool grounded = hit.collider && hit.transform.parent.TryGetComponent<DestructibleGround>(out DestructibleGround ground);
            
            if (grounded)
                Jump(_jumpForce * _jumpMulti);
            else
                Jump(_jumpForce);

            
            if (!grounded)
            {
                RaycastHit2D shotHit = Physics2D.Raycast(_transform.position, Vector3.down * _shotRange, _shotRange);
                if (shotHit.collider && shotHit.transform.parent.TryGetComponent<DestructibleGround>(out DestructibleGround groundShot))
                {
                    Shot(groundShot);
                }
            }
        }
    }

    private void Shot(DestructibleGround ground = null)
    {
        //make the shoot
        if (ground != null)
        {
            ground.DestroyGround(_radius);
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
