using System;

public interface ICountable
{
    public float Max { get; }
    public float Current { get; }

    public event Action Changed;
}
