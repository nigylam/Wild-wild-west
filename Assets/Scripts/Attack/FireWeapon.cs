using System.Collections;
using UnityEngine;

public class FireWeapon : Weapon
{
    [SerializeField] private Transform _muzzle;
    [SerializeField] private ParticleSystem _shotPrefab;
    [SerializeField] private AudioClip _reloadSound;

    private Camera _camera;
    private Coroutine _playReloadAfterShot;
    private float _maxShootDistance = 100f;

    protected override void OnDisable()
    {
        base.OnDisable();

        if (_playReloadAfterShot != null)
            StopCoroutine(_playReloadAfterShot);
    }

    public void Initialize(Camera camera)
    {
        _camera = camera;
    }

    protected override void Attack()
    {
        if (CanAttack == false)
            return;

        base.Attack();

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
        Instantiate(_shotPrefab, _muzzle.position, Quaternion.LookRotation(direction));
    }

    protected override void PlaySound()
    {
        base.PlaySound();

        if (_playReloadAfterShot != null)
            StopCoroutine(_playReloadAfterShot);

        _playReloadAfterShot = StartCoroutine(PlayReloadAfterShot());
    }

    private IEnumerator PlayReloadAfterShot()
    {
        while (AudioSource.isPlaying)
            yield return null;

        AudioSource.clip = _reloadSound;
        AudioSource.Play();
    }
}