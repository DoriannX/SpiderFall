using UnityEngine;

public class DestructibleGround : MonoBehaviour
{
    //Component
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void DestroyGround(float radius)
    {
        Collider2D[] circlecCasts = Physics2D.OverlapCircleAll(_transform.position, radius);

        foreach(Collider2D blocAround in circlecCasts)
        {
            GameObject parent = blocAround.transform.parent.gameObject;
            if (parent.TryGetComponent<DestructibleGround>(out DestructibleGround ground))
                Destroy(parent);
        }
        Destroy(gameObject);

    }
}
