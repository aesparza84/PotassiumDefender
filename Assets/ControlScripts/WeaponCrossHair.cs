using UnityEngine;

public class WeaponCrossHair : MonoBehaviour
{
    [SerializeField] private GameObject crossHairCanvas;

    private void OnEnable()
    {
        MenuCameraManager.OnEnterGameplay += Activate;
        GameCurator.OnCleanUpGame += DeActivate;
    }

    private void OnDisable()
    {
        MenuCameraManager.OnEnterGameplay -= Activate;
        GameCurator.OnCleanUpGame -= DeActivate;
    }

    private void DeActivate()
    {
        if (crossHairCanvas != null)
            crossHairCanvas.SetActive(false);
    }

    private void Activate()
    {
        if (crossHairCanvas != null)
            crossHairCanvas.SetActive(true);
    }
}
