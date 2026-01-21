using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable, ICountable
{
    [SerializeField] private float _maxHealth = 100f;

    private float _currentHealth;

    public event Action Dead;
    public event Action Changed;
    public event Action<Vector3> Hited;

    public float Max => _maxHealth;

    public float Current
    {
        get { return _currentHealth; }
        private set
        {
            _currentHealth = value;
            Changed?.Invoke();
        }
    }

    private void OnEnable()
    {
        Restart();
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Current -= damage;

        if (Current <= 0)
            Dead?.Invoke();
        else
            Hited?.Invoke(hitPoint);
    }

    public void Restart()
    {
        Current = _maxHealth;
    }
}
