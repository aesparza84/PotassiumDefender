using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Raw input reading
    /// </summary>
    private PlayerControlHub inputHub;

    [Header("Settings Config")]
    [SerializeField] private PlayerMovementSettings _settings;

    [Header("Components")]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform groundCheckOrigin;
    [SerializeField] private Transform orientationTransform;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckSize = .35f;
    [SerializeField] private float groundCheckDistance = .5f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;
    [SerializeField] private float gravityAccel;

    [Header("Speed")]
    [SerializeField] private float horizontalAcceleration = 4;
    [SerializeField] private float maxHorizontalSpeed = 10;
    [SerializeField] private float maxFallSpeed;

    [Header("Drag")]
    [SerializeField] private float movementDrag = 1;
    [SerializeField] private float idleDrag = 5;

    [SerializeField] private LayerMask groundMask;

    private float currGravity;
    private float currSpeed;
    private float currFall;
    private Vector3 prevDirInput;
    private Vector3 dirInput;
    private Vector3 finalHorizontal;

    private bool jumpPressed;
    private bool isJumping;
    private bool isGrounded;
    private Vector3 groundPoint;


    private float groundCheckDisableTimer=0.5f;
    private float groundCheckTimer;

    //Events
    public event Action OnJump;
    public event Action<float> OnLand;
    //public event Action<Vector2> OnMove;
    //public event Action OnIdle;
    

    private void Awake()
    {
        if (_settings != null)
        {
            this.groundCheckSize = _settings.groundCheckSize;
            this.groundCheckDistance = _settings.groundCheckDistance;
            this.jumpHeight = _settings.jumpHeight;
            this.gravityAccel = _settings.gravityAccel;
            this.gravity = _settings.gravity;
            this.horizontalAcceleration = _settings.horizontalAcceleration;
            this.maxHorizontalSpeed = _settings.maxHorizontalSpeed;
            this.maxFallSpeed = _settings.maxFallSpeed;
            this.movementDrag = _settings.movementDrag;
            this.idleDrag = _settings.idleDrag;
        }
    }
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
        GroundCheck();
        //GravityGroundCheck();
        HorizontalMovement();
        VerticalMovement();
    }

    private void Update()
    {
        //Disable groundcheck
        if (groundCheckTimer > 0.0f)
        {
            groundCheckTimer -= Time.deltaTime;
        }
        else
        {
            groundCheckTimer = 0.0f;
        }

        DebugRays();
    }

    private void DebugRays()
    {
        //Horizontal
        Debug.DrawRay(transform.position, new Vector3(rigidBody.linearVelocity.x, 0, rigidBody.linearVelocity.z), Color.red);

        //Vertical
        Debug.DrawRay(transform.position, new Vector3(0, rigidBody.linearVelocity.y, 0), Color.blue);
    }
    private void Jump()
    {
        if (!isGrounded)
            return;

        if (jumpPressed) 
            return;

        OnJump?.Invoke();

        jumpPressed = true;
        isJumping = true;
        isGrounded = false;

        groundCheckTimer = groundCheckDisableTimer;
        rigidBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }
    private void GroundCheck()
    {
        if (groundCheckOrigin == null)
            return;

        if (groundCheckTimer > 0.0f)
            return;

        //Ray cast the ground sphere
        if (Physics.SphereCast(groundCheckOrigin.position, groundCheckSize, Vector3.down, out RaycastHit hit, groundCheckDistance))
        {           

            //Initial 'Grounded' reset
            if (!isGrounded)
            {
                isGrounded = true;
                isJumping = false;
                OnLand?.Invoke(rigidBody.linearVelocity.y);
                Debug.Log($"Land Vel - {rigidBody.linearVelocity.y}");
            }

            groundPoint = hit.point;

            Vector3 targetPos = rigidBody.position;
            targetPos.y = hit.point.y;
            rigidBody.position = Vector3.Lerp(rigidBody.position, targetPos, Time.fixedDeltaTime * 5);
        }
        else
        {
            //Initial set to 'un-Grounded'
            if (isGrounded)
            {
                isGrounded = false;
            }

        }
    }

    private void HorizontalMovement()
    {
        if (dirInput != Vector3.zero)
        {
            if (rigidBody.linearDamping != movementDrag)
                rigidBody.linearDamping = movementDrag;

            //APply camera direction
            finalHorizontal = (orientationTransform.forward * dirInput.z) + (orientationTransform.right * dirInput.x);

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

        //Debug HOTIZONTAL speed
        currSpeed = new Vector3(rigidBody.linearVelocity.x,0,rigidBody.linearVelocity.z).magnitude;
    }
    private void VerticalMovement()
    {
        if (!isGrounded)
        {
            //apply gravity on the RigidBody
            currGravity += gravity * gravityAccel * Time.fixedDeltaTime;
            rigidBody.AddForce(Vector3.down * currGravity, ForceMode.Force);

            //clamp Y-Magnitude
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

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundPoint, groundCheckSize);
        }
    }
}
