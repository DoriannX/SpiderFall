using UnityEngine;

public class DestructibleGround : MonoBehaviour
{

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void DestroyGround(float radius)
    {
        print("destroy");
        RaycastHit2D[] circlecCasts = Physics2D.CircleCastAll(_transform.position, radius, Vector3.zero);
        Destroy(gameObject);
        foreach(RaycastHit2D blocAround in circlecCasts)
        {
            GameObject parent = blocAround.transform.parent.gameObject;
            if (parent.TryGetComponent<DestructibleGround>(out DestructibleGround ground))
                Destroy(parent);
        }
         
    }
}
