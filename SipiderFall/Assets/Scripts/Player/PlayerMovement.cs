using CandyCoded.HapticFeedback;
using Cinemachine;
using System.Collections;
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
    [SerializeField] Transform _tutoBlock;

    public bool CanMove = false;

    //ground
    float _groundRange = .1f;

    bool _isTuto = true;

    public static PlayerMovement Instance;
    

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

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
        if (CanMove && (Mathf.Abs(Input.acceleration.x) > 0))
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
        if (_tutoBlock)
        {
            if (_isTuto && _transform.position.y < _tutoBlock.position.y)
            {
                _isTuto = false;
                StartCoroutine(Dialogue.Instance.FinishTuto());

            }
        }
        else
            Debug.LogError("no tutoblock in Player movement");

    }

    

    public void MoveDebug(InputAction.CallbackContext ctx)
    {
        if(CanMove)
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
