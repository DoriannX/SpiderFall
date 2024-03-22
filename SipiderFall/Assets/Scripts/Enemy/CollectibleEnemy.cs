using UnityEngine;

public class CollectibleEnemy : MonoBehaviour
{

    //Components
    Transform _playerTransform;
    Rigidbody2D _rb;
    Transform _transform;
    Collider2D _collider;
    [SerializeField] LineController _rope;
    EnemyFeedback _enemyFeedback;
    SpriteRenderer _sprite;

    //Gathering rope
    bool _gathered = false;
    [SerializeField] float _ropeSize;
    [SerializeField] float _enemyMass;
    [SerializeField] LayerMask _notTouchingDeadEnemies;

    plusOneFeedback _plusOneFeedback;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = transform;
        _collider = GetComponentInChildren<Collider2D>();
        _enemyFeedback = GetComponentInChildren<EnemyFeedback>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _playerTransform = Player.Instance.gameObject.transform;
        _plusOneFeedback = GetComponent<plusOneFeedback>();
    }

    private void OnEnable()
    {
        StartCoroutine(_enemyFeedback.Blink(() => _gathered));
        Tools.SetLayer(gameObject, 0);
        _rb.excludeLayers = 0;
        _collider.excludeLayers = 0;
        _sprite.transform.localScale = Vector3.one;
    }

    private void Update()
    {
        _sprite.transform.localScale = Vector3.one;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(this.enabled && !_gathered && collision.gameObject.transform.parent.gameObject == Player.Instance.gameObject)
        {
            if (_plusOneFeedback)
                _plusOneFeedback.ShowPlusOne();
            else
                Debug.LogError("No plus one feedback");
            TutoManager.Instance.ToggleTiltPhoneTuto(true);
            _collider.isTrigger = false;
            ConfigureRigidbody(_rb, false, _notTouchingDeadEnemies, _enemyMass, -10);
            _gathered=true;
            Tools.SetLayer(gameObject, 6);

            if(_playerTransform.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                CreateJoint(gameObject, rb, false, _ropeSize, true);
            }
            if (_rope)
            {
                LineController rope = Instantiate(_rope, _playerTransform);
                rope.SetUpLine(new Transform[] { _playerTransform, _transform });
            }
            else
                Debug.LogError("not rope in CollectibleEnemy");
            if(_playerTransform.gameObject.TryGetComponent<GatheredEnemyCounter>(out GatheredEnemyCounter gatheredEnemyCounter)){
                gatheredEnemyCounter.AddGatheredEnemy();
            }
            Destroy(_enemyFeedback);
            _sprite.color = Color.gray;
        }
    }

    private Rigidbody2D ConfigureRigidbody(Rigidbody2D rb, bool isKinematic, LayerMask excludeLayers, float mass, float gravityScale)
    {
        rb.isKinematic = isKinematic;
        rb.mass = mass;
        rb.gravityScale = gravityScale;
        rb.mass = mass;
        rb.excludeLayers = excludeLayers;
        return rb;
    }

    private void CreateJoint(GameObject connectedToJoint, Rigidbody2D connectedBody, bool autoConfigureDistance, float distance, bool maxDistanceOnly)
    {
        DistanceJoint2D joint = connectedToJoint.AddComponent<DistanceJoint2D>();
        joint.connectedBody = connectedBody;
        joint.autoConfigureDistance = autoConfigureDistance;
        joint.distance = distance;
        joint.maxDistanceOnly = maxDistanceOnly;
    }
}
