using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float currTime;
    private bool isTicking;
    void Start()
    {
        currTime = 0.0f;
    }

    void Update()
    {
        if (isTicking)
            currTime += Time.deltaTime;
    }

    public float GetTime()
    {
        return currTime;
    }

    public void RestTimer()
    {
        isTicking = false;
        currTime = 0.0f;
    }

    public void StartTimer()
    {
        isTicking = true;
    }

    public void StopTimer()
    {
        isTicking = false;
    }
}
