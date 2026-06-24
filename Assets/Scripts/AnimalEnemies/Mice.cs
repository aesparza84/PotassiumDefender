using UnityEngine;

public class Mice : Animal
{
    void Start()
    {
        
        AnimalHungerMax = AnimalHungerMax == 0 ? 1 : AnimalHungerMax;

        SetInfoAtStart();
    }

    private void FixedUpdate()
    {
        TravelToGoal();
    }
}
