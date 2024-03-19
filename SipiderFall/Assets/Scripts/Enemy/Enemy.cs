using Cinemachine;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //stats
    [SerializeField] float _maxHealth = 10;
    float _health;

    //components    
    CinemachineImpulseSource _impulseSource;
    Rigidbody2D _rb;
    CollectibleEnemy _collectibleEnemy;
    Collider2D _collider;
    EnemyFeedback _enemyFeedback;
    EnemyMovement _enemyMovement;
    EnemyAttack _enemyAttack;



    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponentInChildren<Collider2D>();
        _collectibleEnemy = GetComponent<CollectibleEnemy>();
        _enemyFeedback = GetComponentInChildren<EnemyFeedback>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyAttack = GetComponent<EnemyAttack>();
    }

    private void Start()
    {
        _health = _maxHealth;
        _collectibleEnemy.enabled = false;
    }
    public void TakeDamage(float damage)
    {
        _health -= damage;
        StartCoroutine(_enemyFeedback.ChangeSizeRenderer());
        if (_health <= 0)
        {
            Die();
        }

    }

    public void Die()
    {
        _enemyFeedback.ResetSprite();
        TutoManager.Instance.ToggleArrowTuto(true);
        _impulseSource.GenerateImpulse(new Vector3(0, 1));
        Handheld.Vibrate();
        _collectibleEnemy.enabled = true;
        _collider.isTrigger = true;

        _rb.velocity = Vector2.zero;
        _rb.isKinematic = true;
        Destroy(_enemyMovement);
        Destroy(_enemyAttack);
        Destroy(this);
    }

    public float GetHealthRatio()
    {
        return _health / _maxHealth;
    }

    
}
