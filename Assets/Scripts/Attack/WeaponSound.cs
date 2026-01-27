using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponSound : MonoBehaviour
{
    [SerializeField] protected AudioClip AttackSound;

    [SerializeField, Range(0, 3)] private float _pitchOffset;

    protected AudioSource AudioSource;

    private float _pitch;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        _pitch = AudioSource.pitch;
    }

    public virtual void PlayAttackSound()
    {
        PlaySound(AttackSound);
    }

    protected virtual void PlaySound(AudioClip audioclip)
    {
        AudioSource.clip = audioclip;
        AudioSource.pitch = Random.Range(_pitch - _pitchOffset, _pitch + _pitchOffset);
        AudioSource.Play();
    }
}
