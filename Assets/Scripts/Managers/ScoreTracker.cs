using UnityEngine;
using System;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField]
    private GameTimer timer;

    private int scoreTotal;
    private int batHitCount, mouseHitCount, pigHitCount;

    private float timerCount;

    public static event Action<int, int, int, int, float> UpdateScoreUI;
    

    void Start()
    {
        timerCount = 0.0f;

        RestartValues();
    }

    public void AddPoint(AnimalType animal)
    {
        switch (animal)
        {
            case AnimalType.BAT:
                ++batHitCount;
                break;

            case AnimalType.MICE:
                ++mouseHitCount;
                break;

            case AnimalType.PIG:
                ++pigHitCount;
                break;
        }
    }

    public void CalculateTotalResult()
    {
        timerCount = timer.GetTime();
        scoreTotal = (int)((batHitCount * 2) + (mouseHitCount * 1) + (pigHitCount * 4) + timerCount); // + Timer 

        UpdateScoreUI?.Invoke(scoreTotal, batHitCount, mouseHitCount, pigHitCount, timerCount);
    }

    public void RestartValues()
    {
        scoreTotal = 0;
        batHitCount = 0;
        mouseHitCount = 0;
        pigHitCount = 0;
    }
}
