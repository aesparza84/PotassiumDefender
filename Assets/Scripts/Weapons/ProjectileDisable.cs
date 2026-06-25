using UnityEngine;

public class ProjectileDisable : MonoBehaviour
{
    public DefaultProjectile projectile;

    private void OnTriggerEnter(Collider other)
    {
        projectile.GetComponent<IProjectile>().DisableProjectile();
    }
}
