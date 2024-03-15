using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollowPlayer : MonoBehaviour
{
    //Components
    [SerializeField] GameObject _player;
    Transform _transform;
    Transform _playerTransform;

    //Camera
    [SerializeField] Vector3 _offset = new Vector3(0f, 0f, -10f);
    Vector3 _velocity = Vector3.zero;

    private void Awake()
    {
        _transform = transform;
        if (_player)
            _playerTransform = _player.transform;
        else
            Debug.LogError("You forgot to put the player in the serialize field  in CameraFollowPlayer script");
    }

    void FixedUpdate()
    {
        if (_player)
        {

            _transform.position = new Vector3(0, _transform.position.y, 0);

            /*Vector3 targetPosition = new Vector3(0, _playerTransform.position.y + _offset.y, -10);
            if(_playerTransform.position.y - _transform.position.y > 1)
                _transform.position = new Vector3(0, _playerTransform.position.y - Vector3.down.y, -10);
            else if(_playerTransform.position.y - _transform.position.y < -1)
                _transform.position = new Vector3(0, _playerTransform.position.y + Vector3.up.y, -10);
            else
                _transform.position = Vector3.SmoothDamp(_transform.position, targetPosition, ref _velocity, _smoothTime);*/
        }
        else
            Debug.LogError("You forget to put the player in serialize field in CameraFollowPlayer script");
    }
}
