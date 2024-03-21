using UnityEngine;

public class EnemyLongRangePlayerDetecter : MonoBehaviour
{
    [HideInInspector] public bool PlayerDetected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject && collision.transform.parent.TryGetComponent<Player>(out Player player))
        {
            PlayerDetected = true;
        }
    }
}
