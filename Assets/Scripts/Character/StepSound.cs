using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StepSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _stepSounds;
    [SerializeField] private StepAnimationEventSender _stepAnimations;
    [SerializeField, Range(0, 3)] private float _pitchOffset;
    [SerializeField, Range(0,1)] private float _stepVolume;
    [SerializeField, Range(0, 1)] private float _jumpVolume;

    private AudioSource _audioSource;
    private float _pitch;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _pitch = _audioSource.pitch;
    }

    private void OnEnable()
    {
        _stepAnimations.RightStep += OnStep;
        _stepAnimations.LeftStep += OnStep;
    }

    private void OnDisable()
    {
        _stepAnimations.RightStep -= OnStep;
        _stepAnimations.LeftStep -= OnStep;
    }

    private void OnStep()
    {
        PlayRandomStepSound(_stepVolume);
    }

    public void OnJumpStarted() 
    {
        _audioSource.Stop();
    }

    public void OnLanded()
    {
        PlayRandomStepSound(_jumpVolume);
    }

    private void PlayRandomStepSound(float volume)
    {
        _audioSource.clip = _stepSounds[Random.Range(0, _stepSounds.Count - 1)];
        _audioSource.pitch = Random.Range(_pitch - _pitchOffset, _pitch + _pitchOffset);
        _audioSource.volume = volume;
        _audioSource.Play();
    }
}
