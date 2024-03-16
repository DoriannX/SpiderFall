using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    //stats

    [SerializeField] float _maxHealth = 10;
    [SerializeField] float _enemyDamage = 10;
    float _health;

    //detection
    [SerializeField] float _borderDetecterRange = .1f;
    [SerializeField] float _groundRange = .1f;

    //movement
    [SerializeField] float _moveForce = 1;
    [SerializeField] float _maxVelocity = 1;
    Vector3 _direction = Vector3.right;

    //components    
    CinemachineImpulseSource _impulseSource;
    Transform _transform;
    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;
    [SerializeField] EnemyPlayerDetecter _enemyPlayerDetecter;

    //spriteFeedback
    float _actualColor = 45/360;
    [SerializeField] float _distortionSize = 1;


    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _transform = transform;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _health = _maxHealth;
        if (_enemyPlayerDetecter)
            _enemyPlayerDetecter.PlayerDetected.AddListener(Attack);
        else
            Debug.LogError("no enemyPlayerDetecter on enemy");
    }
    public void TakeDamage(float damage)
    {
        _health -= damage;
        _actualColor = (_health / _maxHealth) * 45/360;
        if (_spriteRenderer)
        {
            _spriteRenderer.color = Color.HSVToRGB(_actualColor, 1, 1);
            StartCoroutine(ChangeSizeRenderer(_spriteRenderer, _distortionSize));
        }
        else
            Debug.LogError("There's no sprite renderer on enemy");
        if (_health <= 0)
        {
            Die();
        }
    }

    IEnumerator ChangeSizeRenderer(SpriteRenderer spriteRenderer, float distortionSize)
    {

        spriteRenderer.transform.localScale += Vector3.one * distortionSize;
        yield return new WaitForSeconds(.05f);

        spriteRenderer.transform.localScale -= (Vector3.one * distortionSize) / 2;
        yield return new WaitForSeconds(.05f);

        spriteRenderer.transform.localScale += (Vector3.one * distortionSize) / 4;
        yield return new WaitForSeconds(.05f);

        spriteRenderer.transform.localScale = Vector3.one;

    }

    public void Die()
    {
        _impulseSource.GenerateImpulse(new Vector3(0, 1));
        Handheld.Vibrate();
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        Patrol();
    }

    private void Update()
    {
        DetectBorderAndWall();
    }

    private void Patrol()
    {
        //Set a target velocity
        Vector2 targetVelocity = _direction * _maxVelocity;

        //Find the change of velocity needed to reach target
        Vector3 velocityChange = targetVelocity - _rb.velocity;

        //Convert to acceleration, which is change of velocity over time
        Vector3 acceleration = velocityChange / Time.fixedDeltaTime;

        //Clamp it to your maximum acceleration magnitude
        acceleration = Vector3.ClampMagnitude(acceleration, _maxVelocity);

        //Then AddForce
        _rb.AddForce(acceleration, ForceMode2D.Force);
    }

    private void DetectBorderAndWall()
    {
        RaycastHit2D leftBorderDetecter = Physics2D.Raycast(_transform.position + new Vector3(0, -.6f, 0) + Vector3.left / 2, Vector3.down * _borderDetecterRange, _borderDetecterRange);
        RaycastHit2D rightBorderDetecter = Physics2D.Raycast(_transform.position + new Vector3(0, -.6f, 0) + Vector3.right / 2, Vector3.down * _borderDetecterRange, _borderDetecterRange);
        RaycastHit2D leftWallDetecter = Physics2D.Raycast(_transform.position + new Vector3(-.6f, 0, 0), Vector3.left * _borderDetecterRange, _borderDetecterRange);
        RaycastHit2D rightWallDetecter = Physics2D.Raycast(_transform.position + new Vector3(.6f, 0, 0), Vector3.right * _borderDetecterRange, _borderDetecterRange);

        if (_direction == Vector3.left && (!leftBorderDetecter.collider || (leftWallDetecter.collider && leftWallDetecter.collider.transform.parent.TryGetComponent<DestructibleGround>(out DestructibleGround temp))))
        {
            _direction = Vector3.right;
        }
        if (_direction == Vector3.right && (!rightBorderDetecter.collider || (rightWallDetecter.collider && rightWallDetecter.collider.transform.parent.TryGetComponent<DestructibleGround>(out temp))))
        {
            _direction = Vector3.left;
        }
    }

    private void Attack()
    {

        List<GameObject> detectPlayer = _enemyPlayerDetecter.DetectedPlayer;

        foreach(GameObject player in detectPlayer.ToList())  
        {
            if (player.TryGetComponent<Player>(out Player playerScript))
            {
                playerScript.TakeDamage(_enemyDamage);
            }
        }
    }
}
