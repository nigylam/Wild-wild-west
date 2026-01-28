using System;
using UnityEngine;

public class MeleeWeaponAnimationEventSender : MonoBehaviour
{
    public event Action AttackHitEnable;
    public event Action AttackHitDisable;

    public void RaiseAttackHitEnable() => AttackHitEnable?.Invoke();
    public void RaiseAttackHitDisable() => AttackHitDisable?.Invoke();
}
