using UnityEngine;

public class TiltAnimation : MonoBehaviour
{
    Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }
    void AnimateSprite()
    {
        _transform.rotation = Quaternion.Euler(_transform.rotation.x, _transform.rotation.y, (_transform.rotation.y + Mathf.Sin(Time.unscaledTime * 10))*10);
    }

    private void Update()
    {
        AnimateSprite();
    }
}