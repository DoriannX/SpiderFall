using CandyCoded.HapticFeedback;
using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    [SerializeField] float _tiltForce = 1;
    [SerializeField] float _maxFallSpeed = 10;
    [SerializeField] float _fallAcceleration = 1;
    [SerializeField] float _groundStopFallTime;
    [SerializeField] float _airStopFallTime;
    float accelerationX;
    bool _jumping = false;
    public float JumpForce = 1;
    public int JumpMulti = 1;
    float _velocityX;

    //Component
    Rigidbody2D _rb;
    Transform _transform;
    CinemachineImpulseSource _impulseSource;
    PlayerAttack _playerAttack;
    [SerializeField] Transform _tutoBlock;
    PlayerFeedback _feedBack;
    Coroutine stopFallCoroutine;

    public bool CanMove = true;

    //ground
    float _groundRange = .1f;

    public static PlayerMovement Instance;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _playerAttack = GetComponent<PlayerAttack>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _rb = GetComponentInChildren<Rigidbody2D>();
        _transform = transform;
        _feedBack = GetComponentInChildren<PlayerFeedback>();
    }

    private void Start()
    {
        _groundRange += _transform.localScale.x / 2;
        SFXManager.Instance.PlayFall();
    }

    IEnumerator StopFall(float time)
    {
        _jumping = true;
        yield return new WaitForSeconds(time);
        _jumping = false;
    }
    public void StartStopFall(float time)
    {
        if (stopFallCoroutine != null)
        {
            StopCoroutine(stopFallCoroutine);
        }
        stopFallCoroutine = StartCoroutine(StopFall(time));
    }

    private void Update()
    {
        accelerationX = Input.acceleration.x;
        CheckGroundedForSong();
    }
    bool wasGroundedLastFrame;
    private void CheckGroundedForSong()
    {
        bool isGroundedThisFrame = Tools.IsGrounded(gameObject, _groundRange);

        if (isGroundedThisFrame != wasGroundedLastFrame)
        {
            if (!isGroundedThisFrame)
            {
                SFXManager.Instance.PlayFall();
            }
            else
            {
                SFXManager.Instance.StopFall();
            }
        }

        wasGroundedLastFrame = isGroundedThisFrame;
    }
    private void FixedUpdate()
    {
        if (CanMove)
        {
            float movement = _velocityX * Time.fixedDeltaTime * 500;
            _rb.velocity = new Vector3(movement, _rb.velocity.y);
            if (Mathf.Abs(accelerationX) > 0)
            {
                movement = accelerationX * _tiltForce * Time.fixedDeltaTime * 100;
                _rb.velocity = new Vector3(movement, _rb.velocity.y);
            }
        }
        if (!_jumping && !Tools.IsGrounded(gameObject, _groundRange))
        {
            if (Mathf.Abs(_rb.velocity.y) < _maxFallSpeed)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - _fallAcceleration * Time.fixedDeltaTime);
            }
        }
    }

    public void MoveDebug(InputAction.CallbackContext ctx)
    {
        if (CanMove)
            _velocityX = ctx.ReadValue<Vector2>().x;
    }

    void Shot()
    {
        HapticFeedback.LightFeedback();
        bool grounded = Tools.IsGrounded(gameObject, _groundRange);

        if (grounded)
        {
            StartStopFall(_groundStopFallTime);
            Jump(JumpForce * JumpMulti);
        }
        else
        {
            Jump(0);
            StartStopFall(_airStopFallTime);
        }


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
            Shot();
    }

    public void Jump(float force)
    {
        StartCoroutine(_feedBack.Squeeze());
        SFXManager.Instance.PlayJumpSFX();
        _rb.velocity = new Vector3(_rb.velocity.x, 0);
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