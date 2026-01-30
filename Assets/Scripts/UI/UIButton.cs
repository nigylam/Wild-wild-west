using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioClip _hoverSound;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private TextMeshProUGUI _text;

    private AudioSource _audioSource;
    private Button _button;
    private Coroutine _waitSound;

    public event Action Clicked;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.ignoreListenerPause = true;
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);

        if(_waitSound != null )
            StopCoroutine( _waitSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _audioSource.PlayOneShot(_hoverSound);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _audioSource.PlayOneShot(_clickSound);
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    private void OnClick()
    {
        if (_waitSound  != null)
            StopCoroutine(_waitSound);

        _waitSound = StartCoroutine(RaiseClickedAfterSound());
    }

    private IEnumerator RaiseClickedAfterSound()
    {
        float t = 0;

        while (t < _clickSound.length)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        Clicked?.Invoke();
    }
}
