using System;
using UnityEngine;

public class MeleeWeaponAnimationEventSender : MonoBehaviour
{
    public event Action AttackStarted;
    public event Action AttackHitEnable;
    public event Action AttackHitDisable;
    public event Action AttackEnded;

    public void RaiseAttackStarted() => AttackStarted?.Invoke();
    public void RaiseAttackHitEnable() => AttackHitEnable?.Invoke();
    public void RaiseAttackHitDisable() => AttackHitDisable?.Invoke();
    public void RaiseAttackEnded() => AttackEnded?.Invoke();
}
