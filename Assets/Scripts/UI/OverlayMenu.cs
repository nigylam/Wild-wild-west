using System;
using UnityEngine;
using UnityEngine.UI;

public class OverlayMenu : MonoBehaviour
{
    [SerializeField] private Button _restartButton;

    public event Action Restarted;

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestart);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestart);
    }

    private void OnRestart()
    {
        Restarted?.Invoke();
    }
}
