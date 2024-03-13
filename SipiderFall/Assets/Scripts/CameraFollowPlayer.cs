using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject _player;

    [SerializeField] Vector3 _offset = new Vector3(0f, 0f, -10f);
    [SerializeField] float _smoothTime = 0.25f;
    Vector3 _velocity = Vector3.zero;
    Transform _transform;
    Transform _playerTransform;

    private void Awake()
    {
        _transform = transform;
        _playerTransform = _player.transform;
    }

    void FixedUpdate()
    {
        if (_player != null)
        {
            Vector3 targetPosition = new Vector3(0, _playerTransform.position.y + _offset.y, -10);
            if(Mathf.Abs(_playerTransform.position.y - _transform.position.y) > 1)
                _transform.position = new Vector3(0, _playerTransform.position.y - Vector3.down.y, -10);
            else
                _transform.position = Vector3.SmoothDamp(_transform.position, targetPosition, ref _velocity, _smoothTime);
        }
        else
            Debug.LogError("You forget to put the player in serialize field in CameraFollowPlayer script");
    }
}
