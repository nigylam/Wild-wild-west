using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OverlayMenu : MonoBehaviour
{
    private const string StartText = "Wild wild west";
    private const string PauseText = "Pause";
    private const string LoseEndText = "Potracheno";
    private const string WinEndText = "Respect +";
    private const string StartButtonText = "Start";
    private const string RestartButtonText = "Again";

    [SerializeField] private UIButton _restartButton;
    [SerializeField] private UIButton _continueButton;
    [SerializeField] private TextMeshProUGUI _endText;

    private PauseActions _pauseActions;

    public event Action Restarted;
    public event Action Continued;

    public void Initialize(PauseActions actions)
    {
        _pauseActions = actions;
    }

    public void SetStartMenu()
    {
        gameObject.SetActive(true);
        _endText.text = StartText;
        _continueButton.gameObject.SetActive(false);
        _restartButton.SetText(StartButtonText);
        _restartButton.Clicked += OnRestart;
    }

    public void SetPauseMenu()
    {
        gameObject.SetActive(true);
        _endText.text = PauseText;
        _continueButton.gameObject.SetActive(true);
        _restartButton.SetText(RestartButtonText);
        _restartButton.Clicked += OnRestart;
        _continueButton.Clicked += OnContinue;
        _pauseActions.PauseAction.Pause.performed += OnPausePressed;
    }

    public void SetWinMenu() 
    {
        SetEndMenu(true);
    }

    public void SetLoseMenu()
    {
        SetEndMenu(false);
    }

    private void SetEndMenu(bool isWin)
    {
        gameObject.SetActive(true);
        _continueButton.gameObject.SetActive(false);
        _restartButton.SetText(RestartButtonText);
        _restartButton.Clicked += OnRestart;

        if (isWin)
            _endText.text = WinEndText;
        else
            _endText.text = LoseEndText;
    }

    private void OnRestart()
    {
        _restartButton.Clicked -= OnRestart;
        _continueButton.Clicked -= OnContinue;
        _pauseActions.PauseAction.Pause.performed -= OnPausePressed;
        EventSystem.current.SetSelectedGameObject(null);
        gameObject.SetActive(false);
        Restarted?.Invoke();
    }

    private void OnPausePressed(InputAction.CallbackContext context) 
    {
        OnContinue();
    }

    private void OnContinue()
    {
        _continueButton.Clicked -= OnContinue;
        _restartButton.Clicked -= OnRestart;
        _pauseActions.PauseAction.Pause.performed -= OnPausePressed;
        EventSystem.current.SetSelectedGameObject(null);
        gameObject.SetActive(false);
        Continued?.Invoke();
    }
}
