using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OverlayMenu : MonoBehaviour
{
    private const string LoseEndText = "Potracheno";
    private const string WinEndText = "Respect +";

    [SerializeField] private Button _restartButton;
    [SerializeField] private TextMeshProUGUI _endText;

    private UIButtonSound _sound;
    private Coroutine _waitSound;

    public event Action Restarted;

    private void Awake()
    {
        _sound = _restartButton.GetComponent<UIButtonSound>();
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestart);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestart);

        if (_waitSound != null)
            StopCoroutine(_waitSound);
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
        if(_waitSound != null)
            StopCoroutine(_waitSound);

        _waitSound = StartCoroutine(RaiseRestartedAfterSound());
    }

    private IEnumerator RaiseRestartedAfterSound()
    {
        float t = 0;

        while(t < _sound.ClickSoundLength)
        {
            t += Time.deltaTime;
            yield return null;
        }

        Restarted?.Invoke();
    }
}
