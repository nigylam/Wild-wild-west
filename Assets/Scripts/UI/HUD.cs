using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private SmoothSliderBar _healthBar;
    [SerializeField] private TextBar _roundCounter;
    [SerializeField] private Health _playerHealth;

    public void Initialize(ICountable roundCounter)
    {
        _roundCounter.Initialize(roundCounter);
        _healthBar.Initialize(_playerHealth);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        _roundCounter.Enable();
        _healthBar.Enable();
    }

    public void Disable()
    {
        _roundCounter.Disable();
        _healthBar.Disable();
        gameObject.SetActive(false);
    }
}
