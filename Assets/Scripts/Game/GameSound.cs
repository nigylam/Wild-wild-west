using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class GameSound : MonoBehaviour
{
    [SerializeField] private AudioClip _startRound;
    [SerializeField] private AudioClip _endRound;
    [SerializeField] private AudioClip _winGame;
    [SerializeField] private AudioClip _loseGame;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }

    public void PlayStartRound()
    {
        _audioSource.PlayOneShot(_startRound);
    }

    public void PlayEndRound()
    {
        _audioSource.PlayOneShot(_endRound);
    }

    public void PlayWinGame()
    {
        _audioSource.PlayOneShot(_winGame);
    }

    public void PlayLoseGame()
    {
        _audioSource.PlayOneShot(_loseGame);
    }
}
