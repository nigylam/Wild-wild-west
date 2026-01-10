using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float _sensitivity = 0.1f;
    [SerializeField] private float _minPitch = -40f;
    [SerializeField] private float _maxPitch = 70f;

    [Header("Moving")]
    [SerializeField] private float _minYOffset = -0.3f;
    [SerializeField] private float _maxYOffset = 0.6f;
    [SerializeField] private float _minZOffset = -0.4f;
    [SerializeField] private float _maxZOffset = 0.2f;
    [SerializeField] private float _positionLerpSpeed = 10f;

    private Vector3 _baseLocalPos;

    private float _yaw;
    private float _pitch;
    private ThirdPersonActions _actions;

    public float Yaw => _yaw;

    private void Awake()
    {
        _actions = new ThirdPersonActions();

        _yaw = transform.eulerAngles.y;
        _pitch = transform.eulerAngles.x;
        _baseLocalPos = transform.localPosition;
    }

    private void OnEnable()
    {
        _actions.Enable();
    }

    private void OnDisable()
    {
        _actions.Disable();
    }

    private void Update()
    {
        Vector2 look = _actions.Player.Look.ReadValue<Vector2>() * _sensitivity;
        float t = Mathf.InverseLerp(_minPitch, _maxPitch, _pitch);
        float yOffset = Mathf.Lerp(_minYOffset, _maxYOffset, t);
        float zOffset = Mathf.Lerp(_minZOffset, _maxZOffset, t);
        Vector3 targetPos = _baseLocalPos + new Vector3(0f, yOffset, zOffset);

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            _positionLerpSpeed * Time.deltaTime
        );

        _yaw += look.x;
        _pitch -= look.y;
        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

        transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }
}
