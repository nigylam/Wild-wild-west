using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayMenu : MonoBehaviour
{
    private const string LoseEndText = "Potracheno";
    private const string WinEndText = "Respect +";

    [SerializeField] private Button _restartButton;
    [SerializeField] private TextMeshProUGUI _endText;

    public event Action Restarted;

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestart);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestart);
    }

    public void SetWinText() 
    {
        _endText.text = WinEndText;
    }

    public void SetLoseText()
    {
        _endText.text = LoseEndText;
    }

    private void OnRestart()
    {
        Restarted?.Invoke();
    }
}
