using UnityEngine;
using UnityEngine.UI;

public class WinLoseMenu : MonoBehaviour
{

    [SerializeField] Image _backgroundImage;
    bool _isFadeIn = false;
    float _fadeDuration = 0;
    float _timeElapsed = 0;
    bool _isFadeOut = false;

    float _actualAlpha = 0;

    public static WinLoseMenu Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Update()
    {
        FadeIn();
        FadeOut();
    }

    public void StartFadeIn(float fadeDuration)
    {
        print("started fade in");
        _actualAlpha = 0;
        _timeElapsed = 0;
        _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, 0);
        _fadeDuration = fadeDuration;
        _isFadeIn = true;
    }

    private void FadeIn()
    {
        if (_isFadeIn)
        {
            _timeElapsed += Time.unscaledDeltaTime;
            _actualAlpha = Mathf.Lerp(0, 1, _timeElapsed / _fadeDuration);
            _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, _actualAlpha);
        }
        if (_actualAlpha >= 1)
        {
            _isFadeIn = false;
        }

    }

    public void StartFadeOut(float fadeDuration)
    {
        print("started fade out");
        _actualAlpha = 0;
        _timeElapsed = 0;
        _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, 1);
        _fadeDuration = fadeDuration;
        _isFadeOut = true;
    }
    private void FadeOut()
    {
        if (_isFadeOut)
        {
            _timeElapsed += Time.unscaledDeltaTime;
            _actualAlpha = Mathf.Lerp(1, 0, _timeElapsed / _fadeDuration);
            _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, _actualAlpha);
        }
        if (_actualAlpha <= 0)
        {
            _isFadeOut = false;
        }
    }

    public void OpenMenu()
    {
        StartFadeIn(1);
    }

    public void CloseMenu()
    {
        StartFadeOut(1);
    }
}
