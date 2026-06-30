using System;
using UnityEngine;

public class FoodSupply : MonoBehaviour
{
    [SerializeField] private int supplyAmount;
    private int currSupplyCount;

    /// <summary>
    /// Event to trigger game over
    /// </summary>
    public static event Action OnSupplyDestroyed;

    void Start()
    {
        currSupplyCount = supplyAmount;
    }
    
    public void ReduceAmount()
    {
        this.currSupplyCount-= 1;

        if (this.currSupplyCount <= 0)
            DeactivateSupply();
    }
    private void DeactivateSupply()
    {
        OnSupplyDestroyed?.Invoke();
        gameObject.SetActive(false);
    }

    public void RestartSupply()
    {
        currSupplyCount = supplyAmount;
        gameObject.SetActive(true);
    }
}
