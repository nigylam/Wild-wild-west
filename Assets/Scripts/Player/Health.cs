using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 100f;

    private float _currentHealth;

    public event Action Dead;
    public event Action Changed;

    public float Current => _currentHealth;
    public float Max => _maxHealth;

    private void OnEnable()
    {
        Restart();
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        _currentHealth -= damage;
        Changed?.Invoke();

        if (_currentHealth <= 0)
            Dead?.Invoke();
    }

    public void Restart()
    {
        _currentHealth = _maxHealth;
        Changed?.Invoke();
    }
}
