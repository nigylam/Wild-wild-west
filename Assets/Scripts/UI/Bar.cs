using UnityEngine;

public abstract class Bar : MonoBehaviour
{
    protected ICountable Stat;
    private bool _initialized;

    private void OnEnable()
    {
        if (Stat == null)
            return;

        Stat.Changed += ChangeValue;

        ChangeValue();
    }

    private void OnDisable()
    {
        if (Stat != null)
            Stat.Changed -= ChangeValue;
    }

    public virtual void Initialize(ICountable stat)
    {
        Stat = stat;

        if (_initialized == false)
            Stat.Changed += ChangeValue;

        _initialized = true;
    }

    public abstract void ChangeValue();
}
