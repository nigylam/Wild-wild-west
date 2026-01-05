using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerShooter))]
public class Player : MonoBehaviour
{
    [Header("Parametres")]
    [SerializeField] private float _movementForce = 1f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _maxSpeed = 10f;

    [Header("Links")]
    [SerializeField] private Camera _camera;

    private ThirdPersonActions _actions;
    private PlayerMover _mover;
    private PlayerShooter _shooter;

    private void Awake()
    {
        _actions = new ThirdPersonActions();
        _shooter = GetComponent<PlayerShooter>();
        _mover = GetComponent<PlayerMover>();
        _shooter.Initialize(_camera);
        _mover.Initialize(_camera, _actions, _movementForce, _jumpForce, _maxSpeed);
    }

    private void OnEnable()
    {
        _actions.Player.Attack.started += _shooter.Shoot;
    }

    private void OnDisable()
    {
        _actions.Player.Attack.started -= _shooter.Shoot;
    }
}
