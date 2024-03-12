using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject _player;

    [SerializeField] Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] float smoothTime = 0.25f;
    Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (_player != null)
        {
            Vector3 targetPosition = _player.transform.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
            Debug.LogError("You forget to put the player in serialize field in CameraFollowPlayer script");
    }
}
