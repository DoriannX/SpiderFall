using UnityEngine;

public class Tools
{
    public static bool IsGrounded(GameObject detectedObject, float range)
    {
        bool grounded = false;

        Transform detectedObjectTransform = detectedObject.transform;
        Vector3 origin = detectedObjectTransform.position + new Vector3(0, -.6f, 0);
        RaycastHit2D[] groundHit = Physics2D.RaycastAll(origin, Vector3.down * range, range);
        foreach(RaycastHit2D hit in groundHit)
        {
            if(hit.collider.transform.parent.TryGetComponent<DestructibleGround>(out DestructibleGround ground))
            {
                grounded = true; break;
            }
        }
        Debug.DrawRay(origin, Vector3.down * range, Color.green);
        return grounded;
    }
}
