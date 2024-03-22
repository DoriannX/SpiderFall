using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    //stats
    [SerializeField] float _maxHealth = 100;
    float _health;

    //components
    Transform _transform;
    CinemachineImpulseSource _impulseSource;
    Rigidbody2D _rb;

    //Instance
    public static Player Instance;

    [SerializeField] float _damageImpulsionForce;

    public UnityEvent Died;
    SpriteRenderer _renderer;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        _transform = transform;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _health = _maxHealth;
        _renderer.material.SetColor("_Color", _renderer.color);
    }
    
    public float TakeDamage(float damage)
    {
        _renderer.material.SetFloat("_Health", _health / _maxHealth);
        _health -= damage;
        SFXManager.Instance.PlayHit();
        foreach(Transform child in _transform)
        {
            if(child.TryGetComponent<PlayerFeedback>(out PlayerFeedback playerFeedback))
            {
                _rb.AddForce(Vector3.up * _damageImpulsionForce);
                StartCoroutine(playerFeedback.Feedback());
                if (_impulseSource)
                    _impulseSource.GenerateImpulse(1);
                else
                    Debug.LogError("no impulse source in Player");
                break;
            }
        }


        if (_health <= 0) 
        {
            Die();
        }

        return _health;
    }

    private void Die()
    {
        Died?.Invoke();
        gameObject.SetActive(false);
    }

    public float RatioHealth()
    {
        return _health / _maxHealth;
    }
}
