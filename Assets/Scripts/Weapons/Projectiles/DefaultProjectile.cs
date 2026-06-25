using UnityEngine;

public class DefaultProjectile : MonoBehaviour, IProjectile
{
    [SerializeField] private GameObject projectileObject;
    
    private int fillAmount; 
    private float speed;
    private Vector3 direction;


    private bool isActive;

    private void Start()
    {
        isActive = false;
    }
    private void Update()
    {
        if (isActive)
        {
            transform.position += (direction * speed * Time.deltaTime);
        }
    }
    public void DisableProjectile()
    {
        isActive = false;
        projectileObject.SetActive(false);
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
    public void AlignAndShoot(Transform weaponPoint)
    {
        if (projectileObject != null)
            projectileObject.SetActive(true);

        //Align
        gameObject.transform.position = weaponPoint.position;
        gameObject.transform.rotation = weaponPoint.rotation;

        direction = weaponPoint.forward;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (CompareTag("Animal"))
        {
            Debug.Log("Animal Hit");
        }

        DisableProjectile();
    }
}
