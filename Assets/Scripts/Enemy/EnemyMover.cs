using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _attackRange;
    [SerializeField] private MeleeWeapon _weapon;
    [SerializeField] private LayerMask _bridgeLayer;

    private Transform _target;
    private NavMeshAgent _agent;
    private Rigidbody _rigidbody;
    private Coroutine _jump;

    private float _jumpDelay = 0.5f;
    private float _updateRate = 0.2f;
    private float _timer;
    private float _rotationSpeed = 1f;
    private float _angleAttackOffset = 0.001f;
    private float _jumpForce = 2f;
    private float _groundCheckOffset = 0.25f;
    private float _groundCheckHeight = 1.5f;

    private void Update()
    {
        if (_agent.enabled == false)
            return;

        MoveNavmesh();
    }

    public void Initialize(Transform target)
    {
        _agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _agent.enabled = false;
        _agent.radius *= transform.lossyScale.x;
        _agent.height *= transform.lossyScale.y;
        _target = target;

        if (_jump != null)
            StopCoroutine(_jump);

        StartCoroutine(Jump());
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

    private IEnumerator Jump()
    {
        float delay = _jumpDelay;

        while (delay > 0f)
        {
            delay -= Time.deltaTime;
            yield return null;
        }

        _rigidbody.isKinematic = false;
        _rigidbody.AddForce((Vector3.up + transform.forward) * _jumpForce, ForceMode.Impulse);

        if (_jump != null)
            StopCoroutine(_jump);

        while (IsGrounded() == false || IsOnBridge())
            yield return null;

        _agent.enabled = true;
        _rigidbody.isKinematic = true;
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up * _groundCheckOffset, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, _groundCheckHeight))
            return true;

        return false;
    }

    private bool IsOnBridge()
    {
        Ray ray = new Ray(transform.position + Vector3.up * _groundCheckOffset, Vector3.down);

        if (Physics.Raycast(ray, _groundCheckHeight, _bridgeLayer))
            return true;

        return false;
    }

    private void MoveNavmesh()
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
}
