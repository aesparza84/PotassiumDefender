using UnityEngine;

public class DefaultProjectile : MonoBehaviour, IProjectile
{
    [SerializeField] private GameObject projectileObject;
    
    private int fillAmount; 
    private float speed;
    private Vector3 direction;

    private Vector3 startPos;
    private float maxDistance;
    private float currDistance;

    private bool isActive;

    private void Start()
    {
    
    }
    private void Update()
    {
        if (isActive)
        {
            if (Vector3.Distance(startPos, transform.position) > maxDistance)
                DisableProjectile();

            transform.position += (direction.normalized * speed * Time.deltaTime);
        }
    }
    public void DisableProjectile()
    {
        isActive = false;
        projectileObject.SetActive(false);

        //Destroy
        Destroy(gameObject);
    }
    public void FillEnemy(Animal a)
    {
        Debug.Log("Animal Fed");

        //--Animal feed Interface
        //a.Feed(fillAmount)
    }

    /// <summary>
    /// Positions and activates the projectile. Used when calling weapon.Shoot()
    /// </summary>
    /// <param name="weaponPoint"></param>
    public void AlignAndShoot(Vector3 targetPos)
    {
        if (projectileObject != null)
            projectileObject.SetActive(true);

        //Align
        direction = targetPos - transform.position;

        startPos = transform.position;
        isActive = true;
    }
    public void SetFillAmount(int amount)
    {
        this.fillAmount = amount;
    }
    public void SetSpeed(float weaponSpeed)
    {
        this.speed = weaponSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Animal"))
        {
            Debug.Log("Animal Hit");
        }

        DisableProjectile();
    }
    public void SetMaxDistance(float maxDist)
    {
        this.maxDistance = maxDist;
    }
}
