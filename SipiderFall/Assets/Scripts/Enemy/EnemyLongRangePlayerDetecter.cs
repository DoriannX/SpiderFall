using UnityEngine;
using UnityEngine.Events;

public class EnemyLongRangePlayerDetecter : MonoBehaviour
{
    [HideInInspector] public bool PlayerDetected = false;
    public UnityEvent PlayerDetectedEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject && collision.transform.parent.TryGetComponent<Player>(out Player player))
        {
            PlayerDetected = true;
            PlayerDetectedEvent.Invoke();
        }
    }
}
