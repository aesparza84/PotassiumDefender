using UnityEngine;

public class CameraAim : MonoBehaviour
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

        Cursor.lockState =  CursorLockMode.Locked;

        inputHub.OnMouseAimActive += OnAimActive;
        inputHub.OnMouseAimStop += OnAimStop;
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
        float x = aimVector.x * Time.deltaTime * x_sensitivity;
        float y = aimVector.y * Time.deltaTime * y_sensitivity;

        yRot += x;
        xRot -= y;

        xRot = Mathf.Clamp(xRot, -90, 90);

        if (cameraAnchor != null)
        {
            cameraAnchor.rotation = Quaternion.Euler(xRot, yRot, 0);
        }

        playerBody.rotation = Quaternion.Euler(0, yRot, 0);
    }

    private void OnDisable()
    {
        if (inputHub == null)
            return;

        inputHub.OnMouseAimActive -= OnAimActive;
        inputHub.OnMouseAimStop -= OnAimStop;
    }
}
