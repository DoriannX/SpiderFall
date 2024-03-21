using Cinemachine;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType {LongRangeEnemy, NormalEnemy, FlyingEnemy };
    //stats
    [SerializeField] float _maxHealth = 10;
    [SerializeField] Collider2D _playerDetecter;
    float _health;

    //components    
    CinemachineImpulseSource _impulseSource;
    Rigidbody2D _rb;
    CollectibleEnemy _collectibleEnemy;
    Collider2D _collider;
    EnemyFeedback _enemyFeedback;
    EnemyMovement _enemyMovement;
    EnemyAttack _enemyAttack;
    public EnemyType CurrentEnemyType;
    SpriteRenderer _sprite;



    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponentInChildren<Collider2D>();
        _collectibleEnemy = GetComponent<CollectibleEnemy>();
        _enemyFeedback = GetComponentInChildren<EnemyFeedback>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyAttack = GetComponent<EnemyAttack>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _health = _maxHealth;
        _collectibleEnemy.enabled = false;
    }
    public void TakeDamage(float damage)
    {
        _health -= damage;
        StartCoroutine(CheckLife());

    }

    IEnumerator CheckLife()
    {
        StartCoroutine(_enemyFeedback.ChangeSizeRenderer());
        yield return StartCoroutine(_enemyFeedback.ChangeSizeRenderer());
        if (_health <= 0)
        {
            Die();
        }

    }

    public void Die()
    {
        _collectibleEnemy.enabled = true;
        _sprite.transform.localScale = Vector3.one;
        Tools.SetLayer(gameObject, 0);
        Destroy(_playerDetecter);
        TutoManager.Instance.NextFeedback();
        _enemyFeedback.ResetSprite();
        TutoManager.Instance.ToggleArrowTuto(true);
        _impulseSource.GenerateImpulse(new Vector3(0, 1));
        Handheld.Vibrate();
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
