using UnityEngine;

public class WeaponFollower : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    private void FixedUpdate()
    {
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }

}
