using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    [SerializeField] EnemyPlayerDetecter _enemyPlayerDetecter;
    public float EnemyDamage = 10;
    [SerializeField] float _rangeShotFrequency;
    [SerializeField] float _shootSpeed;
    [SerializeField] GameObject _projectile;
    [SerializeField] EnemyLongRangePlayerDetecter _enemyLongRangePlayerDetecter;

    [SerializeField] GameObject _explosionParticle;
    Enemy _enemy;
    EnemyFeedback _enemyFeedback;

    float _longRangeTimer;
    Transform _transform;
    public Coroutine Coroutine;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _transform = transform;
        _enemyFeedback = GetComponentInChildren<EnemyFeedback>();
    }

    private void Start()
    {
        if (_enemyPlayerDetecter)
            _enemyPlayerDetecter.PlayerDetected.AddListener(Attack);
        else
            Debug.LogError("no enemyPlayerDetecter on enemy");
        _longRangeTimer = _rangeShotFrequency;
    }

    private void Update()
    {
        LongRangeAttack();
    }
    public void Attack()
    {
        List<GameObject> detectPlayer = _enemyPlayerDetecter.DetectedPlayer;

        foreach (GameObject player in detectPlayer.ToList())  //detectPlayer is being modified in the foreach so it has to be a copy of the list
        {
            if (player.TryGetComponent<Player>(out Player playerScript))
            {
                playerScript.TakeDamage(EnemyDamage);
            }
        }
    }

    public void LongRangeAttack()
    {
        if (_enemy)
        {
            if(Player.Instance.gameObject.activeSelf)
            {  
                if (_enemy.CurrentEnemyType == Enemy.EnemyType.LongRangeEnemy && _enemyLongRangePlayerDetecter.PlayerDetected)
                {
                    _longRangeTimer -= Time.deltaTime;
                    if (_longRangeTimer <= 0)
                    {
                        Vector3 shotDirection = (Player.Instance.transform.position - _transform.position).normalized;
                        if (_projectile)
                        {
                            GameObject projectileSpawned = Instantiate(_projectile, _transform);
                            projectileSpawned.transform.right = Player.Instance.transform.position - transform.position;
                            if (projectileSpawned.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                            {
                                SFXManager.Instance.PlayShotSFX(gameObject);
                                Coroutine = StartCoroutine(_enemyFeedback.Squeeze());
                                GameObject particle = Instantiate(_explosionParticle, _transform);
                                Destroy(particle, 1);
                                particle.transform.right = shotDirection;
                                foreach (Transform child in particle.transform)
                                {
                                    child.GetComponent<ParticleSystem>().Play();
                                }
                                rb.velocity = shotDirection * _shootSpeed;
                            }
                            else
                                Debug.LogError("No rigidbody on projectile");
                        }
                        else
                            Debug.LogError("no projectile in enemy attack");
                        _longRangeTimer = _rangeShotFrequency;
                    }
                }
                else
                    _longRangeTimer = _rangeShotFrequency;
            }
        }
        else
            Debug.LogError("no enemy on enemyAttack");
    }
}