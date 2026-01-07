using UnityEngine;

public class FireWeapon : Weapon
{
    [SerializeField] private Transform _muzzle;
    [SerializeField] private ParticleSystem _shootPrefab;

    private Camera _camera;
    private float _maxShootDistance = 1000f;

    public void Initialize(Camera camera) 
    { 
        _camera = camera;
    }

    public override void Attack()
    {
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        Ray ray = _camera.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxShootDistance) == false)
            return;

        Vector3 direction = (hit.point - _muzzle.position).normalized;

        if(hit.collider.TryGetComponent(out Hitbox hitbox))
            hitbox.ApplyDamage(Damage, hit);

        Instantiate(_shootPrefab, _muzzle.position, Quaternion.LookRotation(direction));
    }


}