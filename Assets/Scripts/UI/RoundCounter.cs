using System;
using UnityEngine;

public class RoundCounter : MonoBehaviour, ICountable
{
    private int _currentRound = 1;

    public float Max => int.MaxValue;

    public float Current
    {
        get { return _currentRound; }
        private set 
        { 
            _currentRound = (int)value;
            Changed?.Invoke();
        }
    }

    public event Action Changed;
     
    public void Increase()
    {
        Current++;
    }

    public void Reset()
    {
        Current = 1;
    }
}
