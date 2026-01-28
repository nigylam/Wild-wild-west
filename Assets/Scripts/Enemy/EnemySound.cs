using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemySound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _idleSounds;
    [SerializeField] private List<AudioClip> _attackSounds;
    [SerializeField] private List<AudioClip> _hitSounds;
    [SerializeField, Range(0, 3)] private float _pitchOffset;
    [SerializeField] private float _idleSoundDelayMin;
    [SerializeField] private float _idleSoundDelayMax;

    private float _pitch;
    private AudioSource _audioSource;
    private Coroutine _playIdleSoundsCoroutine;
    private Coroutine _startPlayIdleSoundsCoroutine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _pitch = _audioSource.pitch;
    }

    private void OnEnable()
    {
        if (_playIdleSoundsCoroutine != null)
            StopCoroutine(_playIdleSoundsCoroutine);

        if (_startPlayIdleSoundsCoroutine != null)
            StopCoroutine(_startPlayIdleSoundsCoroutine);

        _playIdleSoundsCoroutine = StartCoroutine(PlayIdleSounds());
    }

    private void OnDisable()
    {
        if (_playIdleSoundsCoroutine != null)
            StopCoroutine(_playIdleSoundsCoroutine);

        if (_startPlayIdleSoundsCoroutine != null)
            StopCoroutine(_startPlayIdleSoundsCoroutine);
    }

    public void OnAttack()
    {
        PlayTriggerSound(_attackSounds);
    }

    public void OnHit()
    {
        PlayTriggerSound(_hitSounds);
    }

    private void PlayTriggerSound(List<AudioClip> sounds)
    {
        if (_playIdleSoundsCoroutine != null)
            StopCoroutine(_playIdleSoundsCoroutine);

        PlayRandomSound(sounds);

        if (_startPlayIdleSoundsCoroutine != null)
            StopCoroutine(_startPlayIdleSoundsCoroutine);

        _startPlayIdleSoundsCoroutine = StartCoroutine(StartPlayIdleSounds());
    }
    
    private IEnumerator StartPlayIdleSounds()
    {
        while(_audioSource.isPlaying)
        {
            yield return null;
        }

        if (_playIdleSoundsCoroutine != null)
            StopCoroutine(_playIdleSoundsCoroutine);

        _playIdleSoundsCoroutine = StartCoroutine(PlayIdleSounds());
    }

    private IEnumerator PlayIdleSounds()
    {
        while (enabled)
        {
            float timer = 0;
            float delay = Random.Range(_idleSoundDelayMin, _idleSoundDelayMax);

            while (timer < delay)
            {
                yield return null;
                timer += Time.deltaTime;
            }

            PlayRandomSound(_idleSounds);
        }
    }

    private void PlayRandomSound(List<AudioClip> soundsList)
    {
        _audioSource.clip = soundsList[Random.Range(0, soundsList.Count - 1)];
        _audioSource.pitch = Random.Range(_pitch - _pitchOffset, _pitch + _pitchOffset);
        _audioSource.Play();
    }
}
