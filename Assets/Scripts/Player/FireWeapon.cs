using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireWeapon : Weapon
{
    [SerializeField] private Transform _muzzle;
    [SerializeField] private ParticleSystem _shootPrefab;
    [SerializeField] private float _fireRate;
    [SerializeField] private AudioClip _shotSound;
    [SerializeField] private AudioClip _reloadSound;

    private AudioSource _audioSource;
    private Camera _camera;
    private Coroutine _shootCoolDown;
    private Coroutine _playReloadAfterShot;
    private float _maxShootDistance = 100f;
    private bool _canShoot = true;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _canShoot = true;
        _audioSource.Stop();
    }

    private void OnDisable()
    {
        _canShoot = true;
        _audioSource.Stop();

        if (_shootCoolDown != null)
            StopCoroutine(_shootCoolDown);

        if (_playReloadAfterShot != null)
            StopCoroutine(_playReloadAfterShot);
    }

    public void Initialize(Camera camera)
    {
        _camera = camera;
    }

    public override void Attack()
    {
        if (_canShoot == false)
            return;

        StartShootCooldown();

        Vector2 screenCenter = new Vector2(0.5f, 0.5f);
        Ray ray = _camera.ViewportPointToRay(screenCenter);
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, _maxShootDistance))
        {
            targetPoint = hit.point;

            if (hit.collider.TryGetComponent(out Hitbox hitbox))
                hitbox.ApplyDamage(Damage, hit);
        }
        else
        {
            targetPoint = ray.origin + ray.direction * _maxShootDistance;
        }

        Vector3 direction = (targetPoint - _muzzle.position).normalized;
        Instantiate(_shootPrefab, _muzzle.position, Quaternion.LookRotation(direction));

        PlayShotSound();
    }

    private void PlayShotSound()
    {
        _audioSource.clip = _shotSound;
        _audioSource.Play();

        if (_playReloadAfterShot != null)
            StopCoroutine(_playReloadAfterShot);

        _playReloadAfterShot = StartCoroutine(PlayReloadAfterShot());
    }

    private void StartShootCooldown()
    {
        _canShoot = false;

        if (_shootCoolDown != null)
            StopCoroutine(_shootCoolDown);

        _shootCoolDown = StartCoroutine(ShootCooldown());
    }

    private IEnumerator ShootCooldown()
    {
        float time = 0;

        while (time < _fireRate)
        {
            time += Time.deltaTime;
            yield return null;
        }

        _canShoot = true;
    }

    private IEnumerator PlayReloadAfterShot()
    {
        while (_audioSource.isPlaying)
            yield return null;

        _audioSource.clip = _reloadSound;
        _audioSource.Play();
    }
}