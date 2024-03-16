using UnityEngine;
using UnityEngine.Events;

public class FeetDetection : MonoBehaviour
{
    //Events
    [HideInInspector] public UnityEvent FeetEnterTrigger;

    //Enemy touched
    [HideInInspector] public GameObject EnemyTouched;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.layer != 6)
        {
            EnemyTouched = collision.gameObject;
            FeetEnterTrigger?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == EnemyTouched)
            EnemyTouched = null;
    }
}
