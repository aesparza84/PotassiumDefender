using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected float shotDelay;
    [SerializeField] protected int fillAmount;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] [Range(1,100)]protected float projectileMaxDistance;
    protected float currDelay;

    // -- Projectile prefab
    [SerializeField] protected GameObject projectilePrefab;
}
