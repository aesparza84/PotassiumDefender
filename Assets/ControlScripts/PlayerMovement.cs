using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Raw input reading
    /// </summary>
    private PlayerControlHub inputHub;

    [Header("Components")]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform groundCheckOrigin;
    
    [Header("Ground Check")]
    [SerializeField] private float groundCheckSize = .35f;
    [SerializeField] private float groundCheckDistance = .5f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;


    [Header("Speed")]
    [SerializeField] private float horizontalAcceleration = 4;
    [SerializeField] private float maxHorizontalSpeed = 10;
    [SerializeField] private float maxFallSpeed;

    [Header("Drag")]
    [SerializeField] private float movementDrag = 1;
    [SerializeField] private float idleDrag = 5;

    private float currGravity;
    private float currSpeed;
    private float currFall;
    private Vector3 prevDirInput;
    private Vector3 dirInput;
    private Vector3 finalHorizontal;

    private bool jumpPressed;
    private bool isGrounded;

    void Start()
    {
        if (rigidBody == null)
            rigidBody = gameObject.GetComponent<Rigidbody>();

        if (inputHub == null)
            inputHub = gameObject.GetComponent<PlayerControlHub>();

        inputHub.OnMoveActive += OnMoveActive;
        inputHub.OnMoveStop += OnMoveStop;
        
        inputHub.OnJumpActive += OnJumpActive;
        inputHub.OnJumpStop += OnJumpStop;
    }

    private void OnJumpStop()
    {
        jumpPressed = false;
    }

    private void OnMoveStop()
    {
        dirInput.x = 0;
        dirInput.z = 0;
    }

    private void OnJumpActive()
    {
        Jump();
    }
    private void OnMoveActive(Vector2 obj)
    {
        dirInput.x = obj.x;
        dirInput.z = obj.y;
    }

    private void FixedUpdate()
    {
        HorizontalMovement();
        GroundCheck();
        VerticalMovement();
    }


    private void Jump()
    {
        if (!isGrounded || jumpPressed) 
            return;

        jumpPressed = true;

        Vector3 jumpForce = new Vector3(0, Mathf.Sqrt(-2.0f* currGravity * jumpHeight),0);
        rigidBody.AddForce(jumpForce, ForceMode.Force);
    }
    private void VerticalMovement()
    {
        if (!isGrounded)
        {
            currGravity += gravity * Time.fixedDeltaTime;
            rigidBody.AddForce(Vector3.down * currGravity,ForceMode.Force);

            //clamp gravity
            Vector3 verticalMag = new Vector3(0, rigidBody.linearVelocity.y, 0);
            if (verticalMag.magnitude > maxFallSpeed)
            {
                verticalMag = verticalMag.normalized * maxFallSpeed;
                rigidBody.linearVelocity = new Vector3(rigidBody.linearVelocity.x, verticalMag.y, rigidBody.linearVelocity.z);
            }
            
            rigidBody.linearDamping = 0.0f;
        }
        else
        {
            currGravity = 0.0f;
        }

        currFall = new Vector3(0, rigidBody.linearVelocity.y, 0).magnitude;
    }
    private void GroundCheck()
    {
        if (groundCheckOrigin == null)
            return;

        Vector3 origin = new Vector3(groundCheckOrigin.position.x,
                groundCheckOrigin.position.y - groundCheckDistance,
                groundCheckOrigin.position.z);

        if (Physics.SphereCast(groundCheckOrigin.position,
            groundCheckSize,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void HorizontalMovement()
    {
        if (dirInput != Vector3.zero)
        {
            if (rigidBody.linearDamping != movementDrag)
                rigidBody.linearDamping = movementDrag;

            finalHorizontal = (transform.forward * dirInput.z) + (transform.right * dirInput.x);

            rigidBody.AddForce(finalHorizontal.normalized * horizontalAcceleration * Time.fixedDeltaTime, ForceMode.VelocityChange);
            
            prevDirInput = dirInput;
        }
        else
        {
            if (rigidBody.linearDamping != idleDrag)
                rigidBody.linearDamping = idleDrag;
        }

        //Clamp Horizontal Velocity
        Vector3 horizontalVel = new Vector3(rigidBody.linearVelocity.x, 0, rigidBody.linearVelocity.z);

        if (horizontalVel.magnitude >= maxHorizontalSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxHorizontalSpeed;
            rigidBody.linearVelocity = new Vector3(horizontalVel.x, rigidBody.linearVelocity.y, horizontalVel.z);
        }

        currSpeed = rigidBody.linearVelocity.magnitude;
    }

    private void OnDisable()
    {
        if (inputHub == null)
            return;

        inputHub.OnMoveActive -= OnMoveActive;
        inputHub.OnMoveStop -= OnMoveStop;
        inputHub.OnJumpActive -= OnJumpActive;
        inputHub.OnJumpStop -= OnJumpStop;
    }

    private void OnDrawGizmos()
    {
        if (groundCheckOrigin != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 origin = new Vector3(groundCheckOrigin.position.x,
                groundCheckOrigin.position.y - groundCheckDistance,
                groundCheckOrigin.position.z);

            Gizmos.DrawWireSphere(origin, groundCheckSize);
        }
    }
}
