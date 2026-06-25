using UnityEngine;

public class CameraAimPanTilt : MonoBehaviour
{
    [SerializeField] private Transform cameraAnchor;
    [SerializeField] private Transform playerBody;


    [SerializeField] private float x_sensitivity;
    [SerializeField] private float y_sensitivity;


    private float xRot;
    private float yRot;
    private Vector2 aimVector;


    private PlayerControlHub inputHub;

    private void Start()
    {
        if (inputHub == null)
        {
            inputHub = gameObject.GetComponent<PlayerControlHub>();
        }

        Cursor.lockState = CursorLockMode.Locked;

        //inputHub.OnMouseAimActive += OnAimActive;
        //inputHub.OnMouseAimStop += OnAimStop;
    }

    private void OnAimStop()
    {
        aimVector = Vector2.zero;
    }

    private void OnAimActive(Vector2 obj)
    {
        aimVector = obj;
    }

    private void Update()
    {
        cameraAnchor.rotation = transform.rotation;
        playerBody.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    //private void OnDisable()
    //{
    //    if (inputHub == null)
    //        return;

    //    inputHub.OnMouseAimActive -= OnAimActive;
    //    inputHub.OnMouseAimStop -= OnAimStop;
    //}
}
