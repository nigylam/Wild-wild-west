using UnityEngine;

public class MeleeAttackBehaviour : StateMachineBehaviour
{
    [Range(0f, 1f)] 
    [SerializeField] float _hitStart = 0.3f;
    [Range(0f, 1f)] 
    [SerializeField] float _hitEnd = 0.55f;

    private MeleeWeaponAnimationEventSender _sender;

    private bool _isHitSent;

    public override void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _sender = animator.GetComponent<MeleeWeaponAnimationEventSender>();
        _isHitSent = false;
    }

    public override void OnStateUpdate(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float t = stateInfo.normalizedTime % 1f;

        if (_isHitSent == false && t >= _hitStart)
        {
            _sender?.RaiseAttackHitEnable();
            _isHitSent = true;
        }

        if (_isHitSent && t >= _hitEnd)
        {
            _sender?.RaiseAttackHitDisable();
        }
    }
}
