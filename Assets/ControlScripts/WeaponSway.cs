using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("External")]
    [SerializeField] private PlayerControlHub playerInputs;

    private Vector3 mouseVector;
    private Vector3 moveVector;
    private Vector3 basePos;

    [SerializeField] private float swayPosLerp;
    [SerializeField] private float swayRotLerp;
    [SerializeField] private float bobSpeed;
    [SerializeField] private float bobDistance;
    private float bobTimer;

    [Header("Sway Distance")]
    [SerializeField] private float posStep = 0.01f;
    [SerializeField] private float maxPosStep = 0.06f;
    private Vector3 swayPos;

    [Header("Sway Rotation")]
    [SerializeField] private float rotStep = 4f;
    [SerializeField] private float maxRotStep = 5f;
    private Vector3 swayEuler;

    private void Start()
    {
        basePos = transform.localPosition;

        if (playerInputs == null)
            return;

        playerInputs.OnMouseAimActive += OnAim;
        playerInputs.OnMouseAimStop += OnAimStop;
        playerInputs.OnMoveActive += OnMoveActive;
        playerInputs.OnMoveStop += OnMoveStop;
    }

    private void OnMoveStop()
    {
        moveVector = Vector3.zero;
    }

    private void OnMoveActive(Vector2 obj)
    {
        moveVector = obj;
    }

    private void OnAimStop()
    {
        this.mouseVector = Vector2.zero;
    }

    private void OnAim(Vector2 obj)
    {
        this.mouseVector = obj;
    }

    private void Update()
    {
        //SwayWeaponPos();
        SwayWeaponRotation();
        Align();

        if (moveVector != Vector3.zero)
        {
            Bob();
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition , basePos , 5 * Time.deltaTime);
        }
    }

    private void SwayWeaponPos()
    {
        Vector3 invert = mouseVector * -posStep;
        invert.x = Mathf.Clamp(invert.x, -maxPosStep, maxPosStep);
        invert.y = Mathf.Clamp(invert.y, -maxPosStep, maxPosStep);
        swayPos = invert;
    }

    private void SwayWeaponRotation()
    {
        Vector3 invert = mouseVector * -rotStep;
        invert.x = Mathf.Clamp(invert.x, -maxRotStep, maxRotStep);
        invert.y = Mathf.Clamp(invert.y, -maxRotStep, maxRotStep);
        swayEuler = new Vector3(invert.y, invert.x, invert.x);
    }

    private void Bob()
    {
        if (bobTimer > float.MaxValue)
            bobTimer = 0.0f;

        bobTimer += Time.deltaTime;
        float nextY = -Mathf.Abs(Mathf.Sin(bobTimer * bobSpeed) * bobDistance);
        float nextX = Mathf.Cos(bobTimer * bobSpeed) * bobDistance;

        transform.localPosition = new Vector3(
            nextX,
            nextY,
            transform.localPosition.z);
    }


    private void Align()
    {
        //transform.localPosition = Vector3.Lerp(
        //    transform.localPosition,
        //    basePos + (swayPos) + bobPos,
        //    swayPosLerp*Time.deltaTime);
        
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation, 
            Quaternion.Euler(swayEuler) ,
            swayRotLerp * Time.deltaTime);
    }

    private void OnDisable()
    {
        if (playerInputs == null)
            return;

        playerInputs.OnMouseAimActive -= OnAim;
        playerInputs.OnMouseAimStop -= OnAimStop;

        playerInputs.OnMoveActive += OnMoveActive;
        playerInputs.OnMoveStop += OnMoveStop;
    }
}
