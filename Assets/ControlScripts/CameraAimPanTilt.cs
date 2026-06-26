using Unity.Cinemachine;
using UnityEngine;

public class CameraAimPanTilt : MonoBehaviour
{
    [SerializeField] private Transform cameraAnchor;
    [SerializeField] private Transform playerBody;


    [SerializeField] private float x_sensitivity;
    [SerializeField] private float y_sensitivity;
    [SerializeField] private bool invertY;

    private CinemachineInputAxisController cinemachineAim;

    private void Start()
    {
        cinemachineAim = GetComponent<CinemachineInputAxisController>();

        Cursor.lockState = CursorLockMode.Locked;

        //Initial set using the Editor fields
        SetCameraSensitivity(x_sensitivity, y_sensitivity);
    }

    private void Update()
    {
        cameraAnchor.rotation = transform.rotation;
        playerBody.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }   
    
    private void SetCameraSensitivity(float x, float y)
    {
        foreach (var c in cinemachineAim.Controllers)
        {
            if (c.Name == "Look X (Pan)")
            {
                c.Input.Gain = x;
            }
            else if (c.Name == "Look Y (Tilt)")
            {
                if (invertY)
                    y *= -1;

                c.Input.Gain = y;
            }
        }
    }

}
