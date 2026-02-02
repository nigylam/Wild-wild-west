using System;
using UnityEngine;

public class Health : MonoBehaviour, ICountable
{
    [SerializeField] private float _maxHealth = 100f;

    private float _currentHealth;
    private bool _active = true;

    public event Action Dead;
    public event Action Changed;

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

    public void TakeDamage(float damage)
    {
        if(_active == false) 
            return;

        Current -= damage;

        if (Current <= 0)
        {
            _active = false;
            Dead?.Invoke();
        }
    }

    public void Restart()
    {
        Current = _maxHealth;
        _active = true;
    }
}
