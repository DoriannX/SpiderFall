using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //Component
    [SerializeField] GameObject _feet;
    [SerializeField] ObstacleDetecter _obstacleDetecter;
    FeetDetection _feetDetect;
    Rigidbody2D _rb;
    Transform _transform;
    CinemachineImpulseSource _impulseSource;
    PlayerMovement _playerMovement;

    //Attack
    [SerializeField] float _radius = 1;
    [SerializeField] float _playerDamage = 1;
    public float ShotRange = 1;
    [SerializeField] LineController _ray;


    private void Awake()
    {

        _rb = GetComponent<Rigidbody2D>();

        if (_feet)
            _feetDetect = _feet.GetComponent<FeetDetection>();
        else
            Debug.LogError("feetDetect not in PlayerAttack");

        _transform = transform;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        if(_feet)
            _feetDetect.FeetEnterTrigger.AddListener(JumpAttack);
        else
            Debug.LogError("feetDetect not in PlayerAttack");
    }

    void JumpAttack()
    {
        if (_feet)
        {
            if (_feetDetect.EnemyTouched.transform.parent.TryGetComponent<Enemy>(out Enemy enemy))
            {
                if (_rb)
                {
                    if (_playerMovement)
                    {
                        _playerMovement.Jump(_playerMovement.JumpForce * _playerMovement.JumpMulti);
}
                    else
                        Debug.LogError("No playerMovement in PlayerAttack");
                }
                else
                    Debug.LogError("There's no rigidbody on Player");

                enemy.TakeDamage(_playerDamage);
            }
        }
        else
            Debug.LogError("feetDetect not in PlayerAttack");
    }

    public void Shot()
    {
        _impulseSource.GenerateImpulse(new Vector3(0, .05f));
        List<GameObject> objectDetected = new List<GameObject>();
        if (_obstacleDetecter)
            objectDetected = _obstacleDetecter.DetectedObject;
        else
            Debug.LogError("No obstacle detecter in PlayerAttack");
        GameObject targetPoint = new GameObject();
        targetPoint.transform.position = _transform.position + Vector3.down * ShotRange;
        
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
                groundShot.DestroyGround(_radius);
                targetPoint.transform.position = groundShot.transform.position;
            }
            
        }
        LineController shotRay = Instantiate(_ray, _transform);
        shotRay.SetUpLine(new Transform[] { _transform, targetPoint.transform });
        Destroy(shotRay.gameObject, .5f);
    }


}
