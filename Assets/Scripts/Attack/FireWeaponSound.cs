using System;
using System.Collections;
using UnityEngine;

public class FireWeaponSound : WeaponSound
{
    [SerializeField] private AudioClip _reloadSound;

    private Coroutine _playReloadAfterShot;

    public event Action ShotSoundPlayed;

    private void OnDisable()
    {
        if (_playReloadAfterShot != null)
            StopCoroutine(_playReloadAfterShot);
    }

    public override void PlayAttackSound()
    {
        base.PlayAttackSound();

        if (_playReloadAfterShot != null)
            StopCoroutine(_playReloadAfterShot);

        _playReloadAfterShot = StartCoroutine(PlayReloadAfterShot());
    }

    private IEnumerator PlayReloadAfterShot()
    {
        while (AudioSource.isPlaying)
            yield return null;

        ShotSoundPlayed?.Invoke();
        PlaySound(_reloadSound);
    }
}
