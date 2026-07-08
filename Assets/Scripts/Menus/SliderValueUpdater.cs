using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueUpdater : MonoBehaviour
{
    private TextMeshProUGUI displayText;

    [SerializeField]
    private Slider slider;

    void Start()
    {
        displayText = GetComponent<TextMeshProUGUI>();
        UpdateTextDisplay();
        
    }


    public void UpdateTextDisplay()
    {
        displayText.text = (Mathf.Round(slider.value * 100) / 100).ToString();   
    }
   

}
