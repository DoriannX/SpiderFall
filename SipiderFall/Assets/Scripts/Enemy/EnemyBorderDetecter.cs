using UnityEngine;

public class EnemyBorderDetecter : MonoBehaviour
{

    Transform _transform;
    [SerializeField] float _borderDetecterRange = .1f;
    [SerializeField] private LayerMask _borderDetectedLayer;

    private void Awake()
    {
        _transform = transform;
    }

    public Vector3 DetectBorderAndWall(Vector3 direction)
    {
        RaycastHit2D leftBorderDetecter = Physics2D.Raycast(_transform.position + new Vector3(0, -.6f, 0) + Vector3.left / 2, Vector3.down * _borderDetecterRange, _borderDetecterRange, _borderDetectedLayer);
        RaycastHit2D rightBorderDetecter = Physics2D.Raycast(_transform.position + new Vector3(0, -.6f, 0) + Vector3.right / 2, Vector3.down * _borderDetecterRange, _borderDetecterRange, _borderDetectedLayer);
        RaycastHit2D leftWallDetecter = Physics2D.Raycast(_transform.position + new Vector3(-.6f, 0, 0), Vector3.left * _borderDetecterRange, _borderDetecterRange, _borderDetectedLayer);
        RaycastHit2D rightWallDetecter = Physics2D.Raycast(_transform.position + new Vector3(.6f, 0, 0), Vector3.right * _borderDetecterRange, _borderDetecterRange, _borderDetectedLayer);
        
        if (direction == Vector3.left && (!leftBorderDetecter.collider || (leftWallDetecter.collider && !leftWallDetecter.collider.transform.parent.TryGetComponent<Player>(out Player temp))))
        {
            direction = Vector3.right;
        }
        if (direction == Vector3.right && (!rightBorderDetecter.collider || (rightWallDetecter.collider && !rightWallDetecter.collider.transform.parent.TryGetComponent<Player>(out temp))))
        {
            direction = Vector3.left;
        }
        return direction;
    }

    public Vector3 DetectOnlyWall(Vector3 direction)
    {
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D leftWallDetecter = Physics2D.Raycast(_transform.position + new Vector3(-.6f, 0, 0), Vector3.left * _borderDetecterRange, _borderDetecterRange, _borderDetectedLayer);
        RaycastHit2D rightWallDetecter = Physics2D.Raycast(_transform.position + new Vector3(.6f, 0, 0), Vector3.right * _borderDetecterRange, _borderDetecterRange, _borderDetectedLayer);
        Physics2D.queriesHitTriggers = true;

        if (direction == Vector3.left && leftWallDetecter.collider && !leftWallDetecter.collider.TryGetComponent<EnemyLongRangePlayerDetecter>(out EnemyLongRangePlayerDetecter temp))
        {
            direction = Vector3.right;
        }
        if (direction == Vector3.right && rightWallDetecter.collider && !rightWallDetecter.collider.TryGetComponent<EnemyLongRangePlayerDetecter>(out EnemyLongRangePlayerDetecter temp2))
        {
            direction = Vector3.left;
        }

        return direction;
    }
}