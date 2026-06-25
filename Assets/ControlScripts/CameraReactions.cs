using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CameraReactions : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovment;

    private CinemachineVirtualCamera camera;
    private CinemachineBasicMultiChannelPerlin cameraNoise;
    private void Start()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        cameraNoise = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();


        if (playerMovment != null)
        {
            playerMovment.OnMove += ReactOnMove;
            playerMovment.OnIdle += ReactOnIdle;
            playerMovment.OnJump += ReactOnJump;
            playerMovment.OnLand += ReactOnLand;
        }
    }

    private void OnDisable()
    {
        if (playerMovment != null)
        {
            playerMovment.OnMove -= ReactOnMove;
            playerMovment.OnIdle -= ReactOnIdle;
            playerMovment.OnJump -= ReactOnJump;
            playerMovment.OnLand -= ReactOnLand;
        }
    }

    private void ReactOnLand(float obj)
    {
        
    }

    private void ReactOnJump()
    {

    }

    private void ReactOnIdle()
    {

    }

    private void ReactOnMove(Vector2 obj)
    {

    }

    private void Update()
    {
        
    }
}
