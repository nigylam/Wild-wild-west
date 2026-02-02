using UnityEngine;

[CreateAssetMenu(fileName = "Round", menuName = "Round / Create round", order = 51)]
public class Round : ScriptableObject
{
    [SerializeField] private int _enemiesCount;
    [SerializeField] private int _bossesCount;
    [SerializeField] private float _roundLength;

    public int EnemiesCount => _enemiesCount;
    public int BossesCount => _bossesCount;
    public float RoundLength => _roundLength;
}
