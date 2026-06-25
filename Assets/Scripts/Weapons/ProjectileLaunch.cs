using UnityEngine;

public class ProjectileLaunch : MonoBehaviour
{
    public DefaultProjectile projectile;
    public Transform shootPoint;

    public float speed;

    private IProjectile projectileAPI;
    private void Start()
    {
        projectileAPI = projectile.GetComponent<IProjectile>();
    
        projectileAPI.SetFillAmount(1);

    }
    private void OnTriggerEnter(Collider other)
    {
        projectileAPI.SetSpeed(speed);

        projectile.AlignAndShoot(shootPoint);
    }
}
