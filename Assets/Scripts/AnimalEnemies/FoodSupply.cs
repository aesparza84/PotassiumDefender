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

    /// <summary>
    /// Event fired when losing health
    /// </summary>
    public event Action<float> OnSupplyHit;

    private float startHeight;
    private float endHeight;

    void Start()
    {
        startHeight = transform.position.y;
        endHeight = -startHeight;
        
        currSupplyCount = supplyAmount;
    }

    private void OnEnable()
    {
        GameCurator.OnInitializeGame += OnInitialize;
    }

    private void OnInitialize(Transform obj)
    {
        transform.position = new Vector3(transform.position.x, startHeight, transform.position.z);
    }

    private void OnDisable()
    {
        GameCurator.OnInitializeGame -= OnInitialize;
    }

    public void ReduceAmount()
    {
        this.currSupplyCount-= 1;
        float hp = (float)currSupplyCount / (float)supplyAmount;

        float newHeight = Mathf.Lerp(startHeight, endHeight, 1 - hp);
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);


        if (this.currSupplyCount <= 0)
            DestroySupply();

        OnSupplyHit?.Invoke(hp);
    }
    private void DestroySupply()
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
