using JetBrains.Annotations;
using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(GameTimer))]
public class GameCurator : MonoBehaviour
{
    [Header("External")]
    [SerializeField] private Transform playerSpawnPosition;

    /// <summary>
    /// Timer tracking how long player has survived
    /// </summary>
    private GameTimer timer;

    /// <summary>
    /// Event to set up game area when leaving main-menu
    /// </summary>
    public static event Action<Transform> OnInitializeGame;

    /// <summary>
    /// Event when game ends, signals game area clean up
    /// </summary>
    public static event Action OnCleanUpGame;


    /// <summary>
    /// Fires event alerting of game over. Includes total time in float seconds
    /// </summary>
    public static event System.Action<float> OnGameOver;

    private void Start()
    {
        if (timer == null)
            timer = GetComponent<GameTimer>();

        FoodSupply.OnSupplyDestroyed += OnSupplyDestroyed;

        timer.StartTimer();
    }

    /// <summary>
    /// Reacts to when player hits PLAY from main menu, raises Initialize event
    /// </summary>
    public void OnMainMenuPlay()
    {
        OnInitializeGame?.Invoke(playerSpawnPosition);
        timer.ResetTimer();
        timer.StartTimer();
    }

    /// <summary>
    /// Reacts to when player hits Game End triggers, raises: CleanUp event | GameOver event
    /// </summary>
    private void OnGameCleanUp()
    {
        OnCleanUpGame?.Invoke();
    }

    /// <summary>
    /// Reacts player loses (supply destroyed), raises GameOver event(elapsedTime)
    /// </summary>
    private void OnSupplyDestroyed()
    {
        timer.StopTimer();
        float time = timer.GetTime();
        int min = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);

        Debug.Log($"Game Over | {min:00}:{sec:00}");
        
        //GLOBAL game over event, with timer
        OnGameOver?.Invoke(time);

        OnGameCleanUp();
    }

    private void OnDisable()
    {
        FoodSupply.OnSupplyDestroyed -= OnSupplyDestroyed;
    }
}
