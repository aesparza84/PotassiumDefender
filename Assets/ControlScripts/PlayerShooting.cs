using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    /// <summary>
    /// Raw input reading
    /// </summary>
    private PlayerControlHub inputHub;

    /// <summary>
    /// Attatched weapon container
    /// </summary>
    [SerializeField] private WeaponHolder weaponHolder;

    private void Awake()
    {
        if (inputHub == null)
            inputHub = gameObject.GetComponent<PlayerControlHub>();

        inputHub.OnShootActive += OnShootActive;
        inputHub.OnShootStop += OnShootStop;
    }

    private void OnShootStop()
    {

    }

    private void OnShootActive()
    {
        if (weaponHolder == null)
            return;

        weaponHolder.ShootWeapon();
    }

    private void OnDisable()
    {
        if (inputHub == null)
            return;

        inputHub.OnShootActive -= OnShootActive;
        inputHub.OnShootStop -= OnShootStop;
    }
}
