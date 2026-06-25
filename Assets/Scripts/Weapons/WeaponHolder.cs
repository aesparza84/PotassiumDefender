using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    /// <summary>
    /// The origin of the aiming ray, same point as Camera
    /// </summary>
    [SerializeField] private Transform rayOrigin;

    /// <summary>
    /// How far the ray will shoot out
    /// </summary>
    [SerializeField][Range(1,60)] private float maxRayDistance;

    /// <summary>
    /// Size of the Raycast-sphere
    /// </summary>
    [SerializeField][Range(0.1f, 1)] private float aimRayRadius;

    private BaseWeapon heldWeapon;

    //Debug
    private Vector3 hitPoint;

    private void Start()
    {
        
    }

    private void Update()
    {
        PerformRayCast();
    }
    private void PerformRayCast()
    {
        hitPoint = rayOrigin.transform.position + (rayOrigin.transform.forward * maxRayDistance);

        if (Physics.SphereCast(rayOrigin.position, aimRayRadius, rayOrigin.forward, out RaycastHit hit, maxRayDistance))
        {
            hitPoint = hit.point;
        }
    }
    public void ShootWeapon()
    {

    }

    private void OnDrawGizmos()
    {
        if (rayOrigin != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(rayOrigin.transform.position, rayOrigin.transform.position + rayOrigin.transform.forward * maxRayDistance);

            Vector3 tip = rayOrigin.transform.position + (rayOrigin.transform.forward * maxRayDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(tip, aimRayRadius);
            Gizmos.color = Color.greenYellow;
            Gizmos.DrawWireSphere(hitPoint, aimRayRadius);
        }
    }
}
