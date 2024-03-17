using CandyCoded.HapticFeedback;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    [SerializeField] float _tiltForce = 1;
    [SerializeField] float _maxVelocity = 1;
    public float JumpForce = 1;
    public int JumpMulti = 1;
    float _velocityX;

    //Component
    Rigidbody2D _rb;
    Transform _transform;
    CinemachineImpulseSource _impulseSource;
    PlayerAttack _playerAttack;

    //ground
    float _groundRange = .1f;

    

    private void Awake()
    {
        _playerAttack = GetComponent<PlayerAttack>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _rb = GetComponentInChildren<Rigidbody2D>();
        _transform = transform;
    }

    private void Start()
    {
        _groundRange += _transform.localScale.x / 2;
    }

    private void Update()
    {
        if ((Mathf.Abs(Input.acceleration.x) > 0))
        {
            _velocityX = Input.acceleration.x * _tiltForce;
        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_rb.velocity.x) < _maxVelocity || Mathf.Sign(_velocityX) != Mathf.Sign(_rb.velocity.x))
        {
            _rb.AddForce(new Vector3(_velocityX * Time.fixedDeltaTime * 1000, 0));
        }
        else
        {
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxVelocity, _rb.velocity.y);
        }
    }

    public void MoveDebug(InputAction.CallbackContext ctx)
    {
        _velocityX = ctx.ReadValue<Vector2>().x;
    }

    void Shot()
    {
        HapticFeedback.LightFeedback();
        bool grounded = Tools.IsGrounded(gameObject, _groundRange);

        if (grounded)
        {
            if (_impulseSource)
                _impulseSource.GenerateImpulse(new Vector3(0, .5f));
            else
                Debug.LogError("No impulseSource in Player");
            Jump(JumpForce * JumpMulti);
        }
        else
            Jump(JumpForce);


        if (!grounded)
        {
            if (_playerAttack)
                _playerAttack.DistanceAttack();
            else
                Debug.LogError("No playerAttack in PlayerMovement");
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Shot();
        }
    }

    public void Jump(float force)
    {
        if (_rb.velocity.y < 0)
            _rb.velocity = new Vector3(_rb.velocity.x, 0);
        if (_rb.velocity.y <= _maxVelocity)
            _rb.AddForce(Vector3.up * force);
    }

    public void JumpDebug(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Shot();
        }
    }
}
