using System.Collections;
using UnityEngine;

public class DestructibleGround : MonoBehaviour
{
    //Component
    private Transform _transform;
    BlocFeedback _feedback;

    private void Awake()
    {
        _transform = transform;
        _feedback = GetComponentInChildren<BlocFeedback>();
    }

    public IEnumerator DestroyGround(float radius)
    {
        Collider2D[] circlecCasts = Physics2D.OverlapCircleAll(_transform.position, radius);

        foreach (Collider2D blocAround in circlecCasts)
        {
            if(blocAround){
                GameObject parent = blocAround.transform.parent.gameObject;
                if (Tools.TryGetComponentInChildren<BlocFeedback>(parent, out BlocFeedback feedback) && parent.TryGetComponent<DestructibleGround>(out DestructibleGround ground))
                {

                    StartCoroutine(feedback.ScaleAndDestroy());
                }
            }
        }
        yield return StartCoroutine(_feedback.ScaleAndDestroy());

    }
}
