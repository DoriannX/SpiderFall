using CandyCoded.HapticFeedback;
using Cinemachine;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    [SerializeField] float _tiltForce = 1;
    [SerializeField] float _maxVelocity = 1;
    public float JumpForce = 1;
    public int JumpMulti = 1;

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
    }

    private void Start()
    {
        
        _rb = GetComponentInChildren<Rigidbody2D>();
        _transform = transform;
        _groundRange += _transform.localScale.x / 2;
    }

    private void Update()
    {
        _rb.AddForce(new Vector3(Input.acceleration.x * _tiltForce, 0));
    }

    void StartMove()
    {
        HapticFeedback.LightFeedback();
        foreach (Transform child in _transform)
        {
            child.gameObject.layer = 2;
        }
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
                _playerAttack.Shot();
            else
                Debug.LogError("No playerAttack in PlayerMovement");
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartMove();
        }
    }

    public void Jump(float force)
    {
        if (_rb.velocity.y < 0)
            _rb.velocity = new Vector3(_rb.velocity.x, 0);
        if (_rb.velocity.y <= _maxVelocity)
            _rb.AddForce(Vector3.up * force);
    }
}
