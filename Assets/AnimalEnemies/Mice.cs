using UnityEngine;
using UnityEngine.AI;

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
