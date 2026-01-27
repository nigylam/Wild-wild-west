using UnityEngine;

public class MeleeWeaponSound : WeaponSound
{
    [SerializeField] private AudioClip _damageSound;

    public void PlayDamageSound()
    {
        PlaySound(_damageSound);
    }
}