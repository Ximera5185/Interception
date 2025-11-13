using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSiren : MonoBehaviour
{
    private const float MinVolume = 0f;
    private const float MaxVolume = 1f;

    private AudioClip _audioClip;
    private AudioSource _audioSource;
    private Coroutine _coroutine;

    private float _lerpVolume = 0.1f;
    private float _targetVolume;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        const string PathAudioFile = "Sound/Sirena";

        _audioSource = GetComponent<AudioSource>();

        _audioClip = Resources.Load<AudioClip>(PathAudioFile);

        if (_audioClip != null)
        {
            _audioSource.clip = _audioClip;
            _audioSource.volume = MinVolume;
        }
    }

    private void StopCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);

            _coroutine = null;
        }
    }

    private void StartCoroutine()
    {
        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(UpdateVolume(_targetVolume));
        }
    }

    private IEnumerator UpdateVolume(float targetVolume)
    {
        while (!Mathf.Approximately(_audioSource.volume, targetVolume))
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetVolume, _lerpVolume * Time.deltaTime);

            yield return null;

            if (_audioSource.volume == MinVolume)
            {
                _audioSource.Stop();
            }
        }
    }
    public void PlaySound()
    {
        _targetVolume = MaxVolume;

        StopCoroutine();

        _audioSource.Play();

        StartCoroutine();
    }

    public void StopSound()
    {
        _targetVolume = MinVolume;

        StopCoroutine();
        StartCoroutine();
    }
}