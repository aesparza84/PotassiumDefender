using UnityEngine;

public class DualBannanas : BaseWeapon, IWeapon
{
    [Header("External")]
    [SerializeField] private Transform shootPointLeft;
    [SerializeField] private Transform shootPointRight;
    [SerializeField] private Transform BanannaLeft;
    [SerializeField] private Transform BanannaRight;
    
    //Recoil fields
    private Vector3 BanannaLeft_StartPos;
    private Quaternion BanannaLeft_StartQuat;
    private Vector3 BanannaRight_StartPos;
    private Quaternion BanannaRight_StartQuat;

    [Header("Recoil")]
    [SerializeField] private float posLerpSpeed;
    [SerializeField] private float rotLerpSpeed;
    [SerializeField] private Vector3 recoilVector;
    [SerializeField] private Vector3 recoilEuler;

    /// <summary>
    /// Position that the weapon should aim at
    /// </summary>
    private Vector3 hitPoint;

    private bool useRightPoint;
    private void Start()
    {
        BanannaLeft_StartPos = BanannaLeft.localPosition;
        BanannaLeft_StartQuat = BanannaLeft.localRotation;
        BanannaRight_StartPos = BanannaRight.localPosition;
        BanannaRight_StartQuat = BanannaRight.localRotation;


        useRightPoint = false;
    }
    private void Update()
    {
        if (currDelay > 0)
        {
            currDelay -= Time.deltaTime;
        } else
        {
            currDelay = 0.0f;
        }

        RealignWeapon();
    }

    private void RealignWeapon()
    {
        //Local positions
        if (BanannaLeft.localPosition != BanannaLeft_StartPos)
        {
            Vector3 nextPos = Vector3.Lerp(BanannaLeft.localPosition, BanannaLeft_StartPos, posLerpSpeed * Time.deltaTime);
            BanannaLeft.localPosition = nextPos;
        }

        if (BanannaRight.localPosition != BanannaRight_StartPos)
        {
            Vector3 nextPos = Vector3.Lerp(BanannaRight.localPosition, BanannaRight_StartPos, posLerpSpeed * Time.deltaTime);
            BanannaRight.localPosition = nextPos;
        }

        //Local rotations
        if (BanannaRight.localRotation != BanannaRight_StartQuat)
        {
            BanannaRight.localRotation = Quaternion.Lerp(BanannaRight.localRotation, BanannaRight_StartQuat, rotLerpSpeed * Time.deltaTime);
        }

        if (BanannaLeft.localRotation != BanannaLeft_StartQuat)
        {
            BanannaLeft.localRotation = Quaternion.Lerp(BanannaLeft.localRotation, BanannaLeft_StartQuat, rotLerpSpeed * Time.deltaTime);
        }
    }


    public void Shoot()
    {
        if (currDelay > 0.0f)
            return;

        currDelay = shotDelay;

        GameObject obj;

        Vector3 dir;
        Quaternion rot;
        if (useRightPoint)
        {
            dir = hitPoint - shootPointRight.position;
            rot = Quaternion.LookRotation(dir);
            obj = Instantiate(this.projectilePrefab, shootPointRight.position, rot);

            //Apply recoil
            BanannaRight.localPosition += recoilVector;
            BanannaRight.localRotation *= Quaternion.Euler(recoilEuler);
            //BanannaRight.localRotation *= Quaternion.AngleAxis(35, Vector3.right);
        }
        else
        {
            dir = hitPoint - shootPointLeft.position;
            rot = Quaternion.LookRotation(dir);
            obj = Instantiate(this.projectilePrefab, shootPointLeft.position, rot);

            //Apply recoil
            BanannaLeft.localPosition += recoilVector;
            BanannaLeft.localRotation *= Quaternion.Euler(recoilEuler);
            //BanannaLeft.localRotation *= Quaternion.AngleAxis(35, Vector3.right);
        }

        IProjectile proj = obj.GetComponent<IProjectile>();
        proj.SetFillAmount(this.fillAmount);
        proj.SetSpeed(this.projectileSpeed);
        proj.SetMaxDistance(this.projectileMaxDistance);
        proj.AlignAndShoot(hitPoint);

        useRightPoint = !useRightPoint;
    }

    public void UpdateHitPoint(Vector3 hitPoint)
    {
        this.hitPoint = hitPoint;
    }
}