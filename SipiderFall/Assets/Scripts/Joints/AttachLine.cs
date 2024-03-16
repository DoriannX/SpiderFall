using UnityEngine;

public class AttachLine : MonoBehaviour
{
    HingeJoint2D _joint;

    private void Awake()
    {
        
    }

    public void AttachToObject(Rigidbody2D rb)
    {
        _joint = GetComponent<HingeJoint2D>();
        _joint.connectedBody = rb;
    }
}
