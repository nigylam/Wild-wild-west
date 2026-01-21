using UnityEngine;

public class DeathBehaviour : StateMachineBehaviour
{
    private EnemyAnimationEventSender _sender;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _sender = animator.GetComponent<EnemyAnimationEventSender>();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _sender?.RaiseAnimationEnded();
    }
}
