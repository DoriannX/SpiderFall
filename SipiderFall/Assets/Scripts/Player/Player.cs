using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
     
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        _transform = transform;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _health = _maxHealth;
    }

    IEnumerator FixInputs()
    {

        yield return new WaitForSecondsRealtime(1);
    }
    
    public float TakeDamage(float damage)
    {
        
        _health -= damage;
        foreach(Transform child in _transform)
        {
            if(child.TryGetComponent<PlayerFeedback>(out PlayerFeedback playerFeedback))
            {
                _rb.AddForce(Vector3.up * _damageImpulsionForce * Time.deltaTime * 1000);
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
