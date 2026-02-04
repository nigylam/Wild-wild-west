using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private SmoothSliderBar _healthBar;
    [SerializeField] private TextBar _roundCounter;

    public void Initialize(ICountable roundCounter, Player player)
    {
        _roundCounter.Initialize(roundCounter);
        player.SetHealthBar(_healthBar);
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
