using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class plusOneFeedback : MonoBehaviour
{
    [SerializeField] float _fadeSpeed;
    [SerializeField] Vector2 _moveSpeed;
    [SerializeField] GameObject _plusOnePrefab;

    public void ShowPlusOne()
    {
        GameObject plusOne = Instantiate(_plusOnePrefab, transform.position, Quaternion.identity);
        Destroy(plusOne, 2f);

        Rigidbody2D rb = plusOne.GetComponent<Rigidbody2D>();
        _moveSpeed = new Vector2(Random.Range(-10f, 10f), _moveSpeed.y);
        rb.velocity = Vector2.one * _moveSpeed;

        SpriteRenderer spriteRenderer = plusOne.GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(FadeOut(spriteRenderer));
    }

    private IEnumerator FadeOut(SpriteRenderer spriteRenderer)
    {
        Color startColor = spriteRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0); // Alpha = 0

        float elapsedTime = 0f;
        while (elapsedTime < _fadeSpeed)
        {
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / _fadeSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = endColor; // Assure que l'alpha est bien à 0
    }
}
