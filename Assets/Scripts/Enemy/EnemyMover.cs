using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMover : MonoBehaviour
{
    private Transform _target;
    private NavMeshAgent _agent;
    private float _updateRate = 0.2f;
    private float _timer;

    public void Initialize(Transform target)
    {
        _target = target;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer > 0) 
            return;

        _timer = _updateRate;
        _agent.SetDestination(_target.position);
    }
}
