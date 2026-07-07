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


    public static event Action OnSupplyBelowHalf;
    public bool belowHalfCalled;


    /// <summary>
    /// Event fired when losing health
    /// </summary>
    public event Action<float> OnSupplyHit;

    public static event Action OnStaticSupplyHit;

    private float startHeight;
    private float endHeight;

    void Start()
    {
        startHeight = transform.localPosition.y;
        endHeight = -startHeight;
        RestartSupply();
        
        GameCurator.OnInitializeGame += OnInitialize;
    }

    private void OnEnable()
    {
        GameCurator.OnInitializeGame += OnInitialize;
    }

    private void OnInitialize(Transform obj)
    {
        RestartSupply();
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
        transform.localPosition = new Vector3(transform.localPosition.x, newHeight, transform.localPosition.z);

        //Notify for music layer
        if (this.currSupplyCount <= supplyAmount * 0.5f && !belowHalfCalled)
        {
            OnSupplyBelowHalf?.Invoke();
            belowHalfCalled = true;
        } 

        if (this.currSupplyCount <= 0)
            DestroySupply();

        OnSupplyHit?.Invoke(hp);
        OnStaticSupplyHit?.Invoke();
    }
    private void DestroySupply()
    {
        OnSupplyDestroyed?.Invoke();
        //gameObject.SetActive(false);
    }

    public void RestartSupply()
    {
        currSupplyCount = supplyAmount;
        transform.localPosition = new Vector3(transform.localPosition.x, startHeight, transform.localPosition.z);
        belowHalfCalled = false;
        //gameObject.SetActive(true);
    }
}
