using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RestartMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI batText, mouseText, pigText, totalScoreText, timeDurationText;

    void Start()
    {
        totalScoreText.text = $"Total Score: {0}";
        batText.text = $"Bats: {0}";
        mouseText.text = $"Mouse: {0}";
        pigText.text = $"Pig: {0}";
        timeDurationText.text = $"Duration: {0}";
    }

    private void OnEnable()
    {
        ScoreTracker.UpdateScoreUI += SetText;
    }

    private void OnDisable()
    {
        ScoreTracker.UpdateScoreUI -= SetText;
    }


    private void SetText(int totalScore, int batHit, int mouseHit, int pigHit, float timeDuration )
    {
        totalScoreText.text = $"Total Score: {totalScore}";
        batText.text = $"Bats: {batHit}";
        mouseText.text = $"Mouse: {mouseHit}";
        pigText.text = $"Pig: {pigHit}";
        timeDurationText.text = $"Duration: {timeDuration}";
    }

    
}
