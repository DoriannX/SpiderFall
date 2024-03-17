using System.Collections;
using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{

    float _actualColor = 200 / 360;
    SpriteRenderer _sprite;
    [SerializeField] float _distortionSize;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    IEnumerator ChangeSize(SpriteRenderer sprite, float distortionSize)
    {
        print("change size");
        Tools.SetLayer(sprite.transform.parent.gameObject, 6);

        sprite.transform.localScale += Vector3.one * distortionSize;
        yield return new WaitForSeconds(.05f);

        sprite.transform.localScale -= (Vector3.one * distortionSize) / 2;
        yield return new WaitForSeconds(.05f);

        sprite.transform.localScale += (Vector3.one * distortionSize) / 4;
        yield return new WaitForSeconds(.05f);

        sprite.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(2f);
        Tools.SetLayer(sprite.transform.parent.gameObject, 3);
        

        yield return null;

    }

    IEnumerator ChangeColor(SpriteRenderer sprite) 
    {
        _actualColor = Player.Instance.RatioHealth() * 260 / 360;
        sprite.color = Color.HSVToRGB(_actualColor, 1, 1);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, .5f);
        yield return new WaitForSeconds(2.15f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        yield return null;
    }

    public void Feedback()
    {
        StartCoroutine(ChangeColor(_sprite));
        StartCoroutine(ChangeSize(_sprite, _distortionSize));
    }
}