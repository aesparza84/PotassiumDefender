using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Scriptable Objects/PlayerMovementSettings")]
public class PlayerMovementSettings : ScriptableObject
{
    [Header("Ground Check")]
    public float groundCheckSize = .35f;
    public float groundCheckDistance = .5f;

    [Header("Jump")]
    public float jumpHeight = 0;
    public float gravityAccel = 0;
    public float gravity = 9;

    [Header("Speed")]
    public float horizontalAcceleration = 4;
    public float maxHorizontalSpeed = 10;
    public float maxFallSpeed;

    [Header("Drag")]
    public float movementDrag = 2;
    public float idleDrag = 5;
}
