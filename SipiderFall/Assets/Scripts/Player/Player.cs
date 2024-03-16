using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    //stats
    [SerializeField] float _maxHealth = 100;
    float _health;

    //components
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Collider2D _collider;

    //spriteFeedback
    [SerializeField] float _distortionSize = 1;
    float _actualColor = 200/360;

    //Instance
    public static Player Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _health = _maxHealth;
    }

    IEnumerator Feedback(SpriteRenderer spriteRenderer, float distortionSize, GameObject gameObjectToFeedback)
    {   
        Tools.SetLayer(gameObjectToFeedback, 6);
        spriteRenderer.color = Color.HSVToRGB(_actualColor, 1, 1);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, .1f);
        spriteRenderer.transform.localScale += Vector3.one * distortionSize;
        yield return new WaitForSeconds(.05f);

        spriteRenderer.transform.localScale -= (Vector3.one * distortionSize) / 2;
        yield return new WaitForSeconds(.05f);

        spriteRenderer.transform.localScale += (Vector3.one * distortionSize) / 4;
        yield return new WaitForSeconds(.05f);

        spriteRenderer.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(2f);
        print(gameObjectToFeedback);
        Tools.SetLayer(gameObjectToFeedback, 3); 
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);

        yield return null;

    }
    public float TakeDamage(float damage)
    {
        
        _health -= damage;
        _actualColor = (_health / _maxHealth) * 260 / 360;
        if (_spriteRenderer)
        {
            if (_collider)
            {
                StartCoroutine(Feedback(_spriteRenderer, _distortionSize, gameObject));
            }
            else
                Debug.LogError("no collider on player");
        }
        else
            Debug.LogError("no sprite renderer on player");


        if (_health <= 0) 
        {
            Die();
        }

        return _health;
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
