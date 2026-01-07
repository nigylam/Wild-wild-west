using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float Damage;

    public abstract void Attack();
}
