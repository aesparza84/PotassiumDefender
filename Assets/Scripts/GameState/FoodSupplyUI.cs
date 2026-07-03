using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FoodSupplyUI : MonoBehaviour
{
    [Header("External")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image baseImage;
    [SerializeField] private Image fillImage;
    private FoodSupply foodSupply;

    [SerializeField] private bool showUI;

    private void Start()
    {
        if (foodSupply == null)
            foodSupply = GetComponent<FoodSupply>();

        foodSupply.OnSupplyHit += OnFoodSupplyHit;

        //Start Off
        OnCleanUp();
    }

    private void OnEnable()
    {
        GameCurator.OnCleanUpGame += OnCleanUp;
        GameCurator.OnInitializeGame += OnInitializeGame;
    }

    private void OnInitializeGame(Transform obj)
    {
        if (baseImage != null)
            baseImage.gameObject.SetActive(true);

        if (fillImage != null)
        {
            fillImage.gameObject.SetActive(true);
            fillImage.fillAmount = 1;    
        }
    }

    private void OnCleanUp()
    {
        if (baseImage != null)
            baseImage.gameObject.SetActive(false);

        if (fillImage != null)
        {
            fillImage.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        GameCurator.OnCleanUpGame -= OnCleanUp;
        GameCurator.OnInitializeGame -= OnInitializeGame;

        if (foodSupply != null)
        {
            foodSupply.OnSupplyHit -= OnFoodSupplyHit;
        }
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
}
