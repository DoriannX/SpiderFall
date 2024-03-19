using System.Collections;
using UnityEngine;

public class TutoManager : MonoBehaviour
{
    public bool ActivateTuto = false;

    [SerializeField] Transform _blocTutoTransform;
    //tuto
    [SerializeField] GameObject _tutoMap;
    [SerializeField] GameObject _handTuto;
    [SerializeField] GameObject _tiltPhoneTuto;
    [SerializeField] GameObject _ArrowTuto;
    [HideInInspector] public bool IsTuto;

    //Player
    Transform _playerTransform;

    //Instance
    public static TutoManager Instance;

    Coroutine _slowDownTime = null;

    IEnumerator SlowDownTime(float targetTimeScale, float duration)
    {
        float start = Time.timeScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(start, targetTimeScale, elapsed / duration);
            yield return null;
        }

        Time.timeScale = targetTimeScale;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (IsTuto && ActivateTuto && _playerTransform.position.y < _blocTutoTransform.position.y)
        {
            FinishTuto();
        }
    }

    private void Start()
    {
        _playerTransform = Player.Instance.transform;
        if (ActivateTuto)
        {
            _tutoMap.SetActive(true);
            _handTuto.SetActive(true);
            _tiltPhoneTuto.SetActive(false);
            _ArrowTuto.SetActive(false);
            PlayerMovement.Instance.CanMove = false;
            _slowDownTime = StartCoroutine(SlowDownTime(0, 3));
            IsTuto = true;
        }
        else
        {
            _tutoMap.SetActive(false);
            _handTuto.SetActive(false);
            _ArrowTuto.SetActive(false);
            _tiltPhoneTuto.SetActive(false);
            PlayerMovement.Instance.CanMove = true;
            IsTuto = false;
        }
    }
    public void StartGame()
    {
        if (IsTuto && ActivateTuto)
        {
            StopCoroutine(_slowDownTime);
            Time.timeScale = 1;
        }

    }

    public void ToggleHandTuto(bool state)
    {
        if(IsTuto && _handTuto.activeSelf != state)
        {
            _ArrowTuto.SetActive(!state);
            _tiltPhoneTuto.SetActive(!state);
            _handTuto.SetActive(state);
        }
    }

    public void ToggleTiltPhoneTuto(bool state)
    {
        if(IsTuto && _tiltPhoneTuto.activeSelf != state)
        {
            _ArrowTuto.SetActive(!state);
            _handTuto.SetActive(!state);
            PlayerMovement.Instance.CanMove = state;
            _tiltPhoneTuto.SetActive(state);
        }
    }

    public void ToggleArrowTuto(bool state)
    {
        if (IsTuto && _ArrowTuto.activeSelf != state) {
            _tiltPhoneTuto.SetActive(!state);
            _handTuto.SetActive(!state);
            _ArrowTuto.SetActive(state);
        }
    }

    public void FinishTuto()
    {
        if(IsTuto){
            _handTuto.SetActive(false);
            _ArrowTuto.SetActive(false);
            _tiltPhoneTuto.SetActive(false);
            PlayerMovement.Instance.CanMove = true;
            IsTuto = false;
        }
    }
}
