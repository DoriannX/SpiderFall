using System;
using System.Collections;
using UnityEngine;

public class EnemyFeedback : MonoBehaviour
{

    [SerializeField] float _distortionSize;
    Enemy _enemy;
    SpriteRenderer _sprite;
    float _actualColor = 45 / 360;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    public IEnumerator ChangeSizeRenderer()
    {
        _actualColor = _enemy.GetHealthRatio() * 45 / 360;
        _sprite.color = Color.HSVToRGB(_actualColor, 1, 1);
        _sprite.transform.localScale += Vector3.one * _distortionSize;
        yield return new WaitForSeconds(.05f);

        _sprite.transform.localScale -= (Vector3.one * _distortionSize) / 2;
        yield return new WaitForSeconds(.05f);

        _sprite.transform.localScale += (Vector3.one * _distortionSize) / 4;
        yield return new WaitForSeconds(.05f);

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
        float duration = .5f; // Durée de l'animation en secondes
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
}