using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _attackRange;
    [SerializeField] private MeleeWeapon _weapon;

    private Transform _target;
    private NavMeshAgent _agent;
    private float _updateRate = 0.2f;
    private float _timer;
    private float _rotationSpeed = 1f;
    private float _angleAttackOffset = 0.001f;

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer > 0) 
            return;

        _timer = _updateRate;
        float sqrDistance = Vector3.SqrMagnitude(transform.position - _target.position);

        if (sqrDistance <= _attackRange * _attackRange)
        {
            _agent.isStopped = true;
            RotateTowardsTarget();  
            _weapon.Attack();
        }
        else
        {
            _agent.isStopped = false;
            _agent.SetDestination(_target.position);
        }
    }

    public void Initialize(Transform target)
    {
        _target = target;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = _target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < _angleAttackOffset)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}
