using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodSupplyUI : MonoBehaviour
{
    [Header("External")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image baseImage;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI UI_Num;
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
        MenuCameraManager.OnEnterGameplay += Activate;
    }

    private void Activate()
    {
        if (baseImage != null)
            baseImage.gameObject.SetActive(true);

        if (fillImage != null)
        {
            fillImage.gameObject.SetActive(true);
            fillImage.fillAmount = 0;
        }

        if (UI_Num != null)
        {
            UI_Num.gameObject.SetActive(true);
            UI_Num.text = "100%";
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

        if (UI_Num != null)
        {
            UI_Num.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        GameCurator.OnCleanUpGame -= OnCleanUp;
        MenuCameraManager.OnEnterGameplay -= Activate;

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

        if (UI_Num != null)
        {
            float val = (100 - (1 - obj) * 100);
            UI_Num.text = $"{val.ToString("0.0")}%";
        }
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
