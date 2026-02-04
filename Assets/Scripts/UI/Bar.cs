using UnityEngine;

public abstract class Bar : MonoBehaviour
{
    protected ICountable Stat;

    public virtual void Initialize(ICountable stat)
    {
        Stat = stat;
    }

    public void Enable()
    {
        Stat.Changed += ChangeValue;
        ChangeValue();
    }

    public void Disable()
    {
        Stat.Changed -= ChangeValue;
    }

    public abstract void ChangeValue();
}
