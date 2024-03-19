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
    [HideInInspector] public bool SlowTime = false;

    bool _started = false;

    //Player
    Transform _playerTransform;

    //Instance
    public static TutoManager Instance;

    Coroutine _slowDownTime = null;

    private float start;
    private float elapsed = 0f;
    private float targetPosY = 1;
    private float duration;

    public void StartSlowDownTime(float targetPosY)
    {
        this.start = Time.timeScale;
        this.targetPosY = targetPosY;
    }

    public void Update()
    {
        if (SlowTime && Player.Instance.transform.position.y > targetPosY)
        {
            print(Mathf.Abs(Player.Instance.transform.position.y / targetPosY));
            Time.timeScale = Mathf.Lerp(1, 0, Mathf.Abs(Player.Instance.transform.position.y / targetPosY));
        }

        if (IsTuto && ActivateTuto && _playerTransform.position.y < _blocTutoTransform.position.y)
        {
            FinishTuto();
        }
    }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
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
            StartSlowDownTime((Player.Instance.transform.position + Vector3.down * 5).y);
            SlowTime = true;
            PlayerMovement.Instance.CanMove = false;
            IsTuto = true;
        }
        else
        {
            SlowTime = false;
            _tutoMap.SetActive(false);
            _handTuto.SetActive(false);
            _ArrowTuto.SetActive(false);
            _tiltPhoneTuto.SetActive(false);
            Time.timeScale = 0;
            PlayerMovement.Instance.CanMove = true;
            IsTuto = false;
        }
    }

    public void NextFeedback()
    {
        SlowTime = false;
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        if(!ActivateTuto)
        {
            SlowTime=false;
            Time.timeScale = 1;
            _started = false;
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
