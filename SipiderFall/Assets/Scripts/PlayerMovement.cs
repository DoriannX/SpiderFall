using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    UnityEngine.Gyroscope _gyro;
    Rigidbody2D _rb;
    [SerializeField] float _tiltForce = 1;
    

    private void Start()
    {
        _gyro = Input.gyro;
        _gyro.enabled = true;
        _rb = GetComponentInChildren<Rigidbody2D>();
    }

    private void Update()
    {
        _rb.AddForce(new Vector3(Input.acceleration.x * _tiltForce, 0));
    }
}
