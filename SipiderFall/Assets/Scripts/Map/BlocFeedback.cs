using System.Collections;
using UnityEngine;

public class BlocFeedback : MonoBehaviour
{
    Transform _transform;
    [SerializeField] private float scaleFactor = 1.5f;
    [SerializeField] private float duration = 2f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        _transform = transform;
    }

    public IEnumerator ScaleAndDestroy()
    {
        Vector3 initialScale = transform.localScale;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(initialScale, initialScale * scaleFactor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject.transform.parent.gameObject);
    }
}
