using UnityEngine;

public class WeaponFollower : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    private void Update()
    {
        if (targetTransform != null)
        {
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
        }
    }
}
