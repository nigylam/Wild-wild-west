using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    private Camera _camera;
    private CameraRotator _cameraRotator;
    private ThirdPersonActions _actions;
    private InputAction _moveAction;
    private Rigidbody _rigidbody;
    private Vector3 _forceDirection;
    private float _groundCheckOffset = 0.25f;
    private float _groundCheckHeight = 1f;
    private float _movementForce;
    private float _movementForceDecrease = 0.5f;
    private float _jumpForce;
    private float _maxSpeed;
    private bool _wasGrounded = true;

    public event Action Jumped;
    public event Action Landed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (_actions != null)
        {
            _actions.Player.Jump.started += OnJump;
            _actions.Enable();
        }
    }

    private void OnDisable()
    {
        _actions.Player.Jump.started -= OnJump;
        _actions.Disable();
    }

    private void FixedUpdate()
    {
        Move();
        RotateToCamera();

        if (_wasGrounded == false && IsGrounded())
        {
            Landed?.Invoke();
        }

        _wasGrounded = IsGrounded();
    }

    public void Initialize(Camera camera, CameraRotator cameraRotator, ThirdPersonActions actions, float movementForce, float jumpForce, float maxSpeed)
    {
        _camera = camera;
        _cameraRotator = cameraRotator;
        _actions = actions;
        _moveAction = _actions.Player.Move;
        _movementForce = movementForce;
        _jumpForce = jumpForce;
        _maxSpeed = maxSpeed;
        _actions.Player.Jump.started += OnJump;
        _actions.Enable();
    }

    private void RotateToCamera()
    {
        Quaternion targetRotation = Quaternion.Euler(0, _cameraRotator.Yaw, 0);
        _rigidbody.MoveRotation(targetRotation);
    }

    private void Move()
    {
        Vector3 camForward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(_camera.transform.right, Vector3.up).normalized;

        Vector2 input = _moveAction.ReadValue<Vector2>();
        Vector3 moveDir = camForward * input.y + camRight * input.x;
        _forceDirection += moveDir * _movementForce;

        if (input != new Vector2(0, 1))
            _forceDirection *= _movementForceDecrease;

        _rigidbody.AddForce(_forceDirection, ForceMode.Impulse);
        _forceDirection = Vector3.zero;

        if (_rigidbody.velocity.y < 0)
            _rigidbody.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = _rigidbody.velocity;
        horizontalVelocity.y = 0f;

        if (horizontalVelocity.sqrMagnitude > _maxSpeed * _maxSpeed)
            _rigidbody.velocity = horizontalVelocity.normalized * _maxSpeed + Vector3.up * _rigidbody.velocity.y;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded() == false)
            return;

        _forceDirection += Vector3.up * _jumpForce;
        Jumped?.Invoke();
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up * _groundCheckOffset, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, _groundCheckHeight))
            return true;

        return false;
    }
}
