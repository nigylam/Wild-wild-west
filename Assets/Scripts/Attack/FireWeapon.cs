using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FireWeaponSound))]
public class FireWeapon : Weapon
{
    [SerializeField] private Transform _muzzle;
    [SerializeField] private ParticleSystem _shotEffect;
    [SerializeField] private EffectSpawner _environmentHitEffectSpawner;

    private FireWeaponSound _sound;
    private Camera _camera;
    private float _maxShootDistance = 100f;

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public void Initialize(Camera camera)
    {
        _camera = camera;
        _sound = GetComponent<FireWeaponSound>();
    }

    protected override void Attack()
    {
        if (CanAttack == false)
            return;

        base.Attack();

        Vector2 screenCenter = new Vector2(0.5f, 0.5f);
        Ray ray = _camera.ViewportPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxShootDistance))
        {
            if (hit.collider.TryGetComponent(out Hitbox hitbox))
                hitbox.ApplyDamage(Damage, hit.point, hit.normal);
            else
                _environmentHitEffectSpawner.Spawn(hit.point, Quaternion.LookRotation(hit.normal), transform);
        }

        _shotEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _shotEffect.Play();
        _sound.PlayAttackSound();
    }
}