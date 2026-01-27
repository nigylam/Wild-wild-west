using System;
using UnityEngine;

public class StepAnimationEventSender : MonoBehaviour
{
    public event Action LeftStep;
    public event Action RightStep;
    public event Action JumpStarted;

    public void OnLeftFootStep()
    {
        LeftStep?.Invoke();
    }

    public void OnRightFootStep()
    {
        RightStep?.Invoke();
    }

    public void OnStartJump()
    {
        JumpStarted?.Invoke();
    }
}
