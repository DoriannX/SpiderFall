using Cinemachine;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //stats

    [SerializeField] float _maxHealth = 10;
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
    }
    public void TakeDamage(float damage)
    {
        print(damage);
        _health -= damage;
        _actualColor = (_health / _maxHealth) * 45/360;
        if (_spriteRenderer)
        {
            _spriteRenderer.color = Color.HSVToRGB(_actualColor, 1, 1);
            StartCoroutine(ChangeSizeRenderer(_spriteRenderer, _distortionSize));
            print(_actualColor);
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

    private void Update()
    {
        Patrol();
    }

    public void Patrol()
    {
        RaycastHit2D leftBorderDetecter = Physics2D.Raycast(_transform.position + new Vector3(0, -.6f, 0) + Vector3.left/2, Vector3.down * _borderDetecterRange, _borderDetecterRange);
        RaycastHit2D rightBorderDetecter = Physics2D.Raycast(_transform.position + new Vector3(0, -.6f, 0) + Vector3.right/2, Vector3.down * _borderDetecterRange, _borderDetecterRange);
        RaycastHit2D leftWallDetecter = Physics2D.Raycast(_transform.position + new Vector3(-.6f, 0, 0), Vector3.left * _borderDetecterRange, _borderDetecterRange);
        RaycastHit2D rightWallDetecter = Physics2D.Raycast(_transform.position + new Vector3(.6f, 0, 0), Vector3.right * _borderDetecterRange, _borderDetecterRange);

        Debug.DrawRay(_transform.position + new Vector3(-.6f, 0, 0), Vector3.left * _borderDetecterRange, Color.blue);
        Debug.DrawRay(_transform.position + new Vector3(.6f, 0, 0), Vector3.right * _borderDetecterRange, Color.blue);
        Debug.DrawRay(_transform.position + new Vector3(0, -.6f, 0) + Vector3.left/2, Vector3.down * _borderDetecterRange, Color.blue);
        Debug.DrawRay(_transform.position + new Vector3(0, -.6f, 0) + Vector3.right/2, Vector3.down * _borderDetecterRange, Color.blue);


        if (_direction == Vector3.left && (!leftBorderDetecter.collider || leftWallDetecter.collider))
        {
            _direction = Vector3.right;
        }if (_direction == Vector3.right && (!rightBorderDetecter.collider || rightWallDetecter.collider))
        {
            _direction = Vector3.left;
        }
        _rb.velocity = new Vector3(_direction.x * _moveForce, _rb.velocity.y);

    }
}
