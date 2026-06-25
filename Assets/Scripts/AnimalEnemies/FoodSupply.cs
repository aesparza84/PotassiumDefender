using UnityEngine;

public class FoodSupply : MonoBehaviour
{
    [SerializeField]
    private int supplyAmount;


    void Start()
    {
        supplyAmount = supplyAmount == 0 || supplyAmount < 50 ? 100 : supplyAmount; 
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Animal"))
        {
            supplyAmount -= collision.gameObject.GetComponent<Animal>().AnimalHunger;
            collision.gameObject.SetActive(false);

            if (supplyAmount <= 0) 
            {
                DeactivateSupply();
            }
        }
    }

    private void DeactivateSupply()
    {
        gameObject.SetActive(false);
    }
}
