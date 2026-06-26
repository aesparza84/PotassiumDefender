using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("External")]
    [SerializeField] private PlayerControlHub playerInputs;

    private Vector3 mouseVector;
    private Vector3 basePos;

    [SerializeField] private float swayPosLerp;
    [SerializeField] private float swayRotLerp;

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
    }

    private void OnAim(Vector2 obj)
    {
        this.mouseVector = obj;
    }

    private void Update()
    {
        SwayWeaponPos();
        SwayWeaponRotation();

        Align();
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

    private void Align()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            basePos + (swayPos),
            swayPosLerp*Time.deltaTime);
        
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation, 
            Quaternion.Euler(swayEuler),
            swayRotLerp * Time.deltaTime);
        
    }

    private void OnDisable()
    {
        if (playerInputs == null)
            return;

        playerInputs.OnMouseAimActive -= OnAim;
    }
}
