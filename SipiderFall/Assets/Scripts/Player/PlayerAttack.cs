using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //Component
    [SerializeField] ObstacleDetecter _obstacleDetecter;
    Transform _transform;
    CinemachineImpulseSource _impulseSource;
    PlayerFeedback _feedback;

    //Attack
    [SerializeField] float _radius = 1;
    [SerializeField] float _playerDamage = 1;
    public float ShotRange = 1;
    [SerializeField] LineController _ray;


    private void Awake()
    {

        _transform = transform;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _feedback = GetComponentInChildren<PlayerFeedback>();
    }

    public void DistanceAttack()
    {
        if ((gameObject.activeSelf))
        {
            _impulseSource.GenerateImpulse(new Vector3(0, .05f));
            List<GameObject> objectDetected = new List<GameObject>();
            if (_obstacleDetecter)
                objectDetected = _obstacleDetecter.DetectedObject;
            else
                Debug.LogError("No obstacle detecter in PlayerAttack");
            GameObject targetPoint = new GameObject();
            targetPoint.transform.position = _transform.position + Vector3.down * ShotRange;
            Destroy(targetPoint, .6f);
            foreach (GameObject shotHit in objectDetected.ToList()) // objectDetected is being modified in the foreach so it has to be a copy of the list
            {
                _impulseSource.GenerateImpulse(new Vector3(0, .25f));
                if (shotHit.transform.parent.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.TakeDamage(_playerDamage);
                    targetPoint.transform.position = enemy.transform.position;
                    break;
                }
                if (shotHit.transform.parent.TryGetComponent<DestructibleGround>(out DestructibleGround groundShot))
                {
                    StartCoroutine(groundShot.DestroyGround(_radius));
                    targetPoint.transform.position = groundShot.transform.position;
                    break;
                }

            }
            LineController shotRay = Instantiate(_ray, _transform);
            shotRay.SetUpLine(new Transform[] { _transform, targetPoint.transform });
            Destroy(shotRay.gameObject, .5f);
        }
    }


}
