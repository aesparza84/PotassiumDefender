using UnityEngine;

public class LeaderBoardInputToggler : MonoBehaviour
{
    //External
    [SerializeField] private GameObject InputHolder;

    void Start()
    {
        FadeTransition.OnReadyForScoreInput += ShowInput;      
        
        if (InputHolder != null)
            InputHolder.SetActive(false);
    }

    public void ShowInput(bool isActive)
    {
        if (InputHolder != null)
            InputHolder.SetActive(isActive);
    }


    private void OnDisable()
    {
        FadeTransition.OnReadyForScoreInput -= ShowInput;

    }
}
