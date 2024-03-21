using UnityEngine;

public class ProjectileObstacleDetecter : MonoBehaviour
{
    EnemyAttack _enemyAttack;

    private void Awake()
    {
        _enemyAttack = GetComponentInParent<EnemyAttack>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject && collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(_enemyAttack.EnemyDamage);
        }
        if(!collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            Destroy(gameObject);
    }
}
