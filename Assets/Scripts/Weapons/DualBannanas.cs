using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class DualBannanas : BaseWeapon, IWeapon
{
    [SerializeField] private Transform shootPointLeft;
    [SerializeField] private Transform shootPointRight;

    /// <summary>
    /// Position that the weapon should aim at
    /// </summary>
    private Vector3 hitPoint;

    private bool useRightPoint;
    private void Start()
    {
        useRightPoint = false;
    }
    private void Update()
    {
        if (currDelay > 0)
        {
            currDelay -= Time.deltaTime;
        } else
        {
            currDelay = 0.0f;
        }
    }
    public void Shoot()
    {
        if (currDelay > 0.0f)
            return;

        currDelay = shotDelay;

        GameObject obj;

        Vector3 dir;
        Quaternion rot;
        if (useRightPoint)
        {
            dir = hitPoint - shootPointRight.position;
            rot = Quaternion.LookRotation(dir);
            obj = Instantiate(this.projectilePrefab, shootPointRight.position, rot);
        }
        else
        {
            dir = hitPoint - shootPointLeft.position;
            rot = Quaternion.LookRotation(dir);
            obj = Instantiate(this.projectilePrefab, shootPointLeft.position, rot);
        }

        IProjectile proj = obj.GetComponent<IProjectile>();
        proj.SetFillAmount(this.fillAmount);
        proj.SetSpeed(this.projectileSpeed);
        proj.SetMaxDistance(this.projectileMaxDistance);
        proj.AlignAndShoot(hitPoint);

        useRightPoint = !useRightPoint;
    }

    public void UpdateHitPoint(Vector3 hitPoint)
    {
        this.hitPoint = hitPoint;
    }
}