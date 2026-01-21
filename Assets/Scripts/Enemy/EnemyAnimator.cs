using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
    private readonly int AnimatorIsMoving = Animator.StringToHash("IsMoving");
    private readonly int AnimatorAttack = Animator.StringToHash("Attack");
    private readonly int AnimatorDeath = Animator.StringToHash("Death");
    private readonly int AnimatorHit = Animator.StringToHash("Hit");

    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyAnimationEventSender _deathEventSender;
    [SerializeField] private MeleeWeaponAnimationEventSender _attackEventSender;

    public event Action DeathAnimationEnded;

    private void OnEnable()
    {
        _deathEventSender.AnimationEnded += OnDeathAnimationEnd;
    }

    private void OnDisable()
    {
        _deathEventSender.AnimationEnded -= OnDeathAnimationEnd;
    }

    public void OnStop()
    {
        _animator.SetBool(AnimatorIsMoving, false);
    }

    public void OnStartMoving()
    {
        _animator.SetBool(AnimatorIsMoving, true);
    }

    public void OnAttack()
    {
        _animator.SetTrigger(AnimatorAttack);
    }

    public void OnHit()
    {
        _animator.SetTrigger(AnimatorHit);
    }

    public void OnDeath()
    {
        _animator.SetTrigger(AnimatorDeath);
    }

    private void OnDeathAnimationEnd()
    {
        DeathAnimationEnded?.Invoke();
    }
}
