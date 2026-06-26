using UnityEngine;

public class WeaponFollower : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    private void Update()
    {
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }

}
