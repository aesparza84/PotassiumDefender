using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    protected float shotDelay;
    [SerializeField] protected int fillAMount;

    // -- Projectile prefab
    [SerializeField] private GameObject projectilePrefab;
    private IProjectile projectileInterface;

    
}
