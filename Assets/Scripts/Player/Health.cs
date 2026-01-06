using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 100f;

    private float _currentHealth;

    public event Action Dead;

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
            Dead?.Invoke();
    }
}
