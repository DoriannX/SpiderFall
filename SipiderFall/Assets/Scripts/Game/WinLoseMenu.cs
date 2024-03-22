using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseMenu : MonoBehaviour
{

    [SerializeField] CanvasGroup _backgroundImage;
    [SerializeField] GameObject _selecter;
    [SerializeField] float _selecterForce;
    [SerializeField] TextMeshProUGUI _nextLevel;
    bool _isMenuOpened = false;
    SpriteRenderer _selecterSprite;
    bool _isFadeIn = false;
    float _fadeDuration = 0;
    float _timeElapsed = 0;
    bool _isFadeOut = false;

    float _actualAlpha = 0;

    public static WinLoseMenu Instance;

    private void Awake()
    {
        _selecterSprite = _selecter.GetComponentInChildren<SpriteRenderer>();
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        FinishLevelDetecter.Instance.LevelFinished.AddListener(OpenMenu);
        _selecterSprite.color = new Color(_selecterSprite.color.r, _selecterSprite.color.g, _selecterSprite.color.b, 0);
        _backgroundImage.alpha = 0;
        Player.Instance.Died.AddListener(OpenMenu);
    }

    private void Update()
    {
        if(_isMenuOpened)
        {
            FadeIn();
            FadeOut();
            SetSelecterPos();
            CheckSelecterPos();
            SetNextLevelColor();
        }
    }

    public void StartFadeIn(float fadeDuration)
    {
        _actualAlpha = 0;
        _timeElapsed = 0;
        _backgroundImage.alpha = 0;
        _selecterSprite.color = new Color(_selecterSprite.color.r, _selecterSprite.color.g, _selecterSprite.color.b, 0);
        _fadeDuration = fadeDuration;
        _isFadeIn = true;
    }

    private void FadeIn()
    {
        if (_isFadeIn)
        {
            _timeElapsed += Time.unscaledDeltaTime;
            _actualAlpha = Mathf.Lerp(0, 1, _timeElapsed / _fadeDuration);
            _backgroundImage.alpha = _actualAlpha;

            _selecterSprite.color = new Color(_selecterSprite.color.r, _selecterSprite.color.g, _selecterSprite.color.b, _actualAlpha);
        }
        if (_actualAlpha >= 1)
        {
            _isFadeIn = false;
        }

    }

    public void StartFadeOut(float fadeDuration)
    {
        _actualAlpha = 0;
        _timeElapsed = 1;
        _backgroundImage.alpha = 1;

        _selecterSprite.color = new Color(_selecterSprite.color.r, _selecterSprite.color.g, _selecterSprite.color.b, 1);
        _fadeDuration = fadeDuration;
        _isFadeOut = true;
    }
    private void FadeOut()
    {
        if (_isFadeOut)
        {
            _timeElapsed += Time.unscaledDeltaTime;
            _actualAlpha = Mathf.Lerp(1, 0, _timeElapsed / _fadeDuration);
            _backgroundImage.alpha = _actualAlpha;
            _selecterSprite.color = new Color(_selecterSprite.color.r, _selecterSprite.color.g, _selecterSprite.color.b, _actualAlpha);
        }
        if (_actualAlpha <= 0)
        {
            _isFadeOut = false;
        }
    }

    public void OpenMenu()
    {
        SFXManager.Instance.StopFall();
        _isMenuOpened = true;
        StartFadeIn(1);
    }

    public void CloseMenu()
    {
        _isMenuOpened = false;
        StartFadeOut(1);
    }

    private void CheckSelecterPos()
    {
        if(_selecter.transform.position.x < -5)
        {
            GameManager.Instance.RestartLevel();
            _isMenuOpened = false;
        }
        if (FinishLevelDetecter.Instance.Won && _selecter.transform.position.x > 5)
        {
            GameManager.Instance.NextLevel();
            _isMenuOpened = false;
        }
    }

    void SetSelecterPos()
    {
        _selecter.transform.position = Vector3.Lerp(_selecter.transform.position, new Vector3((Input.acceleration.x) * _selecterForce, Camera.main.transform.position.y), .1f);
    }

    void SetNextLevelColor()
    {
        if (FinishLevelDetecter.Instance.Won)
            _nextLevel.color = Color.white;
        else
            _nextLevel.color = Color.red;
    }
}
