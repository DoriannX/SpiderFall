using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    [SerializeField] EnemyPlayerDetecter _enemyPlayerDetecter;
    [SerializeField] float _enemyDamage = 10;

    private void Start()
    {
        if (_enemyPlayerDetecter)
            _enemyPlayerDetecter.PlayerDetected.AddListener(Attack);
        else
            Debug.LogError("no enemyPlayerDetecter on enemy");
    }
    public void Attack()
    {
        List<GameObject> detectPlayer = _enemyPlayerDetecter.DetectedPlayer;

        foreach (GameObject player in detectPlayer.ToList())  //detectPlayer is being modified in the foreach so it has to be a copy of the list
        {
            if (player.TryGetComponent<Player>(out Player playerScript))
            {
                playerScript.TakeDamage(_enemyDamage);
            }
        }
    }
}