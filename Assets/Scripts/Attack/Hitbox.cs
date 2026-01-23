using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private float _damageMultiplier = 1f;
    [SerializeField] private Health _health;

    public void ApplyDamage(float baseDamage, Vector3 hitPoint, Vector3 hitNormal)
    {
        float finalDamage = baseDamage * _damageMultiplier;
        _health.TakeDamage(finalDamage, hitPoint, hitNormal);
    }
}
