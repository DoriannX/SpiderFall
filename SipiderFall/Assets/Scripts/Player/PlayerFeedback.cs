using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerFeedback : MonoBehaviour
{

    float _actualColor = 200 / 360;
    SpriteRenderer _sprite;
    [SerializeField] float _distortionSize;

    [SerializeField] float _squeezeDuration = 0.5f;
    [SerializeField] Vector3 _horizontalSqueezeScale = new Vector3(1.2f, 0.8f, 1f);
    [SerializeField] Vector3 _verticalSqueezeScale = new Vector3(0.8f, 1.2f, 1f);

    [SerializeField] GameObject _enemyDetecter;

    private Vector3 _originalScale;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _originalScale = _sprite.transform.localScale;
    }

    IEnumerator ChangeSize(SpriteRenderer sprite, float distortionSize)
    {
        Tools.SetLayer(sprite.transform.parent.gameObject, 6);
        _enemyDetecter.layer = 3;
        sprite.transform.localScale += Vector3.one * distortionSize;
        yield return new WaitForSecondsRealtime(.05f);

        sprite.transform.localScale -= (Vector3.one * distortionSize) / 2;
        yield return new WaitForSecondsRealtime(.05f);

        sprite.transform.localScale += (Vector3.one * distortionSize) / 4;
        yield return new WaitForSecondsRealtime(.05f);

        sprite.transform.localScale = Vector3.one;
        

        yield return null;

    }

    IEnumerator ChangeColor(SpriteRenderer sprite)
    {
        _actualColor = Player.Instance.RatioHealth() * 260 / 360;
        sprite.color = Color.HSVToRGB(_actualColor, 1, 1);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, .5f);
        yield return new WaitForSeconds(2.15f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        yield return null;
    }

    public IEnumerator Feedback()
    {
        yield return StartCoroutine(ChangeSize(_sprite, _distortionSize));
        //StartCoroutine(ChangeColor(_sprite));
        yield return new WaitForSecondsRealtime(2f);
        Tools.SetLayer(_sprite.transform.parent.gameObject, 3);
    }

    public IEnumerator Squeeze()
    {
        // Squeeze horizontally
        yield return StartCoroutine(SqueezeTo(_horizontalSqueezeScale));

        // Then squeeze vertically
        yield return StartCoroutine(SqueezeTo(_verticalSqueezeScale));

        // Then return to original scale
        yield return StartCoroutine(SqueezeTo(_originalScale));
    }

    private IEnumerator SqueezeTo(Vector3 targetScale)
    {
        float elapsedTime = 0;

        while (elapsedTime < _squeezeDuration)
        {
            _sprite.transform.localScale = Vector3.Lerp(_sprite.transform.localScale, targetScale, elapsedTime / _squeezeDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        _sprite.transform.localScale = targetScale;
    }
}