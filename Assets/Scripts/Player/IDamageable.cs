using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
