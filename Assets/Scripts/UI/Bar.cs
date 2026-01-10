using UnityEngine;

public abstract class Bar : MonoBehaviour
{
    [SerializeField] protected Health Health;

    private void OnEnable()
    {
        Health.Changed += ChangeValue;
    }

    private void OnDisable()
    {
        Health.Changed -= ChangeValue;
    }

    private void Start()
    {
        ChangeValue();
    }

    public abstract void ChangeValue();
}
