using System;
using System.Collections;
using UnityEngine;

public class EnemyFeedback : MonoBehaviour
{

    [SerializeField] float _distortionSize;
    Enemy _enemy;
    SpriteRenderer _sprite;
    float _actualColor = 45 / 360;
    private Vector3 _originalScale;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _originalScale = _sprite.transform.localScale;
    }

    public IEnumerator ChangeSizeRenderer()
    {
        _actualColor = _enemy.GetHealthRatio() * 45 / 360;
        _sprite.color = Color.HSVToRGB(_actualColor, 1, 1);
        _sprite.transform.localScale += Vector3.one * _distortionSize;
        yield return new WaitForSecondsRealtime(.05f);

        _sprite.transform.localScale -= (Vector3.one * _distortionSize) / 2;
        yield return new WaitForSecondsRealtime(.05f);

        _sprite.transform.localScale += (Vector3.one * _distortionSize) / 4;
        yield return new WaitForSecondsRealtime(.05f);

        _sprite.transform.localScale = Vector3.one;

    }

    public void ResetSprite()
    {
        _sprite.transform.localScale = Vector3.one;
    }

    public IEnumerator Blink(Func<bool> condition)
    {
        while (!condition())
        {
            yield return StartCoroutine(FadeTo(.1f));
            yield return StartCoroutine(FadeTo(1f));
        }
    }


    private IEnumerator FadeTo(float targetOpacity)
    {
        Color baseColor = _sprite.color;
        float startOpacity = _sprite.color.a;
        float duration = .5f; // Dur�e de l'animation en secondes
        float elapsedTime = 0f;

        while (Mathf.Abs(_sprite.color.a - targetOpacity) > 0.01f)
        {
            elapsedTime += Time.deltaTime;
            float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / duration);
            _sprite.color = new Color(baseColor.r, baseColor.g, baseColor.b, newOpacity);

            if (elapsedTime >= duration)
            {
                _sprite.color = new Color(baseColor.r, baseColor.g, baseColor.b, targetOpacity);
                break;
            }
            yield return null;
        }
    }

    public IEnumerator Squeeze()
    {
        // Squeeze horizontally
        yield return StartCoroutine(SqueezeTo(new Vector3(2, 0.8f, 1f)));

        // Then squeeze vertically
        yield return StartCoroutine(SqueezeTo(new Vector3(0.8f, 3, 1f)));

        // Then return to original scale
        yield return StartCoroutine(SqueezeTo(_originalScale));
    }

    private IEnumerator SqueezeTo(Vector3 targetScale)
    {
        float elapsedTime = 0;

        while (elapsedTime < 0.1f)
        {
            _sprite.transform.localScale = Vector3.Lerp(_sprite.transform.localScale, targetScale, elapsedTime / .1f);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        _sprite.transform.localScale = targetScale;
    }
}