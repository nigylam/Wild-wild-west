using System;
using UnityEngine;

public class EnemyAnimationEventSender : MonoBehaviour
{
    public event Action AnimationEnded;

    public void RaiseAnimationEnded() => AnimationEnded?.Invoke();
}
