using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FoodSupplyUI : MonoBehaviour
{
    [Header("External")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image fillImage;
    private FoodSupply foodSupply;

    [SerializeField] private bool showUI;

    private void Start()
    {
        if (foodSupply == null)
            foodSupply = GetComponent<FoodSupply>();

        foodSupply.OnSupplyHit += OnFoodSupplyHit;
    }

    private void OnFoodSupplyHit(float obj)
    {
        if (fillImage == null)
            return;

        fillImage.fillAmount = 1-obj;
    }

    private void Update()
    {
        if (showUI)
        {
            if (!canvas.isActiveAndEnabled)
                canvas.gameObject.SetActive(true);
        }
        else
        {
            if (canvas.isActiveAndEnabled)
                canvas.gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        if (foodSupply != null)
        {
            foodSupply.OnSupplyHit -= OnFoodSupplyHit;
        }
    }
}
