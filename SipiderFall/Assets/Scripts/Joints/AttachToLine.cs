using UnityEngine;

public class AttachToLine : MonoBehaviour
{
    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void AttachObjectToLine(HingeJoint2D joint)
    {
        joint.connectedBody = _rb;
    }

}
