using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    //stats
    [SerializeField] float _maxHealth = 100;
    float _health;

    //components
    Transform _transform;
    CinemachineImpulseSource _impulseSource;

    //Instance
    public static Player Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        _transform = transform;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        _health = _maxHealth;
    }
    
    public float TakeDamage(float damage)
    {
        
        _health -= damage;
        foreach(Transform child in _transform)
        {
            if(child.TryGetComponent<PlayerFeedback>(out PlayerFeedback playerFeedback))
            {
                StartCoroutine(playerFeedback.Feedback());
                if (_impulseSource)
                    _impulseSource.GenerateImpulse(10);
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
        gameObject.SetActive(false);
    }

    public float RatioHealth()
    {
        return _health / _maxHealth;
    }
}
