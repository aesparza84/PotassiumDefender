using UnityEngine;

public class FoodSupply : MonoBehaviour
{
    [SerializeField]
    private int supplyAmount;


    void Start()
    {
        supplyAmount = supplyAmount == 0 || supplyAmount < 50 ? 100 : supplyAmount; 
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Animal"))
    //    {
    //        supplyAmount -= collision.gameObject.GetComponent<Animal>().AnimalHunger;

    //        if (supplyAmount <= 0) 
    //        {
    //            DeactivateSupply();
    //        }
    //    }
    //}
    
    public void ReduceAmount()
    {
        this.supplyAmount -= 1;

        if (this.supplyAmount <= 0)
            DeactivateSupply();
    }
    private void DeactivateSupply()
    {
        gameObject.SetActive(false);
    }
}
