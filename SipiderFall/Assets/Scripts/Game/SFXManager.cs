using UnityEngine;

public class SFXManager : MonoBehaviour
{

    public static SFXManager Instance;

    [SerializeField] AudioClip _jump;
    AudioSource _source;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        _source = GetComponent<AudioSource>();
    }

    public void PlayJumpSFX()
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();

        newAudioSource.clip = _jump;
        newAudioSource.Play();

        Destroy(newAudioSource, _jump.length);
    }
}
