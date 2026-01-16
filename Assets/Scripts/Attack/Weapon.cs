using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float Damage;

    [SerializeField] private float _cooldownTime;
    [SerializeField] private AudioClip _attackSound;

    protected AudioSource AudioSource;
    protected bool CanAttack = true;

    private Coroutine _cooldown;

    protected virtual void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    protected virtual void OnEnable()
    {
        CanAttack = true;
        AudioSource.Stop();
    }

    protected virtual void OnDisable()
    {
        CanAttack = true;
        AudioSource.Stop();

        if (_cooldown != null)
            StopCoroutine(_cooldown);
    }

    public bool TryAttack()
    {
        if(CanAttack)
        {
            Attack();
            return true;
        }

        return false;
    }

    protected virtual void Attack()
    {
        StartCooldown();
        PlaySound();
    }

    protected virtual void PlaySound()
    {
        AudioSource.clip = _attackSound;
        AudioSource.Play();
    }

    private void StartCooldown()
    {
        CanAttack = false;

        if (_cooldown != null)
            StopCoroutine(_cooldown);

        _cooldown = StartCoroutine(ShootCooldown());
    }

    private IEnumerator ShootCooldown()
    {
        float time = 0;

        while (time < _cooldownTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        CanAttack = true;
    }
}
