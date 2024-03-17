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
}