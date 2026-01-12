using UnityEngine;

public abstract class Bar : MonoBehaviour
{
    protected ICountable Stat;

    private void OnDisable()
    {
        if (Stat != null)
            Stat.Changed -= ChangeValue;
    }

    public virtual void Initialize(ICountable stat)
    {
        if (Stat != null)
            Stat.Changed -= ChangeValue;

        Stat = stat;
        stat.Changed += ChangeValue;
    }

    public abstract void ChangeValue();
}
