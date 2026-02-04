using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundSet", menuName = "Round / Create round set", order = 51)]
public class RoundSet : ScriptableObject
{
    [SerializeField] private List<Round> _rounds;
    [SerializeField] private float _roundDelay;

    private int _currentRound;

    public float RoundDelay => _roundDelay;
    public int RoundsCount => _rounds.Count;

    public Round CurrentRound => _rounds[_currentRound];

    public void StartSet()
    {
        _currentRound = 0;
    }

    public void ProcessRound()
    {
        if (_currentRound + 1 < _rounds.Count)
            _currentRound++;
    }
}
