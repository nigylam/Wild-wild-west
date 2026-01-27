using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioClip _hoverSound;
    [SerializeField] private AudioClip _clickSound;

    private AudioSource _audioSource;

    public float ClickSoundLength => _clickSound.length;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _audioSource.PlayOneShot(_hoverSound);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _audioSource.PlayOneShot(_clickSound);
    }
}
