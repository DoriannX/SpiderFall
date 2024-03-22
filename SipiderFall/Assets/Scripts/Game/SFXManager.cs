using UnityEngine;

public class SFXManager : MonoBehaviour
{

    public static SFXManager Instance;

    [SerializeField] AudioClip _jump;
    [SerializeField] AudioClip _shoot;
    [SerializeField] AudioClip _destroyBloc;
    [SerializeField] AudioClip _win;
    [SerializeField] AudioClip _lose;
    [SerializeField] AudioClip _fall;
    [SerializeField] AudioClip _hit;

    AudioSource _windAudioSource;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void PlayJumpSFX()
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();

        newAudioSource.clip = _jump;
        newAudioSource.Play();

        Destroy(newAudioSource, _jump.length);
    }

    public void PlayShotSFX(GameObject enemy)
    {
        AudioSource newAudioSource = enemy.AddComponent<AudioSource>();
        newAudioSource.spatialBlend = 1;
        newAudioSource.rolloffMode = AudioRolloffMode.Linear;
        newAudioSource.maxDistance = 20;
        newAudioSource.clip = _shoot;
        newAudioSource.Play();

        Destroy(newAudioSource, _shoot.length);
    }

    public void PlayDestroyBloc()
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = _destroyBloc;
        newAudioSource.volume = .1f;
        newAudioSource.Play();

        Destroy(newAudioSource, _destroyBloc.length);
    }

    public void PlayWin()
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();

        newAudioSource.clip = _win;
        newAudioSource.Play();

        Destroy(newAudioSource, _win.length);
    }
    
    public void PlayLose()
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();

        newAudioSource.clip = _lose;
        newAudioSource.Play();

        Destroy(newAudioSource, _lose.length);
    }

    public void PlayFall()
    {
        _windAudioSource = gameObject.AddComponent<AudioSource>();

        _windAudioSource.clip = _fall;
        _windAudioSource.loop = true;
        _windAudioSource.Play();

        if(_windAudioSource)
            Destroy(_windAudioSource, _fall.length);
    }

    public void StopFall()
    {
        if(_windAudioSource)
            Destroy(_windAudioSource);
    }

    public void PlayHit()
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();

        newAudioSource.clip = _hit;
        newAudioSource.Play();

        Destroy(newAudioSource, _lose.length);
    }
}
