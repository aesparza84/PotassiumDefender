using UnityEngine;

public class PlayerFootSteps : MonoBehaviour
{
    [SerializeField] private SoundSO sound;

    [Range(.5f, 1f)]
    [SerializeField] private float stepInterval = 0.8f;
    private float currInterval;
    
    //External
    private PlayerMovement movement;
    private PlayerControlHub inputs;

    private bool isGrounded;
    private bool isMoving;
    private void Start()
    {
        if (inputs == null)
            inputs = GetComponent<PlayerControlHub>();
        
        inputs.OnMoveActive += OnWalking;
        inputs.OnMoveStop += OnStopped;

        if (movement == null)
            movement = GetComponent<PlayerMovement>();

        movement.OnJump += Movement_OnJump;
        movement.OnLand += Movement_OnLand;
        currInterval = 0.0f;
    }

    private void OnStopped()
    {
        isMoving = false;
    }

    private void OnWalking(Vector2 obj)
    {
        isMoving = true;
    }

    private void OnDisable()
    {
        if (movement != null)
        {
            movement.OnJump += Movement_OnJump;
            movement.OnLand += Movement_OnLand;
        }
    }

    private void Movement_OnLand(float obj)
    {
        isGrounded = true;
    }

    private void Movement_OnJump()
    {
        isGrounded = false;
    }

    private void Update()
    {
        if (isGrounded && isMoving)
        {
            if (currInterval <= 0.0f)
            {
                currInterval = stepInterval;
                SoundManager.instance.PlaySound(sound, transform);
            }
        }

        //Tick down event when not valid
        if (currInterval > 0.0f)
        {
            currInterval -= Time.deltaTime;
        }
    }
}
