using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(GameTimer))]
public class GameOverTrigger : MonoBehaviour
{
    /// <summary>
    /// Timer tracking how long player has survived
    /// </summary>
    private GameTimer timer;

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

    private void OnSupplyDestroyed()
    {
        timer.StopTimer();
        float time = timer.GetTime();
        int min = Mathf.FloorToInt(time / 60);
        int sec = Mathf.FloorToInt(time % 60);

        Debug.Log($"Game Over | {min:00}:{sec:00}");
        
        //GLOBAL game over event, with timer
        OnGameOver?.Invoke(time);
    }

    private void OnDisable()
    {
        FoodSupply.OnSupplyDestroyed -= OnSupplyDestroyed;
    }
}
