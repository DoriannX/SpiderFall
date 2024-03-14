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
    float _groundRange = .1f;
    

    private void Start()
    {
        _gyro = Input.gyro;
        _gyro.enabled = true;
        _rb = GetComponentInChildren<Rigidbody2D>();
        _transform = transform;
    }

    private void Update()
    {
        _rb.AddForce(new Vector3(Input.acceleration.x * _tiltForce, 0));
        Debug.DrawRay(_transform.position, Vector3.down * _shotRange, Color.green);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        //if (context.started)
        {
            print(context);
            Jump();

            foreach (Transform child in _transform)
            {
                child.gameObject.layer = 2;
            }

            RaycastHit2D hit = Physics2D.Raycast(_transform.position, Vector3.down * _groundRange, 1);
            bool grounded = hit.collider && hit.transform.parent.TryGetComponent<Ground>(out Ground ground);

            if (!grounded)
            {
                RaycastHit2D shotHit = Physics2D.Raycast(_transform.position, Vector3.down * _shotRange, _shotRange);
                if (shotHit.collider && shotHit.transform.parent.TryGetComponent<Ground>(out Ground groundShot))
                {
                    Shot(groundShot);
                }
            }
        }
    }

    private void Shot(Ground ground = null)
    {
        //make the shoot
        if (ground != null)
        {
            ground.DestroyGround();
        }
        foreach (Transform child in _transform)
        {
            child.gameObject.layer = 3;
        }
    }

    private void Jump()
    {
        if (_rb.velocity.y < 0)
            _rb.velocity = new Vector3(_rb.velocity.x, 0);
        if (_rb.velocity.y <= _maxVelocity)
            _rb.AddForce(Vector3.up * _jumpForce);
    }
}
