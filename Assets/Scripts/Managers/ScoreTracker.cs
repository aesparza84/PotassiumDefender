using UnityEngine;
using System;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField]
    private GameTimer timer;

    private int scoreTotal;
    private int batHitCount, mouseHitCount, pigHitCount;

    private float timerCount;
    private int min;
    private int sec;

    public static event Action<int, int, int, int, float> UpdateScoreUI;
    

    void Start()
    {
        timerCount = 0.0f;

        RestartValues();

        Animal.OnFilledFromPlayer += OnAddToScore;
        GameCurator.OnInitializeGame += OnInitialize;
        GameCurator.OnGameOver += OnGameOver_CalcFinalScore;
    }

    private void OnGameOver_CalcFinalScore(float obj)
    {
        timerCount = obj;
        min = Mathf.FloorToInt(obj / 60);
        sec = Mathf.FloorToInt(obj % 60);
        CalculateTotalResult();
    }

    private void OnInitialize(Transform obj)
    {
        mouseHitCount = 0;
        batHitCount = 0;
        pigHitCount = 0;
        scoreTotal = 0;
    }

    private void OnAddToScore(AnimalType obj)
    {
        AddPoint(obj);
    }

    private void OnDisable()
    {
        GameCurator.OnInitializeGame -= OnInitialize;
        Animal.OnFilledFromPlayer -= OnAddToScore;
        GameCurator.OnGameOver += OnGameOver_CalcFinalScore;
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
        //timerCount = timer.GetTime();
        scoreTotal = (int)((batHitCount * 2) + (mouseHitCount * 1) + (pigHitCount * 3) + timerCount); // + Timer 

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
