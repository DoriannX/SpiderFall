using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPlayerDetecter : MonoBehaviour
{
    [HideInInspector] public List<GameObject> DetectedPlayer;
    [HideInInspector] public UnityEvent PlayerDetected;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 6 && collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            DetectedPlayer.Add(collision.gameObject);
            PlayerDetected?.Invoke();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (DetectedPlayer.Contains(collision.gameObject))
        {
            DetectedPlayer.Remove(collision.gameObject);
        }
    }
}
