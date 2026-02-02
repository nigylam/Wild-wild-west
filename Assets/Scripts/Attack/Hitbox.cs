using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private float _damageMultiplier = 1f;

    private IDamageable _damagable;

    public void Initialize(IDamageable damageable)
    {
        _damagable = damageable;
    }

    public void ApplyDamage(float baseDamage, Vector3 hitPoint, Vector3 hitNormal)
    {
        float finalDamage = baseDamage * _damageMultiplier;
        _damagable.TakeDamage(finalDamage, hitPoint, hitNormal);
    }
}
