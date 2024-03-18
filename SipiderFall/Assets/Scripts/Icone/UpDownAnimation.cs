using UnityEditor;
using UnityEngine;

public class UpDownAnimation : MonoBehaviour
{
    Transform _transform;
    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _transform = transform;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    void AnimateSprite()
    {
        _spriteRenderer.transform.position = new Vector3(_spriteRenderer.transform.position.x, _transform.position.y +Mathf.Sin(Time.unscaledTime * 10));
    }

    private void Update()
    {
        AnimateSprite();
    }
}
